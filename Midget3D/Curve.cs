using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{
	[Serializable()]
	public class MeshCtrlPt : MeshObject
	{
		[NonSerialized]
		private Device device;

		// the regular constructor that the object factory calls.
		public MeshCtrlPt(Device device)
		{
			this.device = device;
			mesh = Mesh.Sphere(device, 1.0f, 15, 20);
			name = ValidateName("Control Point", true);
		}

		// a constructor that only changes the translation,  not the size
		public MeshCtrlPt (Device device, float x, float y, float z, IObject3D owner)
		{
			this.device = device;
			mesh = Mesh.Sphere(device, 1.0f, 15, 20);
			name = ValidateName("Control Point", true);
			this.Translate(x, y, z);
			if (owner != null)
				Parent = owner;
		}
		
		// a constructor, that when called has ultimate power over the sphere.
		public MeshCtrlPt (Device device, float x, float y, float z, float scale, int slices, int stacks, IObject3D owner)
		{	
			this.device = device;
			mesh = Mesh.Sphere(device, scale, slices, stacks);
			name = ValidateName("Control Point", true);
			this.Translate(x, y, z);
			if (owner != null)
				Parent = owner;
		}

		// the Scaling function is no longer acessable.  user is not able to scale control points.
		public new void Scale(float x, float y, float z)
		{}

		// whenever a control point is moved, the curve must be redrawen,  and the manipulte
		// matrix must be updated.
		public new void Translate(float x, float y, float z)
		{
			float relativeChangeX = x - translation.X;
			float relativeChangeY = y - translation.Y;
			float relativeChangeZ = z - translation.Z;

			translation.X = x;
			translation.Y = y;
			translation.Z = z;
			
			if (this.Parent is Curve)
			{
				((Curve)this.Parent).CalcCatmullRom(this.device);
			}
			
			Matrix tempTranslationMatrix = Matrix.Translation(relativeChangeX, relativeChangeY, relativeChangeZ);

			translationMatrix = Matrix.Multiply(tempTranslationMatrix, translationMatrix);
		}

		protected override void Initialize (Device device)
		{
			mesh = Mesh.Sphere(device, 0.1f, 15, 20);
			this.device = device;
		}

	}


	/// <summary>
	/// This is the main curve object,  It has many children which are control points.  from those control
	/// points it generates an array list of vertices.   then lines are drawn inbetween each.
	/// 
	/// </summary>
	/// 
	[Serializable()]
	public class Curve : Object3DCommon
	{
		[NonSerialized]
		private Device device;
		private ArrayList pointList = new ArrayList();
		private ArrayList vertexObjectList = new ArrayList();
		private ArrayList vertexList;
		private float totDist = 0;
		private int vertPerSection = 50;
		
		public Curve(Device device) 
		{
			this.device = device;
			name = ValidateName("Curve", true);

			_children = new ArrayList();

			IObject3D onePoint;
			IObject3D twoPoint;
			IObject3D threePoint;
			IObject3D fourPoint;

			// automatically adds 4 control points to get the first line.
			//	( float x, float y, float z, float scale, int slices, int stacks)
			onePoint = new MeshCtrlPt(device,    0.5f, 0.5f, 0.5f, 0.1f, 10, 10, this);
			twoPoint = new MeshCtrlPt(device,    1, 1, 0.5f, 0.1f, 10, 10, this);
			threePoint = new MeshCtrlPt(device,  2, 1, 0.5f, 0.1f, 10, 10, this);
			fourPoint = new MeshCtrlPt(device,   2.5f, 0.5f, 0.5f, 0.1f, 10, 10, this);
			
			_children.Add(onePoint);
			_children.Add(twoPoint);
			_children.Add(threePoint);
			_children.Add(fourPoint);

			CalcCatmullRom(device);	
		}
		
		// creates the vertix buffer that will hold all the vertices and the line will
		// reference to draw
		protected VertexBuffer CreateVertexBuffer(Device device)
		{
			device.VertexFormat = CustomVertex.PositionColored.Format;
 
			VertexBuffer buf = new VertexBuffer(
				typeof (CustomVertex.PositionColored), // What type of vertices
				vertexList.Count,                     // How many
				device,                               // The device
				0,                                    // Default usage
				CustomVertex.PositionColored.Format,  // Vertex format
				Pool.Managed);                        // Default pooling

			CustomVertex.PositionColored[] verts =
				(CustomVertex.PositionColored[]) buf.Lock(0, 0);

			for( int i = 0; i < vertexList.Count; ++i)
			{
				verts[i] = new CustomVertex.PositionColored(
					(Vector3)vertexList[i],Color.White.ToArgb());
			}
							
			buf.Unlock();
			return buf;

		} 

		public void AddCtrlPt ()
		{
			IObject3D newPoint;

			newPoint = new MeshCtrlPt(device, 1, 0, 0, 0.1f, 10, 10, this);

			if( _children == null)
				_children = new ArrayList();

			_children.Add(newPoint);
			CalcCatmullRom(device);
		}

		// this function is used for path following.  the path following needs to know the total distance in order
		// to be able to calculate how far it can go each frame.
		public float CalcDist()
		{
			float distance = 0;
			for (int i =0 ; i < vertexList.Count-1; i++)
			{
				// basic formula dist = root(a^2 + b^2 + c ^2)
				distance = distance + (float)Math.Sqrt(Math.Pow(((Vector3)vertexList[i]).X - ((Vector3)vertexList[i+1]).X, 2)
											   + Math.Pow(((Vector3)vertexList[i]).Y - ((Vector3)vertexList[i+1]).Y, 2)
											   + Math.Pow(((Vector3)vertexList[i]).Z - ((Vector3)vertexList[i+1]).Z, 2));
		
			}
			return distance;
		}

		// vertex calculating function
		public void CalcCatmullRom(Device device)
		{
			vertexList = new ArrayList(); // clear vertexList..  important

			for (int j = 0; j < (_children.Count - 3); j++) // repeated for every control pt
			{
				for (int og = 0; og < vertPerSection; og++) // repeated for every vertex
				{
					Vector3[] p = new Vector3[4];

					for (int i = 0; i < 4; i++)
					{
						int controlPosition = j + i;
						p[i].X = ((IObject3D)_children[controlPosition]).Translation.X;
						p[i].Y = ((IObject3D)_children[controlPosition]).Translation.Y;
						p[i].Z = ((IObject3D)_children[controlPosition]).Translation.Z;
					}

					vertexList.Add(Vector3.CatmullRom(p[0], p[1], p[2], p[3], (float)(og / (float)vertPerSection)));
					totDist = CalcDist();
				}
			}
		}

		public new void AddChild(IObject3D child)
		{
			// init array if no children already exist, delay init to save memory
			if( _children == null)
				_children = new ArrayList();
			
			// add the child
			_children.Add(child);
		}

		public override void Render( Device device, Matrix world, int keyframe )
		{	
			// calculate the curve using CatmullRom
			CalcCatmullRom(device);

			if (!this.isDynamic)
			{
				InterpolatePosition(keyframe);
			}
			else
			{
				CalculateDynamics(keyframe);
			}

			world = Matrix.Multiply(scalingMatrix * rotationMatrix * translationMatrix,world);

			// render children
			this.RenderChildren ( device, world, keyframe );
			
			device.Transform.World = world;
			
			// render the line
			VertexBuffer vertBuffer = CreateVertexBuffer(device);
			device.SetStreamSource(0, vertBuffer,0);
			device.DrawPrimitives(PrimitiveType.LineStrip, 0, (int)(vertexList.Count / 1.01f));
		}

		public override void Reinitialize(Device device)
		{
			device = this.device;
			// if there are children present
			if (_children != null )
			{
				foreach (Object3DCommon childObj in _children)
				{
					childObj.Reinitialize(device);
				}
			}
		}

		public override int Intersect ( Vector3 rayPosition, Vector3 rayDirection, ref Object3DCommon obj, Matrix worldSpace )
		{
			Matrix myModelSpace = scalingMatrix * rotationMatrix * translationMatrix * worldSpace;

			// TODO: Ryan wants to add real curve intersection here
			// for now, we do a fake init on z-distance:
			int zDistance = 0;

			if (_children != null)
			{
				foreach (Object3DCommon childObj in _children)
				{
					int newZDistance = childObj.Intersect(rayPosition, rayDirection, ref obj, myModelSpace);
				
					if (newZDistance > zDistance)
					{
						zDistance = newZDistance;
					}
				}
			}

			return zDistance;
		}

		public new void RemoveChild(IObject3D child)
		{
			// will throw exception if child doesn't exist
				
			_children.Remove(child);
			CalcCatmullRom(this.device);
		}

		[Browsable(false)]
		public ArrayList PointList
		{
			get { return vertexList; }
		}
		
		[Browsable(false)]
		public float TotalDis
		{
			get { return totDist;}
		}


	}
}