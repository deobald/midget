using System;
using System.Collections;


namespace Midget.Command.Object.Animation
{
	public class AddKeyFrameCommand:ICommand
	{
		#region ICommand Members

		public void Execute()
		{
			foreach(IObject3D obj in objects)
			{
				obj.AddKeyFrame(frameIndex);
			}

			Midget.Events.EventFactory.Instance.GenerateAddKeyFrameEvent(this,objects,frameIndex);
		}

		public void UnExecute()
		{
			ICommand removeKeyFrame = new RemoveKeyFrameCommand(objects,frameIndex);
			removeKeyFrame.Execute();
		}

		#endregion
		
		private ArrayList objects;
		private int frameIndex;

		public AddKeyFrameCommand(ArrayList objects, int frameIndex)
		{
			this.objects = (ArrayList)objects.Clone();
			this.frameIndex = frameIndex;
		}
	}

	public class RemoveKeyFrameCommand:ICommand
	{
		#region ICommand Members

		public void Execute()
		{
			foreach(IObject3D obj in objects)
			{
				obj.RemoveKeyFrame(frameIndex);
			}

			Midget.Events.EventFactory.Instance.GenerateRemoveKeyFrameEvent(this,objects,frameIndex);
		}

		public void UnExecute()
		{
			ICommand addKeyFrame = new AddKeyFrameCommand(objects,frameIndex);
			addKeyFrame.Execute();
		}

		#endregion
		
		private ArrayList objects;
		private int frameIndex;

		public RemoveKeyFrameCommand(ArrayList objects, int frameIndex)
		{
			this.objects = (ArrayList)objects.Clone();
			this.frameIndex = frameIndex;
		}
	}
}
