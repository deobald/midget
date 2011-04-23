using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

using System.ComponentModel;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Midget;

namespace Midget
{
	/// <summary>
	/// The Particle object has been enhanced to now support customizable Median, 
	/// Deviation, Directional Bias, and ObjectType. It's kind of sloppy, but it 
	/// works quite well.
	/// </summary>
	
	[Serializable()]
	public class Particle : MeshObject
	{
		[NonSerialized()]
		private Device device;
		Vector3 startLocation = new Vector3();
		Vector3 startRotation = new Vector3();
		//Vector3 direction = new Vector3();
		[NonSerialized]
		private Random rand = new Random();
		private int directionMedian = 0;
		private int directionDeviation = 1;
		private Vector3 directionBias = new Vector3(0.0f, 0.0f, 0.0f);
		private int birthFrame = 0;		// when the object was created.
		private int deathFrame = 100;	// how many frames you want the object to live
		private int life = 0;			// how old it is at the current frame
		private ObjectFactory.ObjectTypes particleType;
		private bool isRandom = false;
		
		/// <summary>
		/// the regular constructor that the object factory calls.
		/// This constructor should never actually be called.
		/// </summary>
		/// <param name="device"></param>
		public Particle (Device device)
		{
			this.device = device;

			// choose
			mesh = Mesh.Box(device, 2.0f, 2.0f, 2.0f);
			name = ValidateName("Particle", true);

			this.velocity.X = GenerateMeanRandom(directionMedian, directionDeviation) * directionBias.X;
			this.velocity.Y = GenerateMeanRandom(directionMedian, directionDeviation) * directionBias.Y;
			this.velocity.Z = GenerateMeanRandom(directionMedian, directionDeviation) * directionBias.Z;
		
		}
		
		/// <summary>
		/// this guy here is the constructor that the emmitter calls. 
		/// </summary>
		/// <param name="device">The global device</param>
		/// <param name="parent">The parent to this particle (the emitter)</param>
		/// <param name="birthFrame">The start frame for this particle</param>
		/// <param name="dynamicsList">Any dynamics applied to this particle</param>
		/// <param name="isDynamic">Whether or not this particle is dynamic</param>
		public Particle (Device device, IObject3D parent, int birthFrame, int deathFrame, 
			ArrayList dynamicsList, bool isDynamic, ObjectFactory.ObjectTypes particleType, 
			int directionalMedian, int directionalDeviation, Vector3 directionalBias, bool random)
		{
			// init
			this.device = device;
			this.Parent = parent;
			this.birthFrame = birthFrame;
			this.deathFrame = deathFrame;
			this.particleType = particleType;
			this.directionMedian = directionalMedian;
			this.directionDeviation = directionalDeviation;
			this.directionBias = directionalBias;
			this.isRandom = random;
			
			// determine which particle type we are using
			if (this.particleType == ObjectFactory.ObjectTypes.MeshBox)
			{
				mesh = Mesh.Box(device, 2.0f, 2.0f, 2.0f);
			}
			else if (this.particleType == ObjectFactory.ObjectTypes.MeshCylinder)
			{
				mesh = Mesh.Cylinder(device, 1.0f, 1.0f, 3.0f, 9, 9);
			}
			else if (this.particleType == ObjectFactory.ObjectTypes.MeshPolygon)
			{
				mesh = Mesh.Polygon(device, 1.0f, 6);
			}
			else if (this.particleType == ObjectFactory.ObjectTypes.MeshSphere)
			{
				mesh = Mesh.Sphere(device, 1.0f, 9, 9);
			}
			else if (this.particleType == ObjectFactory.ObjectTypes.MeshTeapot)
			{
				mesh = Mesh.Teapot(device);
			}
			else if (this.particleType == ObjectFactory.ObjectTypes.MeshTorus)
			{
				mesh = Mesh.Torus(device, 0.5f, 1.0f, 24, 15);
			}
			else
			{
				mesh = Mesh.Box(device, 1.0f, 1.0f, 1.0f);
			}

			name = ValidateName("Particle", true);

			this.velocity.X = GenerateMeanRandom(directionMedian, directionDeviation) * directionBias.X;
			this.velocity.Y = GenerateMeanRandom(directionMedian, directionDeviation) * directionBias.Y;
			this.velocity.Z = GenerateMeanRandom(directionMedian, directionDeviation) * directionBias.Z;

			startLocation.X = parent.Translation.X;
			startLocation.Y = parent.Translation.Y;
			startLocation.Z = parent.Translation.Z;

			startRotation.X = parent.Rotation.X;
			startRotation.Y = parent.Rotation.Y;
			startRotation.Z = parent.Rotation.Z;
	
			this.isDynamic = isDynamic;
			this.dynamicsList = dynamicsList;
		}

