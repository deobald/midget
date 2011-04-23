using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{
	/// <summary>
	/// Possible types of cameras
	/// </summary>
	public enum CameraType
	{
		/// <summary>
		/// Create a 3D perspecitve camera
		/// </summary>
		Perspective, 
		
		/// <summary>
		/// Create a 2D orthogonal camera
		/// </summary>
		Orthogonal
	};

	/// <summary>
	/// Predefined Cameras
	/// </summary>
	public enum PredefinedCameras
	{
		Perspective, Left, Top, Front, Right, Back
	};


	/// <summary>
	/// Class responsible for the generation of new cameras
	/// </summary>
	sealed public class CameraFactory
	{
		
		/// <summary>
		/// List to store references to the exising cameras
		/// </summary>
		private ArrayList cameraList;
		
		/// <summary>
		/// List to store the string names of the cameras
		/// </summary>
		private ArrayList cameraNameList;
		
		/// <summary>
		/// Number of predfined cameras
		/// </summary>
		private const int NUMBEROFPREDEFINEDCAMERAS = 6;
	


		
		private CameraFactory()
		{
			cameraList = new ArrayList(NUMBEROFPREDEFINEDCAMERAS);
			cameraNameList = new ArrayList(NUMBEROFPREDEFINEDCAMERAS);
			PopulateCameraNames();
		}

		/// <summary>
		/// The only instance of the CameraFactory
		/// </summary>
		public static readonly CameraFactory Instance = new CameraFactory();

		
		/// <summary>
		/// Populates the camera name list with the names for all the built
		/// in camera
		/// </summary>
		private void PopulateCameraNames()
		{	
			// add enough spaces in the list to store all the predifined cameras
			for(int i = 0; i < NUMBEROFPREDEFINEDCAMERAS; ++i)
			{
				cameraNameList.Add(null);
				cameraList.Add(null);
			}

			// initialise the names of all the predifined cameras
			cameraNameList[(int)PredefinedCameras.Perspective] = "Perspective";
			cameraNameList[(int)PredefinedCameras.Left] = "Left";
			cameraNameList[(int)PredefinedCameras.Top] = "Top";	
			cameraNameList[(int)PredefinedCameras.Front] = "Front";	
			cameraNameList[(int)PredefinedCameras.Right] = "Right";	
			cameraNameList[(int)PredefinedCameras.Back] = "Back";	
		}
	
		/// <summary>
		/// Get a predefined camera
		/// </summary>
		/// <param name="camera">The desired predifined camera</param>
		/// <returns>A reference to the camera, this reference may be shared by multiple viewports</returns>
		public Camera CreateCamera(PredefinedCameras camera)
		{
			switch (camera)
			{
				case PredefinedCameras.Perspective:
				{
					
					// if the camera hasn't already been created, create it
					if(cameraList[(int)PredefinedCameras.Perspective] == null)
						return this.GenerateNewCamera("Perspective",(int)PredefinedCameras.Perspective,
							CameraType.Perspective,
							new Vector3(5, 5, 5), 
							new Vector3(0, 0, 0), 
							new Vector3(0, 1, 0));
					else
						return this.GetExistingCamera((int)PredefinedCameras.Perspective);
				}
				case PredefinedCameras.Front:
				{
					// if the camera hasn't already been created, create it
					if(cameraList[(int)PredefinedCameras.Front] == null)
						return this.GenerateNewCamera("Front",(int)PredefinedCameras.Front,
							CameraType.Orthogonal,
							new Vector3(0, 0, -75), 
							new Vector3(0, 0, 1), 
							new Vector3(0, 1, 0));

					else
						return this.GetExistingCamera((int)PredefinedCameras.Front);
				}
				case PredefinedCameras.Left:
				{
					// if the camera hasn't already been created, create it
					if(cameraList[(int)PredefinedCameras.Left] == null)
						return this.GenerateNewCamera("Left",(int)PredefinedCameras.Left,
							CameraType.Orthogonal,
							new Vector3(10, 0, 0), 
							new Vector3(0, 0, 0), 
							new Vector3(0, 1, 0));

					else
						return this.GetExistingCamera((int)PredefinedCameras.Left);
				}
				case PredefinedCameras.Back:
				{
					// if the camera hasn't already been created, create it
					if(cameraList[(int)PredefinedCameras.Back] == null)
						return this.GenerateNewCamera("Back",(int)PredefinedCameras.Back,
							CameraType.Orthogonal,
							new Vector3(0, 0, -10), 
							new Vector3(0, 0, 0), 
							new Vector3(0, 1, 0));

					else
						return this.GetExistingCamera((int)PredefinedCameras.Back);
				}
				case PredefinedCameras.Right:
				{
					// if the camera hasn't already been created, create it
					if(cameraList[(int)PredefinedCameras.Right] == null)
						return this.GenerateNewCamera("Right",(int)PredefinedCameras.Right,
							CameraType.Orthogonal,
							new Vector3(-10, 0, 0), 
							new Vector3(0, 0, 0), 
							new Vector3(0, 1, 0));

					else
						return this.GetExistingCamera((int)PredefinedCameras.Right);
				}
				case PredefinedCameras.Top:
				{
					// if the camera hasn't already been created, create it
					if(cameraList[(int)PredefinedCameras.Top] == null)
						return this.GenerateNewCamera("Top",(int)PredefinedCameras.Top,
							CameraType.Orthogonal,
							new Vector3(0, 10, 0), 
							new Vector3(0, 0, 0), 
							new Vector3(1, 0, 0));
					else
						return this.GetExistingCamera((int)PredefinedCameras.Top);
				}
				default:
					return null;
			}
		}

		/// <summary>
		/// Retrieve a camera that already exists
		/// </summary>
		/// <param name="cameraID">The ID number of the camera</param>
		/// <returns>A reference to the camera, this reference may be shared by multiple viewports</returns>
		public Camera GetExistingCamera(int cameraID)
		{
			// if that camera doesn't exist return a null camera
			if(cameraID >= this.NumberOfCameras)
				return null;
			
			// is a predifined Camera and is null because cameras aren't created until they are needed
			else if (cameraID < NUMBEROFPREDEFINEDCAMERAS && cameraList[cameraID] == null)
				return this.CreateCamera((PredefinedCameras)cameraID);
			
			// the camera already exists
			else
				return (Camera)cameraList[cameraID];
		}

		/// <summary>
		/// Creates and returns a custom camera
		/// </summary>
		/// <param name="cameraName">The name of the camera</param>
		/// <param name="cameraType">The type of camera to be created</param>
		/// <param name="cameraPosition">The position of the camera in the scene</param>
		/// <param name="cameraTarget">The point where the camera is focusing</param>
		/// <param name="cameraUpVector">A vector representing up</param>
		/// <returns>A reference to a customly created camera</returns>
		public Camera CreateCamera(string cameraName, CameraType cameraType,
			Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
		{
			return this.GenerateNewCamera(cameraName, this.NumberOfCameras + 1, cameraType,
															     cameraPosition, cameraTarget, cameraUpVector);
		}

		/// <summary>
		/// The function that actual creates the brand new camera
		/// </summary>
		/// <param name="cameraName">The name of the camera</param>
		/// <param name="location">The location in the cameraList array where the camera is too be stored.  
		/// For a predefined camera this location is suppose to correspone it's enum value</param>
		/// <param name="cameraType">The type of camera to be created</param>
		/// <param name="cameraPosition">The position of the camera in the scene</param>
		/// <param name="cameraTarget">The point where the camera is focusing</param>
		/// <param name="cameraUpVector">A vector representing up</param>
		/// <returns>A reference to a customly created camera</returns>
		private Camera GenerateNewCamera(string cameraName, int location, CameraType cameraType,
			Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
		{
			// if the size of cameralist has to be increased
			if( this.NumberOfCameras < location )
			{
				cameraList.Add(null);
				cameraNameList.Add(null);
			}

			switch (cameraType)
			{
				case CameraType.Orthogonal:
					cameraList[location] =  new OrthogonalCamera( cameraPosition, cameraTarget, cameraUpVector);
					break;

				case CameraType.Perspective:
					cameraList[location] =  new PerspectiveCamera( cameraPosition, cameraTarget, cameraUpVector);
					break;

				default:
					return null;
			}
			
			//store the name of camera
			cameraNameList[location] = cameraName;

			return (Camera)cameraList[location];
		}

		/// <summary>
		/// The number of different cameras currently available
		/// </summary>
		public int NumberOfCameras
		{
			get { return cameraList.Count; }
		}
	}
}
