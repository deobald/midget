using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{
	/// Render surface that doesn't have a swapChain and that can't be seen
	/// </summary>
	public class HiddenRenderSurface : IRenderSurface
	{
		private DeviceManager dm;
		private IObject3D scene;

		private Midget.Cameras.MidgetCamera camera;
		private Color bgColor = Color.Black;

		private int height;
		private int width;

		private Surface renderTarget;
		private Surface depthStencil;

		private event RenderSingleViewHandler renderView;

		public HiddenRenderSurface( DeviceManager initDM, IObject3D initScene, int initWidth, int initHeight)
		{
			this.dm = initDM;
			this.scene = initScene;
			this.width = initWidth;
			this.height = initHeight;

			this.InitRenderTarget();

			this.renderView += new RenderSingleViewHandler(dm.OnRenderSingleView); 
		}

		public void Render(int frameNumber )
		{
			// if the camera hasn't been initialized
			if(camera == null)
				throw new Exception("CRITICAL ERROR! Camera must be initialised before InvisibleRenderSurface can proceed");

			this.renderView(this, new RenderSingleViewEventArgs(scene,frameNumber));
		}

		#region IRenderSurface Members
		

		private void InitRenderTarget()
		{	
			PresentParameters pp = dm.PresentParameters;

			this.renderTarget = dm.Device.CreateRenderTarget(width, height, pp.BackBufferFormat, pp.MultiSample, 
				pp.MultiSampleQuality, true); 
		
			this.depthStencil = dm.Device.CreateDepthStencilSurface(width, height, pp.AutoDepthStencilFormat, pp.MultiSample,
				pp.MultiSampleQuality, true);
		}

		/// <summary>
		/// Return's null.  InvisibleRenderSurface does not have a swapChain
		/// </summary>
		public Microsoft.DirectX.Direct3D.SwapChain SwapChain
		{
			get{ return null; }
		}

		public Midget.Cameras.MidgetCamera Camera
		{
			get { return camera; }
			set { camera = value; }
		}

		public Midget.DrawMode DrawMode
		{
			// always render a solid
			get { return DrawMode.solid; }
			set { }
		}

		public bool NameVisible
		{
			get { return false; }
			set {}
		}

		public System.Drawing.Color BackColor
		{
			get { return bgColor; }
			set { bgColor = value; }
		}

		public string Description
		{
			get { return null; }
		}

		public int ClientWidth
		{
			get { return width; }
		}

		public int ClientHeight
		{
			get
			{ return height; }
		}

		public Microsoft.DirectX.Direct3D.Surface DepthStencil
		{
			get { return depthStencil; }
		}

		public Microsoft.DirectX.Direct3D.Surface RenderTarget
		{
			get { return renderTarget; }
		}

		#endregion
	}
}
