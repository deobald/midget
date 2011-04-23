using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget.Cameras
{
	/// <summary>
	/// Camera Object Specific to Midget with advanced features specific to Midget
	/// </summary>
	public abstract class MidgetCamera : Camera
	{
		private string	_name;
		
		private bool		_rotatable;
		private bool		_zoomable;
		private bool		_pannable;	

		protected float		orthoZoom;

		/// <summary>
		/// Creates a new Midgetcamera
		/// </summary>
		/// <param name="eyePosition">The position of the camera in 3d space</param>
		/// <param name="targetPoint">The position that camera is targeted at</param>
		/// <param name="upVector">The up direction for the camera</param>
		/// <param name="name">The name of the camera</param>
		/// <param name="rotatable">Whether the camera can be rotated</param>
		/// <param name="zoomable">Whether or not the camera can be zoomed</param>
		/// <param name="pannable">Whether or not the camera can be panned</param>
		public MidgetCamera (Vector3 eyePosition, Vector3 targetPoint, Vector3 upVector, string name,
			bool rotatable, bool zoomable, bool pannable)
			: base(eyePosition, targetPoint, upVector)
		{
			_name			= name;
			_rotatable		= rotatable;
			_zoomable		= zoomable;
			_pannable		= pannable;
		}
		

		public abstract void DrawGrid(Device device);

		public abstract void ProjectCoordinates( ref int x, ref int y, int width, int height, Vector3 pickRayOrigin);	//, Vector3 pickRayDirection);

		public abstract void UnProjectCoordinates( int x, int y, int width, int height, ref Vector3 pickRayOrigin, ref Vector3 pickRayDirection);

		#region Midget Camera Properties
		
		/// <summary>
		/// The name of the camera
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		
		/// <summary>
		/// Whether the camera can be rotated
		/// </summary>
		public bool Rotatable
		{
			get { return _rotatable; }
		}
		
		/// <summary>
		/// Whether or not the camera can be Scalable
		/// </summary>
		public bool Zoomable
		{
			get { return _zoomable; }
		}
		
		/// <summary>
		/// Whether not the camera can be panned
		/// </summary>
		public bool Pannable
		{
			get { return _pannable; }
		}

		/// <summary>
		/// Zoom value for orthogonal cameras
		/// </summary>
		public float OrthoZoom
		{
			get { return orthoZoom; }
			set { orthoZoom = value; }
		}

		#endregion
	}
}
