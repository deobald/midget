using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace Midget
{	
	[Serializable()]
	public class GroupObject : Object3DCommon
	{
		public GroupObject()
		{
			//this.Translation = this.ChildrenCenterpoint;
			name = "MyGroup";
		}

		public override void Render ( Device device, Matrix world, int keyframe )
		{
			InterpolatePosition(keyframe);

			// transform the world before rendering the children
			world = Matrix.Multiply(scalingMatrix * rotationMatrix * translationMatrix ,world);

			this.RenderChildren ( device, world, keyframe );
		}

		public override void Reinitialize(Device device)
		{
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

	}
}
