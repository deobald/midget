using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Midget.Cameras;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Midget
{
	/// <summary>
	/// Summary description for RevisedPictureBox.
	/// </summary>
	public class PictureBox3D : System.Windows.Forms.UserControl, IRenderSurface
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private System.Windows.Forms.ContextMenu mnuEdit;
		private System.Windows.Forms.MenuItem mnuEditDisplay;
		private System.Windows.Forms.MenuItem mnuEditDisplayWireframe;
		private System.Windows.Forms.MenuItem mnuEditDisplaySolid;
		private System.Windows.Forms.MenuItem mnuEditDisplayPoint;
		private System.Windows.Forms.MenuItem mnuPanelColor;
		private System.Windows.Forms.MenuItem mnuPanelColorBlack;
		private System.Windows.Forms.MenuItem mnuPanelColorBlue;
		private System.Windows.Forms.MenuItem mnuPanelColorRandom;
		private System.Windows.Forms.MenuItem mnuPanelColorGray;
		private System.Windows.Forms.MenuItem mnuCamera;
		private MidgetCamera	camera			= null;
		private SwapChain		swapChain		= null;
		private Surface			depthStencil	= null;
		private DrawMode		_drawMode		= DrawMode.wireFrame;
		private EditMode		_editMode		= EditMode.None;
		private bool			nameVisible;
		private bool			selected;

		// mouse movement values
		private int			mouseDownX = 0;
		private int			mouseDownY = 0;
		private AxisValue	_startRotation = new AxisValue();	// intial movement values
		private AxisValue	_startMovement = new AxisValue();
		private AxisValue	_startScaling  = new AxisValue();
		private ArcBall		arcBall;							// arcball used for camera rotation

		public const int ShiftKey = 0x10;
		
		private DeviceManager dm;
		private SceneManager  sm;

		private event RenderSingleViewHandler renderView;

		public ArrayList selectedObjects;

		public PictureBox3D(DeviceManager initDM, SceneManager initSM)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// initialize members
			nameVisible = true;				// show the name of this viewport on-screen?
			_drawMode = DrawMode.wireFrame;	// default to wireframe view
			arcBall = new ArcBall(this);
			arcBall.Radius = 1.0f;

			BuildCameraMenu();
			
			// new events
			Midget.Events.EventFactory.DeselectObjects +=new Midget.Events.Object.Selection.DeselectObjectEventHandler(EventFactory_DeselectObjects);
			Midget.Events.EventFactory.SelectAdditionalObject +=new Midget.Events.Object.Selection.SelectAdditionalObjectEventHandler(EventFactory_SelectAdditionalObject);			
			Midget.Events.EventFactory.AdjustCameraEvent +=new Midget.Events.User.AdjustCameraEventHandler(EventFactory_AdjustCameraEvent);
			Midget.Events.EventFactory.SwitchEditModeEvent +=new Midget.Events.User.SwitchEditModeEventHandler(EventFactory_SwitchEditModeEvent);
			selectedObjects = new ArrayList();

			this.dm = initDM;

			// attach listener for render event
			dm.Render += new System.EventHandler(this.Viewport_Render);
			this.renderView += new RenderSingleViewHandler(dm.OnRenderSingleView); 

			this.sm = initSM;

			this.ConfigureRenderTarget();
		}
		
		protected void Viewport_Render(object sender, EventArgs e)
		{
			this.Render();
		}

		/// <summary>
		/// Renders the current viewport
		/// </summary>
		public void Render()
		{
			if(this.swapChain != null && this.depthStencil != null)
			{
				if(this.swapChain.Disposed)
					this.ConfigureRenderTarget();

				try
				{
					this.renderView(this, new RenderSingleViewEventArgs(sm.Scene,sm.CurrentFrameIndex));
				}
				catch (Microsoft.DirectX.DirectXException)
				{
					// try to re-render
					this.renderView(this, new RenderSingleViewEventArgs(sm.Scene,sm.CurrentFrameIndex));
				}
			}
			else
			{
				this.ConfigureRenderTarget();
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mnuEdit = new System.Windows.Forms.ContextMenu();
			this.mnuEditDisplay = new System.Windows.Forms.MenuItem();
			this.mnuEditDisplayWireframe = new System.Windows.Forms.MenuItem();
			this.mnuEditDisplaySolid = new System.Windows.Forms.MenuItem();
			this.mnuEditDisplayPoint = new System.Windows.Forms.MenuItem();
			this.mnuPanelColor = new System.Windows.Forms.MenuItem();
			this.mnuPanelColorBlack = new System.Windows.Forms.MenuItem();
			this.mnuPanelColorGray = new System.Windows.Forms.MenuItem();
			this.mnuPanelColorBlue = new System.Windows.Forms.MenuItem();
			this.mnuPanelColorRandom = new System.Windows.Forms.MenuItem();
			this.mnuCamera = new System.Windows.Forms.MenuItem();
			// 
			// mnuEdit
			// 
			this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuEditDisplay,
																					this.mnuPanelColor,
																					this.mnuCamera});
			this.mnuEdit.Popup += new System.EventHandler(this.mnuEdit_Popup);
			// 
			// mnuEditDisplay
			// 
			this.mnuEditDisplay.Index = 0;
			this.mnuEditDisplay.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.mnuEditDisplayWireframe,
																						   this.mnuEditDisplaySolid,
																						   this.mnuEditDisplayPoint});
			this.mnuEditDisplay.Text = "&Display";
			// 
			// mnuEditDisplayWireframe
			// 
			this.mnuEditDisplayWireframe.Index = 0;
			this.mnuEditDisplayWireframe.Text = "&Wireframe";
			this.mnuEditDisplayWireframe.Click += new System.EventHandler(this.mnuEditDisplayWireframe_Click);
			// 
			// mnuEditDisplaySolid
			// 
			this.mnuEditDisplaySolid.Index = 1;
			this.mnuEditDisplaySolid.Text = "&Solid";
			this.mnuEditDisplaySolid.Click += new System.EventHandler(this.mnuEditDisplaySolid_Click);
			// 
			// mnuEditDisplayPoint
			// 
			this.mnuEditDisplayPoint.Index = 2;
			this.mnuEditDisplayPoint.Text = "&Point";
			this.mnuEditDisplayPoint.Click += new System.EventHandler(this.mnuEditDisplayPoint_Click);
			// 
			// mnuPanelColor
			// 
			this.mnuPanelColor.Index = 1;
			this.mnuPanelColor.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuPanelColorBlack,
																						  this.mnuPanelColorGray,
																						  this.mnuPanelColorBlue,
																						  this.mnuPanelColorRandom});
			this.mnuPanelColor.Text = "Panel Color";
			// 
			// mnuPanelColorBlack
			// 
			this.mnuPanelColorBlack.Index = 0;
			this.mnuPanelColorBlack.Text = "Black";
			this.mnuPanelColorBlack.Click += new System.EventHandler(this.mnuPanelColorBlack_Click);
			// 
			// mnuPanelColorGray
			// 
			this.mnuPanelColorGray.Index = 1;
			this.mnuPanelColorGray.Text = "Gray";
			this.mnuPanelColorGray.Click += new System.EventHandler(this.mnuPanelColorGray_Click);
			// 
			// mnuPanelColorBlue
			// 
			this.mnuPanelColorBlue.Index = 2;
			this.mnuPanelColorBlue.Text = "Blue";
			this.mnuPanelColorBlue.Click += new System.EventHandler(this.mnuPanelColorBlue_Click);
			// 
			// mnuPanelColorRandom
			// 
			this.mnuPanelColorRandom.Index = 3;
			this.mnuPanelColorRandom.Text = "Random";
			this.mnuPanelColorRandom.Click += new System.EventHandler(this.mnuPanelColorRandom_Click);
			// 
			// mnuCamera
			// 
			this.mnuCamera.Index = 2;
			this.mnuCamera.Text = "&Camera";
			// 
			// PictureBox3D
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Name = "PictureBox3D";
			this.Size = new System.Drawing.Size(150, 73);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PictureBox3D_KeyUp);

		}
		#endregion
		

		private void ConfigureRenderTarget()
		{
			try
			{
				// if the device has had a chance to be init
				if(dm != null && dm.Device != null)
				{
					PresentParameters presentParams = dm.PresentParameters;

					presentParams.DeviceWindow = this;

					presentParams.BackBufferHeight = this.ClientSize.Height;
					presentParams.BackBufferWidth =  this.ClientSize.Width;

					swapChain = new SwapChain(dm.Device, presentParams);

					depthStencil = dm.Device.CreateDepthStencilSurface( this.ClientSize.Width, this.ClientSize.Height,
						presentParams.AutoDepthStencilFormat, presentParams.MultiSample, presentParams.MultiSampleQuality,
						true );
			
					// setup arcball
					arcBall.SetWindow(swapChain.PresentParamters.BackBufferWidth, swapChain.PresentParamters.BackBufferHeight, 0.85f);
				}
			}
			catch
			{
				ConfigureRenderTarget();
			}
		}

		protected override void OnResize(EventArgs e)
		{
			// error check for minimized window
			if (this.ParentForm != null)
			{
				if (this.ParentForm.WindowState == FormWindowState.Minimized)
				{
					return;
				}
			}
			else
			{
				return;
			}
			
			base.OnResize (e);
			
			if(this.swapChain != null)
			{
				swapChain.Dispose();
			}

			if(depthStencil != null)
			{
				this.depthStencil.Dispose();
			}

			this.ConfigureRenderTarget();
		
			if (camera != null)
			{
				camera.SetProjectionParameters(camera.FOV, ((float)this.ClientWidth / (float)this.ClientHeight), 
					camera.NearClipPlane, camera.FarClipPlane);
			}
		
			if (dm != null && dm.Device != null)
			{
				this.Render();
			}

		}
		

		//		private void PictureBox3D_SelectObject(object sender, Midget.Event.SelectObjectEventArgs e)
		//		{
		//			// if the user is not selecting multiple objects, clear the list
		//			if (!GetAsyncKeyState(ShiftKey))
		//			{
		//				selectedObjects.Clear();
		//			}
		//
		//			// add the selected object to the list
		//			selectedObjects.Add(e.SelectedObject);
		//
		//		}

		#region MouseEvents

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			
			if (!GetAsyncKeyState(ArcBall.CameraControlKey))
			{
				if (e.Button == MouseButtons.Left)
				{

					// object: movement

					// start the object movement
					mouseDownX = e.X;
					mouseDownY = e.Y;
					
					if(sm.SelectedObjects.Count > 0)
					{
						IObject3D selObject = (IObject3D)sm.SelectedObjects[sm.SelectedObjects.Count - 1];
					
						// set cursors and initial object movement values
						if (_editMode == EditMode.None)
						{
							this.Cursor = Cursors.Default;
						}
						else if (_editMode == EditMode.Move)
						{
							this.Cursor = new Cursor(GetType(), "move.cur");
							_startMovement = selObject.Translation;
						}
						else if (_editMode == EditMode.Rotate)
						{
							this.Cursor = new Cursor(GetType(), "rotate.cur");
							_startRotation = selObject.Rotation;
						}
						else if (_editMode == EditMode.Scale)
						{
							this.Cursor = new Cursor(GetType(), "zoom.cur");
							_startScaling = selObject.Scaling;
						}
					}
				}
			}
			
			base.OnMouseDown(e);
		}


		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			// event toggles
			bool firePickEvent = true;
			bool fireDeselectEvent = true;
			
			if (e.Button == MouseButtons.Left)
			{
				// cursor
				this.Cursor = Cursors.Default;
					
				// TODO: Complete object manipulation and send an undo-able command
				if (_editMode == EditMode.Rotate)
				{
					fireDeselectEvent = false;
				}
				else if (_editMode == EditMode.Move)
				{
					fireDeselectEvent = false;
				}
				else if (_editMode == EditMode.Scale)
				{
					fireDeselectEvent = false;
				}

				// the left mouse button is indicitave of object selection
				IObject3D picked = ObjectPicker.Pick(e.X, e.Y, this.Camera, this);
				if (picked != null)
				{
					if (GetAsyncKeyState(ShiftKey))
					{
						Midget.Events.EventFactory.Instance.GenerateSelectAdditionalObjectRequestEvent(this,picked);
					}
					else
					{
						// determine if this object is in the list
						foreach (IObject3D obj in selectedObjects)
						{
							if (obj == picked)
							{
								firePickEvent = false;
								break;
							}
						}

						// only select this if it's not already in the list
						if (firePickEvent)
						{
							Midget.Events.EventFactory.Instance.GenerateSelectObjectRequestEvent(this,picked);
						}
					}
				}
				else if (picked == null)
				{	
					if (fireDeselectEvent)
					{
						// TODO: Allow deselection of single object

						Midget.Events.EventFactory.Instance.GenerateDeselectAllObjectsEventRequest(this,selectedObjects);
					}
				}
			
			}
			else if (e.Button == MouseButtons.Right)
			{
				// the right mouse button displays menus, if they are available
				if (e.X < 100 && e.Y < 30)
				{
					mnuEdit.Show(this, new Point(e.X, e.Y));
				}
			}
			
			base.OnMouseUp(e);
		}


		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{

			if ( !GetAsyncKeyState(ArcBall.CameraControlKey) && (e.Button == MouseButtons.Left) && selectedObjects.Count != 0 )
			{

				if (_editMode == EditMode.Rotate)
				{

					Vector3 direction = camera.TargetPoint - camera.EyePosition;
					Vector3 horizontal = Vector3.Cross(direction, camera.UpVector);
					Vector3 upVector = camera.UpVector;

					horizontal.Normalize();
					upVector.Normalize();

					float rotateX = (0.5f * (mouseDownX - e.X)) * upVector.X +
						(0.5f * (e.Y - mouseDownY)) * horizontal.X + _startRotation.X;

					float rotateY = (0.5f * (mouseDownX - e.X)) * upVector.Y +
						(0.5f * (e.Y - mouseDownY)) * horizontal.Y + _startRotation.Y;

					float rotateZ = (0.5f * (mouseDownX - e.X)) * upVector.Z +
						(0.5f * (e.Y - mouseDownY)) * horizontal.Z + _startRotation.Z;

					Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
						new AxisValue(rotateX, rotateY, rotateZ),
						Midget.Events.Object.Transformation.Transformation.Rotate);

					mouseDownX = e.X;
					mouseDownY = e.Y;

					dm.UpdateViews();
				}
				else if (_editMode == EditMode.Move)
				{
					// our picking 
					Vector3 pickRayOrigin = new Vector3();
					Vector3 pickRayDir = new Vector3();
					Vector3 pickRayOriginNew = new Vector3();
					Vector3 pickRayDirNew = new Vector3();

					Vector3 direction = camera.TargetPoint - camera.EyePosition;
					Vector3 horizontal = Vector3.Cross(direction, camera.UpVector);
					Vector3 upVector = camera.UpVector;			

					horizontal.Normalize();
					upVector.Normalize();

					// grab the last selected object
					IObject3D selObj = (IObject3D)selectedObjects[selectedObjects.Count - 1];

					// original mouse position
					camera.UnProjectCoordinates(mouseDownX, mouseDownY, this.Width, 
						this.Height, ref pickRayOrigin, ref pickRayDir);

					// new mouse position
					camera.UnProjectCoordinates(e.X, 
						e.Y, this.Width, 
						this.Height, ref pickRayOriginNew, ref pickRayDirNew);

					// transform by world and object model space
					pickRayOrigin.TransformCoordinate(Matrix.Invert(camera.WorldMatrix));
					pickRayOriginNew.TransformCoordinate(Matrix.Invert(camera.WorldMatrix));

					// find the movement vector
					Vector3 result = pickRayOriginNew - pickRayOrigin;

					// pretty crappy compensation
					if (this.Camera is PerspectiveMidgetCamera)
					{
						result *= 10.0f;
					}

					// generate an axis value to move by
					AxisValue axes = new AxisValue(
						selObj.Translation.X + result.X, 
						selObj.Translation.Y + result.Y, 
						selObj.Translation.Z + result.Z);
					
					Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this, selectedObjects,
						axes,
						Midget.Events.Object.Transformation.Transformation.Translate);

					mouseDownX = e.X;
					mouseDownY = e.Y;
						
					dm.UpdateViews();
				}
				else if (_editMode == EditMode.Scale)
				{
					float scaleX = 0.05f * (mouseDownX - e.X) + _startScaling.X;
					float scaleY = 0.05f * (mouseDownY - e.Y) + _startScaling.Y;

					Midget.Events.EventFactory.Instance.GenerateTransformationRequestEvent(this,selectedObjects,
						new AxisValue(scaleY,scaleY,scaleY),
						Midget.Events.Object.Transformation.Transformation.Scale);

					mouseDownX = e.X;
					mouseDownY = e.Y;

					dm.UpdateViews();
				}
			}

			base.OnMouseMove (e);
		}

		#endregion

		#region PopUpMenu
		
		private void mnuEditDisplayWireframe_Click(object sender, System.EventArgs e)
		{
			// change the draw mode for this viewport, then render using the new draw mode
			_drawMode = DrawMode.wireFrame;
			this.Invalidate();
		}

		private void mnuEditDisplaySolid_Click(object sender, System.EventArgs e)
		{
			// change the draw mode for this viewport, then render using the new draw mode
			_drawMode = DrawMode.solid;
			this.Invalidate();
		}

		private void mnuEditDisplayPoint_Click(object sender, System.EventArgs e)
		{
			// change the draw mode for this viewport, then render using the new draw mode
			_drawMode = DrawMode.point;
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.Render();
		}

		private void mnuPanelColorBlack_Click(object sender, System.EventArgs e)
		{
			this.BackColor = Color.Black;
		}

		private void mnuPanelColorGray_Click(object sender, System.EventArgs e)
		{
			this.BackColor = Color.Gray;
		}

		private void mnuPanelColorBlue_Click(object sender, System.EventArgs e)
		{
			this.BackColor = Color.DarkBlue;
		}

		private void mnuPanelColorRandom_Click(object sender, System.EventArgs e)
		{
			// create a random object and pick an int between 0 and 255
			// Yes. This was a 2 AM creation. -steve
			Random R = new Random();
			this.BackColor = Color.FromArgb(R.Next(255), R.Next(255), R.Next(255));
		}

		protected void mnuCamera_CameraClick(object sender, System.EventArgs e)
		{
			// change the camera
			this.camera = CameraFactory.Instance.GetExistingCamera(((MenuItem)sender).Index);
			
			this.Render();
		}
		
		private void BuildCameraMenu()
		{
			// clear out old menu
			for (int i = mnuCamera.MenuItems.Count - 1; i >= 0; ++i)
				mnuCamera.MenuItems.RemoveAt( i );
				
			// get camera names
			ArrayList cameraNames = CameraFactory.Instance.CameraNames;

			// add menu items
			for(int i = 0; i < CameraFactory.Instance.NumberOfCameras; ++i)
				mnuCamera.MenuItems.Add((string)cameraNames[i], new EventHandler(mnuCamera_CameraClick));
		}

		private void mnuEdit_Popup(object sender, System.EventArgs e)
		{
			// if the camera menu is not upto date
			if(mnuCamera.MenuItems.Count != CameraFactory.Instance.NumberOfCameras)
			{
				BuildCameraMenu();
			}

			// check mark the currently selected camera
			for(int i = 0; i < mnuCamera.MenuItems.Count; ++i)
			{
				if(mnuCamera.MenuItems[i].Text == camera.Name)
					mnuCamera.MenuItems[i].Checked = true;
				else
					mnuCamera.MenuItems[i].Checked = false;
			}
		}
		#endregion

		
		#region IRenderSurface Properties 
		[Browsable(false)]
		public int ClientHeight
		{
			get { return this.ClientSize.Height; }
		}
		
		[Browsable(false)]
		public int ClientWidth
		{
			get { return this.ClientSize.Width; }
		}
		
		[Browsable(false)]
		public Surface RenderTarget
		{
			get 
			{ 
				if(this.swapChain != null)
					return swapChain.GetBackBuffer(0,BackBufferType.Mono); 
				else
					return null;
			}
		}
		
		[Browsable(false)]
		public MidgetCamera Camera
		{
			get { return camera; }
			set { camera = value; }
		}
		
		[Browsable(false)]
		public Surface DepthStencil
		{
			get { return depthStencil; }
		}
		
		[Browsable(false)]
		public SwapChain SwapChain
		{
			get { return swapChain; }
		}

		[Browsable(false)]
		public string Description
		{
			get { return camera.Name; }
		}

		[Browsable(true)]
		public DrawMode DrawMode
		{
			get { return _drawMode; }
			set { _drawMode = value; }
		}

		[Browsable(true)]
		public bool NameVisible
		{
			get { return nameVisible; }
			set { nameVisible = value; }
		}

		[Browsable(false)]
		public bool Selected
		{	
			get { return true; }
			set { selected = value; }
		}
		#endregion

		[System.Security.SuppressUnmanagedCodeSecurity, DllImport("User32.dll", CharSet=CharSet.Auto)]
		internal static extern bool GetAsyncKeyState(int vKey);

		private void PictureBox3D_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (selectedObjects.Count >= 1)
			{
				IObject3D selectObj = ((IObject3D)selectedObjects[selectedObjects.Count-1]);
				
				if (e.KeyCode == Keys.Up)
				{
					if (   selectObj.Parent != SceneManager.Instance.Scene)
					{
						//MessageBox.Show(  ((IObject3D)SceneManager.Instance.SelectedObjects[0]).Parent.Name );
						IObject3D parentUp;
						parentUp = selectObj.Parent;
						Midget.Events.EventFactory.Instance.GenerateSelectObjectRequestEvent(this,parentUp);
					}

				}
				else if (e.KeyCode == Keys.Down)
				{
					if (selectObj.Children.Count >=1)
					{
						//MessageBox.Show(  ((IObject3D) ((IObject3D)SceneManager.Instance.SelectedObjects[0]).Children[0]).Name);
						IObject3D childDown;
						childDown = (  (IObject3D)selectObj.Children[0]);
						Midget.Events.EventFactory.Instance.GenerateSelectObjectRequestEvent(this,childDown);
					}
				}
				else if (e.KeyCode == Keys.Left)
				{
					if  (  selectObj.Parent.Children.Count >1)
					{
						int selectIndex = selectObj.Parent.Children.IndexOf(selectObj);
						IObject3D brotherLeft;
						if (selectIndex != 0)
						{
							brotherLeft = ((IObject3D)selectObj.Parent.Children[selectIndex - 1]);
						}
						else
						{
							brotherLeft = ((IObject3D)selectObj.Parent.Children[selectObj.Parent.Children.Count - 1]);
						}
						Midget.Events.EventFactory.Instance.GenerateSelectObjectRequestEvent(this,brotherLeft);
					}
				}
				else if (e.KeyCode == Keys.Right)
				{
					if  (  selectObj.Parent.Children.Count >1)
					{
						int selectIndex = selectObj.Parent.Children.IndexOf(selectObj);
						IObject3D brotherLeft;
						if (selectIndex != selectObj.Parent.Children.Count-1)
						{ 
							brotherLeft = ((IObject3D)selectObj.Parent.Children[selectIndex + 1]);
						}
						else
						{
							brotherLeft = ((IObject3D)selectObj.Parent.Children[0]);
						}
						Midget.Events.EventFactory.Instance.GenerateSelectObjectRequestEvent(this, brotherLeft);
					}
				}
				else if (e.KeyCode == Keys.Delete)
				{
					Midget.Events.EventFactory.Instance.GenerateDeleteObjectRequestEvent(this, this.selectedObjects);
				}
			}
		}

		private void EventFactory_DeselectObjects(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Remove(e.Object);
		}

		private void EventFactory_SelectAdditionalObject(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Add(e.Object);
		}

		private void EventFactory_AdjustCameraEvent(object sender, Midget.Events.User.AdjustCameraEventArgs e)
		{
			// if the cameras are equal redraw the view
			if ( camera != null && camera.Equals(e.Camera) )
			{
				this.Render();
			}
		}

		private void EventFactory_SwitchEditModeEvent(object sender, Midget.Events.User.SwitchEditModeEventArgs e)
		{
			_editMode = e.EditMode;
		}
	}
}
