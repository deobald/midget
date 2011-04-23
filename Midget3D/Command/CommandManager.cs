using System;

using Midget.Events.Object.Transformation;

using Midget.Command.Object.Animation;
using Midget.Command.Object.Lifetime;
using Midget.Command.Object.RelationChange;
using Midget.Command.Object.Selection;
using Midget.Command.Object.Transformation;

namespace Midget.Command
{
	/// <summary>
	/// Summary description for CommandManager.
	/// </summary>
	public sealed class CommandManager
	{		
		private CommandContainer container;
		
		public static readonly CommandManager Instance = new CommandManager();
	
		private CommandManager()
		{
			container = new CommandContainer();
			
			#region EventRequestListeners
			Midget.Events.EventFactory.AddKeyFrameRequest+=new Midget.Events.Object.Animation.AddKeyFrameEventRequestHandler(EventFactory_AddKeyFrameRequest);
			Midget.Events.EventFactory.CreateObjectRequest+=new Midget.Events.Object.Lifetime.CreateObjectRequestEventHandler(EventFactory_CreateObjectRequest);
			Midget.Events.EventFactory.DeleteObjectRequest+=new Midget.Events.Object.Lifetime.DeleteObjectRequestEventHandler(EventFactory_DeleteObjectRequest);
			Midget.Events.EventFactory.DeselectAllObjectsRequest+=new Midget.Events.Object.Selection.DeselectAllObjectsEventRequestHandler(EventFactory_DeselectAllObjectsRequest);
			Midget.Events.EventFactory.DeselectObjectsRequest+=new Midget.Events.Object.Selection.DeselectObjectEventRequestHandler(EventFactory_DeselectObjectsRequest);
			Midget.Events.EventFactory.DuplicateObjectRequest+=new Midget.Events.Object.Lifetime.DuplicateObjectRequestEventHandler(EventFactory_DuplicateObjectRequest);
			Midget.Events.EventFactory.GroupRequest+=new Midget.Events.Object.Relation.GroupRequestEventHandler(EventFactory_GroupRequest);
			Midget.Events.EventFactory.ParentChangeRequest+=new Midget.Events.Object.Relation.ParentChangeRequestEventHandler(EventFactory_ParentChangeRequest);
			Midget.Events.EventFactory.RemoveKeyFrameRequest+=new Midget.Events.Object.Animation.RemoveKeyFrameEventRequestHandler(EventFactory_RemoveKeyFrameRequest);
			Midget.Events.EventFactory.SelectAdditionalObjectRequest+=new Midget.Events.Object.Selection.SelectAdditionalObjectRequestEventHandler(EventFactory_SelectAdditionalObjectRequest);
			Midget.Events.EventFactory.SelectObjectRequest+=new Midget.Events.Object.Selection.SelectObjectRequestEventHandler(EventFactory_SelectObjectRequest);
			Midget.Events.EventFactory.TransformationRequest+=new Midget.Events.Object.Transformation.TransformationRequestEventHandler(EventFactory_TransformationRequest);
			Midget.Events.EventFactory.UngroupRequest+=new Midget.Events.Object.Relation.UngroupRequestEventHandler(EventFactory_UngroupRequest);
			Midget.Events.EventFactory.NewSceneRequest+=new Midget.Events.User.NewSceneEventRequestHandler(EventFactory_NewSceneRequest);
			Midget.Events.EventFactory.OpenSceneRequest +=new Midget.Events.User.OpenSceneEventRequestHandler(EventFactory_OpenSceneRequest);
			Midget.Events.EventFactory.AddDynamicRequest+=new AddDynamicRequestEventHandler(EventFactory_AddDynamicRequest);
			Midget.Events.EventFactory.RemoveDynamicRequest+=new RemoveDynamicRequestEventHandler(EventFactory_RemoveDynamicRequest);
			#endregion

		}

		public bool UndoAvailable
		{
			get { return container.UndoAvailable;}
		}

		public bool RedoAvailable
		{
			get { return container.RedoAvailable;}
		}

