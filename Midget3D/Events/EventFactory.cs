using System;

using Midget.Events.Object;
using Midget.Events.Object.Animation;
using Midget.Events.Object.Lifetime;
using Midget.Events.Object.Relation;
using Midget.Events.Object.Selection;
using Midget.Events.Object.Transformation;
using Midget.Events.User;

namespace Midget.Events
{
	/// <summary>
	/// Factory to generate all the different events
	/// </summary>
	public class EventFactory
	{
		private EventFactory() 
		{
		}
		
		public static event AddKeyFrameEventHandler AddKeyFrame;
		public static event AddKeyFrameEventRequestHandler AddKeyFrameRequest;

		public static event RemoveKeyFrameEventHandler RemoveKeyFrame;
		public static event RemoveKeyFrameEventRequestHandler RemoveKeyFrameRequest;

		public static event CreateObjectEventHandler CreateObject;
		public static event CreateObjectRequestEventHandler CreateObjectRequest;
	
		public static event DeleteObjectEventHandler DeleteObject;
		public static event DeleteObjectRequestEventHandler DeleteObjectRequest;

		public static event DuplicateObjectEventHandler DuplicateObject;
		public static event DuplicateObjectRequestEventHandler DuplicateObjectRequest;

		public static event GroupEventHandler Group;
		public static event GroupRequestEventHandler GroupRequest;

		public static event UngroupEventHandler Ungroup;
		public static event UngroupRequestEventHandler UngroupRequest;

		public static event ParentChangeEventHandler ParentChange;
		public static event ParentChangeRequestEventHandler ParentChangeRequest;

		public static event TransformationEventHandler Transformation;
		public static event TransformationRequestEventHandler TransformationRequest;

		public static event SelectObjectEventHandler SelectObject;
		public static event SelectObjectRequestEventHandler SelectObjectRequest;

		public static event SelectAdditionalObjectEventHandler SelectAdditionalObject;
		public static event SelectAdditionalObjectRequestEventHandler	SelectAdditionalObjectRequest;

		public static event DeselectObjectEventHandler	DeselectObjects;
		public static event DeselectObjectEventRequestHandler DeselectObjectsRequest;

		public static event DeselectAllObjectsEventHandler	DeselectAllObjects;
		public static event DeselectAllObjectsEventRequestHandler	DeselectAllObjectsRequest;
		
		public static event AdjustCameraEventHandler AdjustCameraEvent;
		
		public static event SwitchEditModeEventHandler SwitchEditModeEvent;

		public static event UndoEventHandler UndoEvent;
		public static event RedoEventHandler RedoEvent;

		public static event NewSceneEventHandler NewScene;
		public static event NewSceneEventRequestHandler NewSceneRequest;

		public static event OpenSceneEventHanlder OpenScene;
		public static event OpenSceneEventRequestHandler OpenSceneRequest;

		
		public static event AddDynamicEventHandler AddDynamic;
		public static event AddDynamicRequestEventHandler AddDynamicRequest;

		public static event RemoveDynamicEventHandler RemoveDynamic;
		public static event RemoveDynamicRequestEventHandler RemoveDynamicRequest;

		public static readonly EventFactory Instance = new EventFactory();
		
		
		public void GenerateAddDyanmicEvent(object sender, IObject3D obj, IDynamic dynamic)
		{
			if(AddDynamic != null)
			{
				AddDynamic(sender, new DynamicEventArgs(obj,dynamic));
			}
		}

		public void GenerateAddDyanmicRequestEvent(object sender, IObject3D obj, IDynamic dynamic)
		{
			if(AddDynamicRequest != null)
			{
				AddDynamicRequest(sender, new DynamicEventArgs(obj,dynamic));
			}
		}

		public void GenerateRemoveDyanmicEvent(object sender, IObject3D obj, IDynamic dynamic)
		{
			if(RemoveDynamic != null)
			{
				RemoveDynamic(sender, new DynamicEventArgs(obj,dynamic));
			}
		}

