using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Midget;

using Microsoft.DirectX.Direct3D;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for MaterialEditor.
	/// </summary>
	public class MaterialEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ColorDialog colorDialog;
		private Salamander.Windows.Forms.CollapsiblePanelBar pnlBar;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Salamander.Windows.Forms.CollapsiblePanel pnlColorProp;
		private System.Windows.Forms.PictureBox picSpecular;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.PictureBox picEmissive;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.PictureBox picDiffuse;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.PictureBox picAmbient;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.PictureBox picTexture;
		private System.Windows.Forms.Button btnBrowse;
		
		private Midget.Materials.MidgetMaterial currentMaterial;
		private Salamander.Windows.Forms.CollapsiblePanel pnlTexture;

		private ArrayList selectedObjects;

		public MaterialEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			Midget.Events.EventFactory.SelectAdditionalObject+=new Midget.Events.Object.Selection.SelectAdditionalObjectEventHandler(EventFactory_SelectAdditionalObject);
			Midget.Events.EventFactory.DeselectObjects+=new Midget.Events.Object.Selection.DeselectObjectEventHandler(EventFactory_DeselectObjects);
		
			selectedObjects = new ArrayList();
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
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.pnlBar = new Salamander.Windows.Forms.CollapsiblePanelBar();
			this.pnlTexture = new Salamander.Windows.Forms.CollapsiblePanel();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.picTexture = new System.Windows.Forms.PictureBox();
			this.pnlColorProp = new Salamander.Windows.Forms.CollapsiblePanel();
			this.picSpecular = new System.Windows.Forms.PictureBox();
			this.label8 = new System.Windows.Forms.Label();
			this.picEmissive = new System.Windows.Forms.PictureBox();
			this.label6 = new System.Windows.Forms.Label();
			this.picDiffuse = new System.Windows.Forms.PictureBox();
			this.label4 = new System.Windows.Forms.Label();
			this.picAmbient = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.pnlBar)).BeginInit();
			this.pnlBar.SuspendLayout();
			this.pnlTexture.SuspendLayout();
			this.pnlColorProp.SuspendLayout();
			this.SuspendLayout();
			// 
			// colorDialog
			// 
			this.colorDialog.AnyColor = true;
			this.colorDialog.FullOpen = true;
			// 
			// pnlBar
			// 
			this.pnlBar.BackColor = System.Drawing.SystemColors.Control;
			this.pnlBar.Border = 8;
			this.pnlBar.Controls.Add(this.pnlTexture);
			this.pnlBar.Controls.Add(this.pnlColorProp);
			this.pnlBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBar.Location = new System.Drawing.Point(0, 0);
			this.pnlBar.Name = "pnlBar";
			this.pnlBar.Size = new System.Drawing.Size(216, 536);
			this.pnlBar.Spacing = 8;
			this.pnlBar.TabIndex = 7;
			this.pnlBar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBar_Paint);
			// 
			// pnlTexture
			// 
			this.pnlTexture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlTexture.BackColor = System.Drawing.Color.White;
			this.pnlTexture.Controls.Add(this.btnBrowse);
			this.pnlTexture.Controls.Add(this.picTexture);
			this.pnlTexture.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.pnlTexture.Image = null;
			this.pnlTexture.Location = new System.Drawing.Point(8, 296);
			this.pnlTexture.Name = "pnlTexture";
			this.pnlTexture.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.pnlTexture.Size = new System.Drawing.Size(200, 232);
			this.pnlTexture.StartColour = System.Drawing.SystemColors.Highlight;
			this.pnlTexture.TabIndex = 5;
			this.pnlTexture.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.pnlTexture.TitleFontColour = System.Drawing.Color.White;
			this.pnlTexture.TitleText = "Texture";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(40, 192);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(120, 23);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "&Browse";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// picTexture
			// 
			this.picTexture.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
			this.picTexture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picTexture.Location = new System.Drawing.Point(16, 40);
			this.picTexture.Name = "picTexture";
			this.picTexture.Size = new System.Drawing.Size(168, 136);
			this.picTexture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picTexture.TabIndex = 1;
			this.picTexture.TabStop = false;
			// 
			// pnlColorProp
			// 
			this.pnlColorProp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlColorProp.BackColor = System.Drawing.Color.White;
			this.pnlColorProp.Controls.Add(this.picSpecular);
			this.pnlColorProp.Controls.Add(this.label8);
			this.pnlColorProp.Controls.Add(this.picEmissive);
			this.pnlColorProp.Controls.Add(this.label6);
			this.pnlColorProp.Controls.Add(this.picDiffuse);
			this.pnlColorProp.Controls.Add(this.label4);
			this.pnlColorProp.Controls.Add(this.picAmbient);
			this.pnlColorProp.Controls.Add(this.label1);
			this.pnlColorProp.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.pnlColorProp.Image = null;
			this.pnlColorProp.Location = new System.Drawing.Point(8, 8);
			this.pnlColorProp.Name = "pnlColorProp";
			this.pnlColorProp.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.pnlColorProp.Size = new System.Drawing.Size(200, 280);
			this.pnlColorProp.StartColour = System.Drawing.SystemColors.Highlight;
			this.pnlColorProp.TabIndex = 4;
			this.pnlColorProp.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.pnlColorProp.TitleFontColour = System.Drawing.Color.White;
			this.pnlColorProp.TitleText = "Color";
			// 
			// picSpecular
			// 
			this.picSpecular.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.picSpecular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picSpecular.Location = new System.Drawing.Point(56, 208);
			this.picSpecular.Name = "picSpecular";
			this.picSpecular.Size = new System.Drawing.Size(128, 16);
			this.picSpecular.TabIndex = 18;
			this.picSpecular.TabStop = false;
			this.picSpecular.Click += new System.EventHandler(this.picSpecular_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 208);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 16);
			this.label8.TabIndex = 17;
			this.label8.Text = "Specular";
			// 
			// picEmissive
			// 
			this.picEmissive.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.picEmissive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picEmissive.Location = new System.Drawing.Point(56, 152);
			this.picEmissive.Name = "picEmissive";
			this.picEmissive.Size = new System.Drawing.Size(128, 16);
			this.picEmissive.TabIndex = 14;
			this.picEmissive.TabStop = false;
			this.picEmissive.Click += new System.EventHandler(this.picEmissive_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 152);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 16);
			this.label6.TabIndex = 13;
			this.label6.Text = "Emissive";
			// 
			// picDiffuse
			// 
			this.picDiffuse.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.picDiffuse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picDiffuse.Location = new System.Drawing.Point(56, 96);
			this.picDiffuse.Name = "picDiffuse";
			this.picDiffuse.Size = new System.Drawing.Size(128, 16);
			this.picDiffuse.TabIndex = 10;
			this.picDiffuse.TabStop = false;
			this.picDiffuse.Click += new System.EventHandler(this.picDiffuse_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 9;
			this.label4.Text = "Diffuse";
			// 
			// picAmbient
			// 
			this.picAmbient.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.picAmbient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picAmbient.Location = new System.Drawing.Point(56, 40);
			this.picAmbient.Name = "picAmbient";
			this.picAmbient.Size = new System.Drawing.Size(128, 16);
			this.picAmbient.TabIndex = 6;
			this.picAmbient.TabStop = false;
			this.picAmbient.Click += new System.EventHandler(this.picAmbient_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Ambient";
			// 
			// MaterialEditor
			// 
			this.AutoScroll = true;
			this.Controls.Add(this.pnlBar);
			this.Name = "MaterialEditor";
			this.Size = new System.Drawing.Size(216, 536);
			((System.ComponentModel.ISupportInitialize)(this.pnlBar)).EndInit();
			this.pnlBar.ResumeLayout(false);
			this.pnlTexture.ResumeLayout(false);
			this.pnlColorProp.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		#region Control Painting
		private void panelBar_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint (e);

			Graphics g = e.Graphics;

			Point[] corners = new Point[5];
			corners[0] = new Point(0, 0);
			corners[1] = new Point(this.Width - 1, 0);
			corners[2] = new Point(this.Width - 1, this.Height - 1);
			corners[3] = new Point(0, this.Height - 1);
			corners[4] = corners[0];

			g.DrawLines(Pens.DarkGray, corners);
		}
		#endregion

		private void picAmbient_Click(object sender, System.EventArgs e)
		{
			if(colorDialog.ShowDialog() == DialogResult.OK)
			{
				currentMaterial.Ambient = Color.FromArgb(currentMaterial.Ambient.A,colorDialog.Color);
				picAmbient.BackColor = colorDialog.Color;
			}
		}

		private void picDiffuse_Click(object sender, System.EventArgs e)
		{
			if(colorDialog.ShowDialog() == DialogResult.OK)
			{
				currentMaterial.Diffuse = Color.FromArgb(currentMaterial.Diffuse.A,colorDialog.Color);
				picDiffuse.BackColor = colorDialog.Color;
			}
		}

		private void picEmissive_Click(object sender, System.EventArgs e)
		{
			if(colorDialog.ShowDialog() == DialogResult.OK)
			{
				currentMaterial.Emissive = Color.FromArgb(currentMaterial.Emissive.A,colorDialog.Color);
				picEmissive.BackColor = colorDialog.Color;
			}
		}

		private void picSpecular_Click(object sender, System.EventArgs e)
		{
			if(colorDialog.ShowDialog() == DialogResult.OK)
			{
				currentMaterial.Specular = Color.FromArgb(currentMaterial.Specular.A,colorDialog.Color);
				picSpecular.BackColor = colorDialog.Color;
			}
		}

		private void EventFactory_SelectAdditionalObject(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Add(e.Object);
			LoadMaterial(e.Object.Material);
		}

		private void LoadMaterial(Midget.Materials.MidgetMaterial material)
		{
			currentMaterial = material;

			picAmbient.BackColor = (Color)currentMaterial.Ambient;
			picDiffuse.BackColor = (Color)currentMaterial.Diffuse;
			picEmissive.BackColor = (Color)currentMaterial.Emissive;
			picSpecular.BackColor = (Color)currentMaterial.Specular;

			if(currentMaterial.TexturePath.Length != 0)
			{
				picTexture.Image = Image.FromFile(currentMaterial.TexturePath);
			}

		}

		private void EventFactory_DeselectObjects(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Remove(e.Object);
			
			// if there are objects still selected
			if(selectedObjects.Count != 0)
			{
				LoadMaterial(((IObject3D)selectedObjects[selectedObjects.Count - 1]).Material);
			}
			else
			{
				this.Clear();
			}
		}

		private void Clear()
		{
			foreach (object obj in pnlColorProp.Controls)
			{
				if (obj is TextBox)
					((TextBox)obj).Text = "";
			}

			picTexture.Image = null;

		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{

			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					picTexture.Image = Image.FromFile(openFileDialog.FileName);
//					currentMaterial.Texture = TextureLoader.FromFile(DeviceManager.Instance.Device, openFileDialog.FileName);
					
					currentMaterial.Texture = TextureLoader.FromFile(DeviceManager.Instance.Device, openFileDialog.FileName, 
						D3DX.Default, D3DX.Default, D3DX.Default, 0, Format.Unknown, 
						Pool.Managed, Filter.Box /* Filter.Triangle|Filter.Mirror */ , 
						Filter.Box /* Filter.Triangle|Filter.Mirror */, 0);

					//old:
					//currentMaterial.Texture = GraphicsUtility.CreateTexture(DeviceManager.Instance.Device,openFileDialog.FileName,Format.Unknown);
					currentMaterial.TexturePath = openFileDialog.FileName;
				}
				catch
				{
					MessageBox.Show("Error In Loading Texture");
				}
				
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(picEmissive.BackColor.ToString());
		}
	}
}
