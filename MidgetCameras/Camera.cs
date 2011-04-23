using System;
using Microsoft.DirectX;

namespace Midget.Cameras
{
	/// <summary>
	/// Abstract base class for the various types of cameras that can
	/// be created.  Class provides all necessary opertions for retrieval
	/// and manipulation of the camera's view and perspective.  Camera uses Left-Handed
	/// coordinate system
	/// </summary>
	public abstract class Camera
	{
		#region Attributes for the view matrix
		protected Vector3 _eyePosition;
		protected Vector3 _targetPoint;
		protected Vector3 _upVector;

		protected Matrix  _viewMatrix;
		#endregion

		#region Attributes for the projection matrix
		protected float		_fov;
		protected float		_aspectRatio;
		protected float		_nearClipPlane;
		protected float		_farClipPlane;
		
		protected Matrix	_projectionMatrix;
		#endregion

		private Matrix worldMatrix = Matrix.Identity;

		/// <summary>
		/// Creates a new camera
		/// </summary>
		/// <param name="eyePosition">The position of the camera in 3d space</param>
		/// <param name="targetPoint">The position that camera is targeted at</param>
		/// <param name="upVector">The up direction for the camera</param>
		public Camera(Vector3 eyePosition, Vector3 targetPoint, Vector3 upVector)
		{
			// setup up the view matrix
			SetViewParameters (eyePosition, targetPoint, upVector);

			// setup up projection matrix with default values
			SetProjectionParameters ( (float)Math.PI / 4.0f, 1.0f, 0.0001f, 1000.0f);
		}

		/// <summary>
		/// Changes the components of the view Matrix for the camera
		/// </summary>
		/// <param name="eyePosition">The position of the camera in 3d space</param>
		/// <param name="targetPoint">The position that camera is targeted at</param>
		/// <param name="upVector">The up direction for the camera</param>
		public void SetViewParameters (Vector3 eyePosition, Vector3 targetPoint, Vector3 upVector)
		{
			_eyePosition	= eyePosition;
			_targetPoint	= targetPoint;
			_upVector		= upVector;

			// create new view matrix
			_viewMatrix = Matrix.LookAtLH( eyePosition, targetPoint, upVector);
		}

		/// <summary>
		/// Changes the components of the projection Matrix for the camera
		/// </summary>
		/// <param name="fov">The FOV for the camera</param>
		/// <param name="aspectRatio">The Aspect Ratio of the particular camera (Height/Width)</param>
		/// <param name="nearClipPlane">Near object clipping plane</param>
		/// <param name="farClipPlane">Far object clipping plane</param>
		public abstract void SetProjectionParameters (float fov, float aspectRatio, float nearClipPlane, float farClipPlane);
//		{
//			// set all the properties for projection matrix
//			_fov			= fov;
//			_aspectRatio	= aspectRatio;
//			_nearClipPlane	= nearClipPlane;
//			_farClipPlane	= farClipPlane;
//
//			// create new projection matrix
//			_projectionMatrix = Matrix.PerspectiveFovLH( fov, aspectRatio, nearClipPlane, farClipPlane );
//		}


		#region Properties related to the Camera's View Matrix
		
		/// <summary>
		/// Where in 3d space the camera is positioned
		/// </summary>
		public Vector3 EyePosition
		{
			get { return _eyePosition; }
		}

		/// <summary>
		/// The point which the camera is focus upon
		/// </summary>
		public Vector3 TargetPoint
		{
			get { return _targetPoint; }
		}
		
		/// <summary>
		/// Up vector for the particular camera
		/// </summary>
		public Vector3 UpVector
		{
			get { return _upVector; }
		}
		
		/// <summary>
		/// View Matrix for the camera
		/// </summary>
		public Matrix ViewMatrix
		{
			get { return _viewMatrix; }
			set
			{
				_viewMatrix = value;
				//				_eyePosition = new Vector3(_viewMatrix.M11, _viewMatrix.M21, _viewMatrix.M31);
				//				_targetPoint = new Vector3(_viewMatrix.M21, _viewMatrix.M22, _viewMatrix.M23);
				//				_upVector = new Vector3(_viewMatrix.M31, _viewMatrix.M32, _viewMatrix.M33);
			}
		}

		#endregion

		#region Properties related to the Camera's Project Matrix
		
		/// <summary>
		/// FOV for the camera
		/// </summary>
		public float FOV
		{
			get { return _fov; }
		}
		
		/// <summary>
		/// Aspect Ration of the camera (Height ViewPort / Width ViewPort)
		/// </summary>
		public float AspectRatio
		{
			get { return _aspectRatio; }
		}

		/// <summary>
		/// The distance from the camera to the near clipping plane
		/// </summary>
		public float NearClipPlane
		{
			get { return _nearClipPlane; }
		}
		
		/// <summary>
		/// The distance from the camera to the far clipping plane
		/// </summary>
		public float FarClipPlane
		{
			get { return _farClipPlane; }
		}
		
		/// <summary>
		/// The project matrix for the camera
		/// </summary>
		public Matrix ProjectionMatrix
		{
			get { return _projectionMatrix; }
		}

		#endregion

		public Matrix WorldMatrix
		{
			get { return worldMatrix; }
			set { worldMatrix = value; }
		}	
		
	}
}
