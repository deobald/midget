using System;
using Midget;
using System.Collections;

using Midget.Command.Object.Selection;

namespace Midget.Command.Object.RelationChange
{
	public class GroupCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{	
			if(objects == null || objects.Count == 0)
				throw new Exception ("No selected objects");

			// group objects
			group = SceneManager.Instance.Group(objects);
			Midget.Events.EventFactory.Instance.GenerateGroupEvent(this,objects,group);
			
			ICommand select = new SelectObjectCommand(group);
			select.Execute();
		}

		public void UnExecute()
		{
			// ungroup objects
			ICommand unGroup = new UngroupCommand(group);
			unGroup.Execute();
		}

		#endregion
		
		private ArrayList objects;
		private IObject3D group;

		public GroupCommand(ArrayList objects)
		{
			this.objects = (ArrayList)objects.Clone();
		}
	}

	public class UngroupCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{		
			if(group == null)
				throw new Exception ("No selected objects");

			if(!(group is GroupObject))
			{
				throw new Exception ("No group objects selected");
			}	

			ArrayList objects = SceneManager.Instance.UnGroup(group);

			Midget.Events.EventFactory.Instance.GenerateUngroupEvent(this,group);
			
			ICommand deselectAll = new DeselectAllObjectsCommand(SceneManager.Instance.SelectedObjects);
			deselectAll.Execute();

			// select all the objects
			foreach(IObject3D obj in objects)
			{
				ICommand select = new SelectAdditionalObjectCommand(obj);
				select.Execute();
			}
			

			ArrayList list = new ArrayList();
			list.Add(group);

			ICommand delete = new Midget.Command.Object.Lifetime.DeleteCommand(list);
			delete.Execute();
		}

		public void UnExecute()
		{
			// objects to be grouped will be selected
			ICommand group = new GroupCommand(SceneManager.Instance.SelectedObjects);
			group.Execute();
		}

		#endregion
		
		private IObject3D group;
		

		public UngroupCommand(IObject3D group)
		{
			this.group = group;
		}
	}

	public class ParentChangeCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{	
			if(objects == null || objects.Count == 0)
				throw new Exception ("No selected objects");

			// save all the old parents
			oldParents = new ArrayList();

			foreach (IObject3D obj in objects)
			{
				oldParents.Add(obj.Parent);
				SceneManager.Instance.ChangeParent(obj,parent);
			}

			Midget.Events.EventFactory.Instance.GenerateParentChangeEvent(this, objects, parent);
		}

		public void UnExecute()
		{
			int count = 0;

			foreach (IObject3D obj in objects)
			{
				SceneManager.Instance.ChangeParent(obj, (IObject3D)oldParents[count]);
				++count;
				
				ArrayList temp = new ArrayList();
				temp.Add(obj);

				Midget.Events.EventFactory.Instance.GenerateParentChangeEvent(this, temp, parent);
			}
		}

		#endregion

		private ArrayList objects;
		private IObject3D parent;
		private ArrayList oldParents;

		public ParentChangeCommand(ArrayList objects, IObject3D newParent)
		{
			this.objects = (ArrayList)objects.Clone();
			this.parent = newParent;
		}
	}

}