		public void GenerateRemoveDynamicRequestEvent(object sender, IObject3D obj, IDynamic dynamic)
		{
			if(RemoveDynamicRequest != null)
			{
				RemoveDynamicRequest(sender, new DynamicEventArgs(obj,dynamic));
			}
		}
		
		
		public void GenerateUndoEvent(object sender)
		{
			if(UndoEvent != null)
			{
				UndoEvent(sender, new System.EventArgs());
			}
		}

		public void GenerateRedoEvent(object sender)
		{
			if(RedoEvent != null)
			{
				RedoEvent(sender, new System.EventArgs());
			}
		}
		
		public void GenerateAdjustCameraEvent(object sender, Midget.Cameras.MidgetCamera camera)
		{
			if(AdjustCameraEvent != null)
			{
				AdjustCameraEvent(sender, new AdjustCameraEventArgs(camera));
			}
		}

		public void GenerateSwitchEditModeEvent(object sender, EditMode editMode)
		{
			if (SwitchEditModeEvent != null)
			{
				SwitchEditModeEvent(sender, new SwitchEditModeEventArgs(editMode));
			}
		}

		public void GenerateAddKeyFrameEvent( object sender, System.Collections.ArrayList selectedObjects, int frameIndex)
		{
			if(AddKeyFrame != null)
			{
				AddKeyFrame(sender,new KeyFrameEventArgs(selectedObjects,frameIndex));
			}
		}
		public void GenerateAddKeyFrameRequestEvent( object sender, System.Collections.ArrayList selectedObjects, int frameIndex)
		{
			if(AddKeyFrameRequest != null)
			{
				AddKeyFrameRequest(sender,new KeyFrameEventArgs(selectedObjects,frameIndex));
			}
		}

		public void GenerateRemoveKeyFrameEvent( object sender, System.Collections.ArrayList selectedObjects, int frameIndex)
		{
			if(RemoveKeyFrame != null)
			{
				RemoveKeyFrame(sender,new KeyFrameEventArgs(selectedObjects,frameIndex));
			}
		}

		public void GenerateRemoveKeyFrameRequestEvent( object sender, System.Collections.ArrayList selectedObjects, int frameIndex)
		{
			if(RemoveKeyFrameRequest != null)
			{
				RemoveKeyFrameRequest(sender,new KeyFrameEventArgs(selectedObjects,frameIndex));
			}
		}

		public void GenerateCreateObjectEvent( object sender, IObject3D obj )
		{
			if(CreateObject != null)
			{
				CreateObject(sender,new SingleObjectEventArgs(obj));
			}
		}
		public void GenerateCreateObjectRequestEvent( object sender, ObjectFactory.ObjectTypes objectType, string parameters)
		{
			if(CreateObjectRequest != null )
			{
				CreateObjectRequest(sender, new CreateObjectRequestEventArgs(objectType, parameters));
			}
		}

		public void GenerateDeleteObjectEvent( object sender, System.Collections.ArrayList objects )
		{
			if(DeleteObject != null)
			{
				DeleteObject(sender,new MultiObjectEventArgs(objects));
			}
		}

		public void GenerateDeleteObjectRequestEvent( object sender, System.Collections.ArrayList objects )
		{
			if(DeleteObjectRequest != null)
			{
				DeleteObjectRequest(sender,new MultiObjectEventArgs(objects));
			}
		}


		public void GenerateSelectObjectEvent(object sender, IObject3D obj)
		{
			if(SelectObject != null)
			{
				SelectObject(sender, new SingleObjectEventArgs(obj));
			}
		}


		public void GenerateSelectObjectRequestEvent(object sender, IObject3D obj)
		{
			if(SelectObjectRequest != null)
			{
				SelectObjectRequest(sender, new SingleObjectEventArgs(obj));
			}
		}

		public void GenerateSelectAdditionalObjectEvent (object sender, IObject3D obj)
		{
			if(SelectAdditionalObject != null)
			{
				SelectAdditionalObject(sender, new SingleObjectEventArgs(obj));
			}

		}

