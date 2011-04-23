using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using MDX9Lib.Direct3D;

namespace Midget
{
	/// <summary>
	/// Summary description for RevisedDeviceManager.
	/// </summary>
	public class DeviceManager: MDX9Lib.Direct3D.Direct3DHost
	{
		private GraphicsFont drawingFont = null; // Font for drawing text
		private System.Drawing.Font arialFont = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold); // Font we use 
		private bool isDeviceLost;

		private DeviceManager()
		{
		}

		public static readonly DeviceManager Instance = new DeviceManager();
		
		/// <summary>
		/// EventHandler to inform Viewports that their scene is out of date, and they need
		/// to request to be rendered
		/// </summary>
		public EventHandler Render;
		
		/// <summary>
		/// Update all views that are currently attached to the current device
		/// </summary>
		public void UpdateViews()
		{
			// fire new rendering event, so each viewport asks to be rendered
			if(Render != null)
				this.Render(null,null);
		}
		

		/// <summary>
		/// Set up lights for the current scene
		/// </summary>
		private void SetupLights()
		{
			System.Drawing.Color col = System.Drawing.Color.Yellow;
			// Set up a material. The material here just has the diffuse and ambient
			// colors set to yellow. Note that only one material can be used at a time.
			Material mtrl = new Material();
			mtrl.Diffuse = col;
			mtrl.Ambient = col;
			base.Device.Material = mtrl;
			
			// Set up a white, directional light, with an oscillating direction.
			// Note that many lights may be active at a time (but each one slows down
			// the rendering of our scene). However, here we are just using one. Also,
			// we need to set the D3DRS_LIGHTING renderstate to enable lighting
    
			base.Device.Lights[0].Type = LightType.Directional;
			base.Device.Lights[0].Diffuse = System.Drawing.Color.Pink;
			base.Device.Lights[0].Direction = new Vector3(0.0f, 0.0f, 15.0f);
			//Vector3((float)Math.Cos(Environment.TickCount / 250.0f), 1.0f, (float)Math.Sin(Environment.TickCount / 250.0f));

			base.Device.Lights[0].Commit();			//let d3d know about the light
			base.Device.Lights[0].Enabled = true;	//turn it on

			// Finally, turn on some ambient light.
			// Ambient light is light that scatters and lights all objects evenly
			base.Device.RenderState.Ambient = System.Drawing.Color.FromArgb(0x202020);
		}


		/// <summary>
		/// Event Handler to handle a request by a surface to rendered by the device
		/// </summary>
		/// <param name="sender">The object that made the request</param>
		/// <param name="e">The arguments for the event</param>
		public void OnRenderSingleView(Object sender, RenderSingleViewEventArgs e) 
		{
			// if the device has been lost, check it's state
			if( base.Device != null && this.isDeviceLost)
			{
				try
				{
					// test co-operation level to make sure it is ok to render
					base.Device.TestCooperativeLevel();
				}
				catch (DeviceLostException)
				{
					// don't do anything if the device has been lost
					return;
				}
				catch (DeviceNotResetException)
				{
					base.RefreshPresentParameters();
					// device can be resest so do so
					base.Device.Reset(base.PresentParameters);
				}
				// ok to move on
				this.isDeviceLost = false;
			}

			IRenderSurface renderSurface = (IRenderSurface)sender;

			// render to the surface
			if( base.Device != null)
			{
				this.SetupLights();
				// set the render target
				base.Device.SetRenderTarget(0,renderSurface.RenderTarget);
				
				// set the depth stencil
				base.Device.DepthStencilSurface = renderSurface.DepthStencil;
				
				// set the camera 
				base.Device.Transform.View = renderSurface.Camera.ViewMatrix;
				base.Device.Transform.Projection = renderSurface.Camera.ProjectionMatrix;

				// clear the backBuffer
				base.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, renderSurface.BackColor, 1.0f, 0);

				// setup the render quality for the viewport
				SetupDrawMode(renderSurface.DrawMode);

				// begin the scene
				base.Device.BeginScene();

				// render the scene
				e.Scene.Render(base.Device,renderSurface.Camera.WorldMatrix, e.FrameNumber);
				
				// render viewport descriptive text
				if(renderSurface.NameVisible)
				{
					if (drawingFont == null)
					{
						drawingFont = new GraphicsFont(arialFont);
						drawingFont.InitializeDeviceObjects(base.Device);
					}

					// draw GraphicsFont in 2D
					drawingFont.DrawText(4, 4, Color.Black, renderSurface.Camera.Name);
					drawingFont.DrawText(6, 6, Color.White, renderSurface.Camera.Name);
				}


				base.Device.EndScene();

				// if the their is a window to render too do so
				if( renderSurface.SwapChain != null)
				{
					renderSurface.SwapChain.Present();
				}
			}
		}
	
		/// <summary>
		/// Configure the draw mode of the device
		/// </summary>
		/// <param name="mode">Specifies the draw mode the device is too be switched too</param>
		private void SetupDrawMode(DrawMode mode)
		{
			// if the draw mode is solid turn culling on to save processing time
			if(mode == DrawMode.solid)
			{
				base.Device.RenderState.CullMode = Cull.CounterClockwise;
				base.Device.RenderState.FillMode = FillMode.Solid;
			}
				// else turn on culling so all points, and lines that make up the object
				// are displayed
			else if (mode == DrawMode.wireFrame)
			{
				base.Device.RenderState.CullMode = Cull.None;
				base.Device.RenderState.FillMode = FillMode.WireFrame;
			}
			else if (mode == DrawMode.point)
			{
				base.Device.RenderState.CullMode = Cull.None;
				base.Device.RenderState.FillMode = FillMode.Point;
			}
		}
	}

		public delegate void RenderSingleViewHandler(object sender, RenderSingleViewEventArgs e);

	/// <summary>
	/// Event arguments for a request by a renderSurface to be rendered by any
	/// device that is listening for the event
	/// </summary>
	public class RenderSingleViewEventArgs : EventArgs
	{
		private readonly IObject3D scene;
		private readonly int frameNumber;
  
		public RenderSingleViewEventArgs(IObject3D initScene, int initFrameNumber)
		{
			this.scene = initScene;
			this.frameNumber = initFrameNumber;
		}
		public IObject3D Scene
		{
			get { return scene; }
		}

		public int FrameNumber
		{
			get { return frameNumber; }
		}
	}
}
