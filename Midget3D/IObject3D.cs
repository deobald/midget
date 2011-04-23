using System;
using System.Collections;
using System.ComponentModel;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;
using System.Runtime.Serialization;

namespace Midget
{
	/// <summary>
	/// The root interface to all 3D objects used by Midget
	/// </summary>
	public interface IObject3D
	{
		// general //
		void Reinitialize(Device device);
		void Render(Device device, Matrix world, int keyframe);
		string ValidateName(string originalName, bool isOriginal);
		void Translate(float x, float y, float z);
		void Rotate(float x, float y, float z);
		void Scale(float x, float y, float z);
		[Browsable(false)]
		AxisValue Translation { get; set; }
		[Browsable(false)]
		AxisValue Rotation { get; set; }
		[Browsable(false)]
		AxisValue Scaling { get; set; }
		[Browsable(false)]
		string Name { get; set; }
		[Browsable(false)]
		Midget.Materials.MidgetMaterial Material { get; set; }
		[Browsable(false)]
		bool Selected { get; set; }
		[Browsable(true)]
		float Mass {get; set; }
		[Browsable(false)]
		Vector3 Velocity { get; set; }
		[Browsable(false)]
		AxisValue PivotPoint { get; set; }
		float PivotPointX { get; set; }
		float PivotPointY { get; set; }
		float PivotPointZ { get; set; }

		// keyframing //
		void AddKeyFrame(int index);
		void RemoveKeyFrame(int index);
		[Browsable(false)]
		ArrayList KeyFrameList { get; }
		Object3DMemento CreateMemento();
		void SetMemento (Object3DMemento memento); 

		// dynamics //
		void AddDynamics(IDynamic dynamic);
		void DynamicsToKeys(int startFrame, int endFrame, int currentFrame);
		float GetVertDist (Vector3 a, Vector3 b);
		Rigidity Rigidity { get; set; }
		Object3DMemento DynamicStartMemento { get; set; }
		float Dampening { get; set; }
		[Browsable(false)]
		ArrayList DynamicsList { get; }

		// world space & intersection //
		IObject3D	 Intersect(Vector3 rayPosition, Vector3 rayDirection, Matrix worldSpace);
		[Browsable(false)]
		Matrix WorldSpace { get; }

		// grouping //
		void AddChild(IObject3D child);
		void RemoveChild(IObject3D child);
		[Browsable(false)]
		IObject3D Parent { get; set; }
		[Browsable(false)]
		ArrayList Children { get; }

		// curves //
		[Browsable(false)]
		bool HasPath { get; set; }
		[Browsable(false)]
		IObject3D Path { get; set; }
		[Browsable(false)]
		int PathFrameAmount { get; set; }
	}
	
	[Serializable()]
	public abstract class Object3DCommon : IObject3D
	{

		protected Matrix	scalingMatrix;
		protected Matrix	translationMatrix;
		protected Matrix	rotationMatrix;

		protected string	name;
		protected string	dynamicName;
		protected Vector3   velocity = new Vector3(0.0f, 0.0f, 0.0f);
		protected float		mass = 10.0f;
		protected Object3DMemento dynamicStartMemento;
		protected float		dampening = 20.0f;
		
		protected AxisValue	translation;
		protected AxisValue	rotation;
		protected AxisValue	scaling;
		
		protected ArrayList keyFrameList;
		protected ArrayList dynamicsList;

		protected ArrayList _children = new ArrayList();
		protected AxisValue	pivotPoint;
		protected IObject3D _parent;

		protected int		lastFrameInterpolated;
		protected bool		isDynamic = false;
		protected Rigidity	rigidity = Rigidity.None;
		
		protected Midget.Materials.MidgetMaterial material;
		protected Midget.Materials.MidgetMaterial selectedMaterial;

		// curve stuff
		protected bool		hasPath = false;
		protected IObject3D path;
		protected int		frameAmount = 0;

		protected bool	selected;
		
