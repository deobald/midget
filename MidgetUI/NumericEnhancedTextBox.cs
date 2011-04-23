using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MidgetUI
{
	
	public enum TextBoxType { Integer, Float };

	/// <summary>
	/// Summary description for NumberEnhancedTextBox.
	/// </summary>
	public class NumberEnhancedTextBox : MidgetUI.EnhancedTextBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TextBoxType boxType;
		private bool negativeAllowed;

		private string prevText ="";

		public NumberEnhancedTextBox()
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
		
		[Browsable(true)]
		public TextBoxType TextBoxType
		{
			get { return boxType; }
			set { boxType = value; }
		}

		[Browsable(true)]
		public bool NegativeAllowed
		{
			get { return negativeAllowed; }
			set { negativeAllowed = value; }
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{	
			
			/*if(boxType == TextBoxType.Integer)
			{
				// if the user entered a negative sign
				if(e.KeyChar == '-' && negativeAllowed && this.Text.IndexOf("-") == -1)
				{
					base.OnKeyPress (e);
				}
				else if('0' <= e.KeyChar && e.KeyChar <= '9')
				{
					base.OnKeyPress (e);
				}
			}
			else if (boxType == TextBoxType.Float)
			{

			
			}*/

			prevText = this.Text;
			base.OnKeyPress (e);
		}

		protected override void OnTextChanged(EventArgs e)
		{	

			base.OnTextChanged (e);

			// allow negative
			if (this.Text == "-")
			{
				return;
			}

			if (boxType == TextBoxType.Integer)
			{
				// if negative sign is allowed make sure there is only one
				if( negativeAllowed)
				{
					if (this.Text != "-" && this.Text != "")
					{
														   
						try
						{
							Convert.ToInt32(this.Text);
						}
						catch
						{
							this.Text = prevText;
						}
					}
				}

					// if negative is not allowed and there is one
				else 
				{
					if(this.Text != "")
					{

						try
						{
							Convert.ToUInt32(this.Text);
						}
						catch
						{
							this.Text = prevText;
						}
					}
				}
			}
			else if (boxType == TextBoxType.Float)
			{
				if (this.Text != "-" && this.Text != "" && this.Text != "-." && this.Text != ".")
				{
					try
					{
						Convert.ToSingle(this.Text);
					}
					catch
					{
						this.Text = prevText;
					}
				}
			}
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
		

	}
}
