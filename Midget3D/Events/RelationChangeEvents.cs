using System;
using System.Collections;

namespace Midget.Events.Object.Relation
{
	public delegate void GroupEventHandler(object sender, GroupEventArgs e);
	public delegate void GroupRequestEventHandler(object sender, MultiObjectEventArgs e);

	public delegate void UngroupEventHandler(object sender, SingleObjectEventArgs e);
	public delegate void UngroupRequestEventHandler(object sender, SingleObjectEventArgs e);

	public delegate void ParentChangeEventHandler(object sender, ParentChangeEventArgs e);
	public delegate void ParentChangeRequestEventHandler(object sender, ParentChangeEventArgs e);


	/// <summary>
	/// Object grouping Event notification event
	/// </summary>
	public class GroupEventArgs : MultiObjectEventArgs
	{
		private readonly IObject3D group;

		public GroupEventArgs( ArrayList objects, IObject3D initGroup) : base(objects)
		{
			group = initGroup;
		}
		
		/// <summary>
		/// Newly Formed Group
		/// </summary>
		public IObject3D Group
		{
			get { return group; }
		}
	}

	/// <summary>
	/// Parent object changed event
	/// </summary>
	public class ParentChangeEventArgs : MultiObjectEventArgs
	{
		private readonly IObject3D newParent;

		public ParentChangeEventArgs( ArrayList objects, IObject3D initNewParent) : base (objects)
		{
			newParent = initNewParent;
		}

		public IObject3D NewParent
		{
			get { return newParent; }
		}
	}
}
