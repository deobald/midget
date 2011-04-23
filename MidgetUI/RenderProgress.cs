using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Midget;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for RenderProgress.
	/// </summary>
	public class RenderProgress : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar pbarProgress;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnPause;

		private const string PAUSE = "&Pause";
		private const string RESTART = "&Restart";
		private const string RESUME = "&Resume";
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Timer tmrRenderTime;
		private System.Windows.Forms.Label lblcurrFrameTime;
		private System.Windows.Forms.Label lblttlTime;
		private System.Windows.Forms.Label lbltimeRemaining;

		private FileRenderThread renderThread;

		private TimeSpan currTime;
		private TimeSpan totalTime;
		private System.Windows.Forms.Button btnPreview;
		private TimeSpan remainingTime;

		public RenderProgress(FileRenderThread thread)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.renderThread = thread;

			// setup progress bar
			pbarProgress.Minimum = thread.RenderSettings.StartFrame;
			pbarProgress.Maximum = thread.RenderSettings.EndFrame;

			pbarProgress.Step = 1;

			// add listener to update progress bar
			thread.FrameRendered += new System.EventHandler(this.Frame_Rendered);
			thread.ThreadFinishedEvent += new System.EventHandler(this.Render_Finished);
		}

		protected void Frame_Rendered(object sender, EventArgs e)
		{
			// increase the step by one
			pbarProgress.PerformStep();

			// update estimateTime
			int framesRemaining = pbarProgress.Maximum - pbarProgress.Value;
			remainingTime = TimeSpan.FromTicks((currTime.Ticks + TimeSpan.TicksPerSecond) * framesRemaining);
			


			currTime = TimeSpan.FromTicks(0);
			UpdateTimeLabels();
		}

		protected void Render_Finished(object sender, EventArgs e)
		{
			btnCancel.Enabled = false;
			btnPause.Enabled = false;
			tmrRenderTime.Stop();
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
			this.components = new System.ComponentModel.Container();
			System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RenderProgress));
			this.pbarProgress = new System.Windows.Forms.ProgressBar();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbltimeRemaining = new System.Windows.Forms.Label();
			this.lblttlTime = new System.Windows.Forms.Label();
			this.lblcurrFrameTime = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tmrRenderTime = new System.Windows.Forms.Timer(this.components);
			this.btnPreview = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pbarProgress
			// 
			this.pbarProgress.Location = new System.Drawing.Point(16, 8);
			this.pbarProgress.Name = "pbarProgress";
			this.pbarProgress.Size = new System.Drawing.Size(360, 24);
			this.pbarProgress.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.Enabled = ((bool)(configurationAppSettings.GetValue("btnCancel.Enabled", typeof(bool))));
			this.btnCancel.Location = new System.Drawing.Point(16, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(104, 24);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnPause
			// 
			this.btnPause.Enabled = ((bool)(configurationAppSettings.GetValue("btnPause.Enabled", typeof(bool))));
			this.btnPause.Location = new System.Drawing.Point(16, 72);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(104, 24);
			this.btnPause.TabIndex = 2;
			this.btnPause.Text = ((string)(configurationAppSettings.GetValue("btnPause.Text", typeof(string))));
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lbltimeRemaining);
			this.groupBox1.Controls.Add(this.lblttlTime);
			this.groupBox1.Controls.Add(this.lblcurrFrameTime);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(128, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(248, 88);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Rendering Time Info";
			// 
			// lbltimeRemaining
			// 
			this.lbltimeRemaining.Location = new System.Drawing.Point(128, 64);
			this.lbltimeRemaining.Name = "lbltimeRemaining";
			this.lbltimeRemaining.Size = new System.Drawing.Size(104, 16);
			this.lbltimeRemaining.TabIndex = 6;
			this.lbltimeRemaining.Text = "label6";
			// 
			// lblttlTime
			// 
			this.lblttlTime.Location = new System.Drawing.Point(88, 40);
			this.lblttlTime.Name = "lblttlTime";
			this.lblttlTime.Size = new System.Drawing.Size(152, 16);
			this.lblttlTime.TabIndex = 5;
			this.lblttlTime.Text = "label5";
			// 
			// lblcurrFrameTime
			// 
			this.lblcurrFrameTime.Location = new System.Drawing.Point(88, 16);
			this.lblcurrFrameTime.Name = "lblcurrFrameTime";
			this.lblcurrFrameTime.Size = new System.Drawing.Size(152, 16);
			this.lblcurrFrameTime.TabIndex = 4;
			this.lblcurrFrameTime.Text = "label4";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Current Frame:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Total Time:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Time Reminaing (est):";
			// 
			// tmrRenderTime
			// 
			this.tmrRenderTime.Enabled = true;
			this.tmrRenderTime.Interval = 1000;
			this.tmrRenderTime.Tick += new System.EventHandler(this.tmrRenderTime_Tick);
			// 
			// btnPreview
			// 
			this.btnPreview.Enabled = false;
			this.btnPreview.Location = new System.Drawing.Point(16, 40);
			this.btnPreview.Name = "btnPreview";
			this.btnPreview.Size = new System.Drawing.Size(104, 23);
			this.btnPreview.TabIndex = 4;
			this.btnPreview.Text = "Preview";
			// 
			// RenderProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(378, 135);
			this.Controls.Add(this.btnPreview);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnPause);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.pbarProgress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "RenderProgress";
			this.Text = "Batch Render Progress";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RenderProgress_Closing);
			this.Load += new System.EventHandler(this.RenderProgress_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			renderThread.Thread.Abort();
			
			// reset the time
			tmrRenderTime.Stop();

			btnPause.Text = RESTART;
		}

		private void btnPause_Click(object sender, System.EventArgs e)
		{
			if(btnPause.Text == PAUSE)
			{
				renderThread.Thread.Suspend();
				tmrRenderTime.Stop();
				btnPause.Text = RESUME;
				btnCancel.Enabled = false;
			}
			else if (btnPause.Text == RESUME)
			{
				renderThread.Thread.Resume();
				tmrRenderTime.Start();
				btnPause.Text = PAUSE;
				btnCancel.Enabled = true;
			}
			else
			{	
				renderThread.Reset();
				StartRendering();
				
				btnPause.Text = PAUSE;
				btnCancel.Enabled = true;
			}
		}

		private void RenderProgress_Load(object sender, System.EventArgs e)
		{
			pbarProgress.Minimum = renderThread.RenderSettings.StartFrame;
			pbarProgress.Maximum = renderThread.RenderSettings.EndFrame;
			StartRendering();
		}

		private void StartRendering()
		{
			// setup rendering timers
			pbarProgress.Value = pbarProgress.Minimum;
			currTime = new TimeSpan(0);
			totalTime = new TimeSpan(0);
			remainingTime = new TimeSpan(0);
			UpdateTimeLabels();
			tmrRenderTime.Start();


			// start the thread
			renderThread.Thread.Start();
			
		}

		private void tmrRenderTime_Tick(object sender, System.EventArgs e)
		{
			currTime += TimeSpan.FromTicks(TimeSpan.TicksPerSecond);
			totalTime += TimeSpan.FromTicks(TimeSpan.TicksPerSecond);

			UpdateTimeLabels();
		}

		private void UpdateTimeLabels()
		{
			lblcurrFrameTime.Text = currTime.ToString();
			lblttlTime.Text = totalTime.ToString();
			lbltimeRemaining.Text = remainingTime.ToString();
		}

		private void RenderProgress_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// kill the rendering thread if it has not already finished
			if(renderThread.Thread.ThreadState != System.Threading.ThreadState.Aborted)
			{
				if(renderThread.Thread.ThreadState == System.Threading.ThreadState.Suspended)
				{
					renderThread.Thread.Priority = System.Threading.ThreadPriority.Lowest;
					renderThread.Thread.Resume();
				}
				renderThread.Thread.Abort();
				
				// wait until the rendering thread dies
				renderThread.Thread.Join();
			}
			tmrRenderTime.Stop();
		}
	}
}
