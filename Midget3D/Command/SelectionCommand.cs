using System;
using System.Collections;

namespace Midget.Command.Object.Selection
{
	public class SelectObjectCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{

			// unselect all currently selected items
			deselectAll.Execute();

			// select the new item
			ICommand select = new SelectAdditionalObjectCommand(obj);
			select.Execute();
		}

		public void UnExecute()
		{	
			// deselect the current item
			ICommand select = new SelectAdditionalObjectCommand(obj);
			select.UnExecute();
			
			// undo deselect
			deselectAll.UnExecute();
		}

		#endregion

		private IObject3D obj;
		private ICommand deselectAll;

		public SelectObjectCommand (IObject3D obj)
		{
			this.obj = obj;
			deselectAll = new DeselectAllObjectsCommand((ArrayList)SceneManager.Instance.SelectedObjects.Clone());
		}

	}

	public class SelectAdditionalObjectCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{
			SceneManager.Instance.SelectAdditionalObject(obj);

			obj.Selected = true;

			Midget.Events.EventFactory.Instance.GenerateSelectAdditionalObjectEvent(this,obj);
		}

		public void UnExecute()
		{
			// deselect the object
			ICommand deselect = new DeselectObjectCommand(obj);
			deselect.Execute();
		}

		#endregion

		private IObject3D obj;

		public SelectAdditionalObjectCommand (IObject3D obj)
		{
			this.obj = obj;
		}

	}

	public class DeselectObjectCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{	
			if (SceneManager.Instance.SelectedObjects.Count > 0)
			{
				SceneManager.Instance.DeselectObject(obj);
				obj.Selected = false;
				Midget.Events.EventFactory.Instance.GenerateDeselectObjectEvent(this,obj);
			}
		}

		public void UnExecute()
		{	
			// select object that was unselected
			ICommand command = new SelectAdditionalObjectCommand(obj);
			command.Execute();
		}

		#endregion

		private IObject3D obj;

		public DeselectObjectCommand (IObject3D obj)
		{
			this.obj = obj;
		}

	}

	public class DeselectAllObjectsCommand : ICommand
	{

		#region ICommand Members

		public void Execute()
		{
			foreach(IObject3D obj in objects)
			{
				// deselect the objects
				ICommand command = new DeselectObjectCommand(obj);
				command.Execute();
			}
		}

		public void UnExecute()
		{
			foreach(IObject3D obj in objects)
			{
				// reselect the objects
				ICommand command = new DeselectObjectCommand(obj);
				command.UnExecute();
			}
		}

		#endregion
		
		private ArrayList objects;
	
		public DeselectAllObjectsCommand (ArrayList objects)
		{
			this.objects = (ArrayList)(objects.Clone());
		}
	}
 
}
