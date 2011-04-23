using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;

using Midget;

namespace MidgetUI
{
	/// <summary>
	/// the main window used to display the 4 viewports and menu controls
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.MainMenu mnuMain;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuRendering;
		private System.Windows.Forms.MenuItem mnuFileExit;
		private System.Windows.Forms.MenuItem mnuPrimatives;
		private System.Windows.Forms.MenuItem mnuPrimativeMesh;
		private System.Windows.Forms.MenuItem mnuPrimativePolygon;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshTeapot;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshSphere;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshTorus;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshBox;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshCylinder;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshPolygon;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshText;
		private Midget.DXViewPort dxViewPort1;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuEditUndo;
		private MidgetUI.MagicShelf magicShelf1;
		private MidgetUI.TimeControl timeControl1;
		private System.ComponentModel.IContainer components;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.mnuMain = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuEdit = new System.Windows.Forms.MenuItem();
			this.mnuEditUndo = new System.Windows.Forms.MenuItem();
			this.mnuRendering = new System.Windows.Forms.MenuItem();
			this.mnuPrimatives = new System.Windows.Forms.MenuItem();
			this.mnuPrimativeMesh = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshTeapot = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshSphere = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshTorus = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshBox = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshCylinder = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshPolygon = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshText = new System.Windows.Forms.MenuItem();
			this.mnuPrimativePolygon = new System.Windows.Forms.MenuItem();
			this.dxViewPort1 = new Midget.DXViewPort();
			this.magicShelf1 = new MidgetUI.MagicShelf();
			this.timeControl1 = new MidgetUI.TimeControl();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "notifyIcon1";
			this.notifyIcon1.Visible = true;
			// 
			// mnuMain
			// 
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFile,
																					this.mnuEdit,
																					this.mnuRendering,
																					this.mnuPrimatives});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFileExit});
			this.mnuFile.Text = "&File";
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Index = 0;
			this.mnuFileExit.Text = "E&xit";
			this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			// 
			// mnuEdit
			// 
			this.mnuEdit.Index = 1;
			this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuEditUndo});
			this.mnuEdit.Text = "&Edit";
			// 
			// mnuEditUndo
			// 
			this.mnuEditUndo.Index = 0;
			this.mnuEditUndo.Text = "&Undo";
			this.mnuEditUndo.Click += new System.EventHandler(this.mnuEditUndo_Click);
			// 
			// mnuRendering
			// 
			this.mnuRendering.Enabled = false;
			this.mnuRendering.Index = 2;
			this.mnuRendering.Text = "&Rendering";
			// 
			// mnuPrimatives
			// 
			this.mnuPrimatives.Index = 3;
			this.mnuPrimatives.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuPrimativeMesh,
																						  this.mnuPrimativePolygon});
			this.mnuPrimatives.Text = "&Primatives";
			// 
			// mnuPrimativeMesh
			// 
			this.mnuPrimativeMesh.Index = 0;
			this.mnuPrimativeMesh.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this.mnuPrimativesMeshTeapot,
																							 this.mnuPrimativesMeshSphere,
																							 this.mnuPrimativesMeshTorus,
																							 this.mnuPrimativesMeshBox,
																							 this.mnuPrimativesMeshCylinder,
																							 this.mnuPrimativesMeshPolygon,
																							 this.mnuPrimativesMeshText});
			this.mnuPrimativeMesh.Text = "&Mesh";
			// 
			// mnuPrimativesMeshTeapot
			// 
			this.mnuPrimativesMeshTeapot.Index = 0;
			this.mnuPrimativesMeshTeapot.Text = "&Teapot";
			this.mnuPrimativesMeshTeapot.Click += new System.EventHandler(this.mnuPrimativesMeshTeapot_Click);
			// 
			// mnuPrimativesMeshSphere
			// 
			this.mnuPrimativesMeshSphere.Index = 1;
			this.mnuPrimativesMeshSphere.Text = "&Sphere";
			this.mnuPrimativesMeshSphere.Click += new System.EventHandler(this.mnuPrimativesMeshSphere_Click);
			// 
			// mnuPrimativesMeshTorus
			// 
			this.mnuPrimativesMeshTorus.Index = 2;
			this.mnuPrimativesMeshTorus.Text = "T&orus";
			this.mnuPrimativesMeshTorus.Click += new System.EventHandler(this.mnuPrimativesMeshTorus_Click);
			// 
			// mnuPrimativesMeshBox
			// 
			this.mnuPrimativesMeshBox.Index = 3;
			this.mnuPrimativesMeshBox.Text = "&Box";
			this.mnuPrimativesMeshBox.Click += new System.EventHandler(this.mnuPrimativesMeshBox_Click);
			// 
			// mnuPrimativesMeshCylinder
			// 
			this.mnuPrimativesMeshCylinder.Index = 4;
			this.mnuPrimativesMeshCylinder.Text = "&Cylinder";
			this.mnuPrimativesMeshCylinder.Click += new System.EventHandler(this.mnuPrimativesMeshCylinder_Click);
			// 
			// mnuPrimativesMeshPolygon
			// 
			this.mnuPrimativesMeshPolygon.Index = 5;
			this.mnuPrimativesMeshPolygon.Text = "N-sided &Polygon";
			this.mnuPrimativesMeshPolygon.Click += new System.EventHandler(this.mnuPrimativesMeshPolygon_Click);
			// 
			// mnuPrimativesMeshText
			// 
			this.mnuPrimativesMeshText.Index = 6;
			this.mnuPrimativesMeshText.Text = "Text";
			this.mnuPrimativesMeshText.Click += new System.EventHandler(this.mnuPrimativesMeshText_Click);
			// 
			// mnuPrimativePolygon
			// 
			this.mnuPrimativePolygon.Index = 1;
			this.mnuPrimativePolygon.Text = "&Polygon";
			this.mnuPrimativePolygon.Click += new System.EventHandler(this.mnuPrimativePolygon_Click);
			// 
			// dxViewPort1
			// 
			this.dxViewPort1.Location = new System.Drawing.Point(8, 8);
			this.dxViewPort1.Name = "dxViewPort1";
			this.dxViewPort1.Size = new System.Drawing.Size(728, 608);
			this.dxViewPort1.TabIndex = 0;
			// 
			// magicShelf1
			// 
			this.magicShelf1.Location = new System.Drawing.Point(752, 8);
			this.magicShelf1.Name = "magicShelf1";
			this.magicShelf1.Size = new System.Drawing.Size(208, 552);
			this.magicShelf1.TabIndex = 1;
			// 
			// timeControl1
			// 
			this.timeControl1.Location = new System.Drawing.Point(32, 624);
			this.timeControl1.Name = "timeControl1";
			this.timeControl1.Size = new System.Drawing.Size(656, 104);
			this.timeControl1.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(992, 730);
			this.Controls.Add(this.timeControl1);
			this.Controls.Add(this.magicShelf1);
			this.Controls.Add(this.dxViewPort1);
			this.Menu = this.mnuMain;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Midget";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			// print the app name, and current app build #
			this.Text = "Midget 3D - [build " +
				Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + 
				Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." + 
				Assembly.GetExecutingAssembly().GetName().Version.Build.ToString() + 
				"]";
		}

		private void mnuFileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void mnuPrimativesMeshTeapot_Click(object sender, System.EventArgs e)
		{
			ObjectFactory.CreateObject((int)ObjectFactory.ObjectTypes.MeshTeapot);
		}

		private void mnuPrimativesMeshSphere_Click(object sender, System.EventArgs e)
		{
			ObjectFactory.CreateObject((int)ObjectFactory.ObjectTypes.MeshSphere);
		}

		private void mnuPrimativesMeshTorus_Click(object sender, System.EventArgs e)
		{
			ObjectFactory.CreateObject((int)ObjectFactory.ObjectTypes.MeshTorus);
		}

		private void mnuPrimativesMeshBox_Click(object sender, System.EventArgs e)
		{
			ObjectFactory.CreateObject((int)ObjectFactory.ObjectTypes.MeshBox);
		}

		private void mnuPrimativesMeshCylinder_Click(object sender, System.EventArgs e)
		{
			ObjectFactory.CreateObject((int)ObjectFactory.ObjectTypes.MeshCylinder);
		}

		private void mnuPrimativesMeshPolygon_Click(object sender, System.EventArgs e)
		{
			ObjectFactory.CreateObject((int)ObjectFactory.ObjectTypes.MeshPolygon);
		}

		private void mnuPrimativesMeshText_Click(object sender, System.EventArgs e)
		{
			ObjectFactory.CreateObject((int)ObjectFactory.ObjectTypes.MeshText);
		}

		private void mnuPrimativePolygon_Click(object sender, System.EventArgs e)
		{
			// TODO
		}

		private void mnuEditUndo_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "Ha ha! One day undo will work. But not now.\n\nClick OK to continue.", "Not yet, bucko.", 
				MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
		}



	}
}
