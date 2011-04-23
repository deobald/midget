using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for TabBar.
	/// </summary>
	public class TabBar : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabMeshes;
		private System.Windows.Forms.TabPage tabDynamics;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ToolBar toolbarMeshes;
		private System.Windows.Forms.ToolBarButton toolBarButton4;
		private System.Windows.Forms.ToolBarButton toolBarButton5;
		private System.Windows.Forms.ToolBarButton toolBarButton6;
		private System.Windows.Forms.ToolBarButton toolBarButton7;
		private System.Windows.Forms.TabPage tabCurves;
		private System.Windows.Forms.ToolBar toolbarCurves;
		private System.Windows.Forms.ToolBarButton toolBarButton8;
		private System.Windows.Forms.ToolBarButton toolBarButton9;
		private System.Windows.Forms.ToolBarButton toolBarButton10;
		private System.Windows.Forms.ToolBar toolbarDynamics;
		private System.Windows.Forms.ToolBarButton toolBarButton11;
		private System.Windows.Forms.ToolBarButton toolBarButton12;
		private System.Windows.Forms.ToolBarButton toolBarButton13;
		private System.Windows.Forms.ToolBarButton toolbarTeapot;
		private System.Windows.Forms.ToolBarButton toolbarSphere;
		private System.Windows.Forms.ToolBarButton toolbar;
		private System.Windows.Forms.TabPage tabRender;
		private System.Windows.Forms.TabPage tabGroups;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBar toolbarGroups;
		private System.Windows.Forms.ToolBar toolbarRender;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.ToolBarButton toolBarButton14;
		private System.Windows.Forms.ToolBarButton toolBarButton15;
		private System.Windows.Forms.ToolBarButton toolBarButton16;
		private System.ComponentModel.IContainer components;

		public TabBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TabBar));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabMeshes = new System.Windows.Forms.TabPage();
			this.tabDynamics = new System.Windows.Forms.TabPage();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.toolbarMeshes = new System.Windows.Forms.ToolBar();
			this.toolbarTeapot = new System.Windows.Forms.ToolBarButton();
			this.toolbarSphere = new System.Windows.Forms.ToolBarButton();
			this.toolbar = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
			this.tabCurves = new System.Windows.Forms.TabPage();
			this.toolbarCurves = new System.Windows.Forms.ToolBar();
			this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
			this.toolbarDynamics = new System.Windows.Forms.ToolBar();
			this.toolBarButton11 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton12 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton13 = new System.Windows.Forms.ToolBarButton();
			this.tabRender = new System.Windows.Forms.TabPage();
			this.tabGroups = new System.Windows.Forms.TabPage();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolbarGroups = new System.Windows.Forms.ToolBar();
			this.toolbarRender = new System.Windows.Forms.ToolBar();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton14 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton15 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton16 = new System.Windows.Forms.ToolBarButton();
			this.tabControl.SuspendLayout();
			this.tabMeshes.SuspendLayout();
			this.tabDynamics.SuspendLayout();
			this.tabCurves.SuspendLayout();
			this.tabRender.SuspendLayout();
			this.tabGroups.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabMeshes);
			this.tabControl.Controls.Add(this.tabCurves);
			this.tabControl.Controls.Add(this.tabDynamics);
			this.tabControl.Controls.Add(this.tabGroups);
			this.tabControl.Controls.Add(this.tabRender);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(472, 56);
			this.tabControl.TabIndex = 0;
			// 
			// tabMeshes
			// 
			this.tabMeshes.Controls.Add(this.toolbarMeshes);
			this.tabMeshes.Location = new System.Drawing.Point(4, 22);
			this.tabMeshes.Name = "tabMeshes";
			this.tabMeshes.Size = new System.Drawing.Size(464, 30);
			this.tabMeshes.TabIndex = 0;
			this.tabMeshes.Text = "Meshes";
			// 
			// tabDynamics
			// 
			this.tabDynamics.Controls.Add(this.toolbarDynamics);
			this.tabDynamics.Location = new System.Drawing.Point(4, 22);
			this.tabDynamics.Name = "tabDynamics";
			this.tabDynamics.Size = new System.Drawing.Size(464, 30);
			this.tabDynamics.TabIndex = 3;
			this.tabDynamics.Text = "Dynamics";
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolbarMeshes
			// 
			this.toolbarMeshes.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							 this.toolbarTeapot,
																							 this.toolbarSphere,
																							 this.toolbar,
																							 this.toolBarButton4,
																							 this.toolBarButton5,
																							 this.toolBarButton6,
																							 this.toolBarButton7});
			this.toolbarMeshes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolbarMeshes.DropDownArrows = true;
			this.toolbarMeshes.Location = new System.Drawing.Point(0, 0);
			this.toolbarMeshes.Name = "toolbarMeshes";
			this.toolbarMeshes.ShowToolTips = true;
			this.toolbarMeshes.Size = new System.Drawing.Size(464, 28);
			this.toolbarMeshes.TabIndex = 0;
			// 
			// tabCurves
			// 
			this.tabCurves.Controls.Add(this.toolbarCurves);
			this.tabCurves.Location = new System.Drawing.Point(4, 22);
			this.tabCurves.Name = "tabCurves";
			this.tabCurves.Size = new System.Drawing.Size(464, 30);
			this.tabCurves.TabIndex = 4;
			this.tabCurves.Text = "Curves";
			// 
			// toolbarCurves
			// 
			this.toolbarCurves.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							 this.toolBarButton8,
																							 this.toolBarButton9,
																							 this.toolBarButton10});
			this.toolbarCurves.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolbarCurves.DropDownArrows = true;
			this.toolbarCurves.Location = new System.Drawing.Point(0, 0);
			this.toolbarCurves.Name = "toolbarCurves";
			this.toolbarCurves.ShowToolTips = true;
			this.toolbarCurves.Size = new System.Drawing.Size(464, 28);
			this.toolbarCurves.TabIndex = 0;
			// 
			// toolbarDynamics
			// 
			this.toolbarDynamics.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							   this.toolBarButton11,
																							   this.toolBarButton12,
																							   this.toolBarButton13,
																							   this.toolBarButton1});
			this.toolbarDynamics.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolbarDynamics.DropDownArrows = true;
			this.toolbarDynamics.Location = new System.Drawing.Point(0, 0);
			this.toolbarDynamics.Name = "toolbarDynamics";
			this.toolbarDynamics.ShowToolTips = true;
			this.toolbarDynamics.Size = new System.Drawing.Size(464, 28);
			this.toolbarDynamics.TabIndex = 0;
			// 
			// tabRender
			// 
			this.tabRender.Controls.Add(this.toolbarRender);
			this.tabRender.Location = new System.Drawing.Point(4, 22);
			this.tabRender.Name = "tabRender";
			this.tabRender.Size = new System.Drawing.Size(464, 30);
			this.tabRender.TabIndex = 5;
			this.tabRender.Text = "Render";
			// 
			// tabGroups
			// 
			this.tabGroups.Controls.Add(this.toolbarGroups);
			this.tabGroups.Location = new System.Drawing.Point(4, 22);
			this.tabGroups.Name = "tabGroups";
			this.tabGroups.Size = new System.Drawing.Size(464, 30);
			this.tabGroups.TabIndex = 6;
			this.tabGroups.Text = "Groups";
			// 
			// toolbarGroups
			// 
			this.toolbarGroups.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							 this.toolBarButton2,
																							 this.toolBarButton3});
			this.toolbarGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolbarGroups.DropDownArrows = true;
			this.toolbarGroups.Location = new System.Drawing.Point(0, 0);
			this.toolbarGroups.Name = "toolbarGroups";
			this.toolbarGroups.ShowToolTips = true;
			this.toolbarGroups.Size = new System.Drawing.Size(464, 28);
			this.toolbarGroups.TabIndex = 0;
			// 
			// toolbarRender
			// 
			this.toolbarRender.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							 this.toolBarButton14,
																							 this.toolBarButton15,
																							 this.toolBarButton16});
			this.toolbarRender.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolbarRender.DropDownArrows = true;
			this.toolbarRender.Location = new System.Drawing.Point(0, 0);
			this.toolbarRender.Name = "toolbarRender";
			this.toolbarRender.ShowToolTips = true;
			this.toolbarRender.Size = new System.Drawing.Size(464, 28);
			this.toolbarRender.TabIndex = 0;
			// 
			// TabBar
			// 
			this.Controls.Add(this.tabControl);
			this.Name = "TabBar";
			this.Size = new System.Drawing.Size(472, 56);
			this.tabControl.ResumeLayout(false);
			this.tabMeshes.ResumeLayout(false);
			this.tabDynamics.ResumeLayout(false);
			this.tabCurves.ResumeLayout(false);
			this.tabRender.ResumeLayout(false);
			this.tabGroups.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
