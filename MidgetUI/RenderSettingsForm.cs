using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Midget;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for RenderSettingsForm.
	/// </summary>
	public class RenderSettingsForm : System.Windows.Forms.Form
	{
		private Salamander.Windows.Forms.CollapsiblePanelBar collapsiblePanelBar1;
		private Salamander.Windows.Forms.CollapsiblePanel pnlImageFile;
		private System.Windows.Forms.ComboBox cboFileType;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnPathBrowse;
		private System.Windows.Forms.TextBox txtBoxFilePath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label6;
		private Salamander.Windows.Forms.CollapsiblePanel collapsiblePanel1;
		private Salamander.Windows.Forms.CollapsiblePanel pnlResolution;
		private MidgetUI.NumberEnhancedTextBox txtHeight;
		private MidgetUI.NumberEnhancedTextBox txtWidth;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cboCameras;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private MidgetUI.NumberEnhancedTextBox txtStartFrame;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private FileRenderSettings renderSettings;
		private MidgetUI.NumberEnhancedTextBox txtEndFrame;
		private System.Windows.Forms.Label lblEndFrame;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private CameraFactory cf;

		public RenderSettingsForm(CameraFactory initCF, FileRenderSettings initRenderSettings)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			renderSettings = initRenderSettings;
			cf = initCF;

			LoadRenderSettings();
		}

		private void LoadRenderSettings()
		{	
			PopulateFileTypeCombo();
			PopulateCameraCombo();

			txtBoxFilePath.Text = renderSettings.FilePath;
			txtFileName.Text = renderSettings.FileName;
			
			txtWidth.Text = renderSettings.Width.ToString();
			txtHeight.Text = renderSettings.Height.ToString();

			txtStartFrame.Text = renderSettings.StartFrame.ToString();
			txtEndFrame.Text = renderSettings.EndFrame.ToString();

			cboCameras.SelectedItem = renderSettings.Camera.Name;

			cboFileType.SelectedItem = renderSettings.FileFormat.ToString();
		}
		
		private void PopulateFileTypeCombo()
		{
			// not sure if this works
			FileFormat temp = FileFormat.bmp;
			cboFileType.Items.AddRange(System.Enum.GetNames(temp.GetType()));
		}

		private void PopulateCameraCombo()
		{
			cboCameras.Items.AddRange(CameraFactory.Instance.CameraNames.ToArray());
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RenderSettingsForm));
			this.collapsiblePanelBar1 = new Salamander.Windows.Forms.CollapsiblePanelBar();
			this.pnlResolution = new Salamander.Windows.Forms.CollapsiblePanel();
			this.label7 = new System.Windows.Forms.Label();
			this.txtHeight = new MidgetUI.NumberEnhancedTextBox();
			this.txtWidth = new MidgetUI.NumberEnhancedTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.collapsiblePanel1 = new Salamander.Windows.Forms.CollapsiblePanel();
			this.label10 = new System.Windows.Forms.Label();
			this.txtEndFrame = new MidgetUI.NumberEnhancedTextBox();
			this.txtStartFrame = new MidgetUI.NumberEnhancedTextBox();
			this.lblEndFrame = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cboCameras = new System.Windows.Forms.ComboBox();
			this.pnlImageFile = new Salamander.Windows.Forms.CollapsiblePanel();
			this.label6 = new System.Windows.Forms.Label();
			this.cboFileType = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnPathBrowse = new System.Windows.Forms.Button();
			this.txtBoxFilePath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			((System.ComponentModel.ISupportInitialize)(this.collapsiblePanelBar1)).BeginInit();
			this.collapsiblePanelBar1.SuspendLayout();
			this.pnlResolution.SuspendLayout();
			this.collapsiblePanel1.SuspendLayout();
			this.pnlImageFile.SuspendLayout();
			this.SuspendLayout();
			// 
			// collapsiblePanelBar1
			// 
			this.collapsiblePanelBar1.AutoScroll = true;
			this.collapsiblePanelBar1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.collapsiblePanelBar1.Border = 8;
			this.collapsiblePanelBar1.Controls.Add(this.pnlResolution);
			this.collapsiblePanelBar1.Controls.Add(this.collapsiblePanel1);
			this.collapsiblePanelBar1.Controls.Add(this.pnlImageFile);
			this.collapsiblePanelBar1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.collapsiblePanelBar1.Location = new System.Drawing.Point(0, 0);
			this.collapsiblePanelBar1.Name = "collapsiblePanelBar1";
			this.collapsiblePanelBar1.Size = new System.Drawing.Size(368, 461);
			this.collapsiblePanelBar1.Spacing = 8;
			this.collapsiblePanelBar1.TabIndex = 1;
			// 
			// pnlResolution
			// 
			this.pnlResolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlResolution.BackColor = System.Drawing.SystemColors.Control;
			this.pnlResolution.Controls.Add(this.label7);
			this.pnlResolution.Controls.Add(this.txtHeight);
			this.pnlResolution.Controls.Add(this.txtWidth);
			this.pnlResolution.Controls.Add(this.label5);
			this.pnlResolution.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.pnlResolution.Image = null;
			this.pnlResolution.Location = new System.Drawing.Point(8, 328);
			this.pnlResolution.Name = "pnlResolution";
			this.pnlResolution.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.pnlResolution.Size = new System.Drawing.Size(352, 120);
			this.pnlResolution.StartColour = System.Drawing.SystemColors.Highlight;
			this.pnlResolution.TabIndex = 6;
			this.pnlResolution.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.pnlResolution.TitleFontColour = System.Drawing.Color.White;
			this.pnlResolution.TitleText = "Resolution";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(112, 48);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(48, 16);
			this.label7.TabIndex = 10;
			this.label7.Text = "Width";
			// 
			// txtHeight
			// 
			this.txtHeight.Location = new System.Drawing.Point(160, 72);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.NegativeAllowed = false;
			this.txtHeight.Size = new System.Drawing.Size(64, 20);
			this.txtHeight.TabIndex = 9;
			this.txtHeight.Text = "";
			this.txtHeight.TextBoxType = MidgetUI.TextBoxType.Integer;
			this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
			// 
			// txtWidth
			// 
			this.txtWidth.Location = new System.Drawing.Point(160, 40);
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.NegativeAllowed = false;
			this.txtWidth.Size = new System.Drawing.Size(64, 20);
			this.txtWidth.TabIndex = 8;
			this.txtWidth.Text = "";
			this.txtWidth.TextBoxType = MidgetUI.TextBoxType.Integer;
			this.txtWidth.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(112, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 7;
			this.label5.Text = "Height";
			// 
			// collapsiblePanel1
			// 
			this.collapsiblePanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.collapsiblePanel1.BackColor = System.Drawing.SystemColors.Control;
			this.collapsiblePanel1.Controls.Add(this.label10);
			this.collapsiblePanel1.Controls.Add(this.txtEndFrame);
			this.collapsiblePanel1.Controls.Add(this.txtStartFrame);
			this.collapsiblePanel1.Controls.Add(this.lblEndFrame);
			this.collapsiblePanel1.Controls.Add(this.label9);
			this.collapsiblePanel1.Controls.Add(this.cboCameras);
			this.collapsiblePanel1.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.collapsiblePanel1.Image = null;
			this.collapsiblePanel1.Location = new System.Drawing.Point(8, 176);
			this.collapsiblePanel1.Name = "collapsiblePanel1";
			this.collapsiblePanel1.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.collapsiblePanel1.Size = new System.Drawing.Size(352, 144);
			this.collapsiblePanel1.StartColour = System.Drawing.SystemColors.Highlight;
			this.collapsiblePanel1.TabIndex = 5;
			this.collapsiblePanel1.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.collapsiblePanel1.TitleFontColour = System.Drawing.Color.White;
			this.collapsiblePanel1.TitleText = "Movie Options";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(72, 88);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(80, 16);
			this.label10.TabIndex = 22;
			this.label10.Text = "Start Frame:";
			// 
			// txtEndFrame
			// 
			this.txtEndFrame.Location = new System.Drawing.Point(152, 112);
			this.txtEndFrame.Name = "txtEndFrame";
			this.txtEndFrame.NegativeAllowed = false;
			this.txtEndFrame.Size = new System.Drawing.Size(64, 20);
			this.txtEndFrame.TabIndex = 21;
			this.txtEndFrame.Text = "";
			this.txtEndFrame.TextBoxType = MidgetUI.TextBoxType.Integer;
			this.txtEndFrame.TextChanged += new System.EventHandler(this.txtEndFrame_TextChanged);
			// 
			// txtStartFrame
			// 
			this.txtStartFrame.Location = new System.Drawing.Point(152, 80);
			this.txtStartFrame.Name = "txtStartFrame";
			this.txtStartFrame.NegativeAllowed = false;
			this.txtStartFrame.Size = new System.Drawing.Size(64, 20);
			this.txtStartFrame.TabIndex = 20;
			this.txtStartFrame.Text = "";
			this.txtStartFrame.TextBoxType = MidgetUI.TextBoxType.Integer;
			this.txtStartFrame.TextChanged += new System.EventHandler(this.txtStartFrame_TextChanged);
			// 
			// lblEndFrame
			// 
			this.lblEndFrame.Location = new System.Drawing.Point(80, 112);
			this.lblEndFrame.Name = "lblEndFrame";
			this.lblEndFrame.Size = new System.Drawing.Size(72, 16);
			this.lblEndFrame.TabIndex = 19;
			this.lblEndFrame.Text = "End Frame";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(88, 40);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(48, 16);
			this.label9.TabIndex = 18;
			this.label9.Text = "Camera";
			// 
			// cboCameras
			// 
			this.cboCameras.Location = new System.Drawing.Point(148, 40);
			this.cboCameras.Name = "cboCameras";
			this.cboCameras.Size = new System.Drawing.Size(120, 21);
			this.cboCameras.TabIndex = 17;
			this.cboCameras.SelectionChangeCommitted += new System.EventHandler(this.cboCameras_SelectionChangeCommitted);
			// 
			// pnlImageFile
			// 
			this.pnlImageFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlImageFile.BackColor = System.Drawing.SystemColors.Control;
			this.pnlImageFile.Controls.Add(this.label6);
			this.pnlImageFile.Controls.Add(this.cboFileType);
			this.pnlImageFile.Controls.Add(this.label8);
			this.pnlImageFile.Controls.Add(this.txtFileName);
			this.pnlImageFile.Controls.Add(this.label2);
			this.pnlImageFile.Controls.Add(this.btnPathBrowse);
			this.pnlImageFile.Controls.Add(this.txtBoxFilePath);
			this.pnlImageFile.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.pnlImageFile.Image = null;
			this.pnlImageFile.Location = new System.Drawing.Point(8, 8);
			this.pnlImageFile.Name = "pnlImageFile";
			this.pnlImageFile.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.pnlImageFile.Size = new System.Drawing.Size(352, 160);
			this.pnlImageFile.StartColour = System.Drawing.SystemColors.Highlight;
			this.pnlImageFile.TabIndex = 1;
			this.pnlImageFile.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.pnlImageFile.TitleFontColour = System.Drawing.Color.White;
			this.pnlImageFile.TitleText = "Image File Output";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.TabIndex = 16;
			this.label6.Text = "Output Directory";
			// 
			// cboFileType
			// 
			this.cboFileType.Location = new System.Drawing.Point(136, 128);
			this.cboFileType.Name = "cboFileType";
			this.cboFileType.Size = new System.Drawing.Size(120, 21);
			this.cboFileType.TabIndex = 13;
			this.cboFileType.SelectedIndexChanged += new System.EventHandler(this.cboFileType_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(72, 128);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 16);
			this.label8.TabIndex = 12;
			this.label8.Text = "File Type:";
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(136, 96);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(120, 20);
			this.txtFileName.TabIndex = 11;
			this.txtFileName.Text = "";
			this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(72, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "File Name";
			// 
			// btnPathBrowse
			// 
			this.btnPathBrowse.Location = new System.Drawing.Point(288, 56);
			this.btnPathBrowse.Name = "btnPathBrowse";
			this.btnPathBrowse.Size = new System.Drawing.Size(56, 24);
			this.btnPathBrowse.TabIndex = 9;
			this.btnPathBrowse.Text = "Browse";
			this.btnPathBrowse.Click += new System.EventHandler(this.btnPathBrowse_Click);
			// 
			// txtBoxFilePath
			// 
			this.txtBoxFilePath.Enabled = false;
			this.txtBoxFilePath.Location = new System.Drawing.Point(16, 56);
			this.txtBoxFilePath.Name = "txtBoxFilePath";
			this.txtBoxFilePath.Size = new System.Drawing.Size(264, 20);
			this.txtBoxFilePath.TabIndex = 8;
			this.txtBoxFilePath.Text = "";
			this.txtBoxFilePath.TextChanged += new System.EventHandler(this.txtBoxFilePath_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 0);
			this.label3.Name = "label3";
			this.label3.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 0);
			this.label4.Name = "label4";
			this.label4.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			// 
			// RenderSettingsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 461);
			this.Controls.Add(this.collapsiblePanelBar1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RenderSettingsForm";
			this.Text = "Render Settings";
			((System.ComponentModel.ISupportInitialize)(this.collapsiblePanelBar1)).EndInit();
			this.collapsiblePanelBar1.ResumeLayout(false);
			this.pnlResolution.ResumeLayout(false);
			this.collapsiblePanel1.ResumeLayout(false);
			this.pnlImageFile.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void cboCameras_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			renderSettings.Camera = cf.GetExistingCamera(cboCameras.SelectedIndex);
		}

		private void cboFileType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			renderSettings.FileFormat = (FileFormat)System.Enum.Parse(typeof(FileFormat),(string)cboFileType.SelectedItem);
		}

		private void btnPathBrowse_Click(object sender, System.EventArgs e)
		{
			if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				if (folderBrowserDialog.SelectedPath.Trim().Length != 0)
				{
					txtBoxFilePath.Text = folderBrowserDialog.SelectedPath.Trim();
				}
			}
		}

		private void txtBoxFilePath_TextChanged(object sender, System.EventArgs e)
		{
			base.OnTextChanged(e);

			renderSettings.FilePath = txtBoxFilePath.Text;
		}

		private void txtFileName_TextChanged(object sender, System.EventArgs e)
		{
			base.OnTextChanged(e);

			renderSettings.FileName = txtFileName.Text;
		}

		private void txtStartFrame_TextChanged(object sender, System.EventArgs e)
		{
			base.OnTextChanged(e);
			renderSettings.StartFrame = Convert.ToInt32(txtStartFrame.Text);
		}

		private void txtEndFrame_TextChanged(object sender, System.EventArgs e)
		{
			base.OnTextChanged(e);
			renderSettings.EndFrame = Convert.ToInt32(txtEndFrame.Text);
		}

		private void txtWidth_TextChanged(object sender, System.EventArgs e)
		{
			base.OnTextChanged(e);
			renderSettings.Width = Convert.ToInt32(txtWidth.Text);
		}

		private void txtHeight_TextChanged(object sender, System.EventArgs e)
		{
			base.OnTextChanged(e);
			renderSettings.Height = Convert.ToInt32(txtHeight.Text);
		}
	}
}
