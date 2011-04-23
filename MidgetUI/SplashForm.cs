using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Midget
{
	/// <summary>
	/// Summary description for frmSplash.
	/// </summary>
	public class frmSplash : System.Windows.Forms.Form
	{
		private int numberOfMessages;
		public System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.PictureBox pctImage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSplash()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			// show the splash form and update text as events occur
			this.numberOfMessages = 6;
			this.Splash();
			
			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmSplash));
			this.lblStatus = new System.Windows.Forms.Label();
			this.pctImage = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// lblStatus
			// 
			this.lblStatus.BackColor = System.Drawing.SystemColors.Highlight;
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.lblStatus.Location = new System.Drawing.Point(0, 480);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(640, 24);
			this.lblStatus.TabIndex = 0;
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pctImage
			// 
			this.pctImage.BackColor = System.Drawing.SystemColors.Highlight;
			this.pctImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pctImage.BackgroundImage")));
			this.pctImage.Location = new System.Drawing.Point(0, 0);
			this.pctImage.Name = "pctImage";
			this.pctImage.Size = new System.Drawing.Size(640, 480);
			this.pctImage.TabIndex = 1;
			this.pctImage.TabStop = false;
			// 
			// frmSplash
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 504);
			this.Controls.Add(this.pctImage);
			this.Controls.Add(this.lblStatus);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmSplash";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout(false);

		}
		#endregion

		public void Splash()
		{
			this.Show();
			for (int i=0; i< this.numberOfMessages; i++)
			{
				switch (i)
				{
					case 0:
						this.lblStatus.Text = "Loading main interface...";
						this.Refresh();
						System.Threading.Thread.Sleep(1500);
						break;
					case 1:
						this.lblStatus.Text = "Retrieving object meshes...";
						this.lblStatus.Refresh();
						System.Threading.Thread.Sleep(1500);
						break;
					case 2:
						this.lblStatus.Text = "Flexing cloits...";
						this.lblStatus.Refresh();
						System.Threading.Thread.Sleep(1500);
						break;
					case 3:
						this.lblStatus.Text = "Establishing user interface...";
						this.lblStatus.Refresh();
						System.Threading.Thread.Sleep(1500);
						break;
					case 4:
						this.lblStatus.Text = "Adding Dynamic properties...";
						this.lblStatus.Refresh();
						System.Threading.Thread.Sleep(1500);
						break;
					case 5:
						this.lblStatus.Text = "Choppin' Broccoli...";
						this.lblStatus.Refresh();
						System.Threading.Thread.Sleep(1500);
						break;
					default:
						break;
				}
			}
			this.Close();
		}
	}
}