		// the random number generator taakes a number you want to be the average, 
		// and how far you want to stray from the average
		public float GenerateMeanRandom(int median, int deviation)
		{
			float answer = (((float)rand.NextDouble()*2 - 1.0f) * (float)deviation) + (float)median;

			if (isRandom)
			{
				return answer;
			}
			else
			{
				return Math.Abs(answer);
			}
		}
	
		// this is the main particle updater, right now, the only thing being updated is the movement
		// size, colour, etc, can be easily added in
		public void Update (int curFrame)
		{	
			//so begins the movement.   I had to copy ian's entire calcdynamics in here, cause at the bottom of his
			// he translates, and i don't want to do that,  cause i still have some other spead to look at
			if (this.isDynamic)
			{
				this.CalculateDynamics(curFrame);
			}
			else
			{	
				this.Translate(
					this.velocity.X*(curFrame-birthFrame) + startLocation.X, 
					this.velocity.Y*(curFrame-birthFrame) + startLocation.Y, 
					this.velocity.Z*(curFrame-birthFrame) + startLocation.Z);
				this.Rotate(
					startRotation.X, startRotation.Y, startRotation.Z);
			}

			life ++;
		}

		protected override void Initialize (Device device)
		{
			mesh = Mesh.Box(device, 0.1f, 15, 20);

		}
		
		// must override the render so that CalculateDynamics is not called...  must fix.
		public override void Render(Device device, Matrix world, int keyframe)
		{
		
			InterpolatePosition(keyframe);

			world = Matrix.Multiply(scalingMatrix * rotationMatrix * translationMatrix, world);

			// render children
			this.RenderChildren ( device, world, keyframe );
			
			device.Transform.World = world;
			mesh.DrawSubset(0);
		}
		
		[Browsable(false)]
		public int Life
		{
			get { return life; }
			set { life = value; }
		}
		
		[Browsable(false)]
		public int BirthFrame
		{
			get { return birthFrame; }
			set { birthFrame = value; }
		}

		[Browsable(false)]
		public int DeathFrame
		{
			get { return deathFrame; }
			set { deathFrame = value; }
		}

	}

	/// <summary>
	/// Here is the class that is the particle emitter.   I am listening to rap.   Emmitts a bunch of Particle Objects
	/// Not very special,   super intuitive.
	/// </summary>
	/// 
	[Serializable()]
	public class ParticleSystem : Object3DCommon
	{	
		[NonSerialized]
		private Device device;
		private int lastFrame= 0; // this is to make sure that the particles are only updated once per keyframe.
		private int partsPerFrame = 1; // how many particles to emmit per frame..
		private bool isRandom = false;
	
		// particle properties
		private ObjectFactory.ObjectTypes particleType = ObjectFactory.ObjectTypes.MeshSphere;
		private int particleLifetime = 100;
		private int particleDirectionMedian = 0;
		private int particleDirectionDeviation = 1;
		private Vector3 particleDirectionBias = new Vector3(0.1f, 1.0f, 0.1f);

		public ParticleSystem(Device device)
		{
			this.device = device;
			name = ValidateName("Particle System", true);

			for (int i = 0 ; i <1; i ++)
			{
				AddParticle(0);
			}

		}

