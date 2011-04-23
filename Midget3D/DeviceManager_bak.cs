using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace Midget
{
	/// <summary>
	/// DeviceManager is responsible for the rendering of the objects in
	/// the various view ports
	/// </summary>
	public sealed class DeviceManager
	{
		#region Private Member Variables
		
		/// <summary>
		/// DirectX Device that is responsible for rendering the scene
		/// </summary>
		private Device device = null;
						
		/// <summary>
		/// Stores all the 3d objects that are currently inserted into the scene
		/// </summary>
		private ArrayList objectList;

		/// <summary>
		/// Stores all the surfaces that can be rendered too
		/// </summary>
		private ArrayList renderSurfaceList;
		
		/// <summary>
		/// Parameters used to setup the device and the swap chains
		/// </summary>
		PresentParameters presentParams;

		#endregion

		private DeviceManager()
		{
			objectList = new ArrayList();
			renderSurfaceList = new ArrayList();

			// init object delegates
			MidgetEvent.ObjectEventFactory.Instance.CreateObject += new MidgetEvent.CreateObjectEventHandler(this.CreateObject);
			MidgetEvent.ObjectEventFactory.Instance.DeleteObject += new MidgetEvent.DeleteObjectEventHandler(this.DeleteObject);
		}

		// the DM is a singleton, so a method of obtaining an instance must be provided
		public static readonly DeviceManager Instance = new DeviceManager();
		
		/// <summary>
		/// Delegate to be assigned to the CreateObject event
		/// </summary>
		/// <param name="sender">Generally the ObjectEventFactory</param>
		/// <param name="e">The parameters describing the created object</param>
		private void CreateObject(object sender, MidgetEvent.CreateObjectEventArgs e)
		{
			// add this object to the list
			SceneManager.Instance.Scene.addChild(e.CreatedObject);
			objectList.Add(e.CreatedObject);
		}

		private void DeleteObject(object sender, MidgetEvent.DeleteObjectEventArgs e)
		{
			// delete this object from the list
			SceneManager.Instance.Scene.removeChild(e.DeletedObject);
			objectList.Remove(e.DeletedObject);
		}

		/// <summary>
		/// Initialize the device (hardware support, Z-buffer, blending, etc)
		/// </summary>
		/// <param name="renderSurface">Surface where the scene is going to be renedered to</param>
		private void InitializeDevice(IRenderSurface renderSurface)
		{
			// setup the present parameters before the device is created
			SetupPresentParameters(renderSurface);

			// get the capabilities of the graphics card
			Caps gfxCapabilities = Manager.GetDeviceCaps(0,DeviceType.Hardware);

			// if the device supports hardware Vertex processing use it
			if(gfxCapabilities.DeviceCaps.SupportsHardwareTransformAndLight)
				device = new Device(0,DeviceType.Hardware,(System.Windows.Forms.Control)renderSurface, CreateFlags.HardwareVertexProcessing | CreateFlags.MultiThreaded, presentParams);
			else
				device = new Device(0,DeviceType.Hardware,(System.Windows.Forms.Control)renderSurface, CreateFlags.SoftwareVertexProcessing | CreateFlags.MultiThreaded, presentParams);
		
			// enabled z-buffering so objects don't have to be drawn in correct order (back-to-front)
			device.RenderState.ZBufferEnable = true;

			// allow transparent textures
			device.RenderState.SourceBlend = Blend.SourceAlpha;
			device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
		}
		
		/// <summary>
		/// Configures the present parameters
		/// </summary>
		/// <param name="renderSurface">Surface where the scene is going to be renedered to</param>
		private void SetupPresentParameters (IRenderSurface renderSurface)
		{
			System.Windows.Forms.Control view = (System.Windows.Forms.Control)renderSurface;

			// reset Present Parameters for the new swap
			presentParams = new PresentParameters();

			presentParams.Windowed = true;
			presentParams.SwapEffect = SwapEffect.Copy;
			
			// setup the backbuffer
			presentParams.DeviceWindow = view;

			// setup back buffer
			//presentParams.BackBufferFormat = Manager.Adapters[0].CurrentDisplayMode.Format;
			presentParams.BackBufferFormat = Manager.Adapters[0].CurrentDisplayMode.Format;
			presentParams.BackBufferHeight = view.ClientSize.Height;
			presentParams.BackBufferWidth = view.ClientSize.Width;

			// make sure graphics card can support 16-bit depth stenciling
			if(Manager.CheckDepthStencilMatch(0, DeviceType.Hardware,
				presentParams.BackBufferFormat,
				presentParams.BackBufferFormat,
				DepthFormat.D16))
			{
				// enabling automatic depth stencilling
				presentParams.AutoDepthStencilFormat = DepthFormat.D16;
				presentParams.EnableAutoDepthStencil = true;
			}
			else
				throw new Exception("Graphics card does not support 16 bit depth stenciling");
		}

		/// <summary>
		/// Set up lights for the current scene
		/// </summary>
		private void SetupLights()
		{
			System.Drawing.Color col = System.Drawing.Color.Yellow;
			// Set up a material. The material here just has the diffuse and ambient
			// colors set to yellow. Note that only one material can be used at a time.
			Direct3D.Material mtrl = new Direct3D.Material();
			mtrl.Diffuse = col;
			mtrl.Ambient = col;
			device.Material = mtrl;
			
			// Set up a white, directional light, with an oscillating direction.
			// Note that many lights may be active at a time (but each one slows down
			// the rendering of our scene). However, here we are just using one. Also,
			// we need to set the D3DRS_LIGHTING renderstate to enable lighting
    
			device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Diffuse = System.Drawing.Color.Pink;
			device.Lights[0].Direction = new Vector3(0.0f, 0.0f, 15.0f);
			//Vector3((float)Math.Cos(Environment.TickCount / 250.0f), 1.0f, (float)Math.Sin(Environment.TickCount / 250.0f));

			device.Lights[0].Commit();			//let d3d know about the light
			device.Lights[0].Enabled = true;	//turn it on

			// Finally, turn on some ambient light.
			// Ambient light is light that scatters and lights all objects evenly
			device.RenderState.Ambient = System.Drawing.Color.FromArgb(0x202020);
		}

		/// <summary>
		/// Add a viewport (based on its display surface), and set up its swap chain
		/// </summary>
		/// <param name="renderTarget">the display surface to be drawn to</param>
		/// <returns>true if successful, false if an error occurs</returns>
		public bool AddViewport(IRenderSurface renderTarget)
		{
			try
			{
				// check for device existence, create if needed
				if (device == null)
				{
					InitializeDevice(renderTarget);
					
					// get Device SwapChain
					renderTarget.SwapChain = device.GetSwapChain(0);

					// add renderSurface to the list
					renderSurfaceList.Add(renderTarget);
				}

					// this is an additional view
				else
				{
					SetupPresentParameters(renderTarget);

					// create new swap chain
					renderTarget.SwapChain = new SwapChain(device,presentParams);
					
					// add renderSurface to the list
					renderSurfaceList.Add(renderTarget);
				}
			}
			catch	// an error occurred somewhere
			{
				return false;
			}
			
			renderTarget.DepthStencil = device.CreateDepthStencilSurface(renderTarget.ClientWidth,renderTarget.ClientHeight,DepthFormat.D16, MultiSampleType.None,0,true);

			device.RenderState.ZBufferEnable = true;

			return true;
		}
		
		/// <summary>
		/// Add an object to the current scene
		/// </summary>
		/// <param name="obj">The object to be added to the scene</param>
		public void AddObject(IObject3D obj)
		{
			MidgetEvent.ObjectEventFactory.Instance.CreateCreateEvent(obj);
			this.Render();
		}

		/// <summary>
		/// Remove an object from the scene
		/// </summary>
		/// <param name="objectName">The unique name of the object to be deleted</param>
		public void RemoveObject(string objectName)
		{
			MidgetEvent.ObjectEventFactory.Instance.CreateDeleteEvent(objectName);
			this.Render();
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
				device.RenderState.CullMode = Cull.CounterClockwise;
				device.RenderState.FillMode = FillMode.Solid;
			}
				// else turn on culling so all points, and lines that make up the object
				// are displayed
			else if (mode == DrawMode.wireFrame)
			{
				device.RenderState.CullMode = Cull.None;
				device.RenderState.FillMode = FillMode.WireFrame;
			}
			else if (mode == DrawMode.point)
			{
				device.RenderState.CullMode = Cull.None;
				device.RenderState.FillMode = FillMode.Point;
			}
		}

		/// <summary>
		/// Retrieve a reference to the current device
		/// </summary>
		public Device Device
		{
			get{ return device; }
		}

		/// <summary>
		/// Render all the current views
		/// </summary>
		public void Render()
		{
			// lights
			SetupLights();
			
			// render all the viewports
			foreach (IRenderSurface renderSurface in renderSurfaceList)
			{
				RenderSingleView(renderSurface);
			}
		}

		/// <summary>
		/// Renders the current scene in a single viewport
		/// </summary>
		/// <param name="renderSurface">The view that is to be rendered too</param>
		public void RenderSingleView(IRenderSurface renderSurface)
		{	
			// make sure the device is initialised and that the view is initialised
			if((device != null) && (renderSurface.SwapChain != null) && (!renderSurface.SwapChain.Disposed))
			{
				
				// get the backbuffer of the surface that is to be rendered to
				Surface surface =  renderSurface.SwapChain.GetBackBuffer(0,BackBufferType.Mono);
				
				// set the device so it renders to the back buffer
				device.SetRenderTarget(0,surface);
				
				surface.ReleaseGraphics();

				device.DepthStencilSurface = renderSurface.DepthStencil;
				
				// get the camera that current render surface is using
				MidgetCameras.MidgetCamera camera = renderSurface.Camera;

				// adjust the device camera 
				device.Transform.View = camera.ViewMatrix;
				device.Transform.Projection = camera.ProjectionMatrix;
				
				//device.PresentationParameters.DeviceWindow.
				
				// clear the backbuffer
				device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, renderSurface.BackColor, 1.0f, 0);

				// setup the render quality for the viewport
				SetupDrawMode(renderSurface.DrawMode);

				// start drawing the scene
				device.BeginScene();

				// render the scene
				//device.Transform.World = Matrix.Identity;
				SceneManager.Instance.Scene.Render(device, Matrix.Identity, SceneManager.Instance.CurrentFrameIndex);

				// display viewport description text
				Microsoft.DirectX.Direct3D.Font viewportD3DFont 
					= new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font("Arial", 12, FontStyle.Bold));
				viewportD3DFont.DrawText(renderSurface.Description, new Rectangle(4, 4, 100, 30), 
					DrawTextFormat.Left, Color.Black);
				// TODO: FIX: D3DFont line causes NullReferenceExceptions to be thrown sometimes
				viewportD3DFont.DrawText(renderSurface.Description, new Rectangle(5, 5, 100, 30), 
					DrawTextFormat.Left, Color.White);
				viewportD3DFont.Dispose();

				// end the scene and display it
				device.EndScene();
				renderSurface.SwapChain.Present();
			}
		}

		
		public void SetSwapChain(IRenderSurface renderTarget)
		{
			if(renderTarget.SwapChain != null)
			{
				renderTarget.SwapChain.Dispose();

				renderTarget.SwapChain = null;

				SetupPresentParameters(renderTarget);

				// create new swap chain
				renderTarget.SwapChain = new SwapChain(device,presentParams);
			}

			if(renderTarget.DepthStencil != null)
			{
				renderTarget.DepthStencil.Dispose();

				renderTarget.DepthStencil = null;
			
				renderTarget.DepthStencil = device.CreateDepthStencilSurface(renderTarget.ClientWidth,renderTarget.ClientHeight,DepthFormat.D16, MultiSampleType.None,0,true);
			}
		}
		#region Properties

		public ArrayList ObjectList
		{
			get { return objectList; }
			set { objectList = value; }
		}

		#endregion
	}


}
