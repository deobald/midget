using System;
using System.Collections;

namespace Midget.Events.Object.Animation
{
	public delegate void AddKeyFrameEventHandler(object sender, KeyFrameEventArgs e);
	public delegate void AddKeyFrameEventRequestHandler(object sender, KeyFrameEventArgs e);

	public delegate void RemoveKeyFrameEventHandler(object sender, KeyFrameEventArgs e);
	public delegate void RemoveKeyFrameEventRequestHandler(object sender, KeyFrameEventArgs e);

	
	/// <summary>
	/// Object grouping Event notification event
	/// </summary>
	public class KeyFrameEventArgs : MultiObjectEventArgs
	{
		private readonly int index;

		public KeyFrameEventArgs( ArrayList objects, int frameIndex) : base(objects)
		{
			index = frameIndex;
		}
		
		/// <summary>
		/// Key Frame Index
		/// </summary>
		public int FrameIndex
		{
			get { return index; }
		}
	}
}