		public void GenerateSelectAdditionalObjectRequestEvent (object sender, IObject3D obj)
		{
			if(SelectAdditionalObjectRequest != null)
			{
				SelectAdditionalObjectRequest( sender, new SingleObjectEventArgs(obj));
			}
		}

		public void GenerateDeselectObjectEvent (object sender, IObject3D obj)
		{
			if(DeleteObject != null)
			{
				DeselectObjects(sender, new SingleObjectEventArgs(obj));
			}
		}

		public void GenerateDeselectObjectEventRequest (object sender, IObject3D obj)
		{
			if(DeselectObjectsRequest != null)
			{
				DeselectObjectsRequest (sender, new SingleObjectEventArgs(obj));
			}
		}

		public void GenerateDeselectAllObjects (object sender, System.Collections.ArrayList objects)
		{
			if(DeselectAllObjects != null)
			{
				DeselectAllObjects(sender, new MultiObjectEventArgs(objects));
			}
		}
		public void GenerateDeselectAllObjectsEventRequest (object sender, System.Collections.ArrayList objects)
		{
			if(DeselectAllObjectsRequest != null)
			{
				DeselectAllObjectsRequest(sender, new MultiObjectEventArgs(objects));
			}
		}

		public void GenerateGroupEvent (object sender, System.Collections.ArrayList objects, IObject3D group)
		{
			if(Group != null)
			{
				Group(sender, new GroupEventArgs(objects, group));
			}
		}
																	
		public void GenerateGroupRequestEvent(object sender, System.Collections.ArrayList objects)
		{
			if(GroupRequest != null)
			{
				GroupRequest(sender, new MultiObjectEventArgs(objects));
			}
		}

		public void GenerateUngroupEvent(object sender, IObject3D obj)
		{
			if(Ungroup != null)
			{
				Ungroup (sender, new SingleObjectEventArgs(obj));
			}
		}
		public void GenerateUngroupRequestEvent(object sender, IObject3D group) 
		{
			if(UngroupRequest != null)
			{
				UngroupRequest(sender, new SingleObjectEventArgs(group)); 
			}
		}

		public void GenerateParentChangeEvent(object sender, System.Collections.ArrayList objects, IObject3D parent) 
		{
			if(ParentChange != null)
			{
				ParentChange(sender, new ParentChangeEventArgs(objects, parent));
			}
		}
		public void GenerateParentChangeRequestEvent(object sender, System.Collections.ArrayList objects, IObject3D parent) 
		{
			if(ParentChangeRequest != null)
			{
				ParentChangeRequest(sender, new ParentChangeEventArgs(objects, parent));
			}
		}
		public void GenerateTransformationEvent (object sender, System.Collections.ArrayList objects) 
		{
			if(Transformation != null)
			{
				Transformation(sender, new MultiObjectEventArgs(objects));

				// refresh views
				DeviceManager.Instance.UpdateViews();
			}
		}

		public void GenerateTransformationRequestEvent (object sender, System.Collections.ArrayList objects, AxisValue position, Transformation transformation)
		{
			if(TransformationRequest != null)
			{
				TransformationRequest(sender, new TransformationRequestEventArgs(objects,position,transformation));
			}
		}

		public void GenerateNewSceneEvent(object sender)
		{
			if(NewScene != null)
			{
				NewScene(sender, new System.EventArgs());
			}
		}

		public void GenerateNewSceneRequestEvent(object sender)
		{
			if(NewSceneRequest != null)
			{
				NewSceneRequest(sender, new System.EventArgs());
			}
		}

		public void GenerateOpenSceneEvent(object sender)
		{
			if(OpenScene != null)
			{
				OpenScene(sender, new System.EventArgs());
			}
		}

		public void GenerateOpenSceneRequestEvent(object sender)
		{
			if(OpenSceneRequest != null)
			{
				OpenSceneRequest(sender, new System.EventArgs());
			}
		}
	}
}
