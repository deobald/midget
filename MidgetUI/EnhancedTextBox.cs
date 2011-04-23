using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for EnhancedTextBox.
	/// </summary>
	public class EnhancedTextBox : System.Windows.Forms.TextBox
	{
		private string oldText;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EnhancedTextBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
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
			components = new System.ComponentModel.Container();
		}
		#endregion

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter (e);
			
			oldText = this.Text;
		}
		

		/// <summary>
		/// Check to see if the text in the text box has actually changed
		/// </summary>
		public bool CheckForChange()
		{
			if(oldText != this.Text && this.Text != "")
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Sets text in textBox so text is set as the pre-changed text
		/// </summary>
		/// <param name="newText">text box text</param>
		public void SetText(string newText)
		{
			oldText = newText;
			this.Text = newText;
		}

		public string OldText
		{
			get { return oldText; }
		}
	}
}
