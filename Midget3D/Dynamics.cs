using System;
using System.ComponentModel;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{
	
	/// <summary>
	/// Enumerate the rigidity of an object
	/// </summary>
	/// 
	[Serializable()]
	public enum Rigidity { Active, Passive, None };

	/// <summary>
	/// Summary description for Dynamics.
	/// </summary>
	/// 
	public interface IDynamic
	{
		void Calculate (ref Vector3 velocity, float mass, int currentFrame);
		uint StartFrame { get; set; }
		
		[Browsable(false)]
		string Name { get; }
	}
	
	[Serializable()]
	public class Gravity: IDynamic
	{	
		private uint startFrame = 0;
		private readonly string name = "Gravity";

		float gravityForce = 0.15f;

		public Gravity()
		{}

		public override string ToString()
		{
			return "Gravity";
		}

		public void Calculate(ref Vector3 velocity, float mass, int currentFrame)
		{	

			float time = (float) (currentFrame - startFrame);
			
			if(time > startFrame)
			{	
				if ( velocity.Y <= 0)
				{
					velocity.Y = velocity.Y * (1 + gravityForce) - .2f;
				}
				else 
				{
					velocity.Y = (velocity.Y * (1 - gravityForce)) - .2f;
				}
			}
		}
		
		[Description("Frame at which the effect will start at.")]
		public uint StartFrame
		{
			get { return startFrame; }
			set { startFrame = value; }
		}
		
		public string Name
		{ 
			get { return name; }
		}

		public float GravityForce
		{
			get { return gravityForce;}
			set {gravityForce = value;}
		}

	}
	
	[Serializable()]
	public class GeneralForce: IDynamic
	{	
		private uint startFrame = 0;
		private readonly string name = "Gravity";

		float xForce = 1.0f;
		float yForce = 1.0f;
		float zForce = 1.0f;

		public GeneralForce()
		{}

		public override string ToString()
		{
			return "Force";
		}


		public void Calculate(ref Vector3 velocity, float mass, int currentFrame)
		{	

			float time = (float) (currentFrame - startFrame);
			
			if(time > startFrame)
			{	
				velocity.X += xForce;
				velocity.Y += yForce;
				velocity.Z += zForce;
			}
		}
		
		[Description("Frame at which the effect will start at.")]
		public uint StartFrame
		{
			get { return startFrame; }
			set { startFrame = value; }
		}
		
		public string Name
		{ 
			get { return name; }
		}

		public float XForce
		{
			get { return xForce;}
			set {xForce = value;}
		}
		
		public float YForce
		{
			get { return yForce;}
			set {yForce = value;}
		}

		public float ZForce
		{
			get { return zForce;}
			set {zForce = value;}
		}

	}
	
	[Serializable()]
	public class ActiveRigid
	{
		private readonly string name = "Active Rigid" ;
		private Vector3 callerCenter;
		private Vector3 otherObjectCenter;
		[NonSerialized]
		private IObject3D caller;
		private float callerRadius = 0.0f;
		private float otherRadius = 0.0f;

		public ActiveRigid( IObject3D caller)
		{
			this.caller = caller;
		}
		
		
		public void Calculate(ref Vector3 velocity)
		{	
			//if ((velocity.X > 0) || (velocity.Y> 0 ) || (velocity.Z >0))
		{
			foreach (IObject3D tempObj in SceneManager.Instance.ObjectList)
			{
				if (tempObj.Rigidity != Rigidity.None)
				{
					if (this.DidColide(tempObj))
					{
						this.MoveCorrect(tempObj, caller.Velocity);
					}

				}
			}
		}
			
		}

		private bool DidColide(IObject3D otherObject)
		{
			// check for self
			if (caller.Equals(otherObject))
			{
				return false;
			}
			
			if ((caller is MeshObject) && (otherObject is MeshObject))
			{
				float callerRadius = 0.0f; // Radius of bounding sphere of object
				float otherRadius = 0.0f; // Radius of bounding sphere of object

				// Retrieve the vertex buffer data in the mesh object
				VertexBuffer vb = ((MeshObject)caller).mesh.VertexBuffer;
				VertexBuffer vb2 = ((MeshObject)otherObject).mesh.VertexBuffer;
				
				
				// Lock the vertex buffer to generate a simple bounding sphere
				GraphicsStream vertexData = vb.Lock(0, 0, LockFlags.NoSystemLock);
				callerRadius = Geometry.ComputeBoundingSphere(vertexData,
					((MeshObject)caller).mesh.NumberVertices,
					((MeshObject)caller).mesh.VertexFormat,
					out callerCenter);
				vb.Unlock();
				vb.Dispose();

				
				// Lock the vertex buffer to generate a simple bounding sphere
				GraphicsStream vertexData2 = vb2.Lock(0, 0, LockFlags.NoSystemLock);
				otherRadius = Geometry.ComputeBoundingSphere(vertexData2,
					((MeshObject)otherObject).mesh.NumberVertices,
					((MeshObject)otherObject).mesh.VertexFormat,
					out otherObjectCenter);
				vb2.Unlock();
				vb2.Dispose();

				// these couple of lines scale the bounding sphere down or up compared to the largest scaling of the x, y or z
				AxisValue callerScaling = caller.Scaling;
				callerRadius *= Math.Max(Math.Max(callerScaling.X, callerScaling.Y), callerScaling.Z);
				
				AxisValue otherObjectScaling = otherObject.Scaling;
				callerRadius *= Math.Max(Math.Max(otherObjectScaling.X, otherObjectScaling.Y), otherObjectScaling.Z);
				

				callerCenter.X = callerCenter.X + caller.Translation.X;
				callerCenter.Y = callerCenter.Y + caller.Translation.Y;
				callerCenter.Z = callerCenter.Z + caller.Translation.Z;

				otherObjectCenter.X = otherObjectCenter.X + otherObject.Translation.X;
				otherObjectCenter.Y = otherObjectCenter.Y + otherObject.Translation.Y;
				otherObjectCenter.Z = otherObjectCenter.Z + otherObject.Translation.Z;
				
				if (caller.GetVertDist(callerCenter, otherObjectCenter) > (callerRadius + otherRadius))
				{
					return false;
				}


				return true;
			}
			return false;
		}

		private void MoveCorrect (IObject3D  otherObject, Vector3 velocity)
		{
			float velocityBackSegments = 10.0f;
			float xBack = velocity.X /velocityBackSegments;
			float yBack = velocity.Y /velocityBackSegments;
			float zBack = velocity.Z /velocityBackSegments;
			
			int count = 1;

			while (this.DidColide(otherObject))
			{
				count = count ++;
				caller.Translate(caller.Translation.X-(count* xBack), caller.Translation.Y-(count * yBack), caller.Translation.Z-(count * zBack));

			}
			Vector3 hasTraveled = new Vector3 (-(count* xBack), -(count * yBack), -(count * zBack));

			Vector3 normal = new Vector3 (otherObjectCenter.X - callerCenter.X, otherObjectCenter.Y - callerCenter.Y, otherObjectCenter.Z - callerCenter.Z);
						
			float a =  caller.Translation.X;
			float b = caller.Translation.Y;
			float c = caller.Translation.Z;

			float d =  otherObject.Translation.X;
			float e =  otherObject.Translation.Y;
			float f = otherObject.Translation.Z;
			
			Vector3 rebound;
			rebound.X = caller.Velocity.X * -1;
			rebound.Y = caller.Velocity.Y * -1;
			rebound.Z = caller.Velocity.Z * -1;

			// move active objects being collided with
			if (otherObject.Rigidity == Rigidity.Active)
			{
				Vector3 newVelocity = otherObject.Velocity; 

				newVelocity.X += (caller.Mass / otherObject.Mass) * velocity.X - (velocity.X / otherObject.Dampening);
				newVelocity.Y += (caller.Mass / otherObject.Mass) * velocity.Y - (velocity.Y / otherObject.Dampening);
				newVelocity.Z += (caller.Mass / otherObject.Mass) * velocity.Z - (velocity.Z / otherObject.Dampening);

				otherObject.Velocity = newVelocity;
				
			}

			// change the original moving object's velocity
			caller.Velocity = rebound * (otherObject.Mass / caller.Mass);

//			Vector3 rebound;
//			rebound.X = caller.Velocity.X * -(1.0f + dampening) * caller.Velocity.X * normal.X;
//			rebound.Y = caller.Velocity.Y * -(1.0f + dampening) * caller.Velocity.Y * normal.Y;
//			rebound.Z = caller.Velocity.Z * -(1.0f + dampening) * caller.Velocity.Z * normal.Z;
//			caller.Velocity = rebound;

		}

		public string Name
		{ 
			get { return name; }
		}
		
	}

	//	public class Vortex: IDynamic
	//	{	
	//		private uint startFrame;
	//		private readonly string name = "Vortex";
	//		
	//		private float xAmplitude = 0.5f;
	//		private float yAmplitude = 0.5f;
	//		private float zAmplitude = 0.5f;
	//
	//		private float x_speed = 0.1f;
	//		private float y_speed = 0.1f;
	//		private float z_speed = 0.1f;
	//
	//		public Vortex()
	//		{}
	//
	//		public void Calculate(ref Vector3 velocity, float mass, int currentFrame)
	//		{
	//			float time = (float) (currentFrame - startFrame);
	//			
	//			if(time >= startFrame)
	//			{
	//				time = (float) (currentFrame - startFrame);
	//				time = time < 0 ? 0 : time;
	//				// if you want a "spiral" motion, set amplitutde = time * someVal
	//				float x_amplitude = (float)(xAmplitude);
	//				float z_amplitude = (float)(zAmplitude);
	//				velocity.X += (float) Math.Sin((double)x_speed) * x_amplitude;
	//				//+ currentEffect.Memento.translation.X;
	//				velocity.Z += ((float) Math.Cos((double)z_speed) -1) * z_amplitude;
	//			}
	//		}
	//		
	//		[Description("Frame at which the effect will start at.")]
	//		public uint StartFrame
	//		{
	//			get { return startFrame; }
	//			set { startFrame = value; }
	//		}
	//		
	//		public float XAmplitude
	//		{
	//			get { return xAmplitude; }
	//			set { xAmplitude = value; }
	//		}
	//
	//		public float YAmplitude
	//		{
	//			get { return yAmplitude; }
	//			set { yAmplitude = value; }
	//		}
	//
	//		public string Name
	//		{ 
	//			get { return name; }
	//		}
	//
	//		private float Xspeed
	//		{
	//			get {return x_speed;}
	//			set {x_speed = value;}
	//		}
	//
	//		private float Yspeed
	//		{
	//			get {return y_speed;}
	//			set {y_speed = value;}
	//		}
	//
	//		private float Zspeed
	//		{
	//			get {return z_speed;}
	//			set {z_speed = value;}
	//		}
	//	}

}
