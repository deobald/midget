using System;

namespace Midget.Command
{
	public class UserActionCommand
	{
		private UserActionCommand(){}

		public static void NewScene()
		{
			// delete all the objects in the scene
			ICommand delete = new Midget.Command.Object.Lifetime.DeleteCommand(SceneManager.Instance.ObjectList);
			delete.Execute();

			CommandManager.Instance.Clear();
		}

		public static void OpenScene(string fileName)
		{
			// delete all the objects in the scene
			ICommand delete = new Midget.Command.Object.Lifetime.DeleteCommand(SceneManager.Instance.ObjectList);
			delete.Execute();

			CommandManager.Instance.Clear();

			SceneManager.Instance.LoadScene(fileName);
			DeviceManager.Instance.UpdateViews();
			Midget.Events.EventFactory.Instance.GenerateOpenSceneEvent(null);

		}
	}
}
