using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;

// TEMPORARY
using System.Windows.Forms;
using System.Drawing;

namespace Midget
{
	/// <summary>
	/// The root interface to all 3D objects used by Midget
	/// </summary>
	public interface IObject3D
	{
		bool Render(Device device);
		bool Translate(float x, float y, float z);
		bool Rotate(float x, float y, float z);
		bool Scale(float x, float y, float z);
		int	 Intersect(Vector3 rayPosition, Vector3 rayDirection);

		string Name { get; set; }
		AxisValue Translation { get; set; }
		AxisValue Rotation { get; set; }
		AxisValue Scaling { get; set; }
	}

	/// <summary>
	/// Structure which allows objects and cameras to store axis 
	/// coordinate/angle information easily
	/// </summary>
	public class AxisValue
	{
		private float x;
		private float y;
		private float z;

		public AxisValue()
		{
			x = 0.0f;
			y = 0.0f;
			z = 0.0f;
		}

		public float X
		{
			get { return x; }
			set { x = (float)value; }
		}

		public float Y
		{
			get { return y; }
			set { y = (float)value; }
		}

		public float Z
		{
			get { return z; }
			set { z = (float)value; }
		}
	}

	public abstract class PrimativeObject : IObject3D
	{
		protected VertexBuffer	vb;
		protected Matrix		manipulateMatrix;
		protected string		name;
		protected AxisValue		translation;
		protected AxisValue		rotation;
		protected AxisValue		scaling;

		public PrimativeObject()
		{
			manipulateMatrix = Matrix.Identity;

			DeviceManager.Instance.Device.VertexFormat = CustomVertex.TransformedColored.Format ;
			CustomVertex.TransformedColored [] verts = new CustomVertex.TransformedColored [3];

			verts [0] = new CustomVertex.TransformedColored 
				(5.0f, 0.5f, 0.5F, 1, Color.Red.ToArgb ());
				//(this .Width / 2, this .Height / 4, 0.5F, 1, Color.Blue.ToArgb ());

			verts [1] = new CustomVertex.TransformedColored
				(-3.0f, 1.5f, -3.0f, 1, Color.Orange.ToArgb ());
				//(this .Width * 3 / 4, this .Height * 3 / 4, 0.5F, 1, Color.Green.ToArgb ());

			verts [2] = new CustomVertex.TransformedColored
				(-2.0f, 3.0f, 0.5F, 1, Color.Red.ToArgb ());

			vb = new VertexBuffer ( typeof ( CustomVertex.TransformedColored ),
				verts.Length ,
				DeviceManager.Instance.Device,
				0,
				CustomVertex.TransformedColored.Format ,
				Pool.Default
				);

			//vb = buf;
		}

		public bool Render(Device device)
		{
			try
			{
				//device.Transform.World = manipulateMatrix;
				//vb = new VertexBuffer();
				device.SetStreamSource(0, vb, 0);
				device.DrawPrimitives(PrimitiveType.LineStrip, 0, 1);
			}
			catch
			{
				return false;
			}
			
			return true;
		}

		public bool Translate(float x, float y, float z) { return true; }
		public bool Rotate(float x, float y, float z) { return true; }
		public bool Scale(float x, float y, float z) { return true; }
		public int	 Intersect(Vector3 rayPosition, Vector3 rayDirection) { return -1; }

		public string Name {
			get
			{
				return "line";
			}
			set
			{
				name = value.ToString();
			}
		}

