using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{
	/// <summary>
	/// Function to determine which object was selected, if any
	/// </summary>
	sealed public class ObjectPicker
	{
		public static IObject3D Pick(int X, int Y, Midget.Cameras.MidgetCamera camera, IRenderSurface renderSurface)
		{
			Vector3 pickRayDirection	= new Vector3();
			Vector3 pickRayOrigin		= new Vector3();
		
			camera.UnProjectCoordinates(X,Y, renderSurface.ClientWidth, renderSurface.ClientHeight, ref pickRayOrigin, ref pickRayDirection);

			//			// compute the vector of the pick ray in screen space
			//			Vector3 v = new Vector3();
			//			v.X =  ( ( ( 2.0f * X ) / renderSurface.ClientWidth ) - 1 ) / camera.ProjectionMatrix.M11;
			//			v.Y = -( ( ( 2.0f * Y ) / renderSurface.ClientHeight ) - 1 ) / camera.ProjectionMatrix.M22;
			//			v.Z =  1.0f;
			//		
			//			// Get the inverse view matrix
			//			Matrix m = Matrix.Invert(camera.ViewMatrix);
			//		
			//			// Transform the screen space pick ray into 3D space
			//			pickRayDirection.X  = v.X*m.M11 + v.Y*m.M21 + v.Z*m.M31;
			//			pickRayDirection.Y  = v.X*m.M12 + v.Y*m.M22 + v.Z*m.M32;
			//			pickRayDirection.Z  = v.X*m.M13 + v.Y*m.M23 + v.Z*m.M33;
			//		
			//			pickRayDirection.Normalize();
			//
			//			pickRayOrigin.X = m.M41;
			//			pickRayOrigin.Y = m.M42;
			//			pickRayOrigin.Z = m.M43;
			//		
			//			// calc origin as intersection with near frustum
			//
			//			pickRayOrigin += pickRayDirection * 1.0f;

			
			//			int zDistance = -1;
			//			IObject3D closestObj = null;


			//			foreach (IObject3D obj in DeviceManager.Instance.ObjectList)
			//			{
			//				
			//				// transform world space to object space
			//				Vector3 pickRayOriginTemp = new Vector3(pickRayOrigin.X, pickRayOrigin.Y, pickRayOrigin.Z);
			//				Vector3 pickRayDirectionTemp = new Vector3(pickRayDirection.X,pickRayDirection.Y,pickRayDirection.Z);
			//
			//				// convert ray from 3d space to model space
			//				pickRayOriginTemp.TransformCoordinate(Matrix.Invert(obj.WorldSpace));	//Matrix.Invert(Matrix.Identity));
			//				pickRayDirectionTemp.TransformNormal(Matrix.Invert(obj.WorldSpace));	//inverse);
			//
			//				
			//				if (obj.Intersect(pickRayOriginTemp, pickRayDirectionTemp) > zDistance)
			//				{
			//					closestObj = obj;
			//				}
			//			}

			return SceneManager.Instance.Scene.Intersect(pickRayOrigin, pickRayDirection, camera.WorldMatrix);

			//return closestObj;
		}
	}
}		

