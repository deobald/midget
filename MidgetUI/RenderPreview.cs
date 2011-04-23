using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Midget;
namespace MidgetUI
{
	/// <summary>
	/// Summary description for RenderPreview.
	/// </summary>
	public class RenderPreview : System.Windows.Forms.Form
	{
		private DeviceManager dm;
		private SceneManager sm;
		private Midget.RenderPanel renderPanel;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RenderPreview(DeviceManager dm, SceneManager sm, FileRenderSettings settings)
		{
			// init DeviceManager and SceneManager
			this.dm = dm;
			this.sm = sm;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			renderPanel.DeviceManager = dm;
			renderPanel.SceneManager = sm;
			renderPanel.Camera = settings.Camera;
			renderPanel.Width = settings.Width;
			renderPanel.Height = settings.Height;
			this.renderPanel.Size = new System.Drawing.Size(settings.Width, settings.Height);
		}

		public void Render(int frameNumber)
		{
			renderPanel.Render(frameNumber);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RenderPreview));
			this.renderPanel = new Midget.RenderPanel();
			this.SuspendLayout();
			// 
			// renderPanel
			// 
			this.renderPanel.Camera = null;
			this.renderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderPanel.DrawMode = Midget.DrawMode.solid;
			this.renderPanel.Location = new System.Drawing.Point(0, 0);
			this.renderPanel.Name = "renderPanel";
			this.renderPanel.NameVisible = false;
			this.renderPanel.Size = new System.Drawing.Size(616, 413);
			this.renderPanel.TabIndex = 0;
			// 
			// RenderPreview
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(616, 413);
			this.Controls.Add(this.renderPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RenderPreview";
			this.Text = "RenderPreview";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
