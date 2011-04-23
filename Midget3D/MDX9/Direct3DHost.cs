using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using MDX9Lib.Direct3D;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using System.Runtime.InteropServices;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// used for PeekMessage
	/// </summary>
	internal enum PeekMessageFlags
	{
		NoRemove = 0,
		Remove = 1,
		Yield = 2
	}

	/// <summary>
	/// used for PeekMessage
	/// </summary>
	internal struct Message
	{
		public IntPtr hwnd;
		public uint message;
		public int wParam;
		public int lParam;
		public uint time;
		public System.Drawing.Point pt;
	}

	/// <summary>
	/// 
	/// <remarks></remarks>
	/// </summary>
	public class Direct3DHost : System.ComponentModel.Component
	{	
		[System.Security.SuppressUnmanagedCodeSecurity, DllImport("User32.dll", CharSet=CharSet.Auto)]
		internal static extern bool PeekMessage(ref Message msg, IntPtr hWnd, int wFilterMin, int wFilterMax, PeekMessageFlags flags);


		/// <summary>
		/// Occurs when the device has been initialized (usually only once per application run).
		/// </summary>
		public event EventHandler InitDeviceObjects;

		/// <summary>
		/// Occurs when the Direct3D device has been lost or reset. A common cause for this is when the viewport has been resized.
		/// </summary>
		public event EventHandler InvalidateDeviceObjects;

		/// <summary>
		/// Occurs when the Direct3D device has been restored. This occurs once during Direct3D initialization, as well as after InvalidateDeviceObjects when the viewport has been resized, the device reset, etc.
		/// </summary>
		public event EventHandler RestoreDeviceObjects;
		/// <summary>
		/// Occurs when the Direct3D device has been destroyed.
		/// </summary>
		public event EventHandler DeleteDeviceObjects;

		/// <summary>
		/// Occurs when the viewport has been resized.
		/// </summary>
		public event CancelEventHandler ViewportResized;

		private System.ComponentModel.Container components = null;


		private Direct3DHardware hardwareInfo = null;
		private Direct3DSettings graphicsSettings = null;
		private readonly PresentParameters presentParameters = new PresentParameters();

		private System.Windows.Forms.Control viewport = null;
		private Device device = null;

		private bool autoDeviceReset = true;

		private RenderStates renderStates = null;
		private SamplerStates samplerStates = null;
		private TextureStates textureStates = null;
		private Caps graphicsCaps;
		private CreateFlags behavior;
		private SurfaceDescription backBufferDesc;	// Surface desc of the backbuffer

		private bool isWindowed = false;

		private ProcessingBehavior processingBehavior = ProcessingBehavior.ConserveCpu;

		private bool fullscreenAtStart = false;
		private bool showFullScreenCursor = false;
		private bool clipFullScreenCursor = false;


		// Saved window bounds for mode switches
		private System.Drawing.Rectangle rectWindowBounds;
		// Saved client area size for mode switches
		private System.Drawing.Rectangle rectClient;


		[Browsable(false)]
		public Direct3DHardware HardwareInfo
		{
			get { return hardwareInfo; }
		}

		[Browsable(false)]
		public SurfaceDescription BackBuffer
		{
			get { return backBufferDesc; }
		}
		
		[Browsable(false)]
		public RenderStates RenderStates
		{
			get { return renderStates; }
		}

		[Browsable(false)]
		public SamplerStates SamplerStates
		{
			get { return samplerStates; }
		}

		[Browsable(false)]
		public Device Device
		{
			get { return device; }
		}

		/// <summary>
		/// Gets or sets the current <see cref="ProcessingBehavior"/>.
		/// </summary>
		/// <value>The current <see cref="ProcessingBehavior"/>.</value>
		public ProcessingBehavior ProcessingBehavior
		{
			get { return processingBehavior; }
			set { processingBehavior = value; }
		}

		/// <summary>
		/// The destination for Direct3D rendering
		/// <remarks>This can be any valid control, or the form itself.</remarks>
		/// </summary>
		public System.Windows.Forms.Control Viewport
		{
			get { return this.viewport; }
			set
			{
				this.viewport = value;
			}
		}

		/// <summary>
		/// When true, StartDirect3D will initiate fullscreen mode when called
		/// </summary>
		[Browsable(true)]
		public Boolean FullScreenAtStart
		{
			get { return fullscreenAtStart; }
			set { fullscreenAtStart = value; }
		}

		public Boolean ShowFullScreenCursor
		{
			get { return showFullScreenCursor; }
			set { showFullScreenCursor = value; }
		}

		public Boolean ClipFullScreenCursor
		{
			get { return clipFullScreenCursor; }
			set { clipFullScreenCursor = value; }
		}

		[Browsable(true)]
		public Boolean AutoDeviceReset
		{
			get { return autoDeviceReset; }
			set { autoDeviceReset = value; }
		}

		/// <summary>
		/// returns the PresentParameters for the Direct3D device
		/// </summary>
		public PresentParameters PresentParameters
		{
			get { return this.presentParameters; }
		}

		/// <summary>
		/// Constructor for Direct3DHost
		/// </summary>
		/// <param name="container">the container for this component</param>
		public Direct3DHost(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
			InitDirect3DHost();
		}

		/// <summary>
		/// Constructor for Direct3DHost
		/// </summary>
		public Direct3DHost()
		{
			InitializeComponent();
			InitDirect3DHost();
		}

		private void InitDirect3DHost()
		{
			if(!this.DesignMode)
			{
				this.hardwareInfo = new Direct3DHardware();
				this.graphicsSettings = new Direct3DSettings();
			}			
		}

		/// <summary>
		/// Starts up Direct3D.
		/// </summary>
		public void StartDirect3D()
		{
			//	check for viewport
			if(this.viewport == null)
				//	todo: use better exception class (one specific to MDXLib)
				throw new ApplicationException("You must assign a value to the viewport property before attempting to start Direct3D.");

			//	try to create D3D object
			//this.Direct3DEnumerator.ConfirmDeviceCallback = new D3DEnumeration.ConfirmDeviceCallbackType(this.ConfirmDevice);
			this.hardwareInfo.Load();

			rectWindowBounds = new System.Drawing.Rectangle(this.viewport.Location, this.viewport.Size);
			rectClient = this.viewport.ClientRectangle;

			ChooseInitialD3DSettings();

			Initialize3DEnvironment();

			//Application.AddMessageFilter(this);
		}


		public void RefreshPresentParameters()
		{
			this.presentParameters.Windowed = graphicsSettings.IsWindowed;
			this.presentParameters.BackBufferCount = 1;
			this.presentParameters.MultiSample = graphicsSettings.MultiSampleType;
			this.presentParameters.MultiSampleQuality = graphicsSettings.MultiSampleQuality;
			this.presentParameters.SwapEffect = SwapEffect.Discard;
			//	hack: depth stencil
			//this.presentParameters.EnableAutoDepthStencil = hardwareInfo.AppUsesDepthBuffer;
			this.presentParameters.EnableAutoDepthStencil = true;
			this.presentParameters.AutoDepthStencilFormat = graphicsSettings.DepthStencilBufferFormat;
			this.presentParameters.DeviceWindow = this.viewport;

				this.presentParameters.BackBufferWidth  = rectClient.Right - rectClient.Left;
				this.presentParameters.BackBufferHeight = rectClient.Bottom - rectClient.Top;
				this.presentParameters.BackBufferFormat = graphicsSettings.DeviceMode.BackBufferFormat;
				this.presentParameters.FullScreenRefreshRateInHz = 0;
				this.presentParameters.PresentationInterval = PresentInterval.Immediate;
		}

		private void Initialize3DEnvironment()
		{
			DisplayAdapter adapterInfo = graphicsSettings.DisplayAdapter;
			DisplayDevice deviceInfo = graphicsSettings.DisplayDevice;

			isWindowed = graphicsSettings.IsWindowed;

			// Prepare window for possible windowed/fullscreen change
			// AdjustWindowForChange();

			// Set up the presentation parameters
			RefreshPresentParameters();

			if(deviceInfo.Caps.PrimitiveMiscCaps.IsNullReference)
			{
				// Warn user about null ref device that can't render anything
				throw new ApplicationException("null reference device");
			}

			CreateFlags createFlags = new CreateFlags();
			switch(graphicsSettings.VertexProcessingType)
			{
				case VertexProcessingType.Software:
					createFlags = CreateFlags.SoftwareVertexProcessing;
					break;
				case VertexProcessingType.Mixed:
					createFlags = CreateFlags.MixedVertexProcessing;
					break;
				case VertexProcessingType.Hardware:
					createFlags = CreateFlags.HardwareVertexProcessing;
					break;
				case VertexProcessingType.PureHardware:
					createFlags = CreateFlags.HardwareVertexProcessing | CreateFlags.PureDevice;
					break;
				default:
					throw new ApplicationException("Unable to determine vertex processing method.");
			}

			// Create the device
			device = new Device(graphicsSettings.AdapterOrdinal,
				graphicsSettings.DisplayDevice.DeviceType,
				this.viewport,
				createFlags,
				this.presentParameters);

			if( device != null )
			{
				// Cache our local objects
				renderStates = device.RenderState;
				samplerStates = device.SamplerState;
				textureStates = device.TextureState;
				// When moving from fullscreen to windowed mode, it is important to
				// adjust the window size after recreating the device rather than
				// beforehand to ensure that you get the window size you want.  For
				// example, when switching from 640x480 fullscreen to windowed with
				// a 1000x600 window on a 1024x768 desktop, it is impossible to set
				// the window size to 1000x600 until after the display mode has
				// changed to 1024x768, because windows cannot be larger than the
				// desktop.
				if(graphicsSettings.IsWindowed && (this.viewport is System.Windows.Forms.Form))
				{
					// Make sure main window isn't topmost, so error message is visible
					this.viewport.Location = new System.Drawing.Point(rectWindowBounds.Left, rectWindowBounds.Top);
					this.viewport.Size = new System.Drawing.Size(( rectWindowBounds.Right - rectWindowBounds.Left ), ( rectWindowBounds.Bottom - rectWindowBounds.Top));
				}

				// Store device Caps
				graphicsCaps = device.DeviceCaps;
				behavior = createFlags;

				// Store render target surface desc
				Surface BackBuffer = device.GetBackBuffer(0,0, BackBufferType.Mono);
				backBufferDesc = BackBuffer.Description;
				BackBuffer.Dispose();
				BackBuffer = null;

				// Set up the fullscreen cursor
				if(showFullScreenCursor && !graphicsSettings.IsWindowed)
				{
					System.Windows.Forms.Cursor ourCursor = this.viewport.Cursor;
					device.SetCursor(ourCursor, true);
					device.ShowCursor(true);
				}

				// Confine cursor to fullscreen window
				if(clipFullScreenCursor)
				{
					if (!isWindowed)
					{
						System.Drawing.Rectangle rcWindow = this.viewport.ClientRectangle;
					}
				}
				
				// Setup the event handlers for our device
				device.DeviceLost += InvalidateDeviceObjects;
				device.DeviceReset += RestoreDeviceObjects;
				device.Disposing += DeleteDeviceObjects;
				device.DeviceResizing += new CancelEventHandler(EnvironmentResized);

				// Initialize the app's device-dependent objects
				try
				{
					if(InitDeviceObjects != null)
						InitDeviceObjects(null, null);

					if(RestoreDeviceObjects != null)
						RestoreDeviceObjects(null, null);

					return;
				}
				catch
				{
					// Cleanup before we try again
					if(InvalidateDeviceObjects != null)
						InvalidateDeviceObjects(null, null);

					if(DeleteDeviceObjects != null)
						DeleteDeviceObjects(null, null);

					device.Dispose();
					device = null;

					if(this.viewport.Disposing)
						return;
				}
			}

			//	HACK: removed fallback to reference rasterizer
			/*
			// If that failed, fall back to the reference rasterizer
			if( deviceInfo.DevType == Direct3D.DeviceType.Hardware )
			{
				if (FindBestWindowedMode(false, true))
				{
					isWindowed = true;
					if(viewport is System.Windows.Forms.Form)
					{
						// Make sure main window isn't topmost, so error message is visible
						this.viewport.Location = new System.Drawing.Point(windowBoundsRect.Left, windowBoundsRect.Top);
						this.viewport.Size = new System.Drawing.Size(( windowBoundsRect.Right - windowBoundsRect.Left ), ( windowBoundsRect.Bottom - windowBoundsRect.Top));
						//AdjustWindowForChange();
					}

					// Let the user know we are switching from HAL to the reference rasterizer
					//DisplayErrorMsg( null, AppMsgType.WarnSwitchToRef);

					Initialize3DEnvironment();
				}
			}
			*/
		}

		/// <summary>
		/// Fired when our environment was resized.
		/// </summary>
		/// <param name="sender">The device that's resizing our environment.</param>
		/// <param name="e">Set the cancel member to true to turn off automatic device reset.</param>
		public void EnvironmentResized(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(e != null)
				e.Cancel = !this.autoDeviceReset;

			if(this.ViewportResized != null)
				this.ViewportResized(sender, e);

		}

		/// <summary>
		/// Resets the device, causing InvalidateDeviceObjects and RestoreDeviceObjects to occur.
		/// <remarks>This method can be used to reset the device for a new viewport size when AutoDeviceReset is false.</remarks>
		/// </summary>
		public void ResetDevice()
		{
			this.device.Reset(this.device.PresentationParameters);
			this.EnvironmentResized(device, null);
		}

		/// <summary>
		/// Stops Direct3D.
		/// </summary>
		public void StopDirect3D()
		{
			//	TODO: make sure Direct3D is being properly shutdown here... objects disposed, etc.

			this.device.Dispose();
			//Application.RemoveMessageFilter(this);
		}

		private Boolean ChooseInitialD3DSettings()
		{
			//if(this.viewport.GetType().IsSubclassOf(typeof(System.Windows.Forms.Form))

			bool bFoundFullScreen = FindBestFullScreenMode(false, false);
			bool bFoundWindowed = FindBestWindowedMode(false, false);
			if (this.FullScreenAtStart && bFoundFullScreen)
				this.graphicsSettings.IsWindowed = false;
			return (bFoundFullScreen || bFoundWindowed);

			//return FindBestWindowedMode(false, false);
		}


		/// <summary>
		/// Sets up GraphicsSettings with best available windowed mode, subject to 
		/// the doesRequireHardware and doesRequireReference constraints.  
		/// </summary>
		/// <param name="requiresHardware">Does the device require hardware support.</param>
		/// <param name="requiresReference">Does the device require the ref device.</param>
		/// <returns>true if a mode is found, false otherwise.</returns>
		public Boolean FindBestWindowedMode(Boolean requiresHardware, Boolean requiresReference)
		{
			// Get display mode of primary adapter (which is assumed to be where the window 
			// will appear)
			DisplayMode primaryDesktopDisplayMode = Manager.Adapters[0].CurrentDisplayMode;

			DisplayAdapter bestAdapter = null;
			DisplayDevice bestDevice = null;
			DisplayDeviceMode bestDeviceMode = null;

			foreach (DisplayAdapter displayAdapter in this.hardwareInfo.DisplayAdapters)
			{
				foreach (DisplayDevice displayDevice in displayAdapter.DisplayDevices)
				{
					if (requiresHardware && displayDevice.DeviceType != DeviceType.Hardware)
						continue;
					if (requiresReference && displayDevice.DeviceType != DeviceType.Reference)
						continue;

					foreach (DisplayDeviceMode deviceMode in displayDevice.DeviceModes)
					{
						bool adapterMatchesBackBuffer = (deviceMode.BackBufferFormat == deviceMode.AdapterFormat);
						if (!deviceMode.IsWindowed)
							continue;
						if (deviceMode.AdapterFormat != primaryDesktopDisplayMode.Format)
							continue;
						// If we haven't found a compatible DeviceCombo yet, or if this set
						// is better (because it's a HAL, and/or because formats match better),
						// save it
						if (bestDeviceMode == null || 
							bestDeviceMode.DeviceType != DeviceType.Hardware && 
							displayDevice.DeviceType == DeviceType.Hardware ||
							deviceMode.DeviceType == DeviceType.Hardware && adapterMatchesBackBuffer)
						{
							bestAdapter = displayAdapter;
							bestDevice = displayDevice;
							bestDeviceMode = deviceMode;
							if (displayDevice.DeviceType == DeviceType.Hardware && adapterMatchesBackBuffer)
							{
								// This windowed device combo looks great -- take it
								goto EndWindowedDeviceComboSearch;
							}
							// Otherwise keep looking for a better windowed device combo
						}
					}
				}
			}
			EndWindowedDeviceComboSearch:
				if (bestDeviceMode == null)
					return false;

			this.graphicsSettings.WindowedDisplayAdapter = bestAdapter;
			this.graphicsSettings.WindowedDisplayDevice = bestDevice;
			this.graphicsSettings.WindowedDeviceMode = bestDeviceMode;
			this.graphicsSettings.IsWindowed = true;
			this.graphicsSettings.WindowedDisplayMode = primaryDesktopDisplayMode;
			this.graphicsSettings.WindowedWidth = this.viewport.ClientRectangle.Right - this.viewport.ClientRectangle.Left;
			this.graphicsSettings.WindowedHeight = this.viewport.ClientRectangle.Bottom - this.viewport.ClientRectangle.Top;
			//	todo: fix depth buffer setting
			//if (Direct3DEnumerator.AppUsesDepthBuffer)
				graphicsSettings.WindowedDepthStencilBufferFormat = (DepthFormat)bestDeviceMode.DepthStencilFormats[0];
			this.graphicsSettings.WindowedMultiSampleType = (MultiSampleType)bestDeviceMode.MultiSampleTypes[0];
			this.graphicsSettings.WindowedMultiSampleQuality = 0;
			this.graphicsSettings.WindowedVertexProcessingType = (VertexProcessingType)bestDeviceMode.VertexProcessingTypes[0];
			this.graphicsSettings.WindowedPresentInterval = (PresentInterval)bestDeviceMode.PresentIntervals[0];

			return true;
		}

		public Boolean FindBestFullScreenMode(Boolean requiresHardware, Boolean requiresReference)
		{
			// For fullscreen, default to first HAL DeviceCombo that supports the current desktop 
			// display mode, or any display mode if HAL is not compatible with the desktop mode, or 
			// non-HAL if no HAL is available
			DisplayMode adapterDesktopDisplayMode = new DisplayMode();
			DisplayMode bestAdapterDesktopDisplayMode = new DisplayMode();
			DisplayMode bestDisplayMode = new DisplayMode();
			bestAdapterDesktopDisplayMode.Width = 0;
			bestAdapterDesktopDisplayMode.Height = 0;
			bestAdapterDesktopDisplayMode.Format = 0;
			bestAdapterDesktopDisplayMode.RefreshRate = 0;

			DisplayAdapter bestAdapter = null;
			DisplayDevice bestDevice = null;
			DisplayDeviceMode bestDeviceMode = null;

			foreach (DisplayAdapter displayAdapter in hardwareInfo.DisplayAdapters)
			{
				adapterDesktopDisplayMode = Manager.Adapters[displayAdapter.Ordinal].CurrentDisplayMode;
				foreach (DisplayDevice displayDevice in displayAdapter.DisplayDevices)
				{
					if (requiresHardware && displayDevice.DeviceType != DeviceType.Hardware)
						continue;
					if (requiresReference && displayDevice.DeviceType != DeviceType.Reference)
						continue;
					foreach (DisplayDeviceMode deviceMode in displayDevice.DeviceModes)
					{
						bool adapterMatchesBackBuffer = (deviceMode.BackBufferFormat == deviceMode.AdapterFormat);
						bool adapterMatchesDesktop = (deviceMode.AdapterFormat == adapterDesktopDisplayMode.Format);
						if (deviceMode.IsWindowed)
							continue;
						// If we haven't found a compatible set yet, or if this set
						// is better (because it's a HAL, and/or because formats match better),
						// save it
						if (bestDeviceMode == null ||
							bestDeviceMode.DeviceType != DeviceType.Hardware && displayDevice.DeviceType == DeviceType.Hardware ||
							bestDeviceMode.DeviceType == DeviceType.Hardware && bestDeviceMode.AdapterFormat != adapterDesktopDisplayMode.Format && adapterMatchesDesktop ||
							bestDeviceMode.DeviceType == DeviceType.Hardware && adapterMatchesDesktop && adapterMatchesBackBuffer )
						{
							bestAdapterDesktopDisplayMode = adapterDesktopDisplayMode;
							bestAdapter = displayAdapter;
							bestDevice = displayDevice;
							bestDeviceMode = deviceMode;
							if (displayDevice.DeviceType == DeviceType.Hardware && adapterMatchesDesktop && adapterMatchesBackBuffer)

							{
								// This fullscreen device combo looks great -- take it
								goto EndFullScreenDeviceComboSearch;
							}
							// Otherwise keep looking for a better fullscreen device combo
						}
					}
				}
			}
			EndFullScreenDeviceComboSearch:
				if (bestDeviceMode == null)
					return false;

			// Need to find a display mode on the best adapter that uses pBestDeviceCombo->AdapterFormat
			// and is as close to bestAdapterDesktopDisplayMode's res as possible
			bestDisplayMode.Width = 0;
			bestDisplayMode.Height = 0;
			bestDisplayMode.Format = 0;
			bestDisplayMode.RefreshRate = 0;
			foreach( DisplayMode displayMode in bestAdapter.DisplayModes)
			{
				if( displayMode.Format != bestDeviceMode.AdapterFormat)
					continue;
				if( displayMode.Width == bestAdapterDesktopDisplayMode.Width &&
					displayMode.Height == bestAdapterDesktopDisplayMode.Height && 
					displayMode.RefreshRate == bestAdapterDesktopDisplayMode.RefreshRate )
				{
					// found a perfect match, so stop
					bestDisplayMode = displayMode;
					break;
				}
				else if( displayMode.Width == bestAdapterDesktopDisplayMode.Width &&
					displayMode.Height == bestAdapterDesktopDisplayMode.Height && 
					displayMode.RefreshRate > bestDisplayMode.RefreshRate )
				{
					// refresh rate doesn't match, but width/height match, so keep this
					// and keep looking
					bestDisplayMode = displayMode;
				}
				else if( bestDisplayMode.Width == bestAdapterDesktopDisplayMode.Width )
				{
					// width matches, so keep this and keep looking
					bestDisplayMode = displayMode;
				}
				else if( bestDisplayMode.Width == 0 )
				{
					// we don't have anything better yet, so keep this and keep looking
					bestDisplayMode = displayMode;
				}
			}
			graphicsSettings.FullScreenDisplayAdapter = bestAdapter;
			graphicsSettings.FullScreenDisplayDevice = bestDevice;
			graphicsSettings.FullScreenDeviceMode = bestDeviceMode;

			graphicsSettings.IsWindowed = false;
			graphicsSettings.FullScreenDisplayMode = bestDisplayMode;
			//if (hardwareInfo.AppUsesDepthBuffer)
				graphicsSettings.FullScreenDepthStencilBufferFormat = (DepthFormat)bestDeviceMode.DepthStencilFormats[0];
			graphicsSettings.FullScreenMultiSampleType = (MultiSampleType)bestDeviceMode.MultiSampleTypes[0];
			graphicsSettings.FullScreenMultiSampleQuality = 0;
			graphicsSettings.FullScreenVertexProcessingType = (VertexProcessingType)bestDeviceMode.VertexProcessingTypes[0];
			graphicsSettings.FullScreenPresentInterval = (PresentInterval)bestDeviceMode.PresentIntervals[0];
			return true;
		}

		/// <summary>
		/// I was really sick of those warning messages popping up about Message.blah,
		/// so I wrote this little thing. It seems to clean up some really stupid 
		/// Visual Studio .RESX errors as well.
		/// - steve
		/// </summary>
		private void DummyFunction()
		{
			// create a message and init it
			Message blah;
			blah.hwnd = new System.IntPtr(0);
			blah.lParam = 0;
			blah.message = 0;
			blah.pt = new System.Drawing.Point(0,0);
			blah.time = 0;
			blah.wParam = 0;
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

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}
