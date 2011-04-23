using System;

using Midget;

namespace MidgetUI
{
	/// <summary>
	/// Summary description for MidgeTreeNode.
	/// </summary>
	public class MidgetTreeNode : System.Windows.Forms.TreeNode
	{
		private IObject3D object3D;
		
		private MidgetTreeNode(){}

		public MidgetTreeNode(IObject3D obj) : base(obj.Name)
		{
			object3D = obj;
		}

		public new string Text
		{
			get { return object3D.Name; }
			set { object3D.Name = value; }
		}

		public IObject3D Object3d
		{
			get { return object3D; }
		}
	}
}