		public void AddParticle (int keyframe)
		{
			IObject3D newParticle;

			newParticle = new Particle(device, this, keyframe, this.particleLifetime, 
				this.dynamicsList, this.isDynamic, this.particleType, this.particleDirectionMedian, 
				this.particleDirectionDeviation, this.particleDirectionBias, this.isRandom);

			if( _children == null)
				_children = new ArrayList();

			_children.Add(newParticle);

			newParticle.Translate(this.Translation.X, this.Translation.Y, this.Translation.Z);
			
		}

		public void UpdateSystem (int curFrame)
		{
			
			for(int i = 0; i < _children.Count;)
			{
				if((((Particle)_children[i]).Life >= ((Particle)_children[i]).DeathFrame)
					|| (((Particle)_children[i]).BirthFrame > curFrame))
				{
					RemoveChild((IObject3D)_children[i]);
					//RemoveChild(this);
				}
				else
				{
					//((Particle)_children[i]).update(curFrame);
					++i;
				}
			}

			foreach (IObject3D part in _children)
			{
				if (part is Particle)
				{
					//				if (part.IsDynamic)
					//				{
					//					((Particle)part).CalculateDynamics(((Particle)part).life);
					//					this.RenderChildren(device, 
					//				}
				
					((Particle)part).Update(curFrame);
				}
			}
			for (int i = 0; i < partsPerFrame; i ++)
			{
				this.AddParticle(curFrame);
			}
			
		}

		public override void Render(Device device, Matrix world, int keyframe)
		{
			if (lastFrame == keyframe)
			{}
			else 
			{
				this.UpdateSystem(keyframe);
				lastFrame = keyframe;
			}

			
			if (this.hasPath)
			{
				CalculatePath(keyframe);
			}
			else
			{
				InterpolatePosition(keyframe);
			}
			
			// render children
			this.RenderChildren ( device, world, keyframe );
			
			world = Matrix.Multiply(scalingMatrix * rotationMatrix * translationMatrix, world);
			
			device.Transform.World = world;
		}

		public override void Reinitialize(Device device)
		{	
			material.Initialize(device);
			selectedMaterial.Initialize(device);

			this.device = device;
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
			int zDistance = -1;

			//Matrix myModelSpace = worldSpace * manipulateMatrix;
			Matrix myModelSpace =  Matrix.Multiply(scalingMatrix * rotationMatrix * translationMatrix, worldSpace);
			
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


		[Description("Select the particletype that you want the system to emit")]
		public ObjectFactory.ObjectTypes ParticleType
		{
			get { return particleType; }
			set { particleType = value; }
		}

		[Description("How many frames you want the particles to be rendered for")]
		public int ParticleLifetime
		{
			get { return particleLifetime; }
			set { particleLifetime = value; }
		}

		[Description("How many particles per frame you want to be emitted")]
		public int ParticlesPerFrame
		{
			get { return partsPerFrame; }
			set { partsPerFrame = value; }
		}

		[Description("A flag that allows the direction of the emmitted particles to be truely random and unnormalized")]
		public bool IsRandom
		{
			get { return isRandom;}
			set { isRandom = value;}
		}

		[Description("The average speed you want the particle to be emitted at")]
		public int DirectionMedian
		{
			get { return particleDirectionMedian; }
			set { particleDirectionMedian = value; }
		}

		[Description("How much you want the particles to stray from their designated path")]
		public int DirectionDeviation
		{
			get { return particleDirectionDeviation; }
			set { particleDirectionDeviation = value; }
		}

		[Description("The bias for the emitted particle to be emitted in the x direction")]
		public float DirectionBiasX
		{
			get { return particleDirectionBias.X; }
			set { particleDirectionBias.X = value; }
		}

		[Description("The bias for the emitted particle to be emitted in the y direction")]
		public float DirectionBiasY
		{
			get { return particleDirectionBias.Y; }
			set { particleDirectionBias.Y = value; }
		}

		[Description("The bias for the emitted particle to be emitted in the z direction")]
		public float DirectionBiasZ
		{
			get { return particleDirectionBias.Z; }
			set { particleDirectionBias.Z = value; }
		}

	}



}