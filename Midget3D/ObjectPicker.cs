using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{
	/// <summary>
	/// Summary description for ObjectPicker.
	/// </summary>
	sealed public class ObjectPicker
	{
		public static IObject3D Pick(int X, int Y, Camera camera)
		{
			Vector3 pickRayDirection	= new Vector3();
			Vector3 pickRayOrigin		= new Vector3();

			// compute the vector of the pick ray in screen space
			Vector3 sSpace = new Vector3();

//			dx=tanf(FOV*0.5f)*(x/WIDTH_DIV_2-1.0f)/ASPECT;
//			dy=tanf(FOV*0.5f)*(1.0f-y/HEIGHT_DIV_2);

//			sSpace.X = (float)(Math.Tan(camera.CameraFOV * 0.5f) * (X / 180.0f - 1.0f) / (camera.CameraAspectRation));
//			sSpace.Y = (float)(Math.Tan(camera.CameraFOV * 0.5f) * (1.0f - Y / 152.0f));
//
			sSpace.X = (2.0f * (float)X / 360.0f - 1) / camera.CameraProjection.M11;
			sSpace.Y = - (2.0f * (float)Y / 304.0f - 1) / camera.CameraProjection.M22;
			sSpace.Z = 1.0f;
		
			// get inverse view matrix
//		Matrix inverse = Matrix.Invert(Matrix.LookAtLH(							new Vector3(0, 0, 0), 
//						new Vector3(0, 0, 1), 
//							new Vector3(0, 1, 0))); 
			Matrix inverse = Matrix.Invert(camera.CameraView);
			
			
			// transform the screen space pick ray to 3d space
			pickRayDirection.X  = sSpace.X*inverse.M11 + sSpace.Y*inverse.M21 + sSpace.Z*inverse.M31;
			pickRayDirection.Y  = sSpace.X*inverse.M12 + sSpace.Y*inverse.M22 + sSpace.Z*inverse.M32;
			pickRayDirection.Z  = sSpace.X*inverse.M13 + sSpace.Y*inverse.M23 + sSpace.Z*inverse.M33;
	
			pickRayDirection.Normalize();

			pickRayOrigin.X = inverse.M41;
			pickRayOrigin.Y = inverse.M42;
			pickRayOrigin.Z = inverse.M43;

			// calc origin as intersection with near frustum
			pickRayOrigin += pickRayDirection * camera.CameraNearClipDistance;
			
			// convert ray from 3d space to model space
			pickRayOrigin.TransformCoordinate(Matrix.Invert(DeviceManager.Instance.Device.Transform.World));	//Matrix.Invert(Matrix.Identity));
			pickRayDirection.TransformNormal(Matrix.Invert(DeviceManager.Instance.Device.Transform.World));	//inverse);

			// TEST: see where this will draw a line
			MyLine someLine = new MyLine(DeviceManager.Instance.Device);
			DeviceManager.Instance.AddObject(someLine); 

			foreach (IObject3D obj in DeviceManager.Instance.ObjectList)
			{
				if (obj.Intersect(pickRayOrigin, pickRayDirection) >= 0)
				{
					return obj;
				}
			}

			return null;
		}

		//public static IObject3D Pick(int X, int Y, Camera camera)
		public static IObject3D Pick2(int X, int Y, Camera camera, Control viewport)
		{
			//void calcRay(int x,int y,D3DVECTOR &p1,D3DVECTOR &p2)
//			float dx,dy;
//			Vector3 p1, p2, inv, view;
			//D3DMATRIX invMatrix,viewMatrix;
/*
			dx = Math.Tan(camera.CameraFOV * 0.5f) * (X / (viewport.Width / 2) - 1.0f) / (viewport.Height / viewport.Width);
			dy = Math.Tan(camera.CameraFOV * 0.5f) * (1.0f - Y / (viewport.Height / 2));
			view = camera.CameraView;
			inv = Matrix.Invert(view);

			p1 = new Vector3(dx * camera.CameraNearClipDistance, dy * camera.CameraNearClipDistance, camera.CameraNearClipDistance);
			p2 = new Vector3(dx * camera.CameraFarClipDistance, dy * camera.CameraFarClipDistance, camera.CameraFarClipDistance);

			p1 = Vector3.TransformCoordinate(p1,invMatrix);*/
			return null;
			//p2 = 



			/*dx=tanf(FOV*0.5f)*(x/WIDTH_DIV_2-1.0f)/ASPECT;
			dy=tanf(FOV*0.5f)*(1.0f-y/HEIGHT_DIV_2);
			lpDevice->GetTransform(D3DTRANSFORMSTATE_VIEW,&viewMatrix);
			D3DMath_MatrixInvert(invMatrix,viewMatrix);

			p1=D3DVECTOR(dx*NEAR,dy*NEAR,NEAR);
			p2=D3DVECTOR(dx*FAR,dy*FAR,FAR);
			
			D3DMath_VectorMatrixMultiply(p1,p1,invMatrix);
			D3DMath_VectorMatrixMultiply(p2,p2,invMatrix);*/
		}
	
	}
}
