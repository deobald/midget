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
	/// The user control shelf used to manipulate objects, cameras, and lights
	/// </summary>
	public class MagicShelf : System.Windows.Forms.UserControl
	{
		private IObject3D selectedObject;

		private System.Windows.Forms.ComboBox cboObjectSelector;
		private System.Windows.Forms.GroupBox grpObjectManipulation;
		private System.Windows.Forms.TextBox txtRotateX;
		private System.Windows.Forms.TextBox txtRotateY;
		private System.Windows.Forms.TextBox txtRotateZ;
		private System.Windows.Forms.Label lblRotate;
		private System.Windows.Forms.Label lblRotateX;
		private System.Windows.Forms.Label lblRotateY;
		private System.Windows.Forms.Label lblRotateZ;
		private System.Windows.Forms.Label lblMoveZ;
		private System.Windows.Forms.Label lblMoveY;
		private System.Windows.Forms.Label lblMoveX;
		private System.Windows.Forms.Label lblMove;
		private System.Windows.Forms.TextBox txtScaleZ;
		private System.Windows.Forms.TextBox txtScaleY;
		private System.Windows.Forms.TextBox txtScaleX;
		private System.Windows.Forms.Label lblScaleZ;
		private System.Windows.Forms.Label lblScaleY;
		private System.Windows.Forms.Label lblScaleX;
		private System.Windows.Forms.Label lblScale;
		private System.Windows.Forms.TextBox txtMoveZ;
		private System.Windows.Forms.TextBox txtMoveY;
		private System.Windows.Forms.TextBox txtMoveX;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MagicShelf()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// init object creation/selection delegate
			MidgetEvent.ObjectEventFactory.Instance.CreateObject += new MidgetEvent.CreateObjectEventHandler(this.CreateObject);
			MidgetEvent.ObjectEventFactory.Instance.SelectObject += new MidgetEvent.SelectObjectEventHandler(this.SelectObject);
		}

		private void CreateObject(object sender, MidgetEvent.CreateObjectEventArgs e)
		{
			// make sure list is not empty, so program doesn't crash
			cboObjectSelector_DropDown(sender, new System.EventArgs());

			cboObjectSelector.SelectedItem = cboObjectSelector.Items[cboObjectSelector.Items.Count - 1];
		}	

		private void SelectObject(object sender, MidgetEvent.SelectObjectEventArgs e)
		{	

			selectedObject = e.SelectedObject;

			cboObjectSelector.Text = e.SelectedObject.Name;
			
			txtRotateX.Text = selectedObject.Rotation.X.ToString();
			txtRotateY.Text = selectedObject.Rotation.Y.ToString();
			txtRotateZ.Text = selectedObject.Rotation.Z.ToString();

			txtMoveX.Text = selectedObject.Translation.X.ToString();
			txtMoveY.Text = selectedObject.Translation.Y.ToString();
			txtMoveZ.Text = selectedObject.Translation.Z.ToString();

			txtScaleX.Text = selectedObject.Scaling.X.ToString();
			txtScaleY.Text = selectedObject.Scaling.Y.ToString();
			txtScaleZ.Text = selectedObject.Scaling.Z.ToString();
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
			this.cboObjectSelector = new System.Windows.Forms.ComboBox();
			this.grpObjectManipulation = new System.Windows.Forms.GroupBox();
			this.lblRotateZ = new System.Windows.Forms.Label();
			this.lblRotateY = new System.Windows.Forms.Label();
			this.lblRotateX = new System.Windows.Forms.Label();
			this.lblRotate = new System.Windows.Forms.Label();
			this.txtRotateZ = new System.Windows.Forms.TextBox();
			this.txtRotateY = new System.Windows.Forms.TextBox();
			this.txtRotateX = new System.Windows.Forms.TextBox();
			this.lblMoveZ = new System.Windows.Forms.Label();
			this.lblMoveY = new System.Windows.Forms.Label();
			this.lblMoveX = new System.Windows.Forms.Label();
			this.lblMove = new System.Windows.Forms.Label();
			this.txtScaleZ = new System.Windows.Forms.TextBox();
			this.txtScaleY = new System.Windows.Forms.TextBox();
			this.txtScaleX = new System.Windows.Forms.TextBox();
			this.lblScaleZ = new System.Windows.Forms.Label();
			this.lblScaleY = new System.Windows.Forms.Label();
			this.lblScaleX = new System.Windows.Forms.Label();
			this.lblScale = new System.Windows.Forms.Label();
			this.txtMoveZ = new System.Windows.Forms.TextBox();
			this.txtMoveY = new System.Windows.Forms.TextBox();
			this.txtMoveX = new System.Windows.Forms.TextBox();
			this.grpObjectManipulation.SuspendLayout();
			this.SuspendLayout();
			// 
			// cboObjectSelector
			// 
			this.cboObjectSelector.Location = new System.Drawing.Point(8, 8);
			this.cboObjectSelector.Name = "cboObjectSelector";
			this.cboObjectSelector.Size = new System.Drawing.Size(192, 21);
			this.cboObjectSelector.TabIndex = 0;
			this.cboObjectSelector.DropDown += new System.EventHandler(this.cboObjectSelector_DropDown);
			this.cboObjectSelector.SelectedIndexChanged += new System.EventHandler(this.cboObjectSelector_SelectedIndexChanged);
			// 
			// grpObjectManipulation
			// 
			this.grpObjectManipulation.Controls.Add(this.lblScaleZ);
			this.grpObjectManipulation.Controls.Add(this.lblScaleY);
			this.grpObjectManipulation.Controls.Add(this.lblScaleX);
			this.grpObjectManipulation.Controls.Add(this.lblScale);
			this.grpObjectManipulation.Controls.Add(this.txtMoveZ);
			this.grpObjectManipulation.Controls.Add(this.txtMoveY);
			this.grpObjectManipulation.Controls.Add(this.txtMoveX);
			this.grpObjectManipulation.Controls.Add(this.lblMoveZ);
			this.grpObjectManipulation.Controls.Add(this.lblMoveY);
			this.grpObjectManipulation.Controls.Add(this.lblMoveX);
			this.grpObjectManipulation.Controls.Add(this.lblMove);
			this.grpObjectManipulation.Controls.Add(this.txtScaleZ);
			this.grpObjectManipulation.Controls.Add(this.txtScaleY);
			this.grpObjectManipulation.Controls.Add(this.txtScaleX);
			this.grpObjectManipulation.Controls.Add(this.lblRotateZ);
			this.grpObjectManipulation.Controls.Add(this.lblRotateY);
			this.grpObjectManipulation.Controls.Add(this.lblRotateX);
			this.grpObjectManipulation.Controls.Add(this.lblRotate);
			this.grpObjectManipulation.Controls.Add(this.txtRotateZ);
			this.grpObjectManipulation.Controls.Add(this.txtRotateY);
			this.grpObjectManipulation.Controls.Add(this.txtRotateX);
			this.grpObjectManipulation.Location = new System.Drawing.Point(8, 40);
			this.grpObjectManipulation.Name = "grpObjectManipulation";
			this.grpObjectManipulation.Size = new System.Drawing.Size(192, 504);
			this.grpObjectManipulation.TabIndex = 1;
			this.grpObjectManipulation.TabStop = false;
			this.grpObjectManipulation.Text = "Object Manipulation";
			// 
			// lblRotateZ
			// 
			this.lblRotateZ.Location = new System.Drawing.Point(16, 88);
			this.lblRotateZ.Name = "lblRotateZ";
			this.lblRotateZ.Size = new System.Drawing.Size(16, 16);
			this.lblRotateZ.TabIndex = 8;
			this.lblRotateZ.Text = "Z:";
			// 
			// lblRotateY
			// 
			this.lblRotateY.Location = new System.Drawing.Point(16, 64);
			this.lblRotateY.Name = "lblRotateY";
			this.lblRotateY.Size = new System.Drawing.Size(16, 16);
			this.lblRotateY.TabIndex = 7;
			this.lblRotateY.Text = "Y:";
			// 
			// lblRotateX
			// 
			this.lblRotateX.Location = new System.Drawing.Point(16, 40);
			this.lblRotateX.Name = "lblRotateX";
			this.lblRotateX.Size = new System.Drawing.Size(16, 16);
			this.lblRotateX.TabIndex = 6;
			this.lblRotateX.Text = "X:";
			// 
			// lblRotate
			// 
			this.lblRotate.Location = new System.Drawing.Point(16, 24);
			this.lblRotate.Name = "lblRotate";
			this.lblRotate.Size = new System.Drawing.Size(160, 16);
			this.lblRotate.TabIndex = 5;
			this.lblRotate.Text = "Rotate:";
			// 
			// txtRotateZ
			// 
			this.txtRotateZ.Location = new System.Drawing.Point(32, 88);
			this.txtRotateZ.Name = "txtRotateZ";
			this.txtRotateZ.Size = new System.Drawing.Size(136, 20);
			this.txtRotateZ.TabIndex = 4;
			this.txtRotateZ.Text = "";
			this.txtRotateZ.Leave += new System.EventHandler(this.txtRotate_Leave);
			this.txtRotateZ.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRotate_KeyUp);
			// 
			// txtRotateY
			// 
			this.txtRotateY.Location = new System.Drawing.Point(32, 64);
			this.txtRotateY.Name = "txtRotateY";
			this.txtRotateY.Size = new System.Drawing.Size(136, 20);
			this.txtRotateY.TabIndex = 3;
			this.txtRotateY.Text = "";
			this.txtRotateY.Leave += new System.EventHandler(this.txtRotate_Leave);
			this.txtRotateY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRotate_KeyUp);
			// 
			// txtRotateX
			// 
			this.txtRotateX.Location = new System.Drawing.Point(32, 40);
			this.txtRotateX.Name = "txtRotateX";
			this.txtRotateX.Size = new System.Drawing.Size(136, 20);
			this.txtRotateX.TabIndex = 2;
			this.txtRotateX.Text = "";
			this.txtRotateX.Leave += new System.EventHandler(this.txtRotate_Leave);
			this.txtRotateX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRotate_KeyUp);
			// 
			// lblMoveZ
			// 
			this.lblMoveZ.Location = new System.Drawing.Point(16, 192);
			this.lblMoveZ.Name = "lblMoveZ";
			this.lblMoveZ.Size = new System.Drawing.Size(16, 16);
			this.lblMoveZ.TabIndex = 15;
			this.lblMoveZ.Text = "Z:";
			// 
			// lblMoveY
			// 
			this.lblMoveY.Location = new System.Drawing.Point(16, 168);
			this.lblMoveY.Name = "lblMoveY";
			this.lblMoveY.Size = new System.Drawing.Size(16, 16);
			this.lblMoveY.TabIndex = 14;
			this.lblMoveY.Text = "Y:";
			// 
			// lblMoveX
			// 
			this.lblMoveX.Location = new System.Drawing.Point(16, 144);
			this.lblMoveX.Name = "lblMoveX";
			this.lblMoveX.Size = new System.Drawing.Size(16, 16);
			this.lblMoveX.TabIndex = 13;
			this.lblMoveX.Text = "X:";
			// 
			// lblMove
			// 
			this.lblMove.Location = new System.Drawing.Point(16, 128);
			this.lblMove.Name = "lblMove";
			this.lblMove.Size = new System.Drawing.Size(160, 16);
			this.lblMove.TabIndex = 12;
			this.lblMove.Text = "Move:";
			// 
			// txtScaleZ
			// 
			this.txtScaleZ.Location = new System.Drawing.Point(32, 296);
			this.txtScaleZ.Name = "txtScaleZ";
			this.txtScaleZ.Size = new System.Drawing.Size(136, 20);
			this.txtScaleZ.TabIndex = 11;
			this.txtScaleZ.Text = "";
			this.txtScaleZ.Leave += new System.EventHandler(this.txtScale_Leave);
			this.txtScaleZ.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScale_KeyUp);
			// 
			// txtScaleY
			// 
			this.txtScaleY.Location = new System.Drawing.Point(32, 272);
			this.txtScaleY.Name = "txtScaleY";
			this.txtScaleY.Size = new System.Drawing.Size(136, 20);
			this.txtScaleY.TabIndex = 10;
			this.txtScaleY.Text = "";
			this.txtScaleY.Leave += new System.EventHandler(this.txtScale_Leave);
			this.txtScaleY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScale_KeyUp);
			// 
			// txtScaleX
			// 
			this.txtScaleX.Location = new System.Drawing.Point(32, 248);
			this.txtScaleX.Name = "txtScaleX";
			this.txtScaleX.Size = new System.Drawing.Size(136, 20);
			this.txtScaleX.TabIndex = 9;
			this.txtScaleX.Text = "";
			this.txtScaleX.Leave += new System.EventHandler(this.txtScale_Leave);
			this.txtScaleX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScale_KeyUp);
			// 
			// lblScaleZ
			// 
			this.lblScaleZ.Location = new System.Drawing.Point(16, 296);
			this.lblScaleZ.Name = "lblScaleZ";
			this.lblScaleZ.Size = new System.Drawing.Size(16, 16);
			this.lblScaleZ.TabIndex = 22;
			this.lblScaleZ.Text = "Z:";
			// 
			// lblScaleY
			// 
			this.lblScaleY.Location = new System.Drawing.Point(16, 272);
			this.lblScaleY.Name = "lblScaleY";
			this.lblScaleY.Size = new System.Drawing.Size(16, 16);
			this.lblScaleY.TabIndex = 21;
			this.lblScaleY.Text = "Y:";
			// 
			// lblScaleX
			// 
			this.lblScaleX.Location = new System.Drawing.Point(16, 248);
			this.lblScaleX.Name = "lblScaleX";
			this.lblScaleX.Size = new System.Drawing.Size(16, 16);
			this.lblScaleX.TabIndex = 20;
			this.lblScaleX.Text = "X:";
			// 
			// lblScale
			// 
			this.lblScale.Location = new System.Drawing.Point(16, 232);
			this.lblScale.Name = "lblScale";
			this.lblScale.Size = new System.Drawing.Size(160, 16);
			this.lblScale.TabIndex = 19;
			this.lblScale.Text = "Scale:";
			// 
			// txtMoveZ
			// 
			this.txtMoveZ.Location = new System.Drawing.Point(32, 192);
			this.txtMoveZ.Name = "txtMoveZ";
			this.txtMoveZ.Size = new System.Drawing.Size(136, 20);
			this.txtMoveZ.TabIndex = 18;
			this.txtMoveZ.Text = "";
			this.txtMoveZ.Leave += new System.EventHandler(this.txtMove_Leave);
			this.txtMoveZ.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMove_KeyUp);
			// 
			// txtMoveY
			// 
			this.txtMoveY.Location = new System.Drawing.Point(32, 168);
			this.txtMoveY.Name = "txtMoveY";
			this.txtMoveY.Size = new System.Drawing.Size(136, 20);
			this.txtMoveY.TabIndex = 17;
			this.txtMoveY.Text = "";
			this.txtMoveY.Leave += new System.EventHandler(this.txtMove_Leave);
			this.txtMoveY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMove_KeyUp);
			// 
			// txtMoveX
			// 
			this.txtMoveX.Location = new System.Drawing.Point(32, 144);
			this.txtMoveX.Name = "txtMoveX";
			this.txtMoveX.Size = new System.Drawing.Size(136, 20);
			this.txtMoveX.TabIndex = 16;
			this.txtMoveX.Text = "";
			this.txtMoveX.Leave += new System.EventHandler(this.txtMove_Leave);
			this.txtMoveX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMove_KeyUp);
			// 
			// MagicShelf
			// 
			this.Controls.Add(this.grpObjectManipulation);
			this.Controls.Add(this.cboObjectSelector);
			this.Name = "MagicShelf";
			this.Size = new System.Drawing.Size(208, 600);
			this.grpObjectManipulation.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void cboObjectSelector_DropDown(object sender, System.EventArgs e)
		{
			cboObjectSelector.Items.Clear();

			// add each object in our list to the combobox, by name
			foreach (IObject3D obj in DeviceManager.Instance.ObjectList)
			{
				cboObjectSelector.Items.Add( ((MeshObject)obj).Name );	
			}
		}

		/// <summary>
		/// A new item was chosen in the object selection combobox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboObjectSelector_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// cycle the objects in the system
			foreach (IObject3D obj in DeviceManager.Instance.ObjectList)
			{
				// if a name matches the one selected, fire the object-selection delegate
				if ( ((MeshObject)obj).Name == cboObjectSelector.Text )
				{
					MidgetEvent.ObjectEventFactory.Instance.CreateSelectEvent(obj);
					break;
				}
			}
		}

		private void MoveObject()
		{
			selectedObject.Translate(Convert.ToSingle(txtMoveX.Text), 
				Convert.ToSingle(txtMoveY.Text), 
				Convert.ToSingle(txtMoveZ.Text));
		}

		private void RotateObject()
		{
			selectedObject.Rotate(Convert.ToSingle(txtRotateX.Text), 
				Convert.ToSingle(txtRotateY.Text), 
				Convert.ToSingle(txtRotateZ.Text));
		}

		private void ScaleObject()
		{
			selectedObject.Scale(Convert.ToSingle(txtScaleX.Text), 
				Convert.ToSingle(txtScaleY.Text), 
				Convert.ToSingle(txtScaleZ.Text));
		}

		private void txtMove_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				MoveObject();
			}
		}

		private void txtRotate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				RotateObject();
			}
		}

		private void txtScale_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				ScaleObject();
			}
		}

		private void txtRotate_Leave(object sender, System.EventArgs e)
		{
			RotateObject();
		}

		private void txtMove_Leave(object sender, System.EventArgs e)
		{
			MoveObject();
		}

		private void txtScale_Leave(object sender, System.EventArgs e)
		{
			ScaleObject();
		}
	}
}
