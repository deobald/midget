using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget.Cameras
{

	/// <summary>
	/// Midget Camera that is orthogonal
	/// </summary>
	public class PerspectiveMidgetCamera : MidgetCamera
	{	
		/// <summary>
		/// Creates a new PerspectiveMidgetCamera
		/// </summary>
		/// <param name="eyePosition">The position of the camera in 3d space</param>
		/// <param name="targetPoint">The position that camera is targeted at</param>
		/// <param name="upVector">The up direction for the camera</param>
		/// <param name="name">The name of the camera</param>
		public PerspectiveMidgetCamera(Vector3 eyePosition, Vector3 targetPoint, Vector3 upVector, string name)
			: base(eyePosition, targetPoint, upVector, name, true, true, true)
		{
		}

		/// <summary>
		/// Draws view port background grid
		/// </summary>
		/// <param name="device">The 3d device where the grid is to be drawn too</param>
		public override void DrawGrid(Device device)
		{
			//Add code to do drawing later
		}

		

		public override void ProjectCoordinates(ref int x, ref int y, int width, int height, 
			Vector3 pickRayOrigin)	//, Vector3 pickRayDirection)
		{
//			Vector3 v = new Vector3();
//
//			pickRayOrigin -= pickRayDirection;
//
//			Matrix m = Matrix.Identity(_viewMatrix);
//
//			// un-transform
//			v.X = pickRayOrigin.X / m.M11 + 


			Vector4 ind = new Vector4();
			Vector4 outd = new Vector4();

			ind.X = pickRayOrigin.X;
			ind.Y = pickRayOrigin.Y;
			ind.Z = pickRayOrigin.Z;
			ind.W = 1.0f;

//			Vector3 ind3 = new Vector3(ind.X, ind.Y, ind.Z);
//			outd = Vector3.Transform(ind3, WorldMatrix);
//			Vector3 outd3 = new Vector3(outd.X, outd.Y, outd.Z);
//			ind = Vector3.Transform(outd3, _projectionMatrix);

			outd = Vector4.Transform(ind, WorldMatrix);
			ind = Vector4.Transform(outd, _projectionMatrix);

			if (ind.Z == 0.0f) { return; }

			ind.X /= ind.W;
			ind.Y /= ind.W;
			ind.Z /= ind.W;

			/* Map x, y and z to range 0-1 */
			ind.X = ind.X * 0.5f + 0.5f;
			ind.Y = ind.Y * 0.5f + 0.5f;
			ind.Z = ind.Z * 0.5f + 0.5f;

			/* Map x,y to viewport */
			ind.X = ind.X * width;
			ind.Y = ind.Y * height;

			x = (int)ind.X;
			y = (int)ind.Y;
		}

/*
		GLint gluProject(GLdouble objx, GLdouble objy, GLdouble objz, 
			const GLdouble modelMatrix[16], 
			const GLdouble projMatrix[16],
			const GLint viewport[4],
			GLdouble *winx, GLdouble *winy, GLdouble *winz)
		{

			double in[4];
			double out[4];

			in[0]=objx;
			in[1]=objy;
			in[2]=objz;
			in[3]=1.0;
			__gluMultMatrixVecd(modelMatrix, in, out);
			__gluMultMatrixVecd(projMatrix, out, in);
			if (in[3] == 0.0) return(GL_FALSE);
			in[0] /= in[3];
			in[1] /= in[3];
			in[2] /= in[3];

			// Map x, y and z to range 0-1 //
			in[0] = in[0] * 0.5 + 0.5;
			in[1] = in[1] * 0.5 + 0.5;
			in[2] = in[2] * 0.5 + 0.5;

			// Map x,y to viewport //
			in[0] = in[0] * viewport[2] + viewport[0];
			in[1] = in[1] * viewport[3] + viewport[1];

			*winx=in[0];
			*winy=in[1];
			*winz=in[2];
			return(GL_TRUE);
		}
*/

		public override void UnProjectCoordinates( int x, int y, int width, int height, ref Vector3 pickRayOrigin, ref Vector3 pickRayDirection)
		{
	
			// compute the vector of the pick ray in screen space
			Vector3 v = new Vector3();
			v.X =  ( ( ( 2.0f * x ) / width ) - 1 ) / _projectionMatrix.M11;
			v.Y = -( ( ( 2.0f * y ) / height ) - 1 ) / _projectionMatrix.M22;
			v.Z =  1.0f;
		
			// Get the inverse view matrix
			Matrix m = Matrix.Invert(_viewMatrix);
		
			// Transform the screen space pick ray into 3D space
			pickRayDirection.X  = (v.X * m.M11) + (v.Y * m.M21) + (v.Z * m.M31);
			pickRayDirection.Y  = (v.X * m.M12) + (v.Y * m.M22) + (v.Z * m.M32);
			pickRayDirection.Z  = (v.X * m.M13) + (v.Y * m.M23) + (v.Z * m.M33);
		
			pickRayDirection.Normalize();

			pickRayOrigin.X = m.M41;
			pickRayOrigin.Y = m.M42;
			pickRayOrigin.Z = m.M43;
		
			// calc origin as intersection with near frustum

			pickRayOrigin += pickRayDirection * 1.0f;
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

			// create new projection matrix
			_projectionMatrix = Matrix.PerspectiveFovLH( fov, aspectRatio, nearClipPlane, farClipPlane );
		}
	}
}
