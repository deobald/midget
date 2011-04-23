using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Midget
{
	/// <summary>
	/// Summary description for Scene.
	/// </summary>
	/// 
	public sealed class SceneManager
	{
		IObject3D _scene;
		private int currentFrameIndex = 0;
		private ArrayList objectList;
		private SelectedObjectContainer selectedObjects;
		
		private SceneManager()
		{
			_scene = new SceneObject();
			objectList = new ArrayList();
			selectedObjects = new SelectedObjectContainer();
		}

		public static readonly SceneManager Instance = new SceneManager();

		public IObject3D Scene
		{
			get { return _scene; }
		}

		public void NewScene()
		{	
			_scene = new GroupObject();

			// TODO: make a SceneChanged event, 'cuz this is for sucks
			DeviceManager.Instance.UpdateViews();
		}

		public void SaveScene(string filePath)
		{
			// delete all the objects in the scene
		//	Midget.Command.ICommand deselect = new Midget.Command.Object.Selection.DeselectAllObjectsCommand(SceneManager.Instance.ObjectList);
		//	deselect.Execute();

			Stream stream = File.Open(filePath, FileMode.Create);
			BinaryFormatter bformatter = new BinaryFormatter();
            
			bformatter.Serialize(stream, _scene);
			stream.Close();	
		}

		public void LoadScene(string filePath)
		{
			Stream stream = File.Open(filePath,FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();
        
			try
			{
				_scene = (SceneObject)bformatter.Deserialize(stream);
			}
			finally
			{
				stream.Close();        
			}
    
			_scene.Reinitialize(DeviceManager.Instance.Device);
			Parzor(_scene);
		}

		public int CurrentFrameIndex
		{
			get { return currentFrameIndex; }
			set{ currentFrameIndex = value; }
		}
		
		public void AddObject(IObject3D newObject)
		{
			objectList.Add(newObject);
			_scene.AddChild(newObject);
			newObject.Parent = _scene;
		}

		public void RemoveObject (IObject3D oldObject)
		{
			objectList.Remove(oldObject);
			
			// remove object from it's parent
			oldObject.Parent.RemoveChild(oldObject);
		}

		public void RemoveObject (string objectName)
		{
			foreach(IObject3D obj in objectList)
			{
				if(obj.Name == objectName)
				{
					this.RemoveObject(obj);
					break;
				}
			}
		}

		public void SelectAdditionalObject(IObject3D obj)
		{
			SelectedObjects.Add(obj);
		}

		public void DeselectObject(IObject3D obj)
		{
			SelectedObjects.Remove(obj);
		}

		public IObject3D Group(ArrayList objects)
		{
			IObject3D newGroup = new GroupObject();
			
			// add this group to the scene
			AddObject(newGroup);

			// add all the children to the group
			foreach (IObject3D obj in objects)
			{
				newGroup.AddChild(obj);
			}
			
			return newGroup;
		}

		public ArrayList UnGroup(IObject3D group)
		{	
			// remove all children from this group and move them one level up
			if (group.Children != null)
			{
				ArrayList children = (ArrayList)group.Children.Clone();
				
				
				while(group.Children.Count > 0)
				{
					group.Parent.AddChild((IObject3D)group.Children[0]);
					
				}

				return children;
			}

			return null;
		}

		public void ChangeParent(IObject3D obj, IObject3D newParent)
		{
			newParent.AddChild(obj);
	
		}

		public ArrayList ObjectList
		{ 
			get { return objectList; }
		}

		public ArrayList SelectedObjects
		{
			get { return selectedObjects.SelectedObjects; }
		}

		public void Parzor (IObject3D parent)
		{
			if ((parent is Particle) || (parent  is MeshCtrlPt))
			{
				return;
			}
			if (!(parent is SceneObject))
				objectList.Add(parent);
			foreach (IObject3D obj in parent.Children)
			{
				Parzor(obj);
			}
			return;
																  
		}


	}
}