		public Object3DCommon ()
		{
			this.rotationMatrix = Matrix.Identity;
			this.scalingMatrix = Matrix.Identity;
			this.translationMatrix = Matrix.Identity;

			translation = new AxisValue();
			rotation = new AxisValue();
			scaling = new AxisValue();

			pivotPoint = new AxisValue();

			material = new Midget.Materials.MidgetMaterial();

			dynamicsList = new ArrayList();

			scaling.X = 1;
			scaling.Y = 1;
			scaling.Z = 1;

			lastFrameInterpolated = -1;

			selected = false;

			selectedMaterial = new Midget.Materials.MidgetMaterial();
			selectedMaterial.Emissive = System.Drawing.Color.Snow;
			selectedMaterial.Diffuse = System.Drawing.Color.White;
		}

		protected void RenderChildren( Device device, Matrix world, int keyframe )
		{
			// if there are children to render
			if ( _children != null && _children.Count > 0)
			{
				// render all the children
				foreach (IObject3D obj in _children)
					obj.Render( device, world, keyframe );
			}
		}


		#region IObject3D Members
		
		public abstract void Reinitialize(Device device);
		public abstract void Render(Device device, Matrix world, int keyframe);

		private void Interpolate(int currentFrameIndex, int keyFrameListIndex1, int keyFrameListIndex2)
		{
			// get the two keyframes
			Object3DKeyFrame tempKeyFrame1 = (Object3DKeyFrame) keyFrameList[keyFrameListIndex1];
			Object3DKeyFrame tempKeyFrame2 = (Object3DKeyFrame) keyFrameList[keyFrameListIndex2];
			
		
			if(currentFrameIndex == tempKeyFrame2.Index)
			{
				SetMemento(tempKeyFrame2.Memento);
				return;
			}
			else
				SetMemento(tempKeyFrame1.Memento);
			
			int keyFrameDifference;

			// if the CurrentFrame is the TempFrame then there is no need to to any interpolation
			if(currentFrameIndex == tempKeyFrame1.Index)
			{
				return;
			}
		
			keyFrameDifference = currentFrameIndex - tempKeyFrame1.Index;	

			// calculate Translate Increment
			tempKeyFrame1.TranslateIncrement.X = 
				((tempKeyFrame2.Memento.translation.X - tempKeyFrame1.Memento.translation.X) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;

			tempKeyFrame1.TranslateIncrement.Y = 
				((tempKeyFrame2.Memento.translation.Y - tempKeyFrame1.Memento.translation.Y) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;

			tempKeyFrame1.TranslateIncrement.Z = 
				((tempKeyFrame2.Memento.translation.Z - tempKeyFrame1.Memento.translation.Z) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;

			// calculate Rotate Increment

			tempKeyFrame1.RotateIncrement.X = 
				((tempKeyFrame2.Memento.rotation.X - tempKeyFrame1.Memento.rotation.X) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;

			tempKeyFrame1.RotateIncrement.Y = 
				((tempKeyFrame2.Memento.rotation.Y - tempKeyFrame1.Memento.rotation.Y) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;

			tempKeyFrame1.RotateIncrement.Z = 
				((tempKeyFrame2.Memento.rotation.Z - tempKeyFrame1.Memento.rotation.Z) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;

			// calculate Scale Increment
			
			
			tempKeyFrame1.ScaleIncrement.X = 
				((tempKeyFrame2.Memento.scaling.X - tempKeyFrame1.Memento.scaling.X) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;

			tempKeyFrame1.ScaleIncrement.Y = 
				((tempKeyFrame2.Memento.scaling.Y - tempKeyFrame1.Memento.scaling.Y) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;

			tempKeyFrame1.ScaleIncrement.Z = 
				((tempKeyFrame2.Memento.scaling.Z - tempKeyFrame1.Memento.scaling.Z) /
				(tempKeyFrame2.Index - tempKeyFrame1.Index)) * keyFrameDifference;	


			// Translate
			Translate(
				tempKeyFrame1.Memento.translation.X + tempKeyFrame1.TranslateIncrement.X, 
				tempKeyFrame1.Memento.translation.Y + tempKeyFrame1.TranslateIncrement.Y, 
				tempKeyFrame1.Memento.translation.Z + tempKeyFrame1.TranslateIncrement.Z);

			Debug.Write("Keyframe Movement for " + this.Name + ":\t FRAME [" + currentFrameIndex + "]\n" +
				"\t\t[" + tempKeyFrame1.Memento.translation.X + "," + tempKeyFrame1.Memento.translation.Y + "," + tempKeyFrame1.Memento.translation.Z + "]\t" +
				"[" + tempKeyFrame1.TranslateIncrement.X + "," + tempKeyFrame1.TranslateIncrement.Y + "," + tempKeyFrame1.TranslateIncrement.Z + "]");
			
			// Rotate
			Rotate(
				tempKeyFrame1.Memento.rotation.X + tempKeyFrame1.RotateIncrement.X, 
				tempKeyFrame1.Memento.rotation.Y + tempKeyFrame1.RotateIncrement.Y, 
				tempKeyFrame1.Memento.rotation.Z + tempKeyFrame1.RotateIncrement.Z);
			
			// TODO: FIX SCALING FOR KEYFRAMING!!!
			//Scale
			Scale(
				tempKeyFrame1.Memento.scaling.X + tempKeyFrame1.ScaleIncrement.X, 
				tempKeyFrame1.Memento.scaling.Y + tempKeyFrame1.ScaleIncrement.Y, 
				tempKeyFrame1.Memento.scaling.Z + tempKeyFrame1.ScaleIncrement.Z);

		}
		

		/// <summary>
		/// Determining the location data for the purposes of keyframing always depends on the location of
		/// the current frame in relation to the locations of the other keyframes for a given object. If there
		/// are only one or two keyframes in the list, there is no need to interpolate any values.  If there are
		/// two or more keyframes for the object, we must determine where the current frame is in relation to two
		/// of those keyframes and interpolate the values.  InterpolatePosition() determines the position of the
		/// current frame in relation to the previous and next keyframes.
		/// </summary>
		/// <param name="currentFrameNumber"></param>
		protected void InterpolatePosition(int currentFrameNumber)
		{
			
			// if there is only 1 keyframe, set to that keyframe
			if (keyFrameList != null && keyFrameList.Count == 1 && currentFrameNumber == ((Object3DKeyFrame)keyFrameList[0]).Index)
			{
				SetMemento(((Object3DKeyFrame)keyFrameList[0]).Memento);
			}
			else if (lastFrameInterpolated != currentFrameNumber && keyFrameList != null && keyFrameList.Count > 1)
			{
				int startKeyFrameIndex = -1;
				int endKeyFrameIndex   = -1;
				
				int count = 0;

				// find if this frame occurs between two keyframes
				foreach(Object3DKeyFrame objKeyFrame in keyFrameList)
				{
					// if the keyframe index is less than or equal to the current frame, then the first
					// keyframe used in the comparison is count
					if(objKeyFrame.Index <= currentFrameNumber && count != keyFrameList.Count - 1)
					{
						startKeyFrameIndex = count;
					}
					else if (startKeyFrameIndex > -1 && objKeyFrame.Index >= currentFrameNumber)
					{
						endKeyFrameIndex = count;
						break;
					}
					else if(startKeyFrameIndex == -1 && objKeyFrame.Index > currentFrameNumber)
					{
						SetMemento(objKeyFrame.Memento);
						break;
					}
					else if(count == keyFrameList.Count - 1) 
					{ 
						SetMemento(objKeyFrame.Memento); 
						break; 
					}

					++count;
				}
				
				// two keyframe have been found
				if(startKeyFrameIndex != -1 && endKeyFrameIndex != -1)
				{
					Interpolate(currentFrameNumber, startKeyFrameIndex, endKeyFrameIndex);
				}

				lastFrameInterpolated = currentFrameNumber;
			}
		}
		
		/// <summary>
		/// Calculate the dynamic changes to the current object
		/// </summary>
		/// <param name="currentFrame"></param>
		protected void CalculateDynamics(int currentFrame)
		{
			if(lastFrameInterpolated != currentFrame )
			{
				if(currentFrame == 0)
				{
					velocity = new Vector3();
					SetMemento(dynamicStartMemento);
					return;
				}
				
				lastFrameInterpolated = currentFrame;

				foreach(IDynamic dynamic in dynamicsList)
				{	
					dynamic.Calculate(ref velocity, mass, currentFrame);
					continue;
				}

				

				Translate ( this.velocity.X+ this.Translation.X,
					this.velocity.Y + this.Translation.Y,
					this.velocity.Z+ this.Translation.Z);

				ActiveRigid collision = new ActiveRigid(this);
				collision.Calculate(ref velocity);

			}
		}

		/// <summary>
		/// Calculate a movement path for a curve
		/// </summary>
		/// <param name="currentFrame">The current frame to be calculated on</param>
		protected void CalculatePath (int currentFrame)
		{
			// this is a variable that will tell us the last frame that we have
			int totVert = ((Curve)path).PointList.Count;
			// this guy here tells us how long the line is
			float totalDist = ((Curve)path).TotalDis;
			// the list of points that the curve follows
			ArrayList vertexList = ((Curve)path).PointList;
			
			//starting to get into some mathe here
			// how far we are allowed to go each frame.
			float spacePerFrame = totalDist/ (float)frameAmount;
			// how far we are allowed to go so far in the movement
			float curMaxDist = (float)currentFrame * spacePerFrame;
			// how far we have come so far.
			float curDist= 0.0f;
			
			int i; 
			// to calculate what vertex we are on and how far along the curve we are
			for ( i = 0; (curDist <= curMaxDist) && (i < totVert-2); i++)
			{
				curDist = curDist + GetVertDist ((Vector3)vertexList[i], (Vector3)vertexList[i+1]);
			}

			curDist = curDist - GetVertDist ((Vector3)vertexList[i], (Vector3)vertexList[i+1]);
			int vertexOn = i-1;

			// keep going on the curve until we have reached the max allowable distance for this keyframe
			while ((curDist + GetVertDist ((Vector3)vertexList[vertexOn], (Vector3)vertexList[vertexOn + 1]) < curMaxDist)
				&& (curDist <= totalDist) && (vertexOn + 2 < totVert))
			{
				curDist = curDist + GetVertDist ((Vector3)vertexList[vertexOn], (Vector3)vertexList[vertexOn + 1]);
				vertexOn ++;
				
			}

			float xPos = ((Vector3)vertexList[vertexOn]).X;
			float yPos = ((Vector3)vertexList[vertexOn]).Y;
			float zPos = ((Vector3)vertexList[vertexOn]).Z;
			// and move to the new postition.
			Translate (	xPos, yPos, zPos);
		}

		/// <summary>
		/// A little function that finds the distance between two vector3 points.
		/// </summary>
		/// <param name="a">First Vector</param>
		/// <param name="b">Second Vector</param>
		/// <returns></returns>
		public float GetVertDist (Vector3 a, Vector3 b)
		{
			return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2)
				+ Math.Pow(a.Y - b.Y, 2)
				+ Math.Pow(a.Z - b.Z, 2));
		}

		public void Translate(float x, float y, float z)
		{
			float relativeChangeX = x - translation.X;
			float relativeChangeY = y - translation.Y;
			float relativeChangeZ = z - translation.Z;

			translation.X = x;
			translation.Y = y;
			translation.Z = z;
			
			Matrix tempTranslationMatrix = Matrix.Translation(relativeChangeX, relativeChangeY, relativeChangeZ);

			translationMatrix = Matrix.Multiply(tempTranslationMatrix,translationMatrix);
		}


		public void Rotate(float x, float y, float z)
		{
			rotation.X = x;
			rotation.Y = y;
			rotation.Z = z;

			float pitch	=	(float) (x * (Math.PI / 180));
			float yaw	=	(float) (y * (Math.PI / 180));
			float roll	=	(float) (z * (Math.PI / 180));

			// create a Quaternion to handle the rotation
			Quaternion quat = Quaternion.RotationYawPitchRoll(yaw, pitch, roll);
		
			rotationMatrix = Matrix.Identity;

			rotationMatrix.AffineTransformation(1.0f,new Vector3(pivotPoint.X,pivotPoint.Y,pivotPoint.Z),
				quat,new Vector3(_parent.PivotPoint.X,_parent.PivotPoint.Y,_parent.PivotPoint.Z));
		}

		public void Scale(float x, float y, float z)
		{
			float relativeChangeX = (x / scaling.X);
			float relativeChangeY = (y / scaling.Y);
			float relativeChangeZ = (z / scaling.Z);

			scaling.X = x;
			scaling.Y = y;
			scaling.Z = z;

			//Matrix _scalingMatrix = Matrix.Scaling(x, y, z);

			
			// apply the scaling matrix to the manipulateMatrix - order matters
			//			manipulateMatrix = Matrix.Multiply(Matrix.Scaling(relativeChangeX, 
			//				relativeChangeY, relativeChangeZ), manipulateMatrix);

			scalingMatrix = Matrix.Multiply(Matrix.Scaling(relativeChangeX, 
				relativeChangeY, relativeChangeZ), scalingMatrix);
		}

		public void AddKeyFrame(int index)
		{
			if(keyFrameList == null)
			{
				keyFrameList = new ArrayList();
			}

			RemoveKeyFrame(index);
			keyFrameList.Add(CreateKeyFrame(index));
			keyFrameList.Sort();
		}

		public void RemoveKeyFrame(int index)
		{
			if(keyFrameList != null)
			{
				foreach (Object3DKeyFrame kf in keyFrameList)
				{
					if (kf.Index == index)
					{
						keyFrameList.Remove(kf);
						break;
					}
					else if (kf.Index > index)
					{
						break;
					}
				}
			}
		}

		public void AddDynamics(IDynamic dynamic)
		{
			// if the dynamics list doesn't exist, create a new list attached to the selected object
			if (dynamicsList == null)
			{
				dynamicsList = new ArrayList();
			}

			dynamicsList.Add(dynamic);
			this.isDynamic = true;
			this.rigidity = Rigidity.Active;
			this.dynamicStartMemento = this.CreateMemento();
		}

		/// <summary>
		/// Takes the Dynamic effect and converts it to a Keyframed Animation :P
		/// </summary>
		/// <param name="startFrame"></param>
		/// <param name="endFrame"></param>
		/// <param name="currentFrame"></param>
		public void DynamicsToKeys(int startFrame, int endFrame, int currentFrame)
		{
			float x_change = 0.0f;
			float y_change = 0.0f;
			float z_change = 0.0f;
			int keyFrequency = 10;
			float time;
			
			for (int i = startFrame; i <= endFrame; i += keyFrequency)
			{
			foreach (Object3DDynamics currentEffect in dynamicsList)
			{
				
					switch ( currentEffect.Name )
					{
						case "gravity":
							float gravity = -0.0981f;
							time = (float) (i - currentEffect.Index);
							time = time < 0 ? 0 : time;
							float velocity = (gravity * time * time) * 0.05f;
							//float distance = (1/2)*velocity*time;

							//x_change += currentEffect.Memento.translation.X + velocity;
							y_change = velocity;
						
							break;

						case "vortex":
							time = (float) (i - currentEffect.Index);
							time = time < 0 ? 0 : time;
							float x_amplitude = (float)(time * 0.05f);
							float z_amplitude = (float)(time * 0.05f);
							float x_speed = 0.1f;
							float z_speed = 0.1f;
							x_change = (float) Math.Sin((double) time * x_speed) * x_amplitude;
							//+ currentEffect.Memento.translation.X;
							z_change = ((float) Math.Cos((double) time * z_speed) -1) * z_amplitude;
							//+ currentEffect.Memento.translation.Z;
							break;
					
						default:
							break;
						
					}

					AxisValue translateChange = new AxisValue( 
						(currentEffect.Memento.translation.X + x_change),
						(currentEffect.Memento.translation.Y + y_change),
						(currentEffect.Memento.translation.Z + z_change));
					
					if(keyFrameList	== null)
					{
						keyFrameList = new ArrayList();
					}
					RemoveKeyFrame(i);
					keyFrameList.Add(new Object3DKeyFrame(i, new Object3DMemento(translateChange, rotation, scaling)));
				}
			}
			keyFrameList.Sort();
			this.dynamicsList.Clear();
			this.isDynamic = false;
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
			foreach (IObject3D obj in SceneManager.Instance.ObjectList)
			{
				// if we have a match, we must change the name and validate again
				if ( obj.Name == originalName )
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

		[Browsable(false)]
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		[Browsable(false)]
		public ArrayList KeyFrameList
		{
			get
			{
				return keyFrameList;
			}
		}

		[Browsable(false)]
		public Matrix WorldSpace
		{
			get
			{
				return (scalingMatrix * rotationMatrix * translationMatrix);
			}
		}

		[Browsable(false)]
		public AxisValue Rotation
		{
			get { return rotation; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}

		[Browsable(false)]
		public AxisValue Translation
		{
			get { return translation; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}

		[Browsable(false)]
		public AxisValue Scaling
		{
			get { return scaling; }
			set { ; }	// do not permit setting all 3 axis at once, for now
		}
	
		[Browsable(false)]
		public Object3DMemento DynamicStartMemento
		{
			get { return dynamicStartMemento; }
			set { dynamicStartMemento = value; }
		}

		public Object3DMemento CreateMemento()
		{
			return new Object3DMemento(translation, rotation, scaling);
		}

		public Object3DKeyFrame CreateKeyFrame(int index)
		{
			return new Object3DKeyFrame(index, CreateMemento());
		}

		/// <summary>
		/// SetMemento takes a memento state object and loads the state of the current object from
		/// the state of the memento.
		/// </summary>
		/// <param name="memento"></param>
		public void SetMemento (Object3DMemento memento)
		{
			this.Translate(memento.translation.X,memento.translation.Y,memento.translation.Z);
			this.Rotate(memento.rotation.X, memento.rotation.Y, memento.rotation.Z);
			this.Scale(memento.scaling.X, memento.scaling.Y, memento.scaling.Z);
		}

		public void AddChild(IObject3D child)
		{
			// remove this child from any previous parent
			if (child.Parent != null)
			{
				child.Parent.RemoveChild(child);
			}

			// init array if no children already exist, delay init to save memory
			if( _children == null)
				_children = new ArrayList();
			
			// add the child
			_children.Add(child);

			// make child aware of parent
			child.Parent = this;
		}

		public void RemoveChild(IObject3D child)
		{
			// will throw exception if child doesn't exist
			_children.Remove(child);

			// remove the parent reference
			child.Parent = null;
		}

		[Browsable(false)]
		public IObject3D Parent
		{
			get
			{	
				return _parent;
			}
			set
			{
				_parent = value;
			}
		}

		[Browsable(false)]
		public ArrayList Children
		{
			get
			{
				return _children;
			}
		}

		[Browsable(false)]
		public AxisValue PivotPoint
		{
			get { return pivotPoint; }
			set { pivotPoint = value; }
		}

		public float PivotPointX
		{
			get { return pivotPoint.X; }
			set { pivotPoint.X = value; }
		}

		public float PivotPointY
		{
			get { return pivotPoint.Y; }
			set { pivotPoint.Y = value; }
		}

		public float PivotPointZ
		{
			get { return pivotPoint.Z; }
			set { pivotPoint.Z = value; }
		}

		[Browsable(false)]
		public Midget.Materials.MidgetMaterial Material
		{
			get { return material; }
			set { material = value; }
		}

		public IObject3D Intersect(Vector3 rayPosition, Vector3 rayDirection, Matrix worldSpace)
		{
			Object3DCommon obj = null;
			int result = Intersect (rayPosition, rayDirection, ref obj, worldSpace);

			if( result < 0)
				return null;
			else 
				return obj;
		}

		// curve stuff //
		[Browsable(false)]
		public bool HasPath
		{
			get	{return hasPath;}
			set	{hasPath = value;}
		}

		[Browsable(false)]
		public IObject3D Path
		{
			get	{return path;}
			set	{path = value;}
		}

		[Browsable(false)]
		public int PathFrameAmount
		{
			get	{return frameAmount;}
			set	{frameAmount = value;}
		}
	
		[Browsable(false)]
		public bool Selected
		{
			get {return selected; }
			set {selected = value; }
		}

		// dynamics stuff //
		[Browsable(false)]
		public Vector3 Velocity
		{
			get {return velocity;}
			set { velocity = value;}
		}

		public float Mass
		{
			get { return mass; }
			set { mass = value; }
		}

		public Rigidity Rigidity
		{
			get { return rigidity; }
			set
			{
				rigidity = value; 
				dynamicStartMemento = CreateMemento();
			}
		}

		public float Dampening
		{
			get { return dampening; }
			set { dampening = value; }
		}

		[Browsable(false)]
		public ArrayList DynamicsList
		{
			get { return dynamicsList; }
		}

		public abstract int Intersect ( Vector3 rayPosition, Vector3 rayDirection, ref Object3DCommon obj, Matrix worldSpace );

		#endregion

	}
	
	[Serializable()]
	public class Object3DMemento
	{
		internal AxisValue	translation;
		internal AxisValue	rotation;
		internal AxisValue	scaling;

		internal Object3DMemento(AxisValue translation, AxisValue rotation, AxisValue scaling)
		{
			this.translation	= new AxisValue (translation);
			this.rotation		= new AxisValue (rotation);
			this.scaling		= new AxisValue (scaling);
		}
	}

	[Serializable()]
	public class Object3DKeyFrame : IComparable
	{
		private Object3DMemento _memento;
		private int _index;
		private AxisValue _translateIncrement;
		private AxisValue _scaleIncrement;
		private AxisValue _rotateIncrement;

		public Object3DMemento Memento
		{
			get { return _memento; }
		}

		public int Index
		{
			get { return _index; }
		}

		internal Object3DKeyFrame(int index, Object3DMemento memento)
		{
			_index = index;
			_memento = memento;
			_translateIncrement = new AxisValue();
			_scaleIncrement = new AxisValue();
			_rotateIncrement = new AxisValue();
		}

		public AxisValue TranslateIncrement
		{
			get { return _translateIncrement; }
			set { _translateIncrement = value; }
		}

		public AxisValue RotateIncrement
		{
			get { return _rotateIncrement; }
			set { _rotateIncrement = value; }
		}

		public AxisValue ScaleIncrement
		{
			get { return _scaleIncrement; }
			set { _scaleIncrement = value; }
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return _index - ((Object3DKeyFrame)obj).Index;
		}

		#endregion
	}

	[Serializable()]
	public class Object3DDynamics
	{
		private int _index;
		private Object3DMemento _memento;
		private string _dynamicName;
		private AxisValue _translateIncrement;

		internal Object3DDynamics(string dynamicName, Object3DMemento memento, int index)
		{
			_dynamicName = dynamicName;
			_memento = memento;
			_translateIncrement = new AxisValue();
			_index = index;
		}

		public int Index
		{
			get { return _index; }
		}

		public Object3DMemento Memento
		{
			get { return _memento; }
		}

		public string Name
		{
			get { return _dynamicName; }
			set { _dynamicName = value; }
		}

		public AxisValue TranslateIncrement
		{
			get { return _translateIncrement; }
			set { _translateIncrement = value; }
		}

	}

	/// <summary>
	/// Structure which allows objects and cameras to store axis 
	/// coordinate/angle information easily
	/// </summary>
	[Serializable()]
	public class AxisValue
	{
		private float _x;
		private float _y;
		private float _z;

		public AxisValue()
		{
			_x = 0.0f;
			_y = 0.0f;
			_z = 0.0f;
		}

		public AxisValue(float x, float y, float z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public AxisValue(AxisValue axisValue)
		{
			_x = axisValue.X;
			_y = axisValue.Y;
			_z = axisValue.Z;
		}

		public static AxisValue operator+(AxisValue dest, AxisValue source)
		{
			dest.X += source.X;
			dest.Y += source.Y;
			dest.Z += source.Z;

			return dest;
		}

		public static AxisValue operator-(AxisValue dest, AxisValue source)
		{
			dest.X -= source.X;
			dest.Y -= source.Y;
			dest.Z -= source.Z;

			return dest;
		}

		public static AxisValue operator/(AxisValue dest, int source)
		{
			dest.X /= source;
			dest.Y /= source;
			dest.Z /= source;

			return dest;
		}

		public float X
		{
			get { return _x; }
			set { _x = (float)value; }
		}

		public float Y
		{
			get { return _y; }
			set { _y = (float)value; }
		}

		public float Z
		{
			get { return _z; }
			set { _z = (float)value; }
		}
	}
}
