using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace Midget
{
	public abstract class Camera
	{
		#region Camera Location Variables
		private Vector3 cameraPosition;
		private Vector3 cameraTarget;
		private Vector3 cameraUpVector;
		#endregion

		#region Camera Project Variables
		/// <summary>
		/// Cameras field of view in Radians
		/// </summary>
		protected float cameraFOV = (float)Math.PI / 4;
		
		/// <summary>
		/// Cameras aspect ratio for the view area.  Is defined as height divided
		/// by width of the viewport
		/// </summary>
		protected  float cameraAspectRatio = 1.0f;	//360.0f / 306.0f;	// 1.0f;

		/// <summary>
		/// Near distance from the camera where geometry should no longer be rendered
		/// </summary>
		protected  float cameraNearClipDistance = 1.0f;
		

		/// <summary>
		/// Far distance from the camera where the geometry should no longer be rendered
		/// </summary>
		protected  float cameraFarClipDistance = 100.0f;
		#endregion

		#region Camera State Variables
		
		protected bool rotatable = false;
		protected bool scalable = false;
		protected bool pannable = false;

		#endregion

		private string name = null;

		public Camera(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
		{
			this.cameraPosition = cameraPosition;
			this.cameraTarget = cameraTarget;
			this.cameraUpVector = cameraUpVector;
		}

		public Matrix RotateCamera()
		{
			return Matrix.LookAtLH(cameraPosition,cameraTarget,cameraUpVector);
		}

		public Matrix ScaleCamera()
		{
			return Matrix.LookAtLH(cameraPosition,cameraTarget,cameraUpVector);
		}

		public Matrix PanCamera()
		{
			return Matrix.LookAtLH(cameraPosition,cameraTarget,cameraUpVector);
		}

		public Matrix CameraView
		{
			get
			{
				return Matrix.LookAtLH(cameraPosition,cameraTarget,cameraUpVector);
			}
		}

		public abstract Matrix CameraProjection
		{	
			get;
		}

		#region Properties

		public string CameraName
		{
			get { return name; }
			set { name = value; }
		}

		public float  CameraFOV
		{
			get { return cameraFOV; }
			set { cameraFOV = value; }
		}

		public float  CameraAspectRation
		{
			get { return cameraAspectRatio; }
			set { cameraAspectRatio = value; }
		}

		public float  CameraNearClipDistance
		{
			get { return cameraNearClipDistance; }
			set { cameraNearClipDistance = value; }
		}

		public float  CameraFarClipDistance
		{
			get { return cameraFarClipDistance; }
			set { cameraFarClipDistance = value; }
		}

		public bool Rotatable
		{
			get { return rotatable; }
		}

		public bool Scalable
		{
			get { return scalable; }
		}

		public bool Pannable
		{
			get { return pannable; }
		}

		#endregion

		public abstract bool DrawGrid(Device device);
	}

	class OrthogonalCamera : Camera
	{
		public OrthogonalCamera(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector) :
			base(cameraPosition, cameraTarget, cameraUpVector) 
		{
			//Orthogonol Camera so it can't be rotated
			rotatable = false;
			pannable = true;
			scalable = true;
		}

		public override bool DrawGrid(Device device)
		{
			return true;
		}

		public override Matrix CameraProjection
		{
			get 
			{
				// TODO: Make this a real orthogonal view
//				return Matrix.OrthoLH(5.0f, 5.0f, 0.0f, 100.0f);
				return Matrix.PerspectiveFovLH(cameraFOV, cameraAspectRatio, 
					cameraNearClipDistance, cameraFarClipDistance); 
			}
		}
	}

	class PerspectiveCamera : Camera
	{
		public PerspectiveCamera(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector) :
		base(cameraPosition, cameraTarget, cameraUpVector) 
		{
			//Perspective Camera so all movements are valid
			rotatable = true;
			pannable = true;
			scalable = true;
		}

		public override bool DrawGrid(Device device)
		{
			return true;
		}

		public override Matrix CameraProjection
		{
			get 
			{
				return Matrix.PerspectiveFovLH(cameraFOV, cameraAspectRatio, 
					cameraNearClipDistance, cameraFarClipDistance); 
			}
		}
	}
	
}