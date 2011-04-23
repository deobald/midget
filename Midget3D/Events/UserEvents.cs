using System;

namespace Midget.Events.User
{
	public delegate void AdjustCameraEventHandler(object sender, AdjustCameraEventArgs e);
	public delegate void SwitchEditModeEventHandler(object sender, SwitchEditModeEventArgs e);
	public delegate void UndoEventHandler(object sender, System.EventArgs e);
	public delegate void RedoEventHandler(object sender, System.EventArgs e);
	public delegate void NewSceneEventHandler (object sender, System.EventArgs e);
	public delegate void NewSceneEventRequestHandler (object sender, System.EventArgs e);
	public delegate void OpenSceneEventHanlder(object sender, System.EventArgs e);
	public delegate void OpenSceneEventRequestHandler(object sender, System.EventArgs e);
	
	public class AdjustCameraEventArgs : EventArgs
	{
		private readonly Midget.Cameras.MidgetCamera _camera;

		public AdjustCameraEventArgs ( Midget.Cameras.MidgetCamera camera )
		{
			_camera = camera;
		}

		public Midget.Cameras.MidgetCamera Camera
		{
			get { return _camera; }
		}
	}

	public class SwitchEditModeEventArgs : EventArgs
	{
		private readonly EditMode _editMode;

		public SwitchEditModeEventArgs ( EditMode editMode )
		{
			_editMode = editMode;
		}

		public EditMode EditMode
		{
			get { return _editMode; }
		}
	}
}
