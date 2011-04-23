using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Midget;

namespace TimerControlProject
{
	/// <summary>
	/// Timeline control used to determine animation length and keyframe position
	/// </summary>
	public class TimelineControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel pnlSlider;
		private System.Windows.Forms.Panel pnlTimeline;
		private System.Windows.Forms.Label lblTimeCaption;
		private System.Windows.Forms.Button btnMoveRight;
		private System.Windows.Forms.Button btnMoveLeft;
		private System.Windows.Forms.Button btnSpacer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private int		timeLength = 100;
		private int		currentTime = 0;
		private long	frameRate = 24;
		private int		numLinesToDraw = 101;
		private bool	isSliderMoving = false;
		private int		sliderStartX = 0;
		private long	lastFrameDrawTime = 0;
		private bool	isPlaying = false;

		private System.Windows.Forms.Label lblCurrentFrame;
		private System.Windows.Forms.TextBox txtCurrentFrame;
		private System.Windows.Forms.TextBox txtMaxFrames;
		private System.Windows.Forms.Button btnPlay;
		private System.Windows.Forms.Label lblMaxFrames;
		private System.Windows.Forms.Button btnSetKeyframe;
		private System.Windows.Forms.Panel pnlSliderGhost;
		private System.Windows.Forms.Button btnDeleteKeyframe;
		private System.Windows.Forms.Label lblFrameRate;
		private System.Windows.Forms.TextBox txtFrameRate;
		private System.Windows.Forms.Panel pnlLeft;
		private System.Windows.Forms.Panel pnlRight;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnNext;
		
		// TODO: this is a temporary list of keyframes for testing only
		// Later on, this should be external and use KeyFrame objects, not ints
		private ArrayList keyframeList = new ArrayList();

		private ArrayList selectedObjects = new ArrayList();



		public TimelineControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// set graphical stuff
			SetCaption();

