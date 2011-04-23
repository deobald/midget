using System;
using System.Collections;
using System.ComponentModel;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace Midget
{	
	[Serializable()]
	public abstract class MeshObject : Object3DCommon
	{	
		[NonSerialized()]
		internal Mesh		mesh;

		public MeshObject()
		{
			name = "<mesh-object>";
		}

		public override void Render(Device device, Matrix world, int keyframe)
		{

			// check for dynamics and paths
			// if there is none, interpolate position
			if (this.isDynamic || (this.Rigidity == Rigidity.Active))
			{
				CalculateDynamics(keyframe);
			}
			else if (this.hasPath)
			{
				CalculatePath(keyframe);
			}
			else
			{
				InterpolatePosition(keyframe);
			}

			world = Matrix.Multiply(scalingMatrix * rotationMatrix * translationMatrix,world);

			// render children
			this.RenderChildren ( device, world, keyframe );
			
			// materials
			if(selected)
				device.Material = selectedMaterial.Material;
			else
				device.Material = material.Material;
			
			device.SetTexture(0,material.Texture);
			device.TextureState[0].TextureCoordinateIndex = (int)TextureCoordinateIndex.CameraSpacePosition;
			//device.TextureState[0].TextureTransform = TextureTransform.Projected
			device.SamplerState[0].MinFilter = TextureFilter.Linear;
			device.SamplerState[0].MagFilter = TextureFilter.Linear;
			device.SamplerState[0].AddressU = TextureAddress.Wrap;
			device.SamplerState[0].AddressV = TextureAddress.Wrap;

			device.Transform.World = world;
			mesh.DrawSubset(0);
		}
		
		protected abstract void Initialize (Device device);

		public override void Reinitialize(Device device)
		{	
			// recreate myself
			Initialize(device);

			material.Initialize(device);
			selectedMaterial.Initialize(device);

			// if there are children present
			if (_children != null )
			{
				foreach (Object3DCommon childObj in _children)
				{
					childObj.Reinitialize(device);
				}
			}
		}
		
		protected int CheckIntersect(Vector3 rayPosition, Vector3 rayDirection)
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

		public override int Intersect ( Vector3 rayPosition, Vector3 rayDirection, ref Object3DCommon obj, Matrix worldSpace )
		{
		
			Matrix myModelSpace = scalingMatrix * rotationMatrix * translationMatrix * worldSpace;
			
			// transform world space to object space
			Vector3 pickRayOriginTemp = new Vector3(rayPosition.X, rayPosition.Y, rayPosition.Z);
			Vector3 pickRayDirectionTemp = new Vector3(rayDirection.X,rayDirection.Y,rayDirection.Z);

			// convert ray from 3d space to model space
			pickRayOriginTemp.TransformCoordinate(Matrix.Invert(myModelSpace));
			pickRayDirectionTemp.TransformNormal (Matrix.Invert(myModelSpace));
			
			// check to see if I intersect
			int zDistance = CheckIntersect(pickRayOriginTemp,pickRayDirectionTemp);
			if (zDistance > 0)
			{
				obj = this;
			}

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

	}
	
	[Serializable()]
	public class MeshTeapot : MeshObject
	{
		public MeshTeapot(Device device)
		{
			mesh = Mesh.Teapot(device);
			name = ValidateName("teapot", true);
		}

		protected override void Initialize (Device device)
		{
			mesh = Mesh.Teapot(device);
		}
	}
	
	[Serializable()]
	public class MeshSphere : MeshObject
	{
		private float radius;
		private int slices;
		private int stacks;

		public MeshSphere(Device device)
		{
			radius = 2.0f;
			slices = 15;
			stacks = 15;
			
			name = ValidateName("sphere", true);

			Initialize(device);
			


		}

		protected override void Initialize (Device device)
		{
			mesh = Mesh.Sphere(device, radius, slices, stacks);
		}
		
		[Description("The radius, in units, of the currently selected sphere.")]
		public float Radius
		{
			get { return radius; }
			set
			{
				radius = value; 
				Initialize(mesh.Device);
			}
		}

		[Description("The number of orange-peal slices in thes sphere.")]
		public int Slices
		{
			get { return slices; }
			set
			{ 
				slices = value; 
				Initialize(mesh.Device);
			}
		}

		[Description("The number of stacks (discs) which construct the sphere.")]
		public int Stacks
		{
			get { return stacks; }
			set
			{ 
				stacks = value; 
				Initialize(mesh.Device);
			}
		}
	}
	
	[Serializable()]
	public class MeshTorus : MeshObject
	{
		private float innerRadius;
		private float outerRadius;
		private int sides;
		private int rings;

		public MeshTorus(Device device)
		{	
			name = ValidateName("torus", true);

			innerRadius = 0.5f;
			outerRadius = 1.0f;
			sides = 36;
			rings = 36;

			Initialize(device);
	
		}

		protected override void Initialize (Device device)
		{
			mesh = Mesh.Torus(device, innerRadius, outerRadius, sides, rings);
		}

		[Description("The inner radius of the taurus' 2 defining circles.")]
		public float InnerRadius
		{
			get { return innerRadius; }
			set
			{
				innerRadius = value; 
				Initialize(mesh.Device);	
			}
		}

		[Description("The outer radius of the taurus' 2 defining circles.")]
		public float OuterRadius
		{
			get { return outerRadius; }
			set 
			{
				outerRadius = value;
				Initialize(mesh.Device);
			}
		}

		[Description("The number of sides (slices) to the torus")]
		public int Sides
		{
			get { return sides; }
			set 
			{
				sides = value;
				Initialize(mesh.Device);
			}
		}

		[Description("The number of rings (stacks) comprising the main ring of the torus.")]
		public int Rings
		{
			get { return rings; }
			set 
			{
				rings = value;
				Initialize(mesh.Device);
			}
		}

	}
	
	[Serializable()]
	public class MeshBox : MeshObject
	{
		private float width;
		private float height;
		private float depth;

		public MeshBox(Device device)
		{
			width = 2.0f;
			height = 2.0f;
			depth = 2.0f;

			Initialize(device);
			name = ValidateName("box", true);
		}
	
		protected override void Initialize (Device device)
		{
			mesh = Mesh.Box(device, width, height, depth);
		}

		[Description("The width of the box")]
		public float Width
		{
			get { return width; }
			set 
			{
				width = value;
				Initialize(mesh.Device);
			}
		}

		[Description("The height of the box")]
		public float Height
		{
			get { return height; }
			set 
			{
				height = value;
				Initialize(mesh.Device);
			}
		}

		[Description("The depth of the box")]
		public float Depth
		{
			get { return depth; }
			set 
			{
				depth = value;
				Initialize(mesh.Device);
			}
		}
	}
	
	[Serializable()]
	public class MeshCylinder : MeshObject
	{
		private float radius1;
		private float radius2;
		private float length;
		private int slices;
		private int stacks;

		public MeshCylinder(Device device)
		{	
			radius1 = 1.0f;
			radius2 = 1.0f;
			length = 2.0f;
			slices =  36;
			stacks = 4;
			
			name = ValidateName("cylinder", true);

			Initialize(device);
			
		}

		protected override void Initialize (Device device)
		{
			mesh = Mesh.Cylinder(device, radius1, radius2, length, slices, stacks);
		}

		[Description("The radious of one side of the cylinder")]
		public float Radius1
		{
			get { return radius1; }
			set
			{
				radius1 = value;
				Initialize(mesh.Device);
			}
		}

		[Description("The radius of the other side of cylinder")]
		public float Radius2
		{
			get { return radius2; }
			set
			{
				radius2 = value;
				Initialize(mesh.Device);
			}
		}

		[Description("The length of the cylinder")]
		public float Length
		{
			get { return length; }
			set
			{
				length = value;
				Initialize(mesh.Device);
			}
		}

		[Description("The slices of the cylinder")]
		public int Slices
		{
			get { return slices; }
			set
			{
				slices = value;
				Initialize(mesh.Device);
			}
		}

		[Description("The number of orange-peal slices in this cylinder")]
		public int Stacks
		{
			get { return stacks; }
			set
			{
				stacks = value;
				Initialize(mesh.Device);
			}
		}
	}
	
	[Serializable()]
	public class MeshPolygon : MeshObject
	{
		private float length;
		private int sides;

		public MeshPolygon(Device device)
		{
			Initialize(device);
			length = 2.0f;
			sides = 5;
		}

		public MeshPolygon(Device device, int sides)
		{
			Initialize(device,(float)sides / 10.0f, sides);
			name = ValidateName("n-polygon", true);
			this.sides = sides;
			this.length = ( (float)sides / 10.0f );
		}

		public MeshPolygon(Device device, float length, int sides)
		{
			Initialize(device, length, sides);
			name = ValidateName("n-polygon", true);
			this.sides = sides;
		}

		protected override void Initialize (Device device)
		{
			Initialize(device,length,sides);
		}

		private void Initialize(Device device, float length, int sides)
		{
			mesh = Mesh.Polygon(device, length, sides);
			DeviceManager.Instance.UpdateViews();
		}

		[Description("The length of the sides which make up the polygon.")]
		public float Length
		{
			get { return length; }
			set
			{
				length = value;
				Initialize(mesh.Device, length, sides);
			}
		}

		[Description("The number of sides to the selected n-sided polygon.")]
		public int Sides
		{
			get { return sides; }
			set
			{
				sides = value;
				Initialize(mesh.Device, length, sides);
			}
		}
	}
	
	[Serializable()]
	public class MeshText : MeshObject
	{
		private string text;
		private System.Drawing.Font font;

		public MeshText(Device device)
		{
			Initialize(device);
			name = ValidateName("text", true);
			text = "Midget3D";
			font = new System.Drawing.Font("Arial", 0.5f);
		}

		public MeshText(Device device, System.Drawing.Font font, string text)
		{
			
			this.text = text;
			this.font = font;
			
			name = ValidateName("text", true);

			Initialize(device);
		}

		protected override void Initialize (Device device)
		{

			mesh = Mesh.TextFromFont(device, font, text, 0.001f, 0.4f);
			DeviceManager.Instance.UpdateViews();
		}

		[Description("Text to display in 3D.")]
		public string Text
		{
			get { return text; }
			set
			{
				text = value.ToString();
				Initialize(mesh.Device);
			}
		}

		[Description("Font to render text in.")]
		public System.Drawing.Font Font
		{
			get { return font; }
			set
			{
				font = value;
				Initialize(mesh.Device);
			}
		}
	}
}