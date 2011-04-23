using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Midget;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for ObjectShelf.
	/// </summary>
	public class ObjectShelf : System.Windows.Forms.UserControl
	{
		private Salamander.Windows.Forms.CollapsiblePanelBar panelBar;
		private Salamander.Windows.Forms.CollapsiblePanel mainPanel;
		private System.Windows.Forms.Label lblRotate;
		private MidgetUI.NumberEnhancedTextBox txtRotateX;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private MidgetUI.NumberEnhancedTextBox txtMoveZ;
		private MidgetUI.NumberEnhancedTextBox txtMoveY;
		private MidgetUI.NumberEnhancedTextBox txtMoveX;
		private System.Windows.Forms.Label label4;
		private MidgetUI.NumberEnhancedTextBox txtScaleZ;
		private MidgetUI.NumberEnhancedTextBox txtScaleY;
		private MidgetUI.NumberEnhancedTextBox txtScaleX;
		private System.Windows.Forms.Label label5;
		private MidgetUI.NumberEnhancedTextBox txtRotateY;
		private MidgetUI.NumberEnhancedTextBox txtRotateZ;
		private Salamander.Windows.Forms.CollapsiblePanel parameterPanel;
		private MidgetUI.EnhancedTextBox txtCurrentObject;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ArrayList selectedObjects;

		public ObjectShelf()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			selectedObjects = new ArrayList();

			Midget.Events.EventFactory.Transformation+=new Midget.Events.Object.Transformation.TransformationEventHandler(EventFactory_Transformation);
			Midget.Events.EventFactory.SelectAdditionalObject+=new Midget.Events.Object.Selection.SelectAdditionalObjectEventHandler(EventFactory_SelectAdditionalObject);
			Midget.Events.EventFactory.DeselectObjects+=new Midget.Events.Object.Selection.DeselectObjectEventHandler(EventFactory_DeselectObjects);
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
			this.panelBar = new Salamander.Windows.Forms.CollapsiblePanelBar();
			this.parameterPanel = new Salamander.Windows.Forms.CollapsiblePanel();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.mainPanel = new Salamander.Windows.Forms.CollapsiblePanel();
			this.txtCurrentObject = new MidgetUI.EnhancedTextBox();
			this.txtScaleZ = new MidgetUI.NumberEnhancedTextBox();
			this.txtScaleY = new MidgetUI.NumberEnhancedTextBox();
			this.txtScaleX = new MidgetUI.NumberEnhancedTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMoveZ = new MidgetUI.NumberEnhancedTextBox();
			this.txtMoveY = new MidgetUI.NumberEnhancedTextBox();
			this.txtMoveX = new MidgetUI.NumberEnhancedTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtRotateZ = new MidgetUI.NumberEnhancedTextBox();
			this.txtRotateY = new MidgetUI.NumberEnhancedTextBox();
			this.txtRotateX = new MidgetUI.NumberEnhancedTextBox();
			this.lblRotate = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.panelBar)).BeginInit();
			this.panelBar.SuspendLayout();
			this.parameterPanel.SuspendLayout();
			this.mainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelBar
			// 
			this.panelBar.BackColor = System.Drawing.SystemColors.Control;
			this.panelBar.Border = 8;
			this.panelBar.Controls.Add(this.parameterPanel);
			this.panelBar.Controls.Add(this.mainPanel);
			this.panelBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelBar.Location = new System.Drawing.Point(0, 0);
			this.panelBar.Name = "panelBar";
			this.panelBar.Size = new System.Drawing.Size(240, 520);
			this.panelBar.Spacing = 8;
			this.panelBar.TabIndex = 0;
			this.panelBar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBar_Paint);
			// 
			// parameterPanel
			// 
			this.parameterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.parameterPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.parameterPanel.Controls.Add(this.propertyGrid);
			this.parameterPanel.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.parameterPanel.Image = null;
			this.parameterPanel.Location = new System.Drawing.Point(8, 176);
			this.parameterPanel.Name = "parameterPanel";
			this.parameterPanel.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.parameterPanel.Size = new System.Drawing.Size(224, 336);
			this.parameterPanel.StartColour = System.Drawing.SystemColors.Highlight;
			this.parameterPanel.TabIndex = 1;
			this.parameterPanel.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.parameterPanel.TitleFontColour = System.Drawing.Color.White;
			this.parameterPanel.TitleText = "Parameters";
			// 
			// propertyGrid
			// 
			this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid.CommandsBackColor = System.Drawing.SystemColors.ControlLightLight;
			this.propertyGrid.CommandsVisibleIfAvailable = true;
			this.propertyGrid.HelpBackColor = System.Drawing.SystemColors.Highlight;
			this.propertyGrid.HelpForeColor = System.Drawing.Color.White;
			this.propertyGrid.LargeButtons = false;
			this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid.Location = new System.Drawing.Point(8, 32);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
			this.propertyGrid.Size = new System.Drawing.Size(208, 296);
			this.propertyGrid.TabIndex = 30;
			this.propertyGrid.Text = "propertyGrid";
			this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// mainPanel
			// 
			this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mainPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.mainPanel.Controls.Add(this.txtCurrentObject);
			this.mainPanel.Controls.Add(this.txtScaleZ);
			this.mainPanel.Controls.Add(this.txtScaleY);
			this.mainPanel.Controls.Add(this.txtScaleX);
			this.mainPanel.Controls.Add(this.label5);
			this.mainPanel.Controls.Add(this.txtMoveZ);
			this.mainPanel.Controls.Add(this.txtMoveY);
			this.mainPanel.Controls.Add(this.txtMoveX);
			this.mainPanel.Controls.Add(this.label4);
			this.mainPanel.Controls.Add(this.label3);
			this.mainPanel.Controls.Add(this.label2);
			this.mainPanel.Controls.Add(this.label1);
			this.mainPanel.Controls.Add(this.txtRotateZ);
			this.mainPanel.Controls.Add(this.txtRotateY);
			this.mainPanel.Controls.Add(this.txtRotateX);
			this.mainPanel.Controls.Add(this.lblRotate);
			this.mainPanel.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.mainPanel.Image = null;
			this.mainPanel.Location = new System.Drawing.Point(8, 8);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.mainPanel.Size = new System.Drawing.Size(224, 160);
			this.mainPanel.StartColour = System.Drawing.SystemColors.Highlight;
			this.mainPanel.TabIndex = 0;
			this.mainPanel.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mainPanel.TitleFontColour = System.Drawing.Color.White;
			this.mainPanel.TitleText = "Object";
			// 
			// txtCurrentObject
			// 
			this.txtCurrentObject.Location = new System.Drawing.Point(24, 32);
			this.txtCurrentObject.Name = "txtCurrentObject";
			this.txtCurrentObject.Size = new System.Drawing.Size(176, 20);
			this.txtCurrentObject.TabIndex = 20;
			this.txtCurrentObject.Text = "";
			this.txtCurrentObject.Leave += new System.EventHandler(this.txtCurrentObject_Leave);
			this.txtCurrentObject.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCurrentObject_KeyUp);
			// 
			// txtScaleZ
			// 
			this.txtScaleZ.Location = new System.Drawing.Point(168, 128);
			this.txtScaleZ.Name = "txtScaleZ";
			this.txtScaleZ.NegativeAllowed = true;
			this.txtScaleZ.Size = new System.Drawing.Size(48, 20);
			this.txtScaleZ.TabIndex = 19;
			this.txtScaleZ.Text = "";
			this.txtScaleZ.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtScaleZ.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScaleZ_KeyUp);
			this.txtScaleZ.Leave += new System.EventHandler(this.txtScaleZ_Leave);
			// 
			// txtScaleY
			// 
			this.txtScaleY.Location = new System.Drawing.Point(112, 128);
			this.txtScaleY.Name = "txtScaleY";
			this.txtScaleY.NegativeAllowed = true;
			this.txtScaleY.Size = new System.Drawing.Size(48, 20);
			this.txtScaleY.TabIndex = 18;
			this.txtScaleY.Text = "";
			this.txtScaleY.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtScaleY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScaleY_KeyUp);
			this.txtScaleY.Leave += new System.EventHandler(this.txtScaleY_Leave);
			// 
			// txtScaleX
			// 
			this.txtScaleX.Location = new System.Drawing.Point(56, 128);
			this.txtScaleX.Name = "txtScaleX";
			this.txtScaleX.NegativeAllowed = true;
			this.txtScaleX.Size = new System.Drawing.Size(48, 20);
			this.txtScaleX.TabIndex = 17;
			this.txtScaleX.Text = "";
			this.txtScaleX.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtScaleX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScaleX_KeyUp);
			this.txtScaleX.Leave += new System.EventHandler(this.txtScaleX_Leave);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 16;
			this.label5.Text = "Scale:";
			// 
			// txtMoveZ
			// 
			this.txtMoveZ.Location = new System.Drawing.Point(168, 104);
			this.txtMoveZ.Name = "txtMoveZ";
			this.txtMoveZ.NegativeAllowed = true;
			this.txtMoveZ.Size = new System.Drawing.Size(48, 20);
			this.txtMoveZ.TabIndex = 15;
			this.txtMoveZ.Text = "";
			this.txtMoveZ.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtMoveZ.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMoveZ_KeyUp);
			this.txtMoveZ.Leave += new System.EventHandler(this.txtMoveZ_Leave);
			// 
			// txtMoveY
			// 
			this.txtMoveY.Location = new System.Drawing.Point(112, 104);
			this.txtMoveY.Name = "txtMoveY";
			this.txtMoveY.NegativeAllowed = true;
			this.txtMoveY.Size = new System.Drawing.Size(48, 20);
			this.txtMoveY.TabIndex = 14;
			this.txtMoveY.Text = "";
			this.txtMoveY.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtMoveY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMoveY_KeyUp);
			this.txtMoveY.Leave += new System.EventHandler(this.txtMoveY_Leave);
			// 
			// txtMoveX
			// 
			this.txtMoveX.Location = new System.Drawing.Point(56, 104);
			this.txtMoveX.Name = "txtMoveX";
			this.txtMoveX.NegativeAllowed = true;
			this.txtMoveX.Size = new System.Drawing.Size(48, 20);
			this.txtMoveX.TabIndex = 13;
			this.txtMoveX.Text = "";
			this.txtMoveX.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtMoveX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMoveX_KeyUp);
			this.txtMoveX.Leave += new System.EventHandler(this.txtMoveX_Leave);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Move:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(168, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 11;
			this.label3.Text = "Z";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(112, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "Y";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(56, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 9;
			this.label1.Text = "X";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtRotateZ
			// 
			this.txtRotateZ.Location = new System.Drawing.Point(168, 80);
			this.txtRotateZ.Name = "txtRotateZ";
			this.txtRotateZ.NegativeAllowed = true;
			this.txtRotateZ.Size = new System.Drawing.Size(48, 20);
			this.txtRotateZ.TabIndex = 8;
			this.txtRotateZ.Text = "";
			this.txtRotateZ.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtRotateZ.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRotateZ_KeyUp);
			this.txtRotateZ.Leave += new System.EventHandler(this.txtRotateZ_Leave);
			// 
			// txtRotateY
			// 
			this.txtRotateY.Location = new System.Drawing.Point(112, 80);
			this.txtRotateY.Name = "txtRotateY";
			this.txtRotateY.NegativeAllowed = true;
			this.txtRotateY.Size = new System.Drawing.Size(48, 20);
			this.txtRotateY.TabIndex = 7;
			this.txtRotateY.Text = "";
			this.txtRotateY.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtRotateY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRotateY_KeyUp);
			this.txtRotateY.Leave += new System.EventHandler(this.txtRotateY_Leave);
			// 
			// txtRotateX
			// 
			this.txtRotateX.Location = new System.Drawing.Point(56, 80);
			this.txtRotateX.Name = "txtRotateX";
			this.txtRotateX.NegativeAllowed = true;
			this.txtRotateX.Size = new System.Drawing.Size(48, 20);
			this.txtRotateX.TabIndex = 6;
			this.txtRotateX.Text = "";
			this.txtRotateX.TextBoxType = MidgetUI.TextBoxType.Float;
			this.txtRotateX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRotateX_KeyUp);
			this.txtRotateX.Leave += new System.EventHandler(this.txtRotateX_Leave);
			// 
			// lblRotate
			// 
			this.lblRotate.Location = new System.Drawing.Point(8, 80);
			this.lblRotate.Name = "lblRotate";
			this.lblRotate.Size = new System.Drawing.Size(48, 16);
			this.lblRotate.TabIndex = 5;
			this.lblRotate.Text = "Rotate:";
			// 
			// ObjectShelf
			// 
			this.Controls.Add(this.panelBar);
			this.Name = "ObjectShelf";
			this.Size = new System.Drawing.Size(240, 520);
			this.Resize += new System.EventHandler(this.ObjectShelf_Resize);
			((System.ComponentModel.ISupportInitialize)(this.panelBar)).EndInit();
			this.panelBar.ResumeLayout(false);
			this.parameterPanel.ResumeLayout(false);
			this.mainPanel.ResumeLayout(false);
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

		/// <summary>
		/// Clear all the shelf's fields
		/// </summary>
		public void Clear()
		{	
			// clear fields
			foreach (object obj in mainPanel.Controls)
			{
				if (obj is TextBox)
					((EnhancedTextBox)obj).SetText(null);
			}

			// clear parameter box
			propertyGrid.SelectedObject = null;
			propertyGrid.SelectedObjects = null;
		}

		/// <summary>
		/// Get all values from an object and display them.
		/// Customize the display for this type of object.
		/// </summary>
		/// <param name="obj">The object to get manipulation data from</param>
		private void FillObjectValues(IObject3D obj)
		{
			// fill in this object's values
			if (obj != null)
			{
				txtCurrentObject.SetText(obj.Name);

				txtRotateX.SetText(obj.Rotation.X.ToString());
				txtRotateY.SetText(obj.Rotation.Y.ToString());
				txtRotateZ.SetText(obj.Rotation.Z.ToString());

				txtMoveX.SetText(obj.Translation.X.ToString());
				txtMoveY.SetText(obj.Translation.Y.ToString());
				txtMoveZ.SetText(obj.Translation.Z.ToString());

				txtScaleX.SetText(obj.Scaling.X.ToString());
				txtScaleY.SetText(obj.Scaling.Y.ToString());
				txtScaleZ.SetText(obj.Scaling.Z.ToString());
			}

			propertyGrid.SelectedObject = this.CurrentObject;
		}

		#region Object Manipulation Controls
		// name //
		private void NameEdit()
		{
			if (txtCurrentObject.CheckForChange() && CurrentObject != null)
			{
				CurrentObject.Name = txtCurrentObject.Text;
			}
			DeviceManager.Instance.UpdateViews();
		}
		private void txtCurrentObject_Leave(object sender, System.EventArgs e)
		{
			NameEdit();
		}
		private void txtCurrentObject_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				NameEdit();
			}
		}

		// rotate //
		private void RotateXEdit()
		{
			if (txtRotateX.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(Convert.ToSingle(txtRotateX.Text), CurrentObject.Rotation.Y, CurrentObject.Rotation.Z),
					Midget.Events.Object.Transformation.Transformation.Rotate);
				//CurrentObject.Rotate(Convert.ToSingle(txtRotateX.Text), CurrentObject.Rotation.Y, CurrentObject.Rotation.Z);
			}
		}
		private void RotateYEdit()
		{
			if (txtRotateY.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(CurrentObject.Rotation.X, Convert.ToSingle(txtRotateY.Text), CurrentObject.Rotation.Z),
					Midget.Events.Object.Transformation.Transformation.Rotate);
				//CurrentObject.Rotate(CurrentObject.Rotation.X, Convert.ToSingle(txtRotateY.Text), CurrentObject.Rotation.Z);
			}
		}
		private void RotateZEdit()
		{
			if (txtRotateZ.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(CurrentObject.Rotation.X, CurrentObject.Rotation.Y, Convert.ToSingle(txtRotateZ.Text)),
					Midget.Events.Object.Transformation.Transformation.Rotate);
				//CurrentObject.Rotate(CurrentObject.Rotation.X, CurrentObject.Rotation.Y, Convert.ToSingle(txtRotateZ.Text));
			}
		}
		private void txtRotateX_Leave(object sender, System.EventArgs e)
		{
			RotateXEdit();
		}
		private void txtRotateY_Leave(object sender, System.EventArgs e)
		{
			RotateYEdit();
		}
		
		private void txtRotateZ_Leave(object sender, System.EventArgs e)
		{
			RotateZEdit();
		}
		private void txtRotateX_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				RotateXEdit();
		}
		private void txtRotateY_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				RotateYEdit();
		}
		private void txtRotateZ_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				RotateZEdit();
		}

		// translate //
		private void MoveXEdit()
		{
			if (txtMoveX.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(Convert.ToSingle(txtMoveX.Text), CurrentObject.Translation.Y, CurrentObject.Translation.Z),
					Midget.Events.Object.Transformation.Transformation.Translate);
			}
		}
		private void MoveYEdit()
		{
			if (txtMoveY.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(CurrentObject.Translation.X, Convert.ToSingle(txtMoveY.Text), CurrentObject.Translation.Z),
					Midget.Events.Object.Transformation.Transformation.Translate);
			}
		}
		private void MoveZEdit()
		{
			if (txtMoveZ.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(CurrentObject.Translation.X, CurrentObject.Translation.Y, Convert.ToSingle(txtMoveZ.Text)),
					Midget.Events.Object.Transformation.Transformation.Translate);
			}
		}
		private void txtMoveX_Leave(object sender, System.EventArgs e)
		{
			MoveXEdit();
		}
		private void txtMoveY_Leave(object sender, System.EventArgs e)
		{
			MoveYEdit();
		}
		private void txtMoveZ_Leave(object sender, System.EventArgs e)
		{
			MoveZEdit();
		}
		private void txtMoveX_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				MoveXEdit();
		}
		private void txtMoveY_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				MoveYEdit();
		}
		private void txtMoveZ_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				MoveZEdit();
		}

		// scale //
		private void ScaleXEdit()
		{
			if (txtScaleX.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(Convert.ToSingle(txtScaleX.Text), CurrentObject.Scaling.Y, CurrentObject.Scaling.Z),
					Midget.Events.Object.Transformation.Transformation.Scale);
				//CurrentObject.Scale(Convert.ToSingle(txtScaleX.Text), CurrentObject.Scaling.Y, CurrentObject.Scaling.Z);
			}
		}
		private void ScaleYEdit()
		{
			if (txtScaleY.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(CurrentObject.Scaling.X, Convert.ToSingle(txtScaleY.Text), CurrentObject.Scaling.Z),
					Midget.Events.Object.Transformation.Transformation.Scale);
				//CurrentObject.Scale(CurrentObject.Scaling.X, Convert.ToSingle(txtScaleY.Text), CurrentObject.Scaling.Z);
			}
		}
		private void ScaleZEdit()
		{
			if (txtScaleZ.CheckForChange() && CurrentObject != null)
			{
				Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
					new AxisValue(CurrentObject.Scaling.X, CurrentObject.Scaling.Y, Convert.ToSingle(txtScaleZ.Text)),
					Midget.Events.Object.Transformation.Transformation.Scale);
				//CurrentObject.Scale(CurrentObject.Scaling.X, CurrentObject.Scaling.Y, Convert.ToSingle(txtScaleZ.Text));
			}
		}
		private void txtScaleX_Leave(object sender, System.EventArgs e)
		{
			ScaleXEdit();
		}
		private void txtScaleY_Leave(object sender, System.EventArgs e)
		{
			ScaleYEdit();
		}
		private void txtScaleZ_Leave(object sender, System.EventArgs e)
		{
			ScaleZEdit();
		}
		private void txtScaleX_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				ScaleXEdit();
		}
		private void txtScaleY_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				ScaleYEdit();
		}
		private void txtScaleZ_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				ScaleZEdit();
		}
		#endregion

		private void ObjectShelf_Resize(object sender, System.EventArgs e)
		{
			// TODO: get resize redraw working
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
			this.Invalidate(rect, false);
		}

		/// <summary>
		/// The object shelf's currently-selected object (singular)
		/// </summary>
		IObject3D CurrentObject
		{
			get
			{
				return ( (IObject3D)selectedObjects[selectedObjects.Count - 1] );
			}
		}

		private void EventFactory_Transformation(object sender, Midget.Events.Object.MultiObjectEventArgs e)
		{
			FillObjectValues((IObject3D)selectedObjects[selectedObjects.Count - 1]);
		}

		private void EventFactory_SelectAdditionalObject(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Add(e.Object);
			FillObjectValues(e.Object);
		}

		private void EventFactory_DeselectObjects(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{	
			selectedObjects.Remove(e.Object);

			if(selectedObjects.Count != 0)
			{
				FillObjectValues((IObject3D)selectedObjects[selectedObjects.Count - 1]);
			}
			else
			{
				this.Clear();
			}
		}

		
	}
}
