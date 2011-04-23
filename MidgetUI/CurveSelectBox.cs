///////////////////////////////////////////////////////////////////
//
//	Note: This file used and modified by the Midget3D team with the 
//		  author's permission. Original source code may be found at:
//
//	http://www.reflectionit.nl/
//
///////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Midget;

namespace MidgetUI
{
	/// <summary>
	/// The CurveSelectBox class is used to show a prompt in a dialog box using the static method Show().
	/// </summary>
	/// <remarks>
	/// Copyright © 2003 Reflection IT
	/// (Modified by the Midget3D team with author's permission.)
	/// </remarks>
	public class CurveSelectBox : System.Windows.Forms.Form
	{
		protected System.Windows.Forms.Button buttonOK;
		protected System.Windows.Forms.Button buttonCancel;
		protected System.Windows.Forms.Label labelPrompt;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox lstCurves;

		/// <summary>
		/// Delegate used to validate the object
		/// </summary>
		private CurveSelectBoxValidatingHandler _validator;

		private CurveSelectBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// fill the listbox with all of our curves
			foreach (IObject3D obj in SceneManager.Instance.ObjectList)
			{
				if (obj is Curve)
				{
					lstCurves.Items.Add(obj.Name);
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelPrompt = new System.Windows.Forms.Label();
			this.lstCurves = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(16, 200);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(72, 23);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.CausesValidation = false;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(96, 200);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelPrompt
			// 
			this.labelPrompt.AutoSize = true;
			this.labelPrompt.Location = new System.Drawing.Point(15, 15);
			this.labelPrompt.Name = "labelPrompt";
			this.labelPrompt.Size = new System.Drawing.Size(39, 16);
			this.labelPrompt.TabIndex = 0;
			this.labelPrompt.Text = "prompt";
			// 
			// lstCurves
			// 
			this.lstCurves.Location = new System.Drawing.Point(16, 32);
			this.lstCurves.Name = "lstCurves";
			this.lstCurves.Size = new System.Drawing.Size(152, 160);
			this.lstCurves.TabIndex = 4;
			// 
			// CurveSelectBox
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(186, 248);
			this.ControlBox = false;
			this.Controls.Add(this.lstCurves);
			this.Controls.Add(this.labelPrompt);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CurveSelectBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Title";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonCancel_Click(object sender, System.EventArgs e) {
			this.Validator = null;
			this.Close();
		}

		private void buttonOK_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		/// <summary>
		/// Displays a prompt in a dialog box, waits for the user to input text or click a button.
		/// </summary>
		/// <param name="prompt">String expression displayed as the message in the dialog box</param>
		/// <param name="title">String expression displayed in the title bar of the dialog box</param>
		/// <param name="defaultResponse">String expression displayed in the text box as the default response</param>
		/// <param name="validator">Delegate used to validate the text</param>
		/// <param name="xpos">Numeric expression that specifies the distance of the left edge of the dialog box from the left edge of the screen.</param>
		/// <param name="ypos">Numeric expression that specifies the distance of the upper edge of the dialog box from the top of the screen</param>
		/// <returns>An CurveSelectBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
		public static CurveSelectBoxResult Show(string prompt, string title, string defaultResponse, int xpos, int ypos)
		{
			using (CurveSelectBox form = new CurveSelectBox()) {
				form.labelPrompt.Text = prompt;
				form.Text = title;
				form.lstCurves.Text = defaultResponse;
				if (xpos >= 0 && ypos >= 0) {
					form.StartPosition = FormStartPosition.Manual;
					form.Left = xpos;
					form.Top = ypos;
				}
				//form.Validator = validator;

				DialogResult result = form.ShowDialog();

				CurveSelectBoxResult retval = new CurveSelectBoxResult();
				if (result == DialogResult.OK) {
					retval.Text = form.lstCurves.Text;
					if (retval.Text != null && retval.Text.Trim() != "")
					{
						foreach (IObject3D obj in SceneManager.Instance.ObjectList)
						{
							if (obj.Name == retval.Text)
							{
								retval.SelectedCurve = (Midget.Curve)obj;
								break;
							}
						}
					}
					retval.OK = true;
				}
				return retval;
			}
		}

		/// <summary>
		/// Displays a prompt in a dialog box, waits for the user to input text or click a button.
		/// </summary>
		/// <param name="prompt">String expression displayed as the message in the dialog box</param>
		/// <param name="title">String expression displayed in the title bar of the dialog box</param>
		/// <param name="defaultResponse">String expression displayed in the text box as the default response</param>
		/// <param name="validator">Delegate used to validate the text</param>
		/// <returns>An CurveSelectBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
		public static CurveSelectBoxResult Show(string prompt, string title, string defaultText) {
			return Show(prompt, title, defaultText, -1, -1);
		}

		protected CurveSelectBoxValidatingHandler Validator {
			get {
				return (this._validator);
			}
			set {
				this._validator = value;
			}
		}
	}

	/// <summary>
	/// Class used to store the result of an CurveSelectBox.Show message.
	/// </summary>
	public class CurveSelectBoxResult {
		public bool OK;
		public string Text;
		public Midget.Curve SelectedCurve;
	}

	/// <summary>
	/// EventArgs used to Validate an CurveSelectBox
	/// </summary>
	public class CurveSelectBoxValidatingArgs : EventArgs {
		public string Text;
		public string Message;
		public bool Cancel;
	}

	/// <summary>
	/// Delegate used to Validate an CurveSelectBox
	/// </summary>
	public delegate void CurveSelectBoxValidatingHandler(object sender, CurveSelectBoxValidatingArgs e);

}
