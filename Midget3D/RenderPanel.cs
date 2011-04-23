using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Midget.Cameras;



namespace Midget
{
	/// <summary>
	/// Summary description for RenderPanel.
	/// </summary>
	public class RenderPanel : System.Windows.Forms.UserControl , IRenderSurface	
	{	

		private MidgetCamera	camera			= null;
		private SwapChain		swapChain		= null;
		private Surface			depthStencil	= null;

		private DeviceManager dm;
		private SceneManager  sm;

		private event RenderSingleViewHandler renderView;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public RenderPanel()
		{	
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// init deviceManager and scene (safety)
			// - this should not be called
			try
			{
				this.dm = Midget.DeviceManager.Instance;
				this.sm = Midget.SceneManager.Instance;
			}
			catch 
			{}

			this.renderView += new RenderSingleViewHandler(dm.OnRenderSingleView); 
		}

		public RenderPanel(DeviceManager initDM, SceneManager initSM)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			dm = initDM;
			sm = initSM;

			this.renderView += new RenderSingleViewHandler(dm.OnRenderSingleView); 
		}

		public void Render(int frameNumber)
		{
			if(this.swapChain != null && this.depthStencil != null)
			{
				if(this.swapChain.Disposed)
					this.ConfigureRenderTarget();

				try
				{
					this.renderView(this, new RenderSingleViewEventArgs(sm.Scene,frameNumber));
				}
				catch (Microsoft.DirectX.DirectXException)
				{
					// try to re-render
					this.renderView(this, new RenderSingleViewEventArgs(sm.Scene,frameNumber));
				}
			}
			else
			{
				this.ConfigureRenderTarget();
				this.Render(frameNumber);
			}
		}

		private void ConfigureRenderTarget()
		{
			// if the device has had a chance to be init
			if(dm != null && dm.Device != null)
			{
				PresentParameters presentParams = dm.PresentParameters;

				presentParams.DeviceWindow = this;

				presentParams.BackBufferHeight = this.ClientSize.Height;
				presentParams.BackBufferWidth =  this.ClientSize.Width;

				swapChain = new SwapChain(dm.Device, presentParams);

				depthStencil = dm.Device.CreateDepthStencilSurface( this.ClientSize.Width, this.ClientSize.Height,
					presentParams.AutoDepthStencilFormat, presentParams.MultiSample, presentParams.MultiSampleQuality,
					true );
			}
		}
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region IRenderSurface Members

		public Microsoft.DirectX.Direct3D.SwapChain SwapChain
		{
			get
			{
				// TODO:  Add RenderPanel.SwapChain getter implementation
				return swapChain;
			}
		}

		public Midget.Cameras.MidgetCamera Camera
		{
			get
			{
				return camera;
			}
			set
			{
				camera = value;
			}
		}

		public Midget.DrawMode DrawMode
		{
			get
			{
				// TODO:  Add RenderPanel.DrawMode getter implementation
				return DrawMode.solid;
			}
			set
			{
				// TODO:  Add RenderPanel.DrawMode setter implementation
			}
		}

		public bool NameVisible
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public new Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		public string Description
		{
			get
			{
				return null;
			}
		}

		public int ClientWidth
		{
			get
			{
				return this.ClientSize.Width;
			}
		}

		public int ClientHeight
		{
			get
			{
				return this.ClientSize.Height;
			}
		}

		public Microsoft.DirectX.Direct3D.Surface DepthStencil
		{
			get
			{
				return depthStencil;
			}
		}

		public Microsoft.DirectX.Direct3D.Surface RenderTarget
		{
			get
			{
				if(this.swapChain != null)
					return swapChain.GetBackBuffer(0,BackBufferType.Mono); 
				else
					return null;
			}
		}
		
		public DeviceManager DeviceManager
		{
			set { dm = value; }
		}

		public SceneManager SceneManager
		{
			set { sm = value; }
		}

		#endregion
	}
}
