using System;
using Midget;

namespace MidgetEvent
{
	// event arguments //
	public class SelectObjectEventArgs : EventArgs
	{
		private readonly IObject3D selectedObject;
  
		public SelectObjectEventArgs(IObject3D selectedObject)
		{
			this.selectedObject = selectedObject;
		}

		public IObject3D SelectedObject
		{
			get { return selectedObject; }
		}
	}

	public class CreateObjectEventArgs : EventArgs
	{
		private readonly IObject3D createdObject;

		public CreateObjectEventArgs(IObject3D createdObject)
		{
			this.createdObject = createdObject;
		}

		public IObject3D CreatedObject
		{
			get { return createdObject; }
		}
	}

	// event handlers //
	public delegate void SelectObjectEventHandler(object sender, SelectObjectEventArgs e);
	public delegate void CreateObjectEventHandler(object sender, CreateObjectEventArgs e);

	
	// object event factory //
	public class ObjectEventFactory
	{
		private ObjectEventFactory(){}
		
		// this is a singleton, so a method of obtaining an instance must be provided
		public static readonly ObjectEventFactory Instance = new ObjectEventFactory();

		// object selection //
		public event SelectObjectEventHandler SelectObject;

		public void CreateSelectEvent(IObject3D selectedObject)
		{
			this.OnSelectObject(new SelectObjectEventArgs(selectedObject));
		}

		protected virtual void OnSelectObject(SelectObjectEventArgs e)
		{
			if (SelectObject != null) 
			{
				// Invokes the delegates. 
				SelectObject(this, e);
			}
		}

		// object creation //
		public event CreateObjectEventHandler CreateObject;

		public void CreateCreateEvent(IObject3D selectedObject)
		{
			this.OnCreateObject(new CreateObjectEventArgs(selectedObject));
		}

		protected virtual void OnCreateObject(CreateObjectEventArgs e)
		{
			if (CreateObject != null)
			{
				// Invokes the delegates
				CreateObject(this, e);
			}
		}
	}
}
