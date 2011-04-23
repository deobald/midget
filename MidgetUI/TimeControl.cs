using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MidgetUI
{
	/// <summary>
	/// TimeControl is an interface which allows the user to scrub through time and
	/// alter then number of frames displayed
	/// 
	/// The user can enter a starting frame value or an ending frame value to adjust the time "window".
	/// If the user enters a starting frame that is greater than the end frame, it is assumed that
	/// the user wishes to extend the end frame to START_FRAME+1
	/// 
	/// </summary>
	public class TimeControl : System.Windows.Forms.UserControl
	{
		private Midget.TimeSlider trkTimeSlider;
		private System.Windows.Forms.TextBox txtStartFrame;
		private System.Windows.Forms.TextBox txtEndFrame;
		private System.Windows.Forms.TextBox txtCurrentFrame;
		private System.Windows.Forms.Label lblEndFrame;
		private System.Windows.Forms.Label lblCurrentFrame;
		private System.Windows.Forms.Label lblStartFrame;
		private System.Windows.Forms.Button btnCreateKeyframe;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TimeControl()
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
			this.trkTimeSlider = new Midget.TimeSlider();
			this.txtStartFrame = new System.Windows.Forms.TextBox();
			this.txtEndFrame = new System.Windows.Forms.TextBox();
			this.txtCurrentFrame = new System.Windows.Forms.TextBox();
			this.lblEndFrame = new System.Windows.Forms.Label();
			this.lblCurrentFrame = new System.Windows.Forms.Label();
			this.lblStartFrame = new System.Windows.Forms.Label();
			this.btnCreateKeyframe = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.trkTimeSlider)).BeginInit();
			this.SuspendLayout();
			// 
			// trkTimeSlider
			// 
			this.trkTimeSlider.LargeChange = 3;
			this.trkTimeSlider.Location = new System.Drawing.Point(0, 0);
			this.trkTimeSlider.Maximum = 100;
			this.trkTimeSlider.Name = "trkTimeSlider";
			this.trkTimeSlider.Size = new System.Drawing.Size(656, 45);
			this.trkTimeSlider.TabIndex = 0;
			this.trkTimeSlider.Scroll += new System.EventHandler(this.trkTimeSlider_Scroll);
			// 
			// txtStartFrame
			// 
			this.txtStartFrame.Location = new System.Drawing.Point(0, 48);
			this.txtStartFrame.Name = "txtStartFrame";
			this.txtStartFrame.Size = new System.Drawing.Size(56, 20);
			this.txtStartFrame.TabIndex = 1;
			this.txtStartFrame.Text = "0";
			this.txtStartFrame.Leave += new System.EventHandler(this.txtStartFrame_Leave);
			this.txtStartFrame.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtStartFrame_KeyUp);
			// 
			// txtEndFrame
			// 
			this.txtEndFrame.Location = new System.Drawing.Point(432, 48);
			this.txtEndFrame.Name = "txtEndFrame";
			this.txtEndFrame.Size = new System.Drawing.Size(56, 20);
			this.txtEndFrame.TabIndex = 3;
			this.txtEndFrame.Text = "100";
			this.txtEndFrame.Leave += new System.EventHandler(this.txtEndFrame_Leave);
			this.txtEndFrame.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtEndFrame_KeyUp);
			// 
			// txtCurrentFrame
			// 
			this.txtCurrentFrame.Location = new System.Drawing.Point(248, 48);
			this.txtCurrentFrame.Name = "txtCurrentFrame";
			this.txtCurrentFrame.Size = new System.Drawing.Size(56, 20);
			this.txtCurrentFrame.TabIndex = 2;
			this.txtCurrentFrame.Text = "0";
			this.txtCurrentFrame.Leave += new System.EventHandler(this.txtCurrentFrame_Leave);
			this.txtCurrentFrame.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCurrentFrame_KeyUp);
			// 
			// lblEndFrame
			// 
			this.lblEndFrame.Location = new System.Drawing.Point(368, 48);
			this.lblEndFrame.Name = "lblEndFrame";
			this.lblEndFrame.Size = new System.Drawing.Size(64, 24);
			this.lblEndFrame.TabIndex = 3;
			this.lblEndFrame.Text = "End Frame";
			this.lblEndFrame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCurrentFrame
			// 
			this.lblCurrentFrame.Location = new System.Drawing.Point(168, 48);
			this.lblCurrentFrame.Name = "lblCurrentFrame";
			this.lblCurrentFrame.Size = new System.Drawing.Size(80, 24);
			this.lblCurrentFrame.TabIndex = 4;
			this.lblCurrentFrame.Text = "Current Frame";
			this.lblCurrentFrame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblStartFrame
			// 
			this.lblStartFrame.Location = new System.Drawing.Point(56, 48);
			this.lblStartFrame.Name = "lblStartFrame";
			this.lblStartFrame.Size = new System.Drawing.Size(64, 24);
			this.lblStartFrame.TabIndex = 0;
			this.lblStartFrame.Text = "Start Frame";
			this.lblStartFrame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnCreateKeyframe
			// 
			this.btnCreateKeyframe.BackColor = System.Drawing.Color.Gold;
			this.btnCreateKeyframe.ForeColor = System.Drawing.Color.Firebrick;
			this.btnCreateKeyframe.Location = new System.Drawing.Point(592, 48);
			this.btnCreateKeyframe.Name = "btnCreateKeyframe";
			this.btnCreateKeyframe.Size = new System.Drawing.Size(64, 32);
			this.btnCreateKeyframe.TabIndex = 5;
			this.btnCreateKeyframe.Text = "Set Keyframe";
			this.btnCreateKeyframe.Click += new System.EventHandler(this.btnCreateKeyframe_Click);
			// 
			// TimeControl
			// 
			this.Controls.Add(this.btnCreateKeyframe);
			this.Controls.Add(this.lblStartFrame);
			this.Controls.Add(this.lblCurrentFrame);
			this.Controls.Add(this.lblEndFrame);
			this.Controls.Add(this.txtCurrentFrame);
			this.Controls.Add(this.txtEndFrame);
			this.Controls.Add(this.txtStartFrame);
			this.Controls.Add(this.trkTimeSlider);
			this.Name = "TimeControl";
			this.Size = new System.Drawing.Size(656, 104);
			this.Load += new System.EventHandler(this.TimeControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.trkTimeSlider)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void TimeControl_Load(object sender, System.EventArgs e)
		{
		
		}

		private void trkTimeSlider_Scroll(object sender, System.EventArgs e)
		{
			bool isValid = false;
			if ((trkTimeSlider.Value >= trkTimeSlider.Minimum) || 
				(trkTimeSlider.Value <= trkTimeSlider.Maximum))
				isValid = true;

			if (isValid)
				this.txtCurrentFrame.Text = this.trkTimeSlider.Value.ToString();
	
		}

		private void txtStartFrame_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (txtStartFrame.Text != "")
				{
					int endFrame = trkTimeSlider.Maximum;
					int startFrame = Convert.ToInt32(txtStartFrame.Text);
					int currentFrame = trkTimeSlider.Value;

					if (startFrame >= endFrame)
					{
						endFrame = startFrame + 1;
						txtEndFrame.Text = "" + endFrame;
						trkTimeSlider.Maximum = endFrame;
					}
					if (startFrame >= currentFrame)
						currentFrame = startFrame;

					trkTimeSlider.Minimum = startFrame;
					txtCurrentFrame.Text = "" + currentFrame;
				}
			}
		}

		private void txtCurrentFrame_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (txtCurrentFrame.Text != "")
				{
					int endFrame = trkTimeSlider.Maximum;
					int startFrame = trkTimeSlider.Minimum;
					int currentFrame = Convert.ToInt32(txtCurrentFrame.Text);

					if (currentFrame > endFrame)
					{
						currentFrame = endFrame;
						txtCurrentFrame.Text = "" + currentFrame;
					}
					if (currentFrame < startFrame)
					{
						currentFrame = startFrame;
						txtCurrentFrame.Text = "" + currentFrame;
					}

					trkTimeSlider.Value = currentFrame;
				}
			}
		}

		private void txtEndFrame_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (txtEndFrame.Text != "")
				{
					int endFrame = Convert.ToInt32(txtEndFrame.Text);
					int startFrame = trkTimeSlider.Minimum;
					int currentFrame = Convert.ToInt32(txtCurrentFrame.Text);

					if (endFrame <= startFrame)
					{
						endFrame = startFrame + 1;
						txtEndFrame.Text = "" + endFrame;
					}

					if (currentFrame > endFrame)
						currentFrame = endFrame;

					trkTimeSlider.Maximum = endFrame;
					txtCurrentFrame.Text = "" + currentFrame;
				}
			}
		}

		private void txtStartFrame_Leave(object sender, System.EventArgs e)
		{
			if (txtStartFrame.Text != "")
			{
				int endFrame = trkTimeSlider.Maximum;
				int startFrame = Convert.ToInt32(txtStartFrame.Text);
				int currentFrame = trkTimeSlider.Value;

				if (startFrame >= endFrame)
				{
					endFrame = startFrame + 1;
					txtEndFrame.Text = "" + endFrame;
					trkTimeSlider.Maximum = endFrame;
				}
				if (startFrame >= currentFrame)
					currentFrame = startFrame;

				trkTimeSlider.Minimum = startFrame;
				txtCurrentFrame.Text = "" + currentFrame;
			}
		}

		private void txtCurrentFrame_Leave(object sender, System.EventArgs e)
		{
			if (txtCurrentFrame.Text != "")
			{
				int endFrame = trkTimeSlider.Maximum;
				int startFrame = trkTimeSlider.Minimum;
				int currentFrame = Convert.ToInt32(txtCurrentFrame.Text);

				if (currentFrame > endFrame)
				{
					currentFrame = endFrame;
					txtCurrentFrame.Text = "" + currentFrame;
				}
				if (currentFrame < startFrame)
				{
					currentFrame = startFrame;
					txtCurrentFrame.Text = "" + currentFrame;
				}

				trkTimeSlider.Value = currentFrame;
			}
		
		}

		private void txtEndFrame_Leave(object sender, System.EventArgs e)
		{
			if (txtEndFrame.Text != "")
			{
				int endFrame = Convert.ToInt32(txtEndFrame.Text);
				int startFrame = trkTimeSlider.Minimum;
				int currentFrame = Convert.ToInt32(txtCurrentFrame.Text);

				if (endFrame <= startFrame)
				{
					endFrame = startFrame + 1;
					txtEndFrame.Text = "" + endFrame;
				}

				if (currentFrame > endFrame)
					currentFrame = endFrame;

				trkTimeSlider.Maximum = endFrame;
				txtCurrentFrame.Text = "" + currentFrame;
			}
		
		}

		private void btnCreateKeyframe_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("Keyframe created!!\n(not really, h053r)");
			//KeyFrameInterpolator interp = new KeyFrameInterpolator;

			
		}


	}
}
