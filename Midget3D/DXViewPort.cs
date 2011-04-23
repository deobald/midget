using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{

	public enum EditMode { None, Move, Rotate, Scale };

	/// <summary>
	/// A viewport designed to display a directx scene on a Windows
	/// Form.  Through the use of this control the directx scene can be contained
	/// to a small portion of the screen
	/// </summary>
	public class DXViewPort : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel pnlLeft;
		private System.Windows.Forms.Splitter spltMiddle;
		private System.Windows.Forms.Splitter spltRightMiddle;
		private System.Windows.Forms.Splitter spltLeftMiddle;
		private Midget.PictureBox3D pic3DTopRight;
		private Midget.PictureBox3D pic3DBotRight;
		private Midget.PictureBox3D pic3DTopLeft;
		private Midget.PictureBox3D pic3DBotLeft;
		private System.Windows.Forms.Panel pnlRight;

		#region Windows Form Designer Code


		private void InitializeComponent()
		{
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.pic3DBotLeft = new Midget.PictureBox3D(DeviceManager.Instance, SceneManager.Instance);
			this.spltLeftMiddle = new System.Windows.Forms.Splitter();
			this.pic3DTopLeft = new Midget.PictureBox3D(DeviceManager.Instance, SceneManager.Instance);
			this.spltMiddle = new System.Windows.Forms.Splitter();
			this.pnlRight = new System.Windows.Forms.Panel();
			this.pic3DBotRight = new Midget.PictureBox3D(DeviceManager.Instance, SceneManager.Instance);
			this.spltRightMiddle = new System.Windows.Forms.Splitter();
			this.pic3DTopRight = new Midget.PictureBox3D(DeviceManager.Instance, SceneManager.Instance);
			this.pnlLeft.SuspendLayout();
			this.pnlRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlLeft
			// 
			this.pnlLeft.Controls.Add(this.pic3DBotLeft);
			this.pnlLeft.Controls.Add(this.spltLeftMiddle);
			this.pnlLeft.Controls.Add(this.pic3DTopLeft);
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeft.Location = new System.Drawing.Point(0, 0);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Size = new System.Drawing.Size(312, 480);
			this.pnlLeft.TabIndex = 0;
			// 
			// pic3DBotLeft
			// 
			this.pic3DBotLeft.BackColor = System.Drawing.Color.SteelBlue;
			this.pic3DBotLeft.Camera = null;
			this.pic3DBotLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pic3DBotLeft.DrawMode = Midget.DrawMode.wireFrame;
			this.pic3DBotLeft.Location = new System.Drawing.Point(0, 227);
			this.pic3DBotLeft.Name = "pic3DBotLeft";
			this.pic3DBotLeft.NameVisible = true;
			this.pic3DBotLeft.Selected = true;
			this.pic3DBotLeft.Size = new System.Drawing.Size(312, 253);
			this.pic3DBotLeft.TabIndex = 3;
			// 
			// spltLeftMiddle
			// 
			this.spltLeftMiddle.Dock = System.Windows.Forms.DockStyle.Top;
			this.spltLeftMiddle.Location = new System.Drawing.Point(0, 224);
			this.spltLeftMiddle.Name = "spltLeftMiddle";
			this.spltLeftMiddle.Size = new System.Drawing.Size(312, 3);
			this.spltLeftMiddle.TabIndex = 2;
			this.spltLeftMiddle.TabStop = false;
			// 
			// pic3DTopLeft
			// 
			this.pic3DTopLeft.BackColor = System.Drawing.Color.SteelBlue;
			this.pic3DTopLeft.Camera = null;
			this.pic3DTopLeft.Dock = System.Windows.Forms.DockStyle.Top;
			this.pic3DTopLeft.DrawMode = Midget.DrawMode.wireFrame;
			this.pic3DTopLeft.Location = new System.Drawing.Point(0, 0);
			this.pic3DTopLeft.Name = "pic3DTopLeft";
			this.pic3DTopLeft.NameVisible = true;
			this.pic3DTopLeft.Selected = true;
			this.pic3DTopLeft.Size = new System.Drawing.Size(312, 224);

			this.pic3DTopLeft.TabIndex = 1;
			// 
			// spltMiddle
			// 
			this.spltMiddle.Location = new System.Drawing.Point(312, 0);
			this.spltMiddle.Name = "spltMiddle";
			this.spltMiddle.Size = new System.Drawing.Size(3, 480);
			this.spltMiddle.TabIndex = 1;
			this.spltMiddle.TabStop = false;
			// 
			// pnlRight
			// 
			this.pnlRight.Controls.Add(this.pic3DBotRight);
			this.pnlRight.Controls.Add(this.spltRightMiddle);
			this.pnlRight.Controls.Add(this.pic3DTopRight);
			this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRight.Location = new System.Drawing.Point(315, 0);
			this.pnlRight.Name = "pnlRight";
			this.pnlRight.Size = new System.Drawing.Size(325, 480);
			this.pnlRight.TabIndex = 2;
			// 
			// pic3DBotRight
			// 
			this.pic3DBotRight.BackColor = System.Drawing.Color.SteelBlue;
			this.pic3DBotRight.Camera = null;
			this.pic3DBotRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pic3DBotRight.DrawMode = Midget.DrawMode.wireFrame;
			this.pic3DBotRight.Location = new System.Drawing.Point(0, 227);
			this.pic3DBotRight.Name = "pic3DBotRight";
			this.pic3DBotRight.NameVisible = true;
			this.pic3DBotRight.Selected = true;
			this.pic3DBotRight.Size = new System.Drawing.Size(325, 253);
			this.pic3DBotRight.TabIndex = 2;
			// 
			// spltRightMiddle
			// 
			this.spltRightMiddle.Dock = System.Windows.Forms.DockStyle.Top;
			this.spltRightMiddle.Location = new System.Drawing.Point(0, 224);
			this.spltRightMiddle.Name = "spltRightMiddle";
			this.spltRightMiddle.Size = new System.Drawing.Size(325, 3);
			this.spltRightMiddle.TabIndex = 1;
			this.spltRightMiddle.TabStop = false;
			// 
			// pic3DTopRight
			// 
			this.pic3DTopRight.BackColor = System.Drawing.Color.SteelBlue;
			this.pic3DTopRight.Camera = null;
			this.pic3DTopRight.Dock = System.Windows.Forms.DockStyle.Top;
			this.pic3DTopRight.DrawMode = Midget.DrawMode.wireFrame;
			this.pic3DTopRight.Location = new System.Drawing.Point(0, 0);
			this.pic3DTopRight.Name = "pic3DTopRight";
			this.pic3DTopRight.NameVisible = true;
			this.pic3DTopRight.Selected = true;
			this.pic3DTopRight.Size = new System.Drawing.Size(325, 224);
			this.pic3DTopRight.TabIndex = 0;
			// 
			// DXViewPort
			// 
			this.Controls.Add(this.pnlRight);
			this.Controls.Add(this.spltMiddle);
			this.Controls.Add(this.pnlLeft);
			this.Name = "DXViewPort";
			this.Size = new System.Drawing.Size(640, 480);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		public DXViewPort()
		{
			try
			{

				// required
				InitializeComponent();

				ReCenter();
			
				DeviceManager.Instance.Viewport = this;
				DeviceManager.Instance.StartDirect3D();

				// set up viewport cameras
				pic3DTopLeft.Camera = CameraFactory.Instance.CreateCamera(PredefinedCameras.Top);

				pic3DBotLeft.Camera = CameraFactory.Instance.CreateCamera(PredefinedCameras.Left);

				pic3DTopRight.Camera = CameraFactory.Instance.CreateCamera(PredefinedCameras.Front);

				pic3DBotRight.Camera = CameraFactory.Instance.CreateCamera(PredefinedCameras.Perspective);
				pic3DBotRight.DrawMode = DrawMode.solid;
			}
			catch (Exception e)
			{
				MessageBox.Show("The dxViewPort is broken. (Details below.)\n\n" + e.Message + 
					"\n\n\n" + e.StackTrace, "dxViewPort NRE ERROR! (probably)", MessageBoxButtons.AbortRetryIgnore, 
					MessageBoxIcon.Hand);
			}

			try
			{
				// render to display changes
				DeviceManager.UpdateViews();
			}
			catch
			{
				// ignore failed render
			}
		}

		/// <summary>
		/// Adjust the 4 panes of the main viewport so they are all of equal size
		/// </summary>
		public void ReCenter()
		{
			try
			{
				// adjust the items so they are in the middle
				this.pnlLeft.Width = this.Width / 2 - 8;
				this.pic3DTopLeft.Height = this.Height / 2 - 40;	// 40 is just a magic number
				this.pic3DTopRight.Height = this.Height / 2 - 40;
			}
			catch
			{
				// ignore all errors
			}
		}

		/// <summary>
		/// Get an instance of the device manager
		/// </summary>
		public DeviceManager DeviceManager
		{
			get
			{
				return DeviceManager.Instance;
			}
		}

	}


}
