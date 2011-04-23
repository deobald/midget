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
			MeshCylinder, MeshPolygon, MeshText, PolySphere, MeshControlPoint, Curve, ParticleSystem };
			
		#endregion

		private ObjectFactory()
		{}

		public static readonly ObjectFactory Instance = new ObjectFactory();

		/// <summary>
		/// Creates a new 3d object and its to the scene
		/// </summary>
		/// <param name="iObjectType">The type of object that is too be created</param>
		/// <returns>Whether or not a new 3D object could be successfully added to the scene</returns>
		public static IObject3D CreateObject(int iObjectType)
		{
			// the new object we're creating - this variable is required to fire the CreateObject event
			IObject3D newObject;
			
			switch (iObjectType)
			{
				case (int)ObjectTypes.MeshTeapot:
					newObject = new MeshTeapot(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.MeshSphere:
					newObject = new MeshSphere(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.MeshTorus:
					newObject = new MeshTorus(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.MeshBox:
					newObject = new MeshBox(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.MeshCylinder:
					newObject = new MeshCylinder(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.MeshPolygon:
					newObject = new MeshPolygon(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.MeshText:
					newObject = new MeshText(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.Curve:
					newObject = new Curve(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.MeshControlPoint:
					newObject = new MeshCtrlPt(DeviceManager.Instance.Device);
					break;

				case (int)ObjectTypes.ParticleSystem:
					newObject = new ParticleSystem(DeviceManager.Instance.Device);
					break;

				default:
					return null;
			}
			
			return newObject;
		}

		/// <summary>
		/// Similar to CreateObject(int), this version allows us to enter user data for datatypes 
		/// such as MeshText
		/// </summary>
		/// <param name="iObjectType">The enumerated type of the object we are creating</param>
		/// <param name="text">The text to initialize the object with</param>
		/// <returns>Whether or not the object was successfully created</returns>
		public static IObject3D CreateObject(int iObjectType, string text)
		{
			IObject3D newObject;

			switch (iObjectType)
			{
				case (int)ObjectTypes.MeshPolygon:
					// math makes it very difficult for us to have a polygon of less than 3 sides, so check
					// to make sure we have 3 or more
					int numSides = Convert.ToInt32(text);
					if (numSides > 2)
					{
						newObject = new MeshPolygon(DeviceManager.Instance.Device, numSides);
					}
					else
					{
						return null;
					}
					break;

				case (int)ObjectTypes.MeshText:
					System.Drawing.Font font = new System.Drawing.Font("Arial", 0.5f);
					newObject = new MeshText(DeviceManager.Instance.Device, font, text);
					break;
						
			

				default:
					return null;
			}

				return newObject;
		}

	}
}
