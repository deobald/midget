namespace Midget.Events.Object
{
	/// <summary>
	/// Abstract class for all events related to manipulating 3D Objects in Midget
	/// </summary>
	public abstract class ObjectEventArgs : System.EventArgs 
	{}
	
	
	/// <summary>
	/// class for all object events that involved a single 3d object
	/// </summary>
	public class SingleObjectEventArgs : ObjectEventArgs
	{
		private readonly Midget.IObject3D obj;

		public SingleObjectEventArgs(Midget.IObject3D initObj)
		{
			obj = initObj;
		}
		
		/// <summary>
		/// Object that the event is to be performed on
		/// </summary>
		public Midget.IObject3D Object
		{
			get { return obj; }
		}
	}

	
	/// <summary>
	/// class for all object events that involve multiple 3d Objects
	/// </summary>
	public class MultiObjectEventArgs : ObjectEventArgs
	{
		private readonly System.Collections.ArrayList objects;

		public MultiObjectEventArgs(System.Collections.ArrayList initObjects)
		{
			objects = initObjects;
		}
		
		/// <summary>
		/// Objects that the event is to be performed on
		/// </summary>
		public System.Collections.ArrayList Objects
		{
			get { return objects; }
		}
	}
}