			Midget.Events.EventFactory.SelectAdditionalObject += new Midget.Events.Object.Selection.SelectAdditionalObjectEventHandler(EventFactory_SelectAdditionalObject);
			Midget.Events.EventFactory.DeselectObjects += new Midget.Events.Object.Selection.DeselectObjectEventHandler(EventFactory_DeselectObjects);
			Midget.Events.EventFactory.RemoveKeyFrame +=new Midget.Events.Object.Animation.RemoveKeyFrameEventHandler(EventFactory_RemoveKeyFrame);
			Midget.Events.EventFactory.AddKeyFrame +=new Midget.Events.Object.Animation.AddKeyFrameEventHandler(EventFactory_AddKeyFrame);
		}	

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
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
			this.pnlSlider = new System.Windows.Forms.Panel();
			this.lblTimeCaption = new System.Windows.Forms.Label();
			this.btnMoveRight = new System.Windows.Forms.Button();
			this.btnMoveLeft = new System.Windows.Forms.Button();
			this.btnSpacer = new System.Windows.Forms.Button();
			this.pnlTimeline = new System.Windows.Forms.Panel();
			this.btnPlay = new System.Windows.Forms.Button();
			this.btnSetKeyframe = new System.Windows.Forms.Button();
			this.lblMaxFrames = new System.Windows.Forms.Label();
			this.lblCurrentFrame = new System.Windows.Forms.Label();
			this.txtCurrentFrame = new System.Windows.Forms.TextBox();
			this.txtMaxFrames = new System.Windows.Forms.TextBox();
			this.pnlSliderGhost = new System.Windows.Forms.Panel();
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.pnlRight = new System.Windows.Forms.Panel();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.lblFrameRate = new System.Windows.Forms.Label();
			this.txtFrameRate = new System.Windows.Forms.TextBox();
			this.btnDeleteKeyframe = new System.Windows.Forms.Button();
			this.pnlSlider.SuspendLayout();
			this.pnlLeft.SuspendLayout();
			this.pnlRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlSlider
			// 
			this.pnlSlider.Controls.Add(this.lblTimeCaption);
			this.pnlSlider.Controls.Add(this.btnMoveRight);
			this.pnlSlider.Controls.Add(this.btnMoveLeft);
			this.pnlSlider.Controls.Add(this.btnSpacer);
			this.pnlSlider.Location = new System.Drawing.Point(8, 8);
			this.pnlSlider.Name = "pnlSlider";
			this.pnlSlider.Size = new System.Drawing.Size(184, 20);
			this.pnlSlider.TabIndex = 0;
			this.pnlSlider.Move += new System.EventHandler(this.pnlSlider_Move);
			// 
			// lblTimeCaption
			// 
			this.lblTimeCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTimeCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTimeCaption.Location = new System.Drawing.Point(32, 0);
			this.lblTimeCaption.Name = "lblTimeCaption";
			this.lblTimeCaption.Size = new System.Drawing.Size(120, 20);
			this.lblTimeCaption.TabIndex = 7;
			this.lblTimeCaption.Text = "1 / 1000";
			this.lblTimeCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblTimeCaption.Paint += new System.Windows.Forms.PaintEventHandler(this.lblTimeCaption_Paint);
			this.lblTimeCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTimeCaption_MouseUp);
			this.lblTimeCaption.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTimeCaption_MouseMove);
			this.lblTimeCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTimeCaption_MouseDown);
			// 
			// btnMoveRight
			// 
			this.btnMoveRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveRight.Location = new System.Drawing.Point(152, 0);
			this.btnMoveRight.Name = "btnMoveRight";
			this.btnMoveRight.Size = new System.Drawing.Size(32, 20);
			this.btnMoveRight.TabIndex = 6;
			this.btnMoveRight.Text = ">";
			this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
			// 
			// btnMoveLeft
			// 
			this.btnMoveLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveLeft.Location = new System.Drawing.Point(0, 0);
			this.btnMoveLeft.Name = "btnMoveLeft";
			this.btnMoveLeft.Size = new System.Drawing.Size(32, 20);
			this.btnMoveLeft.TabIndex = 5;
			this.btnMoveLeft.Text = "<";
			this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
			// 
			// btnSpacer
			// 
			this.btnSpacer.CausesValidation = false;
			this.btnSpacer.Enabled = false;
			this.btnSpacer.Location = new System.Drawing.Point(31, -1);
			this.btnSpacer.Name = "btnSpacer";
			this.btnSpacer.Size = new System.Drawing.Size(120, 20);
			this.btnSpacer.TabIndex = 4;
			// 
			// pnlTimeline
			// 
			this.pnlTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlTimeline.Location = new System.Drawing.Point(8, 32);
			this.pnlTimeline.Name = "pnlTimeline";
			this.pnlTimeline.Size = new System.Drawing.Size(456, 32);
			this.pnlTimeline.TabIndex = 1;
			this.pnlTimeline.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTimeline_Paint);
			// 
			// btnPlay
			// 
			this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnPlay.Location = new System.Drawing.Point(32, 8);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(24, 23);
			this.btnPlay.TabIndex = 2;
			this.btnPlay.Text = ">";
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// btnSetKeyframe
			// 
			this.btnSetKeyframe.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSetKeyframe.Location = new System.Drawing.Point(8, 40);
			this.btnSetKeyframe.Name = "btnSetKeyframe";
			this.btnSetKeyframe.Size = new System.Drawing.Size(64, 23);
			this.btnSetKeyframe.TabIndex = 3;
			this.btnSetKeyframe.Text = "Set Key";
			this.btnSetKeyframe.Click += new System.EventHandler(this.btnSetKeyframe_Click);
			// 
			// lblMaxFrames
			// 
			this.lblMaxFrames.Location = new System.Drawing.Point(128, 32);
			this.lblMaxFrames.Name = "lblMaxFrames";
			this.lblMaxFrames.Size = new System.Drawing.Size(64, 16);
			this.lblMaxFrames.TabIndex = 4;
			this.lblMaxFrames.Text = "Count:";
			// 
			// lblCurrentFrame
			// 
			this.lblCurrentFrame.Location = new System.Drawing.Point(128, 8);
			this.lblCurrentFrame.Name = "lblCurrentFrame";
			this.lblCurrentFrame.Size = new System.Drawing.Size(64, 16);
			this.lblCurrentFrame.TabIndex = 5;
			this.lblCurrentFrame.Text = "Current:";
			// 
			// txtCurrentFrame
			// 
			this.txtCurrentFrame.AutoSize = false;
			this.txtCurrentFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCurrentFrame.Location = new System.Drawing.Point(192, 8);
			this.txtCurrentFrame.Name = "txtCurrentFrame";
			this.txtCurrentFrame.Size = new System.Drawing.Size(40, 16);
			this.txtCurrentFrame.TabIndex = 6;
			this.txtCurrentFrame.Text = "";
			this.txtCurrentFrame.Leave += new System.EventHandler(this.txtCurrentFrame_Leave);
			this.txtCurrentFrame.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCurrentFrame_KeyUp);
			// 
			// txtMaxFrames
			// 
			this.txtMaxFrames.AutoSize = false;
			this.txtMaxFrames.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMaxFrames.Location = new System.Drawing.Point(192, 32);
			this.txtMaxFrames.Name = "txtMaxFrames";
			this.txtMaxFrames.Size = new System.Drawing.Size(40, 16);
			this.txtMaxFrames.TabIndex = 7;
			this.txtMaxFrames.Text = "";
			this.txtMaxFrames.Leave += new System.EventHandler(this.txtMaxFrames_Leave);
			this.txtMaxFrames.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMaxFrames_KeyUp);
			// 
			// pnlSliderGhost
			// 
			this.pnlSliderGhost.BackColor = System.Drawing.SystemColors.ControlDark;
			this.pnlSliderGhost.Location = new System.Drawing.Point(144, 8);
			this.pnlSliderGhost.Name = "pnlSliderGhost";
			this.pnlSliderGhost.Size = new System.Drawing.Size(176, 24);
			this.pnlSliderGhost.TabIndex = 8;
			this.pnlSliderGhost.Visible = false;
			this.pnlSliderGhost.Move += new System.EventHandler(this.pnlSliderGhost_Move);
			// 
			// pnlLeft
			// 
			this.pnlLeft.Controls.Add(this.pnlTimeline);
			this.pnlLeft.Controls.Add(this.pnlSlider);
			this.pnlLeft.Controls.Add(this.pnlSliderGhost);
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLeft.Location = new System.Drawing.Point(0, 0);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Size = new System.Drawing.Size(712, 88);
			this.pnlLeft.TabIndex = 9;
			// 
			// pnlRight
			// 
			this.pnlRight.Controls.Add(this.btnPlay);
			this.pnlRight.Controls.Add(this.btnNext);
			this.pnlRight.Controls.Add(this.btnStop);
			this.pnlRight.Controls.Add(this.btnPrevious);
			this.pnlRight.Controls.Add(this.lblFrameRate);
			this.pnlRight.Controls.Add(this.txtFrameRate);
			this.pnlRight.Controls.Add(this.txtCurrentFrame);
			this.pnlRight.Controls.Add(this.lblCurrentFrame);
			this.pnlRight.Controls.Add(this.lblMaxFrames);
			this.pnlRight.Controls.Add(this.btnSetKeyframe);
			this.pnlRight.Controls.Add(this.txtMaxFrames);
			this.pnlRight.Controls.Add(this.btnDeleteKeyframe);
			this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlRight.Location = new System.Drawing.Point(472, 0);
			this.pnlRight.Name = "pnlRight";
			this.pnlRight.Size = new System.Drawing.Size(240, 88);
			this.pnlRight.TabIndex = 10;
			// 
			// btnNext
			// 
			this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnNext.Location = new System.Drawing.Point(80, 8);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(24, 23);
			this.btnNext.TabIndex = 14;
			this.btnNext.Text = ">|";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnStop
			// 
			this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnStop.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(2)));
			this.btnStop.Location = new System.Drawing.Point(56, 8);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(24, 23);
			this.btnStop.TabIndex = 13;
			this.btnStop.Text = "o";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnPrevious
			// 
			this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnPrevious.Location = new System.Drawing.Point(8, 8);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(24, 23);
			this.btnPrevious.TabIndex = 12;
			this.btnPrevious.Text = "|<";
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// lblFrameRate
			// 
			this.lblFrameRate.Location = new System.Drawing.Point(128, 56);
			this.lblFrameRate.Name = "lblFrameRate";
			this.lblFrameRate.Size = new System.Drawing.Size(64, 16);
			this.lblFrameRate.TabIndex = 10;
			this.lblFrameRate.Text = "Framerate:";
			// 
			// txtFrameRate
			// 
			this.txtFrameRate.AutoSize = false;
			this.txtFrameRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFrameRate.Location = new System.Drawing.Point(192, 56);
			this.txtFrameRate.Name = "txtFrameRate";
			this.txtFrameRate.Size = new System.Drawing.Size(40, 16);
			this.txtFrameRate.TabIndex = 11;
			this.txtFrameRate.Text = "24";
			this.txtFrameRate.Leave += new System.EventHandler(this.txtFrameRate_Leave);
			this.txtFrameRate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFrameRate_KeyUp);
			// 
			// btnDeleteKeyframe
			// 
			this.btnDeleteKeyframe.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDeleteKeyframe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnDeleteKeyframe.ForeColor = System.Drawing.Color.DarkRed;
			this.btnDeleteKeyframe.Location = new System.Drawing.Point(72, 40);
			this.btnDeleteKeyframe.Name = "btnDeleteKeyframe";
			this.btnDeleteKeyframe.Size = new System.Drawing.Size(32, 23);
			this.btnDeleteKeyframe.TabIndex = 9;
			this.btnDeleteKeyframe.Text = "X";
			this.btnDeleteKeyframe.Click += new System.EventHandler(this.btnDeleteKeyframe_Click);
			// 
			// TimelineControl
			// 
			this.Controls.Add(this.pnlRight);
			this.Controls.Add(this.pnlLeft);
			this.Name = "TimelineControl";
			this.Size = new System.Drawing.Size(712, 88);
			this.Resize += new System.EventHandler(this.TimelineControl_Resize);
			this.Load += new System.EventHandler(this.TimelineControl_Load);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseMove);
			this.pnlSlider.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		public int Index
		{
			get { return currentTime; }
		}

		public int StartFrame
		{
			get { return 1; }
		}

		public int EndFrame
		{
			get { return Convert.ToInt32(txtMaxFrames.Text); }
		}
		#endregion

		#region GUI-related functions and events
		protected override void OnPaint(PaintEventArgs pe)
		{
			// TODO: Add custom paint code here

			// Calling the base class OnPaint
			base.OnPaint(pe);
		}

		/// <summary>
		/// Find a draw location on the slider based on an integer time value
		/// </summary>
		/// <param name="time">The time value used to calculate the draw location</param>
		/// <param name="useSlider">If we are finding a slider location, we have to take its width into account</param>
		/// <returns></returns>
		private int GetDrawLocationFromTime(int time, bool useSlider)
		{
			int timelineWidth = pnlTimeline.Width;
			if (useSlider)
			{
				timelineWidth -= pnlSlider.Width;
			}
			else
			{
				timelineWidth -= 1;
			}

			return Convert.ToInt32( timelineWidth * ((float)time / (float)timeLength) );
		}

		private void MoveToNextTime()
		{
			// if the end of time has not been reached
			if( currentTime < timeLength )
			{
				++currentTime;
				
				pnlSlider.Left = pnlTimeline.Left + GetDrawLocationFromTime(currentTime, true);

				this.SetCaption();
			}
		}

		private void MoveToPrevTime()
		{
			// if not at start
			if ( currentTime != 0 )
			{
				--currentTime;
				
				pnlSlider.Left = pnlTimeline.Left + GetDrawLocationFromTime(currentTime, true);

				this.SetCaption();
			}
		}

		private void SetCaption()
		{
			// set the label
			lblTimeCaption.Text = currentTime.ToString() + " / " + timeLength.ToString();

			// set the textbox
			txtCurrentFrame.Text = currentTime.ToString();
		}
		

		private void btnMoveRight_Click(object sender, System.EventArgs e)
		{
			this.MoveToNextTime();
		}
		

		private void btnMoveLeft_Click(object sender, System.EventArgs e)
		{
			this.MoveToPrevTime();
		}

		private int NumberOfDigits(int number)
		{
			return ((int)Math.Log((double)number) + 1);
		}

		private void TimelineControl_Load(object sender, System.EventArgs e)
		{
			txtMaxFrames.Text = timeLength.ToString();
			txtCurrentFrame.Text = currentTime.ToString();
			txtFrameRate.Text = frameRate.ToString();

			// set up the ghost to match the actual slider
			pnlSliderGhost.Width = pnlSlider.Width;
			pnlSliderGhost.Height = pnlSlider.Height;
			pnlSliderGhost.Top = pnlSlider.Top;
		}

		private void lblTimeCaption_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			sliderStartX = e.X;
			isSliderMoving = true;
		}

		private void lblTimeCaption_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (isSliderMoving)
			{
				int newLocation = e.X + pnlSlider.Left - sliderStartX;
				if (newLocation < pnlTimeline.Left)
				{
					newLocation = pnlTimeline.Left;
				}
				else if (newLocation > (pnlTimeline.Left + pnlTimeline.Width - pnlSliderGhost.Width))
				{
					newLocation = pnlTimeline.Left + pnlTimeline.Width - pnlSliderGhost.Width;
				}
				
				pnlSliderGhost.Visible = true;
				pnlSliderGhost.Left = newLocation;
				
				// set the time
				currentTime = Convert.ToInt32( ( ((float)newLocation - pnlTimeline.Left) / ((float)pnlTimeline.Width - (float)pnlSlider.Width)) * (float)timeLength );
				
				this.SetCaption();
			}
		}

		private void lblTimeCaption_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (isSliderMoving)
			{
				// move by mouse movement
				int newLocation = pnlSlider.Left + (e.X - sliderStartX);
				sliderStartX = e.X;
				isSliderMoving = false;

				// ensure we haven't overstepped bounds
				if (newLocation < pnlTimeline.Left)
				{
					newLocation = pnlTimeline.Left;
				}
				else if ( newLocation > (pnlTimeline.Width + pnlTimeline.Left - pnlSlider.Width))
				{
					newLocation = pnlTimeline.Left + pnlTimeline.Width - pnlSlider.Width;
				}
				
				// set the slider
				pnlSliderGhost.Visible = false;
				pnlSlider.Left = newLocation;

				this.SetCaption();
			}
		}

		private void txtMaxFrames_UpdateData()
		{
			try
			{
				timeLength = Convert.ToInt32(txtMaxFrames.Text);
				if (currentTime > timeLength)
					currentTime = timeLength;
				pnlSlider.Left = GetDrawLocationFromTime(currentTime, true);	// move the slider

				// redraw everything
				this.SetCaption();
				pnlTimeline.Invalidate();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Input error - invalid value:\n\n" + ex.Message, "Input Error", MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				txtMaxFrames.Text = timeLength.ToString();
			}
		}

		private void txtMaxFrames_Leave(object sender, System.EventArgs e)
		{
			txtMaxFrames_UpdateData();
		}

		private void txtCurrentFrame_UpdateData()
		{
			try
			{
				currentTime = Convert.ToInt32(txtCurrentFrame.Text);

				this.SetCaption();
				pnlSlider.Left = GetDrawLocationFromTime(currentTime, true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Input error - invalid value:\n\n" + ex.Message, "Input Error", MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				txtCurrentFrame.Text = currentTime.ToString();
			}
		}

		private void txtCurrentFrame_Leave(object sender, System.EventArgs e)
		{
			txtCurrentFrame_UpdateData();
		}

		private void txtFrameRate_UpdateData()
		{
			if (txtFrameRate.Text == null || txtFrameRate.Text.Trim() == "")
			{
				txtFrameRate.Text = "24";
			}
			frameRate = Convert.ToInt64(txtFrameRate.Text);
		}

		private void txtFrameRate_Leave(object sender, System.EventArgs e)
		{
			txtFrameRate_UpdateData();
		}

		private void TimelineControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			
		}

		private void TimelineControl_Resize(object sender, System.EventArgs e)
		{
			// reposition controls
			pnlSlider.Left = pnlTimeline.Left + GetDrawLocationFromTime(currentTime, true);

			// redraw controls
			this.Invalidate();
			pnlTimeline.Invalidate();
		}
		#endregion

		#region 3D backend interaction

		/// <summary>
		/// Add a keyframe to the keyframe list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSetKeyframe_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateAddKeyFrameRequestEvent(this,selectedObjects,currentTime);

			// redraw controls
			pnlTimeline.Invalidate();
			lblTimeCaption.Invalidate();
		}

		private void btnDeleteKeyframe_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateRemoveKeyFrameRequestEvent(this,selectedObjects,currentTime);

			// redraw
			pnlTimeline.Invalidate();
		}
		
		private void btnPlay_Click(object sender, System.EventArgs e)
		{
			if (btnPlay.Text == ">")
			{
				btnPlay.Text = "||";
				this.isPlaying = true;
				btnStop.Focus();
				
				// check if we're at the far end of the timeline
				// if so, reset to beginning
				if (currentTime == this.EndFrame)
				{
					currentTime = 0;
				}

				for (int i = currentTime; i <= this.EndFrame; i++)
				{
					// wait, based on ticks, until we are ready to draw the next frame
					while ( (DateTime.Now.Ticks - lastFrameDrawTime) < (TimeSpan.TicksPerSecond / frameRate) )
					{
						// busy wait
					}
					// enter the render phase, and record this time
					lastFrameDrawTime = DateTime.Now.Ticks;

					currentTime = i;

					// redraw controls
					pnlSlider.Left = pnlTimeline.Left + GetDrawLocationFromTime(currentTime, true);
					txtCurrentFrame.Text = currentTime.ToString();
					SetCaption();
					Application.DoEvents();	// ensure that the controls are re-drawn

					SceneManager.Instance.CurrentFrameIndex = currentTime;
					DeviceManager.Instance.UpdateViews();

					// check for pause or stop
					if (!this.isPlaying)
					{
						break;
					}
				}

				// done playing
				btnPlay.Text = ">";
				this.isPlaying = false;
			}
			else
			{
				btnPlay.Text = ">";
				this.isPlaying = false;
			}

			
		}
		
		private void pnlSlider_Move(object sender, System.EventArgs e)
		{
			SceneManager.Instance.CurrentFrameIndex = currentTime;
			DeviceManager.Instance.UpdateViews();
		}

		private void pnlSliderGhost_Move(object sender, System.EventArgs e)
		{
			txtCurrentFrame.Text = currentTime.ToString();

			SceneManager.Instance.CurrentFrameIndex = currentTime;
			DeviceManager.Instance.UpdateViews();
		}

		private void pnlTimeline_Paint(object sender, PaintEventArgs e)
		{
			// calculate the space available to each frame
			float spotPerFrame = (pnlTimeline.ClientSize.Width /*- 30*/) / (float)numLinesToDraw;
			
			float drawLocation = 0.0f;	// was 10.0f
			float drawTextLocation = 0.0f;
			
			int drawNumberIncrement = (int)(timeLength / (numLinesToDraw - 1)) *  10;
			
			Graphics g = e.Graphics;

			// draw each hash mark
			for(int i = 0, drawCount = 0; i < numLinesToDraw; ++i)
			{

				// if on a number divisible by 10 draw a full line
				if( (i % 10) == 0 )
				{
					g.DrawLine(Pens.DarkGray, drawLocation, 0.0f, drawLocation, pnlTimeline.Height);
				
					// draw number
					drawTextLocation = drawLocation;
					if (drawTextLocation >= (pnlTimeline.Width - 10))
					{
						drawTextLocation -= 20;	// TODO: this could be made more accurately right-aligned
					}
					g.DrawString(drawCount.ToString(), new Font("Arial", 10), Brushes.Black, drawTextLocation - 2 * NumberOfDigits(drawCount), pnlTimeline.Height * .5f);
				
					drawCount += drawNumberIncrement;
				}		
				else if( (i % 5) == 0 )
				{
					g.DrawLine(Pens.DarkGray, drawLocation, 0.0f, drawLocation, pnlTimeline.Height * 0.75f);
				}
				else
				{
					g.DrawLine(Pens.DarkGray, drawLocation, 0.0f, drawLocation, pnlTimeline.Height * 0.5f);
				}
				
				drawLocation += spotPerFrame;
			}

			foreach (IObject3D obj in selectedObjects)
			{

				// draw all the keyframes for the selected object
				if (obj.KeyFrameList != null)
				{
					foreach (Object3DKeyFrame kf in obj.KeyFrameList)
					{
						drawLocation = GetDrawLocationFromTime(kf.Index, false);
						g.DrawLine(Pens.OrangeRed, drawLocation, 0.0f, drawLocation, pnlTimeline.Height);
					}
				}
			}
			
		}

		/// <summary>
		/// Set color based on whether or not we are painting a keyframe
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lblTimeCaption_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			foreach (IObject3D obj in selectedObjects)
			{	
				if (obj.KeyFrameList != null) 
				{
					foreach (Object3DKeyFrame kf in obj.KeyFrameList)
					{
						if (currentTime == kf.Index)
						{
							lblTimeCaption.BackColor = Color.OrangeRed;
							return;
						}
					}
				}
			}
			lblTimeCaption.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
		}

		private void btnStop_Click(object sender, System.EventArgs e)
		{
			currentTime = 0;

			// redraw controls
			pnlSlider.Left = pnlTimeline.Left + GetDrawLocationFromTime(currentTime, true);
			txtCurrentFrame.Text = currentTime.ToString();
			SetCaption();

			// render
			SceneManager.Instance.CurrentFrameIndex = currentTime;
			DeviceManager.Instance.UpdateViews();

			// reset play control and stop
			btnPlay.Text = ">";
			this.isPlaying = false;
		}
		
		private void btnPrevious_Click(object sender, System.EventArgs e)
		{
			this.MoveToPrevTime();
		}
		
		private void btnNext_Click(object sender, System.EventArgs e)
		{
			this.MoveToNextTime();
		}
		#endregion

		private void EventFactory_SelectAdditionalObject(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Add(e.Object);

			// refresh the visible keyframes on the timeline
			pnlTimeline.Invalidate();
		}

		private void EventFactory_DeselectObjects(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Remove(e.Object);
			// refresh the visible keyframes on the timeline
			pnlTimeline.Invalidate();
		}

		private void txtCurrentFrame_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtCurrentFrame_UpdateData();
			}
		}

		private void txtMaxFrames_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtMaxFrames_UpdateData();
			}
		}

		private void txtFrameRate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtFrameRate_UpdateData();
			}
		}

		private void EventFactory_RemoveKeyFrame(object sender, Midget.Events.Object.Animation.KeyFrameEventArgs e)
		{
			pnlTimeline.Invalidate();
		}

		private void EventFactory_AddKeyFrame(object sender, Midget.Events.Object.Animation.KeyFrameEventArgs e)
		{
			// redraw
			pnlTimeline.Invalidate();
		}
	}
}
