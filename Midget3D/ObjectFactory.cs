using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{
	/// <summary>
	/// The OrbjectFactory is responsible for the creation of the scene's 3d objects
	/// </summary>
	sealed public class ObjectFactory
	{

		#region Public Enumerations

		/// <summary>
		/// Object types and corresponding names
		/// </summary>
		public enum ObjectTypes { MeshTeapot, MeshSphere, MeshTorus, MeshBox, 
			MeshCylinder, MeshPolygon, MeshText, PolySphere };
			
		#endregion

		private ObjectFactory()
		{}

		public static readonly ObjectFactory Instance = new ObjectFactory();

		/// <summary>
		/// Creates a new 3d object and its to the scene
		/// </summary>
		/// <param name="iObjectType">The type of object that is too be created</param>
		/// <returns>Whether or not a new 3D object could be successfully added to the scene</returns>
		public static bool CreateObject(int iObjectType)
		{
			switch (iObjectType)
			{
				case (int)ObjectTypes.MeshTeapot:
					DeviceManager.Instance.AddObject(new MeshTeapot(DeviceManager.Instance.Device));
					break;

				case (int)ObjectTypes.MeshSphere:
					DeviceManager.Instance.AddObject(new MeshSphere(DeviceManager.Instance.Device));
					break;

				case (int)ObjectTypes.MeshTorus:
					DeviceManager.Instance.AddObject(new MeshTorus(DeviceManager.Instance.Device));
					break;

				case (int)ObjectTypes.MeshBox:
					DeviceManager.Instance.AddObject(new MeshBox(DeviceManager.Instance.Device));
					break;

				case (int)ObjectTypes.MeshCylinder:
					DeviceManager.Instance.AddObject(new MeshCylinder(DeviceManager.Instance.Device));
					break;

				case (int)ObjectTypes.MeshPolygon:
					DeviceManager.Instance.AddObject(new MeshPolygon(DeviceManager.Instance.Device));
					break;

				case (int)ObjectTypes.MeshText:
					DeviceManager.Instance.AddObject(new MeshText(DeviceManager.Instance.Device));
					break;

				default:
					break;
			}

			return true;
		}

	}
}
