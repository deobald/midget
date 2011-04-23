using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// Defines a Direct3D font.
	/// </summary>
	public class Direct3DFont : IDisposable
	{
		/// <summary>
		/// Flags used to control rendering behavior for a <see cref="Direct3DFont"/>.
		/// </summary>
		[System.Flags]
		public enum RenderFlags
		{
			Centered = 0x0001,
			TwoSided = 0x0002, 
			Filtered = 0x0004,
		}

		public const int MaxNumberFontVertices = 50*6;

		private bool isZEnable = false;

		public Boolean ZBufferEnable { get { return this.isZEnable; } set { this.isZEnable = value; } }
			
		
		private System.Drawing.Font font;
		private readonly string fontName; //Never changes after construction, so made readonly for optimization potential.
		//private FontStyle fontStyle; //Not Currently Used.
		private int fontSize;
		private Device device;

		// Stateblocks for setting and restoring render states
		private StateBlock savedState;
		private StateBlock drawState;

		private TextureState textureState0;
		private TextureState textureState1;
		private Sampler samplerState0;
		private RenderStates renderState;
		private Texture fontTexture;
		private VertexBuffer vertexBuffer;
		private CustomVertex.TransformedColoredTextured[] fontVertices = new CustomVertex.TransformedColoredTextured[Direct3DFont.MaxNumberFontVertices];

		private int textureWidth; // Texture dimensions
		private int textureHeight;
		private float textureScale;
		private int spacingPerChar;
		private float[,] textureCoords = new float[128-32,4];


		#region Constructors
		/// <summary>
		/// Creates a new <see cref="Direct3DFont"/> object using a provided <see cref="System.Drawing.Font"/>.
		/// </summary>
		/// <param name="font">The <see cref="System.Drawing.Font"/> to base the 3D version upon.</param>
		public Direct3DFont(System.Drawing.Font font)
		{
			this.fontName = font.Name;
			this.fontSize = (int)font.Size;
			this.font = font;
		}

		/// <summary>
		/// Creates a new <see cref="Direct3DFont"/> object using the provided font name.
		/// </summary>
		/// <param name="fontName">The name of the installed font you wish to use.</param>
		/// <remarks>A <see cref="FontStyle"/> of <see cref="F:FontStyle.Regular"/> will be used with a size of 12.</remarks>
		public Direct3DFont(String fontName) : this(fontName, FontStyle.Regular, 12) {}
		
		/// <summary>
		/// Creates a new <see cref="Direct3DFont"/> object using the provided font name and <see cref="FontStyle"/>.
		/// </summary>
		/// <param name="fontName">The name of the installed font you wish to use.</param>
		/// <param name="style">The style of the font.</param>
		/// <remarks>The size will be set to 12.</remarks>
		public Direct3DFont(String fontName, FontStyle style) : this(fontName, style, 12) {}

		/// <summary>
		/// Creates a new <see cref="Direct3DFont"/> object using the provided font name, <see cref="FontStyle"/>, and size.
		/// </summary>
		/// <param name="fontName">The name of the installed font you wish to use.</param>
		/// <param name="style">The style of the font.</param>
		/// <param name="size">The size of the font.</param>
		public Direct3DFont(String fontName, FontStyle style, Int32 size)
		{
			this.fontName = fontName;
			this.fontSize = size;
			this.font = new System.Drawing.Font(fontName, size, style);
		}
		#endregion

		#region Direct3D device events

		public void InitializeDeviceObjects(Device dev)
		{
			if (dev != null)
			{
				// Set up our events
				dev.DeviceReset += new System.EventHandler(this.RestoreDeviceObjects);
			}

			// Keep a local copy of the device
			device = dev;
			textureState0 = device.TextureState[0];
			textureState1 = device.TextureState[1];
			samplerState0 = device.SamplerState[0];
			renderState = device.RenderState;

			// Create a bitmap on which to measure the alphabet
			Bitmap bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
			g.TextContrast = 0;
       
			// Establish the font and texture size
			textureScale  = 1.0f; // Draw fonts into texture without scaling

			// Calculate the dimensions for the smallest power-of-two texture which
			// can hold all the printable characters
			textureWidth = textureHeight = 128;
			for (;;)
			{
				try
				{
					// Measure the alphabet
					PaintAlphabet(g, true);
				}
				catch (System.InvalidOperationException)
				{
					// Scale up the texture size and try again
					textureWidth *= 2;
					textureHeight *= 2;
					continue;
				}

				break;
			}

			// If requested texture is too big, use a smaller texture and smaller font,
			// and scale up when rendering.
			Caps d3dCaps = device.DeviceCaps;

			// If the needed texture is too large for the video card...
			if (textureWidth > d3dCaps.MaxTextureWidth)
			{
				// Scale the font size down to fit on the largest possible texture
				textureScale = (float)d3dCaps.MaxTextureWidth / (float)textureWidth;
				textureWidth = textureHeight = d3dCaps.MaxTextureWidth;

				for(;;)
				{
					// Create a new, smaller font
					fontSize = (int) Math.Floor(fontSize * textureScale);      
					font = new System.Drawing.Font(font.Name, fontSize, font.Style);
                
					try
					{
						// Measure the alphabet
						PaintAlphabet(g, true);
					}
					catch (System.InvalidOperationException)
					{
						// If that still doesn't fit, scale down again and continue
						textureScale *= 0.9F;
						continue;
					}

					break;
				}
			}

			// Release the bitmap used for measuring and create one for drawing
			bmp.Dispose();
			bmp = new Bitmap(textureWidth, textureHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(bmp);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			g.TextContrast = 0;

			// Draw the alphabet
			PaintAlphabet(g, false);

			// Create a new texture for the font from the bitmap we just created
			fontTexture = Texture.FromBitmap(device, bmp, 0, Pool.Managed);
			RestoreDeviceObjects(null, null);
		}

		/// <summary>
		/// Restore the font after a device has been reset
		/// </summary>
		public void RestoreDeviceObjects(object sender, EventArgs e)
		{
			vertexBuffer = new VertexBuffer(typeof(CustomVertex.TransformedColoredTextured), Direct3DFont.MaxNumberFontVertices,
				device, Usage.WriteOnly | Usage.Dynamic, 0, Pool.Default);

			Surface surf = device.GetRenderTarget( 0 );
			bool bSupportsAlphaBlend = Manager.CheckDeviceFormat(device.DeviceCaps.AdapterOrdinal, 
				device.DeviceCaps.DeviceType, device.DisplayMode.Format, 
				Usage.RenderTarget | Usage.QueryPostPixelShaderBlending, ResourceType.Surface, 
				surf.Description.Format );

			// Create the state blocks for rendering text
			for (int which=0; which < 2; which++)
			{
				device.BeginStateBlock();
				device.SetTexture(0, fontTexture);

				if (isZEnable)
					renderState.ZBufferEnable = true;
				else
					renderState.ZBufferEnable = false;

				if( bSupportsAlphaBlend )
				{
					renderState.AlphaBlendEnable = true;
					renderState.SourceBlend = Blend.SourceAlpha;
					renderState.DestinationBlend = Blend.InvSourceAlpha;
				}
				else
				{
					renderState.AlphaBlendEnable = false;
				}
				renderState.AlphaTestEnable = true;
				renderState.ReferenceAlpha = 0x08;
				renderState.AlphaFunction = Compare.GreaterEqual;
				renderState.FillMode = FillMode.Solid;
				renderState.CullMode = Cull.CounterClockwise;
				renderState.StencilEnable = false;
				renderState.Clipping = true;
				device.ClipPlanes.DisableAll();
				renderState.VertexBlend = VertexBlend.Disable;
				renderState.IndexedVertexBlendEnable = false;
				renderState.FogEnable = false;
				renderState.ColorWriteEnable = ColorWriteEnable.RedGreenBlueAlpha;
				textureState0.ColorOperation = TextureOperation.Modulate;
				textureState0.ColorArgument1 = TextureArgument.TextureColor;
				textureState0.ColorArgument2 = TextureArgument.Diffuse;
				textureState0.AlphaOperation = TextureOperation.Modulate;
				textureState0.AlphaArgument1 = TextureArgument.TextureColor;
				textureState0.AlphaArgument2 = TextureArgument.Diffuse;
				textureState0.TextureCoordinateIndex = 0;
				textureState0.TextureTransform = TextureTransform.Disable; // REVIEW
				textureState1.ColorOperation = TextureOperation.Disable;
				textureState1.AlphaOperation = TextureOperation.Disable;
				samplerState0.MinFilter = TextureFilter.Point;
				samplerState0.MagFilter = TextureFilter.Point;
				samplerState0.MipFilter = TextureFilter.None;

				if (which==0)
					savedState = device.EndStateBlock();
				else
					drawState = device.EndStateBlock();
			}
		}
		#endregion

		/// <summary>
		/// Attempt to draw the font alphabet onto the provided texture graphics.
		/// </summary>
		/// <param name="g"><see cref="Graphics"/> object on which to draw and measure the letters</param>
		/// <param name="measureOnly">If set, the method will test to see if the alphabet will fit without actually drawing</param>
		public void PaintAlphabet(Graphics g, bool measureOnly)
		{
			string str;
			float x = 0;
			float y = 0;
			Point p = new Point(0, 0);
			Size size = new Size(0,0);
            
			// Calculate the spacing between characters based on line height
			size = g.MeasureString(" ", font).ToSize();
			x = spacingPerChar = (int) Math.Ceiling(size.Height * 0.3);

			for (char c = (char)32; c < (char)127; c++)
			{
				str = new String(c, 1);
				// We need to do some things here to get the right sizes.  The default implemententation of MeasureString
				// will return a resolution independant size.  For our height, this is what we want.  However, for our width, we 
				// want a resolution dependant size.
				Size resSize = g.MeasureString(str, font).ToSize();
				size.Height = resSize.Height + 1;

				// Now the Resolution independent width
				if (c != ' ') // We need the special case here because a space has a 0 width in GenericTypoGraphic stringformats
				{
					resSize = g.MeasureString(str, font, p, StringFormat.GenericTypographic).ToSize();
					size.Width = resSize.Width;
				}
				else
					size.Width = resSize.Width;

				if ((x + size.Width + spacingPerChar) > textureWidth)
				{
					x = spacingPerChar;
					y += size.Height;
				}

				// Make sure we have room for the current character
				if ((y + size.Height) > textureHeight)
					throw new System.InvalidOperationException("Texture too small for alphabet");
                
				if (!measureOnly)
				{
					if (c != ' ') // We need the special case here because a space has a 0 width in GenericTypoGraphic stringformats
						g.DrawString(str, font, Brushes.White, new Point((int)x, (int)y), StringFormat.GenericTypographic);
					else
						g.DrawString(str, font, Brushes.White, new Point((int)x, (int)y));
					textureCoords[c-32,0] = ((float) (x + 0           - spacingPerChar)) / textureWidth;
					textureCoords[c-32,1] = ((float) (y + 0           + 0)) / textureHeight;
					textureCoords[c-32,2] = ((float) (x + size.Width  + spacingPerChar)) / textureWidth;
					textureCoords[c-32,3] = ((float) (y + size.Height + 0)) / textureHeight;
				}

				x += size.Width + (2 * spacingPerChar);
			}

		}

		public void DrawText(float x, float y, Color color, string text)
		{
			DrawText(x, y, color, text, RenderFlags.Filtered);
		}

		public void DrawText(float x, float y, Color color, string text, RenderFlags flags)
		{
			if (text == null)
				return;

			// Setup renderstate
			savedState.Capture();
			drawState.Apply();
			device.SetTexture(0, fontTexture);
			device.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
			device.PixelShader = null;
			device.SetStreamSource(0, vertexBuffer, 0);

			// Set filter states
			if ((flags & RenderFlags.Filtered) != 0)
			{
				samplerState0.MinFilter = TextureFilter.Linear;
				samplerState0.MagFilter = TextureFilter.Linear;
			}

			// Adjust for character spacing
			x -= spacingPerChar;
			float startX = x;

			// Fill vertex buffer
			int iv = 0;
			int dwNumTriangles = 0;

			foreach (char c in text)
			{
				if (c == '\n')
				{
					x = startX;
					y += (textureCoords[0,3]-textureCoords[0,1])*textureHeight;
				}

				if ((c-32) < 0 || (c-32) >= 128-32)
					continue;

				float tx1 = textureCoords[c-32,0];
				float ty1 = textureCoords[c-32,1];
				float tx2 = textureCoords[c-32,2];
				float ty2 = textureCoords[c-32,3];

				float w = (tx2-tx1) *  textureWidth / textureScale;
				float h = (ty2-ty1) * textureHeight / textureScale;

				int intColor = color.ToArgb();
				if (c != ' ')
				{
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(x+0-0.5f,y+h-0.5f,0.9f,1.0f), intColor, tx1, ty2);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(x+0-0.5f,y+0-0.5f,0.9f,1.0f), intColor, tx1, ty1);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(x+w-0.5f,y+h-0.5f,0.9f,1.0f), intColor, tx2, ty2);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(x+w-0.5f,y+0-0.5f,0.9f,1.0f), intColor, tx2, ty1);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(x+w-0.5f,y+h-0.5f,0.9f,1.0f), intColor, tx2, ty2);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(x+0-0.5f,y+0-0.5f,0.9f,1.0f), intColor, tx1, ty1);
					dwNumTriangles += 2;

					if (dwNumTriangles*3 > (Direct3DFont.MaxNumberFontVertices-6))
					{
						// Set the data for the vertexbuffer
						vertexBuffer.SetData(fontVertices, 0, LockFlags.Discard);
						device.DrawPrimitives(PrimitiveType.TriangleList, 0, dwNumTriangles);
						dwNumTriangles = 0;
						iv = 0;
					}
				}

				x += w - (2 * spacingPerChar);
			}

			// Set the data for the vertex buffer
			vertexBuffer.SetData(fontVertices, 0, LockFlags.Discard);
			if (dwNumTriangles > 0)
				device.DrawPrimitives(PrimitiveType.TriangleList, 0, dwNumTriangles);

			// Restore the modified renderstates
			savedState.Apply();
		}

		#region scaled text functions

		/// <summary>
		/// Draws scaled 2D text.  Note that x and y are in viewport coordinates
		/// (ranging from -1 to +1).  scaleX and scaleY are the size fraction 
		/// relative to the entire viewport.  For example, a scaleX of 0.25 is
		/// 1/8th of the screen width.  This allows you to output text at a fixed
		/// fraction of the viewport, even if the screen or window size changes.
		/// </summary>
		public void DrawTextScaled(float x, float y, float z, 
			float scaleX, float scaleY, 
			Color color,
			string text)
		{
			this.DrawTextScaled(x,y,z,scaleX, scaleY, color, text, 0);
		}

		/// <summary>
		/// Draws scaled 2D text.  Note that x and y are in viewport coordinates
		/// (ranging from -1 to +1).  scaleX and scaleY are the size fraction 
		/// relative to the entire viewport.  For example, a scaleX of 0.25 is
		/// 1/8th of the screen width.  This allows you to output text at a fixed
		/// fraction of the viewport, even if the screen or window size changes.
		/// </summary>
		/// <exception cref="ArgumentNullException">The device parameter cannot be null.</exception>
		public void DrawTextScaled(float x, float y, float z, 
			float scaleX, float scaleY, 
			Color color,
			string text, RenderFlags flags)
		{
			if (device == null)
				throw new System.ArgumentNullException();

			// Set up renderstate
			savedState.Capture();
			drawState.Apply();
			device.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
			device.PixelShader = null;
			device.SetStreamSource(0, vertexBuffer, 0);

			// Set filter states
			if ((flags & RenderFlags.Filtered) != 0)
			{
				samplerState0.MinFilter = TextureFilter.Linear;
				samplerState0.MagFilter = TextureFilter.Linear;
			}

			Viewport vp = device.Viewport;
			float xpos = (x+1.0f)*vp.Width/2;
			float ypos = (y+1.0f)*vp.Height/2;
			float sz = z;
			float rhw = 1.0f;
			float fLineHeight = (textureCoords[0,3] - textureCoords[0,1]) * textureHeight;

			// Adjust for character spacing
			xpos -= spacingPerChar * (scaleX*vp.Height)/fLineHeight;
			float fStartX = xpos;

			// Fill vertex buffer
			int numTriangles = 0;
			int realColor = color.ToArgb();
			int iv = 0;

			foreach (char c in text)
			{
				if (c == '\n')
				{
					xpos  = fStartX;
					ypos += scaleY*vp.Height;
				}

				if ((c-32) < 0 || (c-32) >= 128-32)
					continue;

				float tx1 = textureCoords[c-32,0];
				float ty1 = textureCoords[c-32,1];
				float tx2 = textureCoords[c-32,2];
				float ty2 = textureCoords[c-32,3];

				float w = (tx2-tx1)*textureWidth;
				float h = (ty2-ty1)*textureHeight;

				w *= (scaleX*vp.Height)/fLineHeight;
				h *= (scaleY*vp.Height)/fLineHeight;

				if (c != ' ')
				{
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(xpos+0-0.5f,ypos+h-0.5f,sz,rhw), realColor, tx1, ty2);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(xpos+0-0.5f,ypos+0-0.5f,sz,rhw), realColor, tx1, ty1);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(xpos+w-0.5f,ypos+h-0.5f,sz,rhw), realColor, tx2, ty2);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(xpos+w-0.5f,ypos+0-0.5f,sz,rhw), realColor, tx2, ty1);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(xpos+w-0.5f,ypos+h-0.5f,sz,rhw), realColor, tx2, ty2);
					fontVertices[iv++] = new CustomVertex.TransformedColoredTextured(new Vector4(xpos+0-0.5f,ypos+0-0.5f,sz,rhw), realColor, tx1, ty1);
					numTriangles += 2;

					if (numTriangles*3 > (Direct3DFont.MaxNumberFontVertices-6))
					{
						// Unlock, render, and relock the vertex buffer
						vertexBuffer.SetData(fontVertices, 0, LockFlags.Discard);
						device.DrawPrimitives(PrimitiveType.TriangleList , 0, numTriangles);
						numTriangles = 0;
						iv = 0;
					}
				}

				xpos += w - (2 * spacingPerChar) * (scaleX*vp.Height)/fLineHeight;
			}

			// Unlock and render the vertex buffer
			vertexBuffer.SetData(fontVertices, 0, LockFlags.Discard);
			if (numTriangles > 0)
				device.DrawPrimitives(PrimitiveType.TriangleList , 0, numTriangles);

			// Restore the modified renderstates
			savedState.Apply();
		}
		#endregion

		#region 3D text functions

		/// <summary>
		/// Renders 3D text.
		/// </summary>
		/// <param name="text">The text to be rendered.</param>
		/// <param name="flags">One or more flags to define how the font is rendered.</param>
		public void Render3DText(String text, RenderFlags flags)
		{
			if (device == null)
				throw new System.ArgumentNullException();

			// Set up renderstate
			savedState.Capture();
			drawState.Apply();
			device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
			device.PixelShader = null;
			device.SetStreamSource(0, vertexBuffer, 0, VertexInformation.GetFormatSize(CustomVertex.PositionNormalTextured.Format));

			// Set filter states
			if ((flags & RenderFlags.Filtered) != 0)
			{
				samplerState0.MinFilter = TextureFilter.Linear;
				samplerState0.MagFilter = TextureFilter.Linear;
			}

			// Position for each text element
			float x = 0.0f;
			float y = 0.0f;

			// Center the text block at the origin
			if ((flags & RenderFlags.Centered) != 0)
			{
				System.Drawing.SizeF sz = GetTextExtent(text);
				x = -(((float)sz.Width)/10.0f)/2.0f;
				y = -(((float)sz.Height)/10.0f)/2.0f;
			}

			// Turn off culling for two-sided text
			if ((flags & RenderFlags.TwoSided) != 0)
				renderState.CullMode = Cull.None;

			// Adjust for character spacing
			x -= spacingPerChar / 10.0f;
			float fStartX = x;

			// Fill vertex buffer
			GraphicsStream strm = vertexBuffer.Lock(0, 0, LockFlags.Discard);
			int numTriangles = 0;

			foreach (char c in text)
			{
				if (c == '\n')
				{
					x = fStartX;
					y -= (textureCoords[0,3]-textureCoords[0,1])*textureHeight/10.0f;
				}

				if ((c-32) < 0 || (c-32) >= 128-32)
					continue;

				float tx1 = textureCoords[c-32,0];
				float ty1 = textureCoords[c-32,1];
				float tx2 = textureCoords[c-32,2];
				float ty2 = textureCoords[c-32,3];

				float w = (tx2-tx1) * textureWidth  / (10.0f * textureScale);
				float h = (ty2-ty1) * textureHeight / (10.0f * textureScale);

				if (c != ' ')
				{
					strm.Write(new CustomVertex.PositionNormalTextured(new Vector3(x+0,y+0,0), new Vector3(0,0,-1), tx1, ty2));
					strm.Write(new CustomVertex.PositionNormalTextured(new Vector3(x+0,y+h,0), new Vector3(0,0,-1), tx1, ty1));
					strm.Write(new CustomVertex.PositionNormalTextured(new Vector3(x+w,y+0,0), new Vector3(0,0,-1), tx2, ty2));
					strm.Write(new CustomVertex.PositionNormalTextured(new Vector3(x+w,y+h,0), new Vector3(0,0,-1), tx2, ty1));
					strm.Write(new CustomVertex.PositionNormalTextured(new Vector3(x+w,y+0,0), new Vector3(0,0,-1), tx2, ty2));
					strm.Write(new CustomVertex.PositionNormalTextured(new Vector3(x+0,y+h,0), new Vector3(0,0,-1), tx1, ty1));
					numTriangles += 2;

					if (numTriangles*3 > (Direct3DFont.MaxNumberFontVertices-6))
					{
						// Unlock, render, and relock the vertex buffer
						vertexBuffer.Unlock();
						device.DrawPrimitives(PrimitiveType.TriangleList , 0, numTriangles);
						strm = vertexBuffer.Lock(0, 0, LockFlags.Discard);
						numTriangles = 0;
					}
				}

				x += w - (2 * spacingPerChar) / 10.0f;
			}

			// Unlock and render the vertex buffer
			vertexBuffer.Unlock();
			if (numTriangles > 0)
				device.DrawPrimitives(PrimitiveType.TriangleList , 0, numTriangles);

			// Restore the modified renderstates
			savedState.Apply();
		}


		/// <summary>
		/// Get the dimensions of a text string.
		/// </summary>
		/// <param name="text">The text to measure.</param>
		private System.Drawing.SizeF GetTextExtent(String text)
		{
			if (null == text || text.Length == 0)
				throw new System.ArgumentNullException();

			float fRowWidth  = 0.0f;
			float fRowHeight = (textureCoords[0,3]-textureCoords[0,1])*textureHeight;
			float fWidth     = 0.0f;
			float fHeight    = fRowHeight;

			foreach (char c in text)
			{
				if (c == '\n')
				{
					fRowWidth = 0.0f;
					fHeight  += fRowHeight;
				}

				if ((c-32) < 0 || (c-32) >= 128-32)
					continue;

				float tx1 = textureCoords[c-32,0];
				float tx2 = textureCoords[c-32,2];

				fRowWidth += (tx2-tx1)*textureWidth - 2*spacingPerChar;

				if (fRowWidth > fWidth)
					fWidth = fRowWidth;
			}

			return new System.Drawing.SizeF(fWidth, fHeight);
		}

		#endregion

		#region IDisposable Members
		/// <summary>
		/// Disposes the internally-stored <see cref="System.Drawing.Font"/>.
		/// </summary>
		public void Dispose()
		{
			if (font != null)
				font.Dispose();

			this.font = null;
		}
		#endregion
	}
}