		public AxisValue Rotation
		{
			get { return rotation; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}

		public AxisValue Translation
		{
			get { return translation; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}

		public AxisValue Scaling
		{
			get { return scaling; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}
	}


	public abstract class MeshObject : IObject3D
	{
		protected Mesh		mesh;
		protected Matrix	manipulateMatrix;

		protected string	name;
		
		protected AxisValue		translation;
		protected AxisValue		rotation;
		protected AxisValue		scaling;

		public MeshObject()
		{
			manipulateMatrix = Matrix.Identity;

			name = "<mesh-object>";

			translation = new AxisValue();
			rotation = new AxisValue();
			scaling = new AxisValue();

			scaling.X = 1;
			scaling.Y = 1;
			scaling.Z = 1;

		}

		public bool Render(Device device)
		{
			try
			{
				device.Transform.World = manipulateMatrix;
				mesh.DrawSubset(0);
			}
			catch
			{
				return false;
			}
			
			return true;
		}

		public bool Translate(float x, float y, float z)
		{
			float relativeChangeX = x - translation.X;
			float relativeChangeY = y - translation.Y;
			float relativeChangeZ = z - translation.Z;

			translation.X = x;
			translation.Y = y;
			translation.Z = z;

			manipulateMatrix = Matrix.Multiply(manipulateMatrix, Matrix.Translation(relativeChangeX, 
				relativeChangeY, relativeChangeZ));

			// force re-render
			DeviceManager.Instance.Render();

			return true;
		}

		public bool Rotate(float x, float y, float z)
		{
			float relativeChangeX = x - rotation.X;
			float relativeChangeY = y - rotation.Y;
			float relativeChangeZ = z - rotation.Z;

			rotation.X = x;
			rotation.Y = y;
			rotation.Z = z;

			manipulateMatrix = Matrix.Multiply(manipulateMatrix, 
                                   Matrix.RotationX(relativeChangeX * (float)(Math.PI / 180)));
			manipulateMatrix = Matrix.Multiply(manipulateMatrix, 
                                   Matrix.RotationY(relativeChangeY * (float)(Math.PI / 180)));
			manipulateMatrix = Matrix.Multiply(manipulateMatrix, 
                                   Matrix.RotationZ(relativeChangeZ * (float)(Math.PI / 180)));

			// force re-render
			DeviceManager.Instance.Render();

			return true;
		}

		public bool Scale(float x, float y, float z)
		{
			float relativeChangeX = (x / scaling.X);
			float relativeChangeY = (y / scaling.Y);
			float relativeChangeZ = (z / scaling.Z);

			scaling.X = x;
			scaling.Y = y;
			scaling.Z = z;

			manipulateMatrix = Matrix.Multiply(manipulateMatrix, Matrix.Scaling(relativeChangeX, 
				relativeChangeY, relativeChangeZ));

			// force re-render
			DeviceManager.Instance.Render();

			return true;
		}

		public int Intersect(Vector3 rayPosition, Vector3 rayDirection)
		{
			// TEMPORARY - ignore the NRE generated by the line
			try
			{
				if (mesh.Intersect(rayPosition, rayDirection))
				{
					return 1;
				}
				else
				{
					return -1;
				}
			} 
			catch (NullReferenceException)
			{ return -1; }
		}

		/// <summary>
		/// Determines if an object name has been used in this system already. If it has, 
		/// a new value is passed back to replace it.
		/// </summary>
		/// <param name="originalName">The object name to be validated</param>
		/// <param name="isOriginal">True when 'originalName' is the first attempted name.</param>
		/// <returns></returns>
		public string ValidateName(string originalName, bool isOriginal)
		{
			string newName = originalName;

			// run through all the names in our current object list
			foreach (IObject3D obj in DeviceManager.Instance.ObjectList)
			{
				// if we have a match, we must change the name and validate again
				if ( (((MeshObject)obj).Name == originalName) )
				{
					// if this is the original name, simply add '1' and try again
					if (isOriginal)
					{
						newName = ValidateName(originalName + "1", false);
					}
					else	// if we are recursing, increment the number
					{
						int nextNumber = Convert.ToInt32(originalName.Substring(originalName.Length - 1, 1)) + 1;
						newName = ValidateName(originalName.Substring(0, originalName.Length - 1) + nextNumber.ToString(), false);
					}
				}
			}

			return newName;
		}

		public string Name
		{
			get { return name; }
			set { name = value.ToString(); }
		}

		public AxisValue Rotation
		{
			get { return rotation; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}

		public AxisValue Translation
		{
			get { return translation; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}

		public AxisValue Scaling
		{
			get { return scaling; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}

	}

	public class MeshTeapot : MeshObject
	{
		public MeshTeapot(Device device)
		{
			mesh = Mesh.Teapot(device);
			name = ValidateName("teapot", true);
		}
	}

	public class MeshSphere : MeshObject
	{
		public MeshSphere(Device device)
		{
			mesh = Mesh.Sphere(device, 1.0f, 15, 20);
			name = ValidateName("sphere", true);
		}
	}

	public class MeshTorus : MeshObject
	{
		public MeshTorus(Device device)
		{
			mesh = Mesh.Torus(device, 1.0f, 5.0f, 36, 36);
			name = ValidateName("torus", true);
		}
	}

	public class MeshBox : MeshObject
	{
		public MeshBox(Device device)
		{
			mesh = Mesh.Box(device, 2.0f, 2.0f, 2.0f);
			name = ValidateName("box", true);
		}
	}

	public class MeshCylinder : MeshObject
	{
		public MeshCylinder(Device device)
		{
			mesh = Mesh.Cylinder(device, 2.0f, 3.0f, 4.0f, 36, 1);
			name = ValidateName("cylinder", true);
		}
	}

	public class MeshPolygon : MeshObject
	{
		public MeshPolygon(Device device)
		{
			mesh = Mesh.Polygon(device, 2.0f, 5);
			name = ValidateName("n-polygon", true);
		}
	}

	public class MeshText : MeshObject
	{
		public MeshText(Device device)
		{
			System.Drawing.Font fnt = new System.Drawing.Font("Arial", 0.5f);
			mesh = Mesh.TextFromFont(device, fnt, "hi2u", 0.001f, 0.4f);
			name = ValidateName("text", true);
		}
	}

	// TEMPORARY
	public class MyLine : MeshObject
	{
		public MyLine(Device device)
		{
			name = "line";
		}
	}

}
