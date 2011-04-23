using Midget;
using System.Collections;

namespace Midget.Command.Object.Transformation
{
	public class TranslateCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{
			//calcualte relative change
			AxisValue relativeChange = new AxisValue(X,Y,Z) - ((IObject3D)objects[objects.Count - 1]).Translation; 

			foreach (IObject3D obj in objects)
			{	
				mementos.Add(obj.CreateMemento());

				obj.Translate(obj.Translation.X + relativeChange.X,  
					obj.Translation.Y + relativeChange.Y , 
					obj.Translation.Z + relativeChange.Z);	

				// change active objects' memento
				if (obj.Rigidity == Rigidity.Active)
				{
					obj.DynamicStartMemento = obj.CreateMemento();
				}
			}

			Midget.Events.EventFactory.Instance.GenerateTransformationEvent(this,objects);
		}

		public void UnExecute()
		{	
			int count = 0;

			foreach (IObject3D obj in objects)
			{	
				obj.SetMemento((Object3DMemento)mementos[count]);
				++count;

				// change active objects' memento
				if (obj.Rigidity == Rigidity.Active)
				{
					obj.DynamicStartMemento = obj.CreateMemento();
				}
			}

			Midget.Events.EventFactory.Instance.GenerateTransformationEvent(this,objects);
		}

		#endregion
	
		private ArrayList objects;
		private float X, Y, Z;
		
		private ArrayList mementos;

