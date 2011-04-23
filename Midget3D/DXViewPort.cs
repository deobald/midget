using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{

	/// <summary>
	/// A viewport designed to display a directx scene on a Windows
	/// Form.  Through the use of this control the directx scene can be contained
	/// to a small portion of the screen
	/// </summary>
	public class DXViewPort : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel pnlLeft;
		private Midget.PictureBox3D pic3DTopLeft;
		private Midget.PictureBox3D pic3DBotLeft;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private Midget.PictureBox3D pic3DTopRight;
		private Midget.PictureBox3D pic3DBotRight;
		private System.Windows.Forms.Splitter spltLeft;

		#region Windows Form Designer Code


		private void InitializeComponent()
		{
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.pic3DBotLeft = new Midget.PictureBox3D();
			this.spltLeft = new System.Windows.Forms.Splitter();
			this.pic3DTopLeft = new Midget.PictureBox3D();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.pic3DBotRight = new Midget.PictureBox3D();
			this.pic3DTopRight = new Midget.PictureBox3D();
			this.pnlLeft.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlLeft
			// 
			this.pnlLeft.Controls.Add(this.pic3DBotLeft);
			this.pnlLeft.Controls.Add(this.spltLeft);
			this.pnlLeft.Controls.Add(this.pic3DTopLeft);
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeft.Location = new System.Drawing.Point(0, 0);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Size = new System.Drawing.Size(360, 611);
			this.pnlLeft.TabIndex = 0;
			// 
			// pic3DBotLeft
			// 
			this.pic3DBotLeft.BackColor = System.Drawing.Color.CornflowerBlue;
			this.pic3DBotLeft.Camera = null;
			this.pic3DBotLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pic3DBotLeft.DrawMode = Midget.DrawMode.wireFrame;
			this.pic3DBotLeft.Location = new System.Drawing.Point(0, 307);
			this.pic3DBotLeft.Name = "pic3DBotLeft";
			this.pic3DBotLeft.NameVisible = true;
			this.pic3DBotLeft.Size = new System.Drawing.Size(360, 304);
			this.pic3DBotLeft.SwapChain = null;
			this.pic3DBotLeft.TabIndex = 3;
			// 
			// spltLeft
			// 
			this.spltLeft.Dock = System.Windows.Forms.DockStyle.Top;
			this.spltLeft.Location = new System.Drawing.Point(0, 304);
			this.spltLeft.Name = "spltLeft";
			this.spltLeft.Size = new System.Drawing.Size(360, 3);
			this.spltLeft.TabIndex = 2;
			this.spltLeft.TabStop = false;
			// 
			// pic3DTopLeft
			// 
			this.pic3DTopLeft.BackColor = System.Drawing.Color.CornflowerBlue;
			this.pic3DTopLeft.Camera = null;
			this.pic3DTopLeft.Dock = System.Windows.Forms.DockStyle.Top;
			this.pic3DTopLeft.DrawMode = Midget.DrawMode.wireFrame;
			this.pic3DTopLeft.Location = new System.Drawing.Point(0, 0);
			this.pic3DTopLeft.Name = "pic3DTopLeft";
			this.pic3DTopLeft.NameVisible = true;
			this.pic3DTopLeft.Size = new System.Drawing.Size(360, 304);
			this.pic3DTopLeft.SwapChain = null;
			this.pic3DTopLeft.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.pic3DBotRight);
			this.panel1.Controls.Add(this.pic3DTopRight);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(360, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(360, 611);
			this.panel1.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 304);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(360, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// pic3DBotRight
			// 
			this.pic3DBotRight.BackColor = System.Drawing.Color.CornflowerBlue;
			this.pic3DBotRight.Camera = null;
			this.pic3DBotRight.DrawMode = Midget.DrawMode.wireFrame;
			this.pic3DBotRight.Location = new System.Drawing.Point(0, 304);
			this.pic3DBotRight.Name = "pic3DBotRight";
			this.pic3DBotRight.NameVisible = true;
			this.pic3DBotRight.Size = new System.Drawing.Size(360, 304);
			this.pic3DBotRight.SwapChain = null;
			this.pic3DBotRight.TabIndex = 1;
			// 
			// pic3DTopRight
			// 
			this.pic3DTopRight.BackColor = System.Drawing.Color.CornflowerBlue;
			this.pic3DTopRight.Camera = null;
			this.pic3DTopRight.Dock = System.Windows.Forms.DockStyle.Top;
			this.pic3DTopRight.DrawMode = Midget.DrawMode.wireFrame;
			this.pic3DTopRight.Location = new System.Drawing.Point(0, 0);
			this.pic3DTopRight.Name = "pic3DTopRight";
			this.pic3DTopRight.NameVisible = true;
			this.pic3DTopRight.Size = new System.Drawing.Size(360, 304);
			this.pic3DTopRight.SwapChain = null;
			this.pic3DTopRight.TabIndex = 0;
			// 
			// DXViewPort
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pnlLeft);
			this.Name = "DXViewPort";
			this.Size = new System.Drawing.Size(720, 611);
			this.Load += new System.EventHandler(this.DXViewPort_Load);
			this.pnlLeft.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		public DXViewPort()
		{
			// required
			InitializeComponent();
			
			
			pic3DTopLeft.Camera = CameraFactory.Instance.CreateCamera(PredefinedCameras.Top);
			DeviceManager.AddViewport(pic3DTopLeft);


			pic3DBotLeft.Camera = CameraFactory.Instance.CreateCamera(PredefinedCameras.Left);
			DeviceManager.AddViewport(pic3DBotLeft);
			

			pic3DTopRight.Camera = CameraFactory.Instance.CreateCamera(PredefinedCameras.Front);
			DeviceManager.AddViewport(pic3DTopRight);

			pic3DBotRight.Camera = CameraFactory.Instance.CreateCamera(PredefinedCameras.Perspective);
			DeviceManager.AddViewport(pic3DBotRight);

			DeviceManager.Render();
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			DeviceManager.Render();
		}

		private void pic3D1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			
		}

		private void DXViewPort_Load(object sender, System.EventArgs e)
		{
		
		}

		private void pic3DBotRight_Resize(object sender, System.EventArgs e)
		{
			pic3DBotRight.SwapChain.PresentParamters.BackBufferHeight = pic3DBotRight.Height;
			pic3DBotRight.SwapChain.PresentParamters.BackBufferWidth = pic3DBotRight.Width;
			DeviceManager.RenderSingleView(pic3DBotRight);
		}

		public DeviceManager DeviceManager
		{
			get
			{
				return DeviceManager.Instance;
			}
		}

	}


}