		public void Undo()
		{
			container.Undo();
		}

		public void Redo()
		{
			container.Redo();
		}	

		public void Clear()
		{
			container = new CommandContainer();
		}
		
		#region EventRequestHandlers
		private void EventFactory_AddKeyFrameRequest(object sender, Midget.Events.Object.Animation.KeyFrameEventArgs e)
		{
			container.Add(new AddKeyFrameCommand(e.Objects,e.FrameIndex));
		}

		private void EventFactory_CreateObjectRequest(object sender, Midget.Events.Object.Lifetime.CreateObjectRequestEventArgs e)
		{
			container.Add(new CreateCommand(e.Type,e.Parameters));
		}

		private void EventFactory_DeleteObjectRequest(object sender, Midget.Events.Object.MultiObjectEventArgs e)
		{
			container.Add(new DeleteCommand(e.Objects));
		}

		private void EventFactory_DeselectAllObjectsRequest(object sender, Midget.Events.Object.MultiObjectEventArgs e)
		{
			container.Add(new DeselectAllObjectsCommand(e.Objects));
		}

		private void EventFactory_DeselectObjectsRequest(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			container.Add(new DeselectObjectCommand(e.Object));
		}

		private void EventFactory_DuplicateObjectRequest(object sender, Midget.Events.Object.Lifetime.DuplicateObjectRequestEventArgs e)
		{
			container.Add(new DuplicateCommand());
		}

		private void EventFactory_GroupRequest(object sender, Midget.Events.Object.MultiObjectEventArgs e)
		{
			container.Add(new GroupCommand(e.Objects));
		}

		private void EventFactory_ParentChangeRequest(object sender, Midget.Events.Object.Relation.ParentChangeEventArgs e)
		{
			container.Add(new ParentChangeCommand(e.Objects,e.NewParent));	
		}

		private void EventFactory_RemoveKeyFrameRequest(object sender, Midget.Events.Object.Animation.KeyFrameEventArgs e)
		{
			container.Add(new RemoveKeyFrameCommand(e.Objects,e.FrameIndex));
		}

		private void EventFactory_SelectAdditionalObjectRequest(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			container.Add(new SelectAdditionalObjectCommand(e.Object));
		}

		private void EventFactory_SelectObjectRequest(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			container.Add(new SelectObjectCommand(e.Object));
		}

		private void EventFactory_TransformationRequest(object sender, Midget.Events.Object.Transformation.TransformationRequestEventArgs e)
		{
			if(e.Transformation == Transformation.Translate)
			{
				container.Add(new TranslateCommand(e.Objects,e.Position.X,e.Position.Y,e.Position.Z));
			}
			else if(e.Transformation == Transformation.Scale)
			{
				container.Add(new ScaleCommand(e.Objects,e.Position.X,e.Position.Y,e.Position.Z));
			}
			else if (e.Transformation == Transformation.Rotate)
			{
				container.Add(new RotateCommand(e.Objects,e.Position.X,e.Position.Y,e.Position.Z));
			}
			else
			{
				container.Add(new PivotCommand(e.Objects,e.Position.X,e.Position.Y,e.Position.Z));
			}
		}	

		private void EventFactory_UngroupRequest(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			container.Add(new UngroupCommand(e.Object));
		}
		#endregion

		private void EventFactory_NewSceneRequest(object sender, EventArgs e)
		{
			UserActionCommand.NewScene();
			DeviceManager.Instance.UpdateViews();
		}

		private void EventFactory_OpenSceneRequest(object sender, EventArgs e)
		{
			DeviceManager.Instance.UpdateViews();
		}

		private void EventFactory_AddDynamicRequest(object sender, DynamicEventArgs e)
		{
			container.Add(new AddDynamicCommand(e.Object,e.Dynamic));


		}

		private void EventFactory_RemoveDynamicRequest(object sender, DynamicEventArgs e)
		{
			container.Add(new RemoveDynamicCommand(e.Object,e.Dynamic));
		}
	}
}
