using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for MidgetTreeView.
	/// </summary>
	public class MidgetTreeView : System.Windows.Forms.TreeView
	{	
		/// <summary>
		/// EventHandler to inform Viewports that their scene is out of date, and they need
		/// to request to be rendered
		/// </summary>
		public event NodeMoveRequestHandler NodeMoveRequest;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MidgetTreeView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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


		protected override void OnItemDrag(ItemDragEventArgs e)
		{
			DoDragDrop(e.Item, DragDropEffects.Move);
		}

		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			drgevent.Effect = DragDropEffects.Move;
		}

		public void AddNode(MidgetTreeNode node,MidgetTreeNode parent)
		{
			parent.Nodes.Add(node);
		}

		public void RemoveNode(MidgetTreeNode node)
		{	
			node.Remove();
		}

		public void MoveNode(MidgetTreeNode node, MidgetTreeNode newParentNode)
		{
			// retrieve old parent
			MidgetTreeNode oldParentNode = (MidgetTreeNode)node.Parent;

			// make sure not just copying to the parent or reorganizing within the parent
			if(!newParentNode.Equals(node) && !newParentNode.Equals(node.Parent) && oldParentNode != null)
			{

				// if child is becoming the parent of it's parent
				foreach (TreeNode childNode in node.Nodes)
				{
					if(childNode.Equals(newParentNode))
					{	
						// clone old child and adjust it's parent
						newParentNode = (MidgetTreeNode)childNode.Clone();
						oldParentNode.Nodes.Add(newParentNode);
							
						oldParentNode.ExpandAll();

						// removce old child
						childNode.Remove();
						break;
					}
				}
				
				// remove node from it's old location
				oldParentNode.Nodes.Remove(node);

				// add the node to it's new parent
				newParentNode.Nodes.Add(node);
				newParentNode.ExpandAll();
			}
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			string [] text = drgevent.Data.GetFormats();

			// if the object being dragged can become a treenode
			if(drgevent.Data.GetDataPresent("MidgetUI.MidgetTreeNode",false))
			{
				Point pt = PointToClient(new Point(drgevent.X,drgevent.Y));
				
				// retrieve the new parent node
				MidgetTreeNode newParentNode = (MidgetTreeNode)GetNodeAt(pt);
				
				// if you have missed the list drop it under the root node
				if(newParentNode == null)
				{
					newParentNode = ((MidgetTreeNode)this.Nodes[0]);
				}
				
				// get newNode location
				MidgetTreeNode node = (MidgetUI.MidgetTreeNode)drgevent.Data.GetData("MidgetUI.MidgetTreeNode");
				
				// retrieve old parent
				MidgetTreeNode oldParentNode = (MidgetTreeNode)node.Parent;

				// make sure not just copying to the parent or reorganizing within the parent
				if(!newParentNode.Equals(node) && !newParentNode.Equals(node.Parent) && oldParentNode != null)
				{
					// Issue update request
					if(NodeMoveRequest != null)
					{
						NodeMoveRequest(this, new NodeMoveRequestEventArgs(node,newParentNode));
					}
						// no one is listening just do it
					else
					{
						MoveNode(node,newParentNode);
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

	public delegate void NodeMoveRequestHandler(object sender, NodeMoveRequestEventArgs e);

	/// <summary>
	/// Event arguments so the treeview can ask for permission to complete the move
	/// </summary>
	public class NodeMoveRequestEventArgs : EventArgs
	{
		private readonly TreeNode node;
		private readonly TreeNode parent;
  
		public NodeMoveRequestEventArgs(TreeNode theNode, TreeNode newParent)
		{
			node = theNode;
			parent = newParent;
		}
		public TreeNode Parent
		{
			get { return parent; }
		}

		public TreeNode Node
		{
			get { return node; }
		}
	}
}
