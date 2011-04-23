using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{
	/// <summary>
	/// the main viewport class used in Midget, which allows the display of a 
	/// Direct3D surface on a Windows Form
	/// </summary>
	public class PictureBox3D : System.Windows.Forms.UserControl, IRenderSurface
	{
		private Camera camera = null;
		private SwapChain swapChain = null;
		private DrawMode drawMode;
		private System.Windows.Forms.ContextMenu mnuEdit;
		private System.Windows.Forms.MenuItem mnuEditDisplay;
		private System.Windows.Forms.MenuItem mnuEditDisplayWireframe;
		private System.Windows.Forms.MenuItem mnuEditDisplaySolid;
		private System.Windows.Forms.MenuItem mnuEditDisplayPoint;
		private System.Windows.Forms.MenuItem mnuPanelColor;
		private System.Windows.Forms.MenuItem mnuPanelColorBlack;
		private System.Windows.Forms.MenuItem mnuPanelColorBlue;
		private System.Windows.Forms.MenuItem mnuPanelColorRandom;
		private System.Windows.Forms.MenuItem mnuPanelColorGray;
		private bool nameVisible;
//		private bool focus;

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mnuEdit = new System.Windows.Forms.ContextMenu();
			this.mnuEditDisplay = new System.Windows.Forms.MenuItem();
			this.mnuEditDisplayWireframe = new System.Windows.Forms.MenuItem();
			this.mnuEditDisplaySolid = new System.Windows.Forms.MenuItem();
			this.mnuEditDisplayPoint = new System.Windows.Forms.MenuItem();
			this.mnuPanelColor = new System.Windows.Forms.MenuItem();
			this.mnuPanelColorBlack = new System.Windows.Forms.MenuItem();
			this.mnuPanelColorGray = new System.Windows.Forms.MenuItem();
			this.mnuPanelColorBlue = new System.Windows.Forms.MenuItem();
			this.mnuPanelColorRandom = new System.Windows.Forms.MenuItem();
			// 
			// mnuEdit
			// 
			this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuEditDisplay,
																					this.mnuPanelColor});
			// 
			// mnuEditDisplay
			// 
			this.mnuEditDisplay.Index = 0;
			this.mnuEditDisplay.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.mnuEditDisplayWireframe,
																						   this.mnuEditDisplaySolid,
																						   this.mnuEditDisplayPoint});
			this.mnuEditDisplay.Text = "&Display";
			// 
			// mnuEditDisplayWireframe
			// 
			this.mnuEditDisplayWireframe.Index = 0;
			this.mnuEditDisplayWireframe.Text = "&Wireframe";
			this.mnuEditDisplayWireframe.Click += new System.EventHandler(this.mnuEditDisplayWireframe_Click);
			// 
			// mnuEditDisplaySolid
			// 
			this.mnuEditDisplaySolid.Index = 1;
			this.mnuEditDisplaySolid.Text = "&Solid";
			this.mnuEditDisplaySolid.Click += new System.EventHandler(this.mnuEditDisplaySolid_Click);
			// 
			// mnuEditDisplayPoint
			// 
			this.mnuEditDisplayPoint.Index = 2;
			this.mnuEditDisplayPoint.Text = "&Point";
			this.mnuEditDisplayPoint.Click += new System.EventHandler(this.mnuEditDisplayPoint_Click);
			// 
			// mnuPanelColor
			// 
			this.mnuPanelColor.Index = 1;
			this.mnuPanelColor.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuPanelColorBlack,
																						  this.mnuPanelColorGray,
																						  this.mnuPanelColorBlue,
																						  this.mnuPanelColorRandom});
			this.mnuPanelColor.Text = "Panel Color";
			// 
			// mnuPanelColorBlack
			// 
			this.mnuPanelColorBlack.Index = 0;
			this.mnuPanelColorBlack.Text = "Black";
			this.mnuPanelColorBlack.Click += new System.EventHandler(this.mnuPanelColorBlack_Click);
			// 
			// mnuPanelColorGray
			// 
			this.mnuPanelColorGray.Index = 1;
			this.mnuPanelColorGray.Text = "Gray";
			this.mnuPanelColorGray.Click += new System.EventHandler(this.mnuPanelColorGray_Click);
			// 
			// mnuPanelColorBlue
			// 
			this.mnuPanelColorBlue.Index = 2;
			this.mnuPanelColorBlue.Text = "Blue";
			this.mnuPanelColorBlue.Click += new System.EventHandler(this.mnuPanelColorBlue_Click);
			// 
			// mnuPanelColorRandom
			// 
			this.mnuPanelColorRandom.Index = 3;
			this.mnuPanelColorRandom.Text = "Random";
			this.mnuPanelColorRandom.Click += new System.EventHandler(this.mnuPanelColorRandom_Click);
			// 
			// PictureBox3D
			// 
			this.BackColor = System.Drawing.Color.White;
			this.ContextMenu = this.mnuEdit;
			this.Name = "PictureBox3D";
			this.Size = new System.Drawing.Size(150, 130);
			this.Load += new System.EventHandler(this.PictureBox3D_Load);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox3D_MouseUp);

		}
		#endregion
	
		public PictureBox3D()
		{
			// Required for Windows Form Designer
			InitializeComponent();

			// initialize members
			nameVisible = true;				// show the name of this viewport on-screen?
			drawMode = DrawMode.wireFrame;	// default to wireframe view
		}

		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			// TODO: object and camera manipulation will occur here, when appropriate
			//		 (it should be determined if OnMouseMove() should be used, or the
			//		  .MouseMove event)
			base.OnMouseMove (e);
		}

		private void PictureBox3D_Load(object sender, System.EventArgs e)
		{
		
		}

		private void mnuEditDisplayWireframe_Click(object sender, System.EventArgs e)
		{
			// change the draw mode for this viewport, then render using the new draw mode
			drawMode = DrawMode.wireFrame;
			this.Invalidate();
		}

		private void mnuEditDisplaySolid_Click(object sender, System.EventArgs e)
		{
			// change the draw mode for this viewport, then render using the new draw mode
			drawMode = DrawMode.solid;
			this.Invalidate();
		}

		private void mnuEditDisplayPoint_Click(object sender, System.EventArgs e)
		{
			// change the draw mode for this viewport, then render using the new draw mode
			drawMode = DrawMode.point;
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			DeviceManager.Instance.RenderSingleView(this);
		}

		private void mnuPanelColorBlack_Click(object sender, System.EventArgs e)
		{
			this.BackColor = Color.Black;
		}

		private void mnuPanelColorGray_Click(object sender, System.EventArgs e)
		{
			this.BackColor = Color.Gray;
		}

		private void mnuPanelColorBlue_Click(object sender, System.EventArgs e)
		{
			this.BackColor = Color.DarkBlue;
		}

		private void mnuPanelColorRandom_Click(object sender, System.EventArgs e)
		{
			// create a random object and pick an int between 0 and 255
			// Yes. This was a 2 AM creation. -steve
			Random R = new Random();
			this.BackColor = Color.FromArgb(R.Next(255), R.Next(255), R.Next(255));
		}

		private void PictureBox3D_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			IObject3D picked = ObjectPicker.Pick(e.X, e.Y, this.Camera);
			if (picked != null)
			{
				MidgetEvent.ObjectEventFactory.Instance.CreateSelectEvent(picked);
			}
		}


		public Camera Camera
		{
			get { return camera; }
			set { camera = value; }
		}

		public SwapChain SwapChain
		{
			get { return swapChain; }
			set { swapChain = value; }
		}

		public DrawMode DrawMode
		{
			get { return drawMode; }
			set { drawMode = value; }
		}

		public bool NameVisible
		{
			get { return nameVisible; }
			set { nameVisible = value; }
		}
	}
}
