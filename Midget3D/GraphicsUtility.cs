///////////////////////////////////////////////////////////////////
//
//	Note: This file contains classes and functions found in the 
//		  DirectX 9.0a SDK
//
///////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace Midget
{
	/// <summary>
	/// Midget graphics utilties for simplifying surface manipulation
	/// </summary>
	public sealed class GraphicsUtility
	{
		private GraphicsUtility()
		{
			// not used
		}

		/// <summary>
		/// Axis to axis quaternion 
		/// Takes two points on unit sphere an angle THETA apart, returns
		/// quaternion that represents a rotation around cross product by theta.
		/// </summary>
		public static Quaternion D3DXQuaternionAxisToAxis(Vector3 fromVector, Vector3 toVector)
		{
			Vector3 vA = Vector3.Normalize(fromVector), vB = Vector3.Normalize(toVector);
			Vector3 vHalf = Vector3.Add(vA,vB);
			vHalf = Vector3.Normalize(vHalf);
			return GraphicsUtility.D3DXQuaternionUnitAxisToUnitAxis2(vA, vHalf);
		}

		/// <summary>
		/// Axis to axis quaternion double angle (no normalization)
		/// Takes two points on unit sphere an angle THETA apart, returns
		/// quaternion that represents a rotation around cross product by 2*THETA.
		/// </summary>
		public static Quaternion D3DXQuaternionUnitAxisToUnitAxis2(Vector3 fromVector, Vector3 toVector)
		{
			Vector3 axis = Vector3.Cross(fromVector, toVector);    // proportional to sin(theta)
			return new Quaternion(axis.X, axis.Y, axis.Z, Vector3.Dot(fromVector, toVector));
		}

		/// <summary>
		/// Helper function to create a texture. It checks the root path first,
		/// then tries the DXSDK media path (as specified in the system registry).
		/// </summary>
		public static Texture CreateTexture(Device device, string textureFilename, Format format)
		{
			// Get the path to the texture
			string path = DXUtil.FindMediaFile(null, textureFilename);

			// Create the texture using D3DX
			return TextureLoader.FromFile(device, path, D3DX.Default, D3DX.Default, D3DX.Default, 0, format, 
				Pool.Managed, Filter.Triangle|Filter.Mirror, 
				Filter.Triangle|Filter.Mirror, 0);
		}




		/// <summary>
		/// Helper function to create a texture. It checks the root path first,
		/// then tries the DXSDK media path (as specified in the system registry).
		/// </summary>
		public static Texture CreateTexture(Device device, string textureFilename)
		{
			return GraphicsUtility.CreateTexture(device, textureFilename, Format.Unknown);
		}
	}
}
