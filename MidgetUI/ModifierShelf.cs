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
	/// Summary description for ModifierShelf.
	/// </summary>
	public class ModifierShelf : System.Windows.Forms.UserControl
	{
		private Salamander.Windows.Forms.CollapsiblePanelBar panelBar;
		private Salamander.Windows.Forms.CollapsiblePanel mainPanel;
		private Salamander.Windows.Forms.CollapsiblePanel parameterPanel;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox lstModifiers;
		private System.Windows.Forms.Button btnRemove;

		private ArrayList selectedObjects;

		public ModifierShelf()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			selectedObjects = new ArrayList();

			Midget.Events.EventFactory.Transformation+=new Midget.Events.Object.Transformation.TransformationEventHandler(EventFactory_Transformation);
			Midget.Events.EventFactory.SelectAdditionalObject+=new Midget.Events.Object.Selection.SelectAdditionalObjectEventHandler(EventFactory_SelectAdditionalObject);
			Midget.Events.EventFactory.DeselectObjects+=new Midget.Events.Object.Selection.DeselectObjectEventHandler(EventFactory_DeselectObjects);
			Midget.Events.EventFactory.AddDynamic +=new Midget.Events.Object.Transformation.AddDynamicEventHandler(EventFactory_AddDynamic);
			Midget.Events.EventFactory.RemoveDynamic +=new Midget.Events.Object.Transformation.RemoveDynamicEventHandler(EventFactory_RemoveDynamic);
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
			this.btnRemove = new System.Windows.Forms.Button();
			this.lstModifiers = new System.Windows.Forms.ListBox();
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
			this.panelBar.Size = new System.Drawing.Size(240, 512);
			this.panelBar.Spacing = 8;
			this.panelBar.TabIndex = 0;
			this.panelBar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBar_Paint);
			// 
			// parameterPanel
			// 
			this.parameterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.parameterPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.parameterPanel.Controls.Add(this.propertyGrid);
			this.parameterPanel.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.parameterPanel.Image = null;
			this.parameterPanel.Location = new System.Drawing.Point(8, 208);
			this.parameterPanel.Name = "parameterPanel";
			this.parameterPanel.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.parameterPanel.Size = new System.Drawing.Size(224, 296);
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
			this.propertyGrid.Size = new System.Drawing.Size(208, 256);
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
			this.mainPanel.Controls.Add(this.btnRemove);
			this.mainPanel.Controls.Add(this.lstModifiers);
			this.mainPanel.EndColour = System.Drawing.SystemColors.InactiveCaptionText;
			this.mainPanel.Image = null;
			this.mainPanel.Location = new System.Drawing.Point(8, 8);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.PanelState = Salamander.Windows.Forms.PanelState.Expanded;
			this.mainPanel.Size = new System.Drawing.Size(224, 192);
			this.mainPanel.StartColour = System.Drawing.SystemColors.Highlight;
			this.mainPanel.TabIndex = 0;
			this.mainPanel.TitleFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mainPanel.TitleFontColour = System.Drawing.Color.White;
			this.mainPanel.TitleText = "Modifiers";
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(40, 160);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(144, 24);
			this.btnRemove.TabIndex = 2;
			this.btnRemove.Text = "&Remove";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// lstModifiers
			// 
			this.lstModifiers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstModifiers.Location = new System.Drawing.Point(8, 32);
			this.lstModifiers.Name = "lstModifiers";
			this.lstModifiers.Size = new System.Drawing.Size(208, 119);
			this.lstModifiers.TabIndex = 1;
			this.lstModifiers.SelectedIndexChanged += new System.EventHandler(this.lstModifiers_SelectedIndexChanged);
			// 
			// ModifierShelf
			// 
			this.Controls.Add(this.panelBar);
			this.Name = "ModifierShelf";
			this.Size = new System.Drawing.Size(240, 512);
			this.Resize += new System.EventHandler(this.ModifierShelf_Resize);
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
			lstModifiers.Items.Clear();	
			propertyGrid.SelectedObject = null;
		}

		/// <summary>
		/// Get all values from an object and display them.
		/// Customize the display for this type of object.
		/// </summary>
		/// <param name="obj">The object to get manipulation data from</param>
		private void FillObjectValues(IObject3D obj)
		{
			// fill in this object's modifiers
			if (obj != null)
			{
				lstModifiers.Items.Clear();

				foreach (IDynamic dynamic in obj.DynamicsList)
				{
					lstModifiers.Items.Add(dynamic);
				}
			}
		}

		private void ModifierShelf_Resize(object sender, System.EventArgs e)
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

		private void lstModifiers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			propertyGrid.SelectedObject = lstModifiers.Items[lstModifiers.SelectedIndex];
		}

		//////////
		/// TODO: add event handler for any added dynamics
		/// //////

		private void EventFactory_AddDynamic(object sender, Midget.Events.Object.Transformation.DynamicEventArgs e)
		{
			FillObjectValues(e.Object);
		}

		private void EventFactory_RemoveDynamic(object sender, Midget.Events.Object.Transformation.DynamicEventArgs e)
		{	
			propertyGrid.SelectedObject = null;
			FillObjectValues(e.Object);
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if(lstModifiers.SelectedItem != null)
			{
				Midget.Events.EventFactory.Instance.GenerateRemoveDynamicRequestEvent(this,(IObject3D)selectedObjects[selectedObjects.Count - 1],(IDynamic)lstModifiers.SelectedItem);
			}
		}
	}
}
