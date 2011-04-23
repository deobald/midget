using System;
using System.Collections;

namespace Midget.Events.Object.Transformation
{	
	public delegate void TransformationEventHandler(object sender, MultiObjectEventArgs e);
	public delegate void TransformationRequestEventHandler(object sender, TransformationRequestEventArgs e);

	public delegate void AddDynamicEventHandler(object sender, DynamicEventArgs e);
	public delegate void AddDynamicRequestEventHandler(object sender, DynamicEventArgs e);

	public delegate void RemoveDynamicEventHandler(object sender, DynamicEventArgs e);
	public delegate void RemoveDynamicRequestEventHandler(object sender, DynamicEventArgs e);

	public enum Transformation { Translate, Rotate, Scale, PivotPoint }

	/// <summary>
	/// class for any events related to requesting changes to the relationship between objects
	/// </summary>
	public class TransformationRequestEventArgs : MultiObjectEventArgs
	{	
		private readonly AxisValue position;
		private readonly Transformation transType;

		public TransformationRequestEventArgs ( ArrayList objects, AxisValue initPosition, Transformation transformation) : base(objects)
		{
			position = initPosition;
			transType = transformation;
		}

		public AxisValue Position
		{
			get { return position; }
		}

		public Transformation Transformation
		{
			get { return transType; }
		}
	};

	public class DynamicEventArgs : SingleObjectEventArgs
	{
		private readonly IDynamic dynamic;

		public DynamicEventArgs(IObject3D obj, IDynamic dynamic) : base(obj)
		{
			this.dynamic = dynamic;
		}

		public IDynamic Dynamic
		{
			get { return dynamic; }
		}
	}

}
