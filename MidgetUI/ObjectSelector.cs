using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Midget;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for ObjectSelector.
	/// </summary>
	public class ObjectSelector : System.Windows.Forms.UserControl
	{
		private SceneManager sm = SceneManager.Instance;

		private ArrayList selectedObjects;
		private MidgetUI.MidgetTreeView treeView;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ObjectSelector()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			InitObjectTree();
			
			Midget.Events.EventFactory.CreateObject += new Midget.Events.Object.Lifetime.CreateObjectEventHandler(EventFactory_CreateObject);
			Midget.Events.EventFactory.DeleteObject += new Midget.Events.Object.Lifetime.DeleteObjectEventHandler(EventFactory_DeleteObject);
			Midget.Events.EventFactory.Group += new Midget.Events.Object.Relation.GroupEventHandler(EventFactory_Group);
			Midget.Events.EventFactory.ParentChange += new Midget.Events.Object.Relation.ParentChangeEventHandler(EventFactory_ParentChange);
			Midget.Events.EventFactory.Ungroup += new Midget.Events.Object.Relation.UngroupEventHandler(EventFactory_Ungroup);
			Midget.Events.EventFactory.SelectAdditionalObject += new Midget.Events.Object.Selection.SelectAdditionalObjectEventHandler(EventFactory_SelectAdditionalObject);
			Midget.Events.EventFactory.DeselectObjects += new Midget.Events.Object.Selection.DeselectObjectEventHandler(EventFactory_DeselectObjects);
			Midget.Events.EventFactory.OpenScene+=new Midget.Events.User.OpenSceneEventHanlder(EventFactory_OpenScene);
	
			treeView.NodeMoveRequest +=new NodeMoveRequestHandler(treeView_NodeMoveRequest);

			selectedObjects = new ArrayList();
		}
		
		private void InitObjectTree()
		{	
			treeView.Nodes.Clear();

			treeView.Nodes.Add(new MidgetTreeNode(sm.Scene));

			// n2 algorithm, not very good but there are never going to be a large number
			// of objects in a scene, if this slows program change algorithm

			for(int i = 0; i < treeView.Nodes.Count; ++i)
			{
				MidgetTreeNode tempNode = (MidgetTreeNode)treeView.Nodes[i];
				AddChildren(tempNode);
			}
		}

		private void AddChildren(MidgetTreeNode node)
		{
			// add each element in the scene that belongs to this 
			foreach (IObject3D obj in sm.ObjectList)
			{
				// if the node is the object's parent add it to the list
				if(obj.Parent == node.Object3d)
				{
					MidgetTreeNode newNode = new MidgetTreeNode(obj);
					node.Nodes.Add(newNode);
					AddChildren(newNode);
				}
			}
		}

		private MidgetTreeNode FindParentNode(MidgetTreeNode node)
		{
			MidgetTreeNode parent = (MidgetTreeNode)treeView.Nodes[0];
			
			while(parent != null)
			{
				if(node.Object3d.Parent.Equals(parent.Object3d))
					return parent;
				
				parent = (MidgetTreeNode)parent.NextVisibleNode;
			}

			return null;
		}

		private MidgetTreeNode FindNode(IObject3D obj)
		{
			MidgetTreeNode node = (MidgetTreeNode)treeView.Nodes[0];
			
			try
			{
				while(node != null)
				{
					if(node.Object3d.Equals(obj))
						return node;
				
					// try to move first child node
					if(node.Nodes.Count != 0)
					{
						node = (MidgetTreeNode)node.Nodes[0];
					}
						// try to go to next sibling
					else if (node.NextNode != null)
					{
						node =  (MidgetTreeNode)node.NextNode;
					}
					else
					{
						node =  (MidgetTreeNode)node.Parent;

						while(node.NextNode == null)
						{
							// go to parent node
							node =  (MidgetTreeNode)node.Parent;

							// if back at top stop
							if(node == (MidgetTreeNode)treeView.Nodes[0])
								return null;
						}

						node = (MidgetTreeNode)node.NextNode;
					}
				}
			}
			catch
			{
				return null;
			}

			return null;
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
			this.treeView = new MidgetUI.MidgetTreeView();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.AllowDrop = true;
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.ImageIndex = -1;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = -1;
			this.treeView.Size = new System.Drawing.Size(376, 672);
			this.treeView.TabIndex = 0;
			// 
			// ObjectSelector
			// 
			this.Controls.Add(this.treeView);
			this.Name = "ObjectSelector";
			this.Size = new System.Drawing.Size(376, 672);
			this.Click += new System.EventHandler(this.ObjectSelector_Click);
			this.ResumeLayout(false);

		}
		#endregion

		private void EventFactory_CreateObject(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			// add all objects to the scene (treeview)
			treeView.AddNode(new MidgetTreeNode(e.Object), (MidgetTreeNode)treeView.Nodes[0]);
		}

		private void EventFactory_Group(object sender, Midget.Events.Object.Relation.GroupEventArgs e)
		{	
			// add the group
			MidgetTreeNode groupNode = new MidgetTreeNode(e.Group);

			treeView.AddNode(groupNode,FindParentNode(groupNode));

			// add all objects to the scene
			foreach(IObject3D obj in e.Objects)
			{
				treeView.MoveNode(FindNode(obj),groupNode);
			}
		}

		private void EventFactory_ParentChange(object sender, Midget.Events.Object.Relation.ParentChangeEventArgs e)
		{	
			// find the parent node
			MidgetTreeNode newParent = FindNode(e.NewParent);

			// add all objects to the scene
			foreach(IObject3D obj in e.Objects)
			{
				treeView.MoveNode(FindNode(obj),newParent);
			}
		}

		private void EventFactory_Ungroup(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			// move each node to new parent
			MidgetTreeNode groupNode = FindNode(e.Object);
			
			while (groupNode.Nodes.Count != 0)
			{	
				MidgetTreeNode obj = (MidgetTreeNode)groupNode.Nodes[0];
				treeView.MoveNode(FindNode(obj.Object3d),(MidgetTreeNode)groupNode.Parent);
			}
		}

		private void treeView_NodeMoveRequest(object sender, NodeMoveRequestEventArgs e)
		{
			System.Collections.ArrayList tempList = new ArrayList();

			tempList.Add( ((MidgetTreeNode)e.Node).Object3d );

			Midget.Events.EventFactory.Instance.GenerateParentChangeRequestEvent(this, tempList, ((MidgetTreeNode)e.Parent).Object3d);  
		}

		private void EventFactory_DeleteObject(object sender, Midget.Events.Object.MultiObjectEventArgs e)
		{
			foreach(IObject3D obj in e.Objects)
			{
				MidgetTreeNode node = FindNode(obj);

				// if the node hasn't already been deleted
				if(node != null)
				{
					node.Remove();
				}
			}
		}

		private void ObjectSelector_Click(object sender, System.EventArgs e)
		{	
			// empty now
		}

		private void EventFactory_SelectAdditionalObject(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Add(e.Object);

			MidgetTreeNode node = FindNode(e.Object);
			
			if(node != null)
			{
				treeView.SelectedNode = node;
			}

		}

		private void EventFactory_DeselectObjects(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Remove(e.Object);
			
			if(selectedObjects.Count != 0)
			{	
				MidgetTreeNode node = FindNode((IObject3D)selectedObjects[selectedObjects.Count - 1]);
				
				if(node != null)
				{
					treeView.SelectedNode = node;
				}
			}		
		}

		private void EventFactory_OpenScene(object sender, EventArgs e)
		{
			InitObjectTree();

		}
	}
}
