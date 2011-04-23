///////////////////////////////////////////////////////////////////
//
//	Note: This file is an enhancement of a file found in the 
//		  DirectX 9.0a SDK
//
///////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Midget
{
	
	/// <summary>
	/// An arc ball class
	/// </summary>
	public class ArcBall
	{
		private int internalWidth;   // ArcBall's window width
		private int internalHeight;  // ArcBall's window height
		private float internalradius;  // ArcBall's radius in screen coords
		private float internalradiusTranslation; // ArcBall's radius for translating the target

		private Quaternion internaldownQuat;               // Quaternion before button down
		private Quaternion internalnowQuat;                // Composite quaternion for current drag
		private Matrix internalrotationMatrix;         // Matrix for arcball's orientation
		private Matrix internalrotationDelta;    // Matrix for arcball's orientation
		private Matrix internaltranslationMatrix;      // Matrix for arcball's position
		private Matrix internaltranslationDelta; // Matrix for arcball's position
		private bool internaldragging;               // Whether user is dragging arcball
		private bool internaluseRightHanded;        // Whether to use RH coordinate system
		private int saveMouseX = 0;      // Saved mouse position
		private int saveMouseY = 0;
		private Vector3 internalvectorDown;         // Button down vector
		PictureBox3D parent; // parent

		private static int cameraControlKey = 0x11;
		private bool cursorInvalid = true;



		/// <summary>
		/// Constructor
		/// </summary>
		public ArcBall(PictureBox3D p)
		{
			internaldownQuat = Quaternion.Identity;
			internalnowQuat = Quaternion.Identity;
			internalrotationMatrix = Matrix.Identity;
			internalrotationDelta = Matrix.Identity;
			internaltranslationMatrix = Matrix.Identity;
			internaltranslationDelta  = Matrix.Identity;
			internaldragging = false;
			internalradiusTranslation = 1.0f;
			internaluseRightHanded = false;

			parent = p;
			// Hook the events 
			p.MouseDown += new MouseEventHandler(this.OnContainerMouseDown);
			p.MouseUp += new MouseEventHandler(this.OnContainerMouseUp);
			p.MouseMove += new MouseEventHandler(this.OnContainerMouseMove);
		}




		/// <summary>
		/// Set the window dimensions
		/// </summary>
		public void SetWindow(int width, int height, float radius)
		{
			// Set ArcBall info
			internalWidth  = width;
			internalHeight = height;
			internalradius = radius;
		}




		/// <summary>
		/// Screen coords to a vector
		/// </summary>
		private Vector3 ScreenToVector(int xpos, int ypos)
		{
			// Scale to screen
			float x   = -(xpos - internalWidth/2)  / (internalradius*internalWidth/2);
			float y   =  (ypos - internalHeight/2) / (internalradius*internalHeight/2);

			if (internaluseRightHanded)
			{
				x = -x;
				y = -y;
			}

			float z   = 0.0f;
			float mag = x*x + y*y;

			if (mag > 1.0f)
			{
				float scale = 1.0f/(float)Math.Sqrt(mag);
				x *= scale;
				y *= scale;
			}
			else
				z = (float)Math.Sqrt(1.0f - mag);

			// Return vector
			return new Vector3(x, y, z);
		}




		/// <summary>
		/// Fired when the containers mouse button is down
		/// </summary>
		private void OnContainerMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (GetAsyncKeyState(CameraControlKey))
			{
				// Store off the position of the cursor when the button is pressed
				saveMouseX = e.X;
				saveMouseY = e.Y;

				if (e.Button == System.Windows.Forms.MouseButtons.Left && parent.Camera.Rotatable)
				{
					// Start drag mode
					internaldragging = true;
					internalvectorDown = ScreenToVector(e.X, e.Y);

					#region new rotate - ignore this
//					// Normalize based on size of window and bounding sphere radius
//					float fDeltaX = (saveMouseX-e.X) * 0.005f; // * internalradiusTranslation / internalWidth;
//					float fDeltaY = (saveMouseY-e.Y) * 0.005f; // * internalradiusTranslation / internalHeight;
//
//
//					// calculate the directional view vector, and use it for our panning plane vectors
//					Vector3 direction = parent.Camera.TargetPoint - parent.Camera.EyePosition;
//					Vector3 panX = Vector3.Cross(direction, parent.Camera.UpVector);
//					Vector3 panY = parent.Camera.UpVector;
//
//					// calculate the view (camera movement) vector
//					internalvectorDown = parent.Camera.TargetPoint + 
//						(panX * e.X) + (panY * e.Y);
					#endregion

					internaldownQuat = internalnowQuat;
				}
			}
		}




		/// <summary>
		/// Fired when the containers mouse button has been released
		/// </summary>
		private void OnContainerMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				// End drag mode
				internaldragging = false;
			}

			Midget.Events.EventFactory.Instance.GenerateAdjustCameraEvent(this,parent.Camera);

			// set cursor
			parent.Cursor = Cursors.Default;
			cursorInvalid = true;
		}




		/// <summary>
		/// Fired when the containers mouse is moving
		/// </summary>
		private void OnContainerMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (GetAsyncKeyState(CameraControlKey))
			{
				if (e.Button == System.Windows.Forms.MouseButtons.Left && parent.Camera.Rotatable)
				{
					if (internaldragging)
					{
						// adjustedY is normally just (e.Y)
						int adjustedY = saveMouseY - (e.Y - saveMouseY);
						// recompute nowQuat
						Vector3 vCur = ScreenToVector(e.X, adjustedY);

						#region new rotate - ignore this
//						// Normalize based on size of window and bounding sphere radius
//						float fDeltaX = (saveMouseX-e.X) * 0.005f; // * internalradiusTranslation / internalWidth;
//						float fDeltaY = (saveMouseY-e.Y) * 0.005f; // * internalradiusTranslation / internalHeight;
//
//
//						// calculate the directional view vector, and use it for our panning plane vectors
//						Vector3 direction = parent.Camera.TargetPoint - parent.Camera.EyePosition;
//						Vector3 panX = Vector3.Cross(direction, parent.Camera.UpVector);
//						Vector3 panY = parent.Camera.UpVector;
//
//						// calculate the view (camera movement) vector
//						Vector3 vCur = parent.Camera.TargetPoint + 
//							(panX * e.X) + (panY * e.Y);
						#endregion

						Quaternion qAxisToAxis = GraphicsUtility.D3DXQuaternionAxisToAxis(internalvectorDown, vCur);
						internalnowQuat = internaldownQuat;
						internalnowQuat = Quaternion.Multiply(internalnowQuat,qAxisToAxis);
						internalrotationDelta = Matrix.RotationQuaternion(qAxisToAxis);
					}
					else
						internalrotationDelta = Matrix.Identity;

					internalrotationMatrix = Matrix.RotationQuaternion(internalnowQuat);
					internaldragging = true;

					// set cursor
					if (cursorInvalid)
					{
						parent.Cursor = new Cursor(GetType(), "rotate.cur");
						cursorInvalid = false;
					}
				}

				if ((e.Button == System.Windows.Forms.MouseButtons.Right) || (e.Button == System.Windows.Forms.MouseButtons.Middle))
				{
					// Normalize based on size of window and bounding sphere radius
					float fDeltaX = (saveMouseX-e.X) * internalradiusTranslation / internalWidth;
					float fDeltaY = (saveMouseY-e.Y) * internalradiusTranslation / internalHeight;

					if (e.Button == System.Windows.Forms.MouseButtons.Middle && parent.Camera.Pannable)
					{
						// camera: panning //

						// calculate the directional view vector, and use it for our panning plane vectors
						Vector3 direction = parent.Camera.TargetPoint - parent.Camera.EyePosition;
						Vector3 panX = Vector3.Cross(direction, parent.Camera.UpVector);
						Vector3 panY = parent.Camera.UpVector;

						// calculate the view (camera movement) vector
						Vector3 view = parent.Camera.TargetPoint + 
							(panX * fDeltaX) + (panY * fDeltaY * 5.0f);

						// determine the translation matrix
						internaltranslationDelta = Matrix.Translation(view.X, view.Y, view.Z);
						internaltranslationMatrix = Matrix.Multiply(internaltranslationMatrix, internaltranslationDelta);

						// set cursor
						if (cursorInvalid)
						{
							parent.Cursor = new Cursor(GetType(), "move.cur");
							cursorInvalid = false;
						}
					}
					if (e.Button == System.Windows.Forms.MouseButtons.Right && parent.Camera.Zoomable)
					{
						// camera: zooming //

						// calculate the camera's directional vector
						Vector3 direction = parent.Camera.TargetPoint - parent.Camera.EyePosition;

						internaltranslationDelta = Matrix.Translation(direction.X * -fDeltaY, direction.Y * -fDeltaY, direction.Z * -fDeltaY);
						internaltranslationMatrix = Matrix.Multiply(internaltranslationMatrix, internaltranslationDelta);
						
						// zoom orthogonal camera
						parent.Camera.OrthoZoom -= fDeltaY * 4.0f;
						if (parent.Camera.OrthoZoom < 4.0f)
						{
							parent.Camera.OrthoZoom = 4.0f;
						}
						parent.Camera.SetProjectionParameters(parent.Camera.FOV, parent.Camera.AspectRatio, 
							parent.Camera.NearClipPlane, parent.Camera.FarClipPlane);

						// set cursor
						if (cursorInvalid)
						{
							parent.Cursor = new Cursor(GetType(), "zoom.cur");
							cursorInvalid = false;
						}

					}

					// Store mouse coordinate
					saveMouseX = e.X;
					saveMouseY = e.Y;

				}	// end of zoom/pan

				// copy the world matrix transformations
				Matrix worldTemp = Matrix.Translation(parent.Camera.TargetPoint);
				worldTemp.Multiply(internalrotationMatrix);
				worldTemp.Multiply(internaltranslationMatrix);
				parent.Camera.WorldMatrix = worldTemp;

				// render this camera's viewport
				parent.Render();

			}
			else
			{
				internaldragging = false;
				cursorInvalid = true;
			}
		}

		#region Various properties of the class
		public float Radius
		{
			set
			{ internalradiusTranslation = value; }
		}
		public bool RightHanded
		{
			get { return internaluseRightHanded; }
			set { internaluseRightHanded = value; }
		}
		public Matrix RotationMatrix
		{
			get { return internalrotationMatrix; }
		}
		public Matrix RotationDeltaMatrix
		{
			get { return internalrotationDelta; }
		}
		public Matrix TranslationMatrix
		{
			get { return internaltranslationMatrix; }
		}
		public Matrix TranslationDeltaMatrix
		{
			get { return internaltranslationDelta; }
		}
		public bool IsBeingDragged
		{
			get { return internaldragging; }
		}

		public static int CameraControlKey
		{
			get { return cameraControlKey; }
			set { cameraControlKey = value; }
		}
		#endregion

		[System.Security.SuppressUnmanagedCodeSecurity, DllImport("User32.dll", CharSet=CharSet.Auto)]
		internal static extern bool GetAsyncKeyState(int vKey);

	}


}