		public TranslateCommand(ArrayList objects, float X, float Y, float Z)
		{
			this.objects = (ArrayList)objects.Clone();
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			mementos = new ArrayList();
		}
	}

	public class RotateCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{
			//calcualte relative change
			AxisValue relativeChange = new AxisValue(X,Y,Z) - ((IObject3D)objects[objects.Count - 1]).Rotation; 

			foreach (IObject3D obj in objects)
			{	
				mementos.Add(obj.CreateMemento());

				obj.Rotate(obj.Rotation.X + relativeChange.X,  
					obj.Rotation.Y + relativeChange.Y , 
					obj.Rotation.Z + relativeChange.Z);	

				// change active objects' memento
				if (obj.Rigidity == Rigidity.Active)
				{
					obj.DynamicStartMemento = obj.CreateMemento();
				}
			}

			Midget.Events.EventFactory.Instance.GenerateTransformationEvent(this,objects);
		}

		public void UnExecute()
		{	
			int count = 0;

			foreach (IObject3D obj in objects)
			{	
				obj.SetMemento((Object3DMemento)mementos[count]);
				++count;
			}

			Midget.Events.EventFactory.Instance.GenerateTransformationEvent(this,objects);
		}

		#endregion
	
		private ArrayList objects;
		private float X, Y, Z;
		
		private ArrayList mementos;

		public RotateCommand(ArrayList objects, float X, float Y, float Z)
		{
			this.objects = (ArrayList)objects.Clone();
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			mementos = new ArrayList();
		}
	}

	public class ScaleCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{
			//calcualte relative change
			AxisValue relativeChange = new AxisValue(X,Y,Z) - ((IObject3D)objects[objects.Count - 1]).Scaling; 

			foreach (IObject3D obj in objects)
			{	
				mementos.Add(obj.CreateMemento());

				obj.Scale(obj.Scaling.X + relativeChange.X,  
					obj.Scaling.Y + relativeChange.Y , 
					obj.Scaling.Z + relativeChange.Z);	

				// change active objects' memento
				if (obj.Rigidity == Rigidity.Active)
				{
					obj.DynamicStartMemento = obj.CreateMemento();
				}
			}

			Midget.Events.EventFactory.Instance.GenerateTransformationEvent(this,objects);
		}

		public void UnExecute()
		{	
			int count = 0;

			foreach (IObject3D obj in objects)
			{	
				obj.SetMemento((Object3DMemento)mementos[count]);
				++count;
			}

			Midget.Events.EventFactory.Instance.GenerateTransformationEvent(this,objects);
		}

		#endregion
	
		private ArrayList objects;
		private float X, Y, Z;
		
		private ArrayList mementos;

		public ScaleCommand(ArrayList objects, float X, float Y, float Z)
		{
			this.objects = (ArrayList)objects.Clone();
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			mementos = new ArrayList();
		}
	}

	public class PivotCommand :ICommand 
	{
		public void Execute()
		{
			//calcualte relative change
			AxisValue relativeChange = new AxisValue(X,Y,Z) - ((IObject3D)objects[objects.Count - 1]).PivotPoint; 

			foreach (IObject3D obj in objects)
			{	
				pivotPoints.Add(obj.PivotPoint);

				obj.PivotPoint = new AxisValue(obj.PivotPoint.X + relativeChange.X,  
					obj.PivotPoint.Y + relativeChange.Y , 
					obj.PivotPoint.Z + relativeChange.Z);	
			}

			Midget.Events.EventFactory.Instance.GenerateTransformationEvent(this,objects);
		}

		public void UnExecute()
		{	
			int count = 0;

			foreach (IObject3D obj in objects)
			{	
				obj.PivotPoint = (AxisValue)pivotPoints[count];
				++count;
			}

			Midget.Events.EventFactory.Instance.GenerateTransformationEvent(this,objects);
		}
		private ArrayList objects;
		private float X, Y, Z;
		
		private ArrayList pivotPoints;

		public PivotCommand(ArrayList objects, float X, float Y, float Z)
		{
			this.objects =(ArrayList)objects.Clone();
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			pivotPoints = new ArrayList();
		}
	}

	public class RenameCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{	
			oldName = obj.Name;
			obj.Name = name;
		}

		public void UnExecute()
		{
			obj.Name = name;
		}

		#endregion

		private IObject3D obj;
		private string name;
		private string oldName;

		public RenameCommand(IObject3D obj, string newName)
		{
			this.obj = obj;
			name = newName;
		}
	}


	public class AddDynamicCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{	
			if( obj == null)
			{
				throw new System.Exception("No object selected");
			}

			// check to make sure dynamic is not already applied
			foreach(IDynamic dyn in obj.DynamicsList)
			{
				if(dyn.GetType() == dynamic.GetType())
				{
					ICommand command = new RemoveDynamicCommand(obj,dyn);
					command.Execute();
					break;
				}
			}

			obj.DynamicsList.Add(dynamic);
			obj.Rigidity = Rigidity.Active;

			Midget.Events.EventFactory.Instance.GenerateAddDyanmicEvent(this,obj,dynamic);

		}

		public void UnExecute()
		{
			ICommand command = new RemoveDynamicCommand(obj,dynamic);
			command.Execute();
		}

		#endregion

		private IObject3D obj;
		private IDynamic dynamic;

		public AddDynamicCommand(IObject3D obj, IDynamic dynamic)
		{
			this.obj = obj;
			this.dynamic = dynamic;
		}
	}

	public class RemoveDynamicCommand : ICommand
	{
		#region ICommand Members

		public void Execute()
		{	
			if( obj == null)
			{
				throw new System.Exception("No object selected");
			}

			try
			{		
				obj.DynamicsList.Remove(dynamic);
			}
			catch 
			{
				throw new System.Exception("No dynamic selected to remove");
			}
			
			obj.Rigidity = Rigidity.None;
			Midget.Events.EventFactory.Instance.GenerateRemoveDyanmicEvent(this,obj,dynamic);
		}

		public void UnExecute()
		{
			ICommand command = new AddDynamicCommand(obj,dynamic);
			command.Execute();
		}

		#endregion

		private IObject3D obj;
		private IDynamic dynamic;

		public RemoveDynamicCommand(IObject3D obj, IDynamic dynamic)
		{
			this.obj = obj;
			this.dynamic = dynamic;
		}
	}
}