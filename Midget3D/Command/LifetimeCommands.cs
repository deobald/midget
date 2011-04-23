using System;
using Midget;
using System.Collections;

using Midget.Command.Object.Selection;

namespace Midget.Command.Object.Lifetime
{
	public class CreateCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{
			if(parameters == null)
				obj = ObjectFactory.CreateObject((int)objectType);
			else
				obj = ObjectFactory.CreateObject((int)objectType,parameters);

			Midget.Events.EventFactory.Instance.GenerateCreateObjectEvent(this,obj);
			
			SceneManager.Instance.AddObject(obj);
			ICommand select = new SelectObjectCommand(obj);
			select.Execute();

		}

		public void UnExecute()
		{
			SceneManager.Instance.RemoveObject(obj);

			ArrayList objects = new ArrayList();
			objects.Add(obj);
	
			Midget.Events.EventFactory.Instance.GenerateDeleteObjectEvent(this,objects);
		}

		#endregion
		
		ObjectFactory.ObjectTypes objectType;
		IObject3D obj;
		string parameters;


		public CreateCommand(ObjectFactory.ObjectTypes type, string initParameters)
		{
			objectType = type;
			parameters = initParameters;
		}
	}

	public class DeleteCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{	
			// deselect all objects that are to be deleted
			deselect.Execute();
			
			// remove all objects from the scene
			foreach(IObject3D obj in objects)
			{
				SceneManager.Instance.RemoveObject(obj);	
			}			
			Midget.Events.EventFactory.Instance.GenerateDeleteObjectEvent(this,objects);
		}

		public void UnExecute()
		{	
			// add objects back to scene
			foreach(IObject3D obj in objects)
			{
				SceneManager.Instance.AddObject(obj);
				Midget.Events.EventFactory.Instance.GenerateCreateObjectEvent(this,obj);
			}

			
			
			// reselect all the objects that were selected
			deselect.UnExecute();
		}

		#endregion
		
		private ArrayList objects;
		ICommand deselect;

		public DeleteCommand(ArrayList objects)
		{
			this.objects = (ArrayList)objects.Clone();
			deselect = new DeselectAllObjectsCommand(objects);
		}
	}

	public class DuplicateCommand :ICommand
	{
		#region ICommand Members

		public void Execute()
		{
			// TODO:  Add DuplicateCommand.Execute implementation
		}

		public void UnExecute()
		{
			// TODO:  Add DuplicateCommand.UnExectute implementation
		}

		#endregion
		
		public DuplicateCommand(){}
	}

}