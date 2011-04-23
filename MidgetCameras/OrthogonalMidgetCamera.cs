using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget.Cameras
{
	/// <summary>
	/// Midget Camera that is orthogonal
	/// </summary>
	public class OrthogonalMidgetCamera : MidgetCamera
	{	
		private Vector3 viewDirection;

		/// <summary>
		/// Creates a new OrthononalMidgetCamera
		/// </summary>
		/// <param name="eyePosition">The position of the camera in 3d space</param>
		/// <param name="targetPoint">The position that camera is targeted at</param>
		/// <param name="upVector">The up direction for the camera</param>
		/// <param name="name">The name of the camera</param>
		public OrthogonalMidgetCamera(Vector3 eyePosition, Vector3 targetPoint, Vector3 upVector, string name)
			: base(eyePosition, targetPoint, upVector, name, false, true, true)
		{
			// find the view direction and remove decimal places
			viewDirection = targetPoint - eyePosition;
			viewDirection.X = (float)Math.Round(viewDirection.X);
			viewDirection.Y = (float)Math.Round(viewDirection.Y);
			viewDirection.Z = (float)Math.Round(viewDirection.Z);
			
			// normalize (without negating) the vector values to 0 or 1
			if (viewDirection.X != 0)
				viewDirection.X /= Math.Abs(viewDirection.X);
			if (viewDirection.Y != 0)
				viewDirection.Y /= Math.Abs(viewDirection.Y);
			if (viewDirection.Z != 0)
				viewDirection.Z /= Math.Abs(viewDirection.Z);

			// set orthogonal zoom
			orthoZoom = 4.0f;
		}

		/// <summary>
		/// Draws view port background grid
		/// </summary>
		/// <param name="device">The 3d device where the grid is to be drawn too</param>
		public override void DrawGrid(Device device)
		{
			//Add code to do drawin later
		}

		public override void ProjectCoordinates(ref int x, ref int y, int width, int height, Vector3 pickRayOrigin)	//, Vector3 pickRayDirection)
		{

		}


		public override void UnProjectCoordinates( int x, int y, int width, int height, ref Vector3 pickRayOrigin, ref Vector3 pickRayDirection)
		{
	
			Vector3 v = new Vector3();

			v.X =  ( ( ( 2.0f * x ) / width ) - 1 ) / _projectionMatrix.M11;
			v.Y = -( ( ( 2.0f * y ) / height ) - 1 ) / _projectionMatrix.M22;
			v.Z = 0.0f;

			// Get the inverse view matrix
			Matrix m = Matrix.Invert(_viewMatrix);
					
			// Transform the screen space pick ray into 3D space
			pickRayOrigin.X  = v.X*m.M11 + v.Y*m.M21 + v.Z*m.M31;
			pickRayOrigin.Y  = v.X*m.M12 + v.Y*m.M22 + v.Z*m.M32;
			pickRayOrigin.Z  = v.X*m.M13 + v.Y*m.M23 + v.Z*m.M33;

			// for ortho cameras, we must extrapolate the ray's start point away from the 
			// origin of the world, so we multiply the direction by a large number inverse
			// to the way the camera is pointing
			pickRayOrigin += viewDirection * (-100000.0f);

			// set our ray direction to that of the ortho camera
			pickRayDirection = viewDirection;
		}

		/// <summary>
		/// Changes the components of the projection Matrix for the camera
		/// </summary>
		/// <param name="fov">The FOV for the camera</param>
		/// <param name="aspectRatio">The Aspect Ratio of the particular camera (Height/Width)</param>
		/// <param name="nearClipPlane">Near object clipping plane</param>
		/// <param name="farClipPlane">Far object clipping plane</param>
		public override void SetProjectionParameters (float fov, float aspectRatio, float nearClipPlane, float farClipPlane)
		{
			// set all the properties for projection matrix
			_fov			= fov;
			_aspectRatio	= aspectRatio;
			_nearClipPlane	= nearClipPlane;
			_farClipPlane	= farClipPlane;

			// error check ortho zoom
			if (orthoZoom == 0.0f)
			{
				orthoZoom = 4.0f;
			}

			// create new projection matrix
			_projectionMatrix = Matrix.OrthoLH(aspectRatio * orthoZoom, 1.0f * orthoZoom, nearClipPlane, farClipPlane);
		}
	}
}
