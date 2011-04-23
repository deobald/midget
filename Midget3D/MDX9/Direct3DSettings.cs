using System;
using MDX9Lib.Direct3D;
using Microsoft.DirectX.Direct3D;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// Contains settings for a Direct3D session.
	/// </summary>
	public class Direct3DSettings
	{
		private Boolean isWindowed;

		private DisplayAdapter windowedDisplayAdapter;
		private DisplayDevice windowedDisplayDevice;
		private DisplayDeviceMode windowedDeviceMode;

		private DisplayMode windowedDisplayMode; // not changable by the user
		private DepthFormat windowedDepthStencilBufferFormat;
		private MultiSampleType windowedMultiSampleType;
		private Int32 windowedMultiSampleQuality;
		private VertexProcessingType windowedVertexProcessingType;
		private PresentInterval windowedPresentInterval;
		private Int32 windowedWidth;
		private Int32 windowedHeight;


		private DisplayAdapter fullScreenDisplayAdapter;
		private DisplayDevice fullScreenDisplayDevice;
		private DisplayDeviceMode fullScreenDeviceMode;

		private DisplayMode fullScreenDisplayMode; // changable by the user
		private DepthFormat fullScreenDepthStencilBufferFormat;
		private MultiSampleType fullScreenMultiSampleType;
		private Int32 fullScreenMultiSampleQuality;
		private VertexProcessingType fullScreenVertexProcessingType;
		private PresentInterval fullScreenPresentInterval;
		
		/// <summary>
		/// Defaut constructor.
		/// </summary>
		public Direct3DSettings()
		{
		}

		#region Window Properties
		/// <summary>
		/// Gets or sets a value indicating whether the application is to run in a window.
		/// </summary>
		/// <value><see cref="Boolean.True"/> if windowed, otherwise <see cref="Boolean.False"/>.</value>
		public Boolean IsWindowed { get { return this.isWindowed; } set { this.isWindowed = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DisplayAdapter"/> for windowed applications.
		/// </summary>
		/// <value>The current <see cref="DisplayAdapter"/> for windowed applications.</value>
		public DisplayAdapter WindowedDisplayAdapter { get { return this.windowedDisplayAdapter; } set { this.windowedDisplayAdapter = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DisplayDevice"/> for windowed applications.
		/// </summary>
		/// <value>The current <see cref="DisplayDevice"/> for windowed applications.</value>
		public DisplayDevice WindowedDisplayDevice { get { return this.windowedDisplayDevice; } set { this.windowedDisplayDevice = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DisplayDeviceMode"/> for windowed applications.
		/// </summary>
		/// <value>The current <see cref="DisplayDeviceMode"/> for windowed applications.</value>
		public DisplayDeviceMode WindowedDeviceMode { get { return this.windowedDeviceMode; } set { this.windowedDeviceMode = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DisplayMode"/> for windowed applications.
		/// </summary>
		/// <value>The current <see cref="DisplayMode"/> for windowed applications.</value>
		public DisplayMode WindowedDisplayMode { get { return this.windowedDisplayMode; } set { this.windowedDisplayMode = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DepthFormat"/> for windowed applications.
		/// </summary>
		/// <value>The current <see cref="DepthFormat"/> for windowed applications.</value>
		public DepthFormat WindowedDepthStencilBufferFormat { get { return this.windowedDepthStencilBufferFormat; } set { this.windowedDepthStencilBufferFormat = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="MultiSampleType"/> for windowed applications.
		/// </summary>
		/// <value>The current <see cref="MultiSampleType"/> for windowed applications.</value>
		public MultiSampleType WindowedMultiSampleType { get { return this.windowedMultiSampleType; } set { this.windowedMultiSampleType = value; } }

		/// <summary>
		/// Gets or sets an <see cref="Int32"/> indicating the multi-sample quality for windowed applications.
		/// </summary>
		/// <value>The relative multi-sample quality.</value>
		public Int32 WindowedMultiSampleQuality { get { return this.windowedMultiSampleQuality; } set { this.windowedMultiSampleQuality = value; } }

		/// <summary>
		/// Gets or sets the <see cref="VertexProcessingType"/> for windowed applications.
		/// </summary>
		/// <value>The <see cref="VertexProcessingType"/> for windowed applications.</value>
		public VertexProcessingType WindowedVertexProcessingType { get { return this.windowedVertexProcessingType; } set { this.windowedVertexProcessingType = value; } }

		/// <summary>
		/// Gets or sets the <see cref="PresentInterval"/> for windowed applications.
		/// </summary>
		/// <value>The <see cref="PresentInterval"/> for windowed applications.</value>
		public PresentInterval WindowedPresentInterval { get { return this.windowedPresentInterval; } set { this.windowedPresentInterval = value; } }

		/// <summary>
		/// Gets or sets an <see cref="Int32"/> indicating the width of the window in pixels.
		/// </summary>
		/// <value>An <see cref="Int32"/> indicating the width of the window in pixels.</value>
		public Int32 WindowedWidth { get { return this.windowedWidth; } set { this.windowedWidth = value; } }

		/// <summary>
		/// Gets or sets an <see cref="Int32"/> indicating the height of the window in pixels.
		/// </summary>
		/// <value>An <see cref="Int32"/> indicating the height of the window in pixels.</value>
		public Int32 WindowedHeight { get { return this.windowedHeight; } set { this.windowedHeight = value; } }
		#endregion

		#region Full-Screen Properties
		/// <summary>
		/// Gets or sets the current <see cref="DisplayAdapter"/> for full-screen applications.
		/// </summary>
		/// <value>The current <see cref="DisplayAdapter"/> for full-screen applications.</value>
		public DisplayAdapter FullScreenDisplayAdapter { get { return this.fullScreenDisplayAdapter; } set { this.fullScreenDisplayAdapter = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DisplayDevice"/> for full-screen applications.
		/// </summary>
		/// <value>The current <see cref="DisplayDevice"/> for full-screen applications.</value>
		public DisplayDevice FullScreenDisplayDevice { get { return this.fullScreenDisplayDevice; } set { this.fullScreenDisplayDevice = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DisplayDeviceMode"/> for full-screen applications.
		/// </summary>
		/// <value>The current <see cref="DisplayDeviceMode"/> for full-screen applications.</value>
		public DisplayDeviceMode FullScreenDeviceMode { get { return this.fullScreenDeviceMode; } set { this.fullScreenDeviceMode = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DisplayMode"/> for full-screen applications.
		/// </summary>
		/// <value>The current <see cref="DisplayMode"/> for full-screen applications.</value>
		public DisplayMode FullScreenDisplayMode { get { return this.fullScreenDisplayMode; } set { this.fullScreenDisplayMode = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="DepthFormat"/> for full-screen applications.
		/// </summary>
		/// <value>The current <see cref="DepthFormat"/> for full-screen applications.</value>
		public DepthFormat FullScreenDepthStencilBufferFormat { get { return this.fullScreenDepthStencilBufferFormat; } set { this.fullScreenDepthStencilBufferFormat = value; } }

		/// <summary>
		/// Gets or sets the current <see cref="MultiSampleType"/> for full-screen applications.
		/// </summary>
		/// <value>The current <see cref="MultiSampleType"/> for full-screen applications.</value>
		public MultiSampleType FullScreenMultiSampleType { get { return this.fullScreenMultiSampleType; } set { this.fullScreenMultiSampleType = value; } }

		/// <summary>
		/// Gets or sets an <see cref="Int32"/> indicating the multi-sample quality for full-screen applications.
		/// </summary>
		/// <value>The relative multi-sample quality.</value>
		public Int32 FullScreenMultiSampleQuality { get { return this.fullScreenMultiSampleQuality; } set { this.fullScreenMultiSampleQuality = value; } }

		/// <summary>
		/// Gets or sets the <see cref="VertexProcessingType"/> for full-screen applications.
		/// </summary>
		/// <value>The <see cref="VertexProcessingType"/> for full-screen applications.</value>
		public VertexProcessingType FullScreenVertexProcessingType { get { return this.fullScreenVertexProcessingType; } set { this.fullScreenVertexProcessingType = value; } }

		/// <summary>
		/// Gets or sets the <see cref="PresentInterval"/> for full-screen applications.
		/// </summary>
		/// <value>The <see cref="PresentInterval"/> for full-screen applications.</value>
		public PresentInterval FullScreenPresentInterval { get { return this.fullScreenPresentInterval; } set { this.fullScreenPresentInterval = value; } }
		#endregion

		#region FullScreen/Window (Autodetermined) Properties
		/// <summary>
		/// Gets or sets the <see cref="DisplayAdapter"/>, based on whether or not the application is windowed.
		/// </summary>
		/// <value>The <see cref="DisplayAdapter"/>, based on whether or not the application is windowed.</value>
		public DisplayAdapter DisplayAdapter
		{
			get { return this.isWindowed ? this.windowedDisplayAdapter : this.fullScreenDisplayAdapter; }
			set { if (this.isWindowed) this.windowedDisplayAdapter = value; else this.fullScreenDisplayAdapter = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="DisplayDevice"/>, based on whether or not the application is windowed.
		/// </summary>
		/// <value>The <see cref="DisplayDevice"/>, based on whether or not the application is windowed.</value>
		public DisplayDevice DisplayDevice
		{
			get { return this.isWindowed ? this.windowedDisplayDevice : this.fullScreenDisplayDevice; }
			set { if (this.isWindowed) this.windowedDisplayDevice = value; else this.fullScreenDisplayDevice = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="DisplayDeviceMode"/>, based on whether or not the application is windowed.
		/// </summary>
		/// <value>The <see cref="DisplayDeviceMode"/>, based on whether or not the application is windowed.</value>
		public DisplayDeviceMode DeviceMode
		{
			get { return this.isWindowed ? this.windowedDeviceMode : this.fullScreenDeviceMode; }
			set { if (this.isWindowed) this.windowedDeviceMode = value; else this.fullScreenDeviceMode = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="DisplayMode"/>, based on whether or not the application is windowed.
		/// </summary>
		/// <value>The <see cref="DisplayMode"/>, based on whether or not the application is windowed.</value>
		public DisplayMode DisplayMode
		{
			get { return this.isWindowed ? this.windowedDisplayMode : this.fullScreenDisplayMode; }
			set { if (this.isWindowed) this.windowedDisplayMode = value; else this.fullScreenDisplayMode = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="DepthFormat"/> for the stencil buffer, based on whether or not the application is windowed.
		/// </summary>
		/// <value>The <see cref="DepthFormat"/> for the stencil buffer, based on whether or not the application is windowed.</value>
		public DepthFormat DepthStencilBufferFormat
		{
			get { return this.isWindowed ? this.windowedDepthStencilBufferFormat : this.fullScreenDepthStencilBufferFormat; }
			set { if (this.isWindowed) this.windowedDepthStencilBufferFormat = value; else this.fullScreenDepthStencilBufferFormat = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="MultiSampleType"/>, based on whether or not the application is windowed.
		/// </summary>
		/// <value>The <see cref="MultiSampleType"/>, based on whether or not the application is windowed.</value>
		public MultiSampleType MultiSampleType
		{
			get { return this.isWindowed ? this.windowedMultiSampleType : this.fullScreenMultiSampleType; }
			set { if (this.isWindowed) this.windowedMultiSampleType = value; else this.fullScreenMultiSampleType = value; }
		}

		/// <summary>
		/// Gets or sets an <see cref="Int32"/> representing the relative multi-sample quality, based on whether or not the application is windowed.
		/// </summary>
		/// <value>An <see cref="Int32"/> representing the relative multi-sample quality, based on whether or not the application is windowed.</value>
		public Int32 MultiSampleQuality
		{
			get { return this.isWindowed ? this.windowedMultiSampleQuality : this.fullScreenMultiSampleQuality; }
			set { if (this.isWindowed) this.windowedMultiSampleQuality = value; else this.fullScreenMultiSampleQuality = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="VertexProcessingType"/>, based on whether or not the application is windowed.
		/// </summary>
		/// <value>The <see cref="VertexProcessingType"/>, based on whether or not the application is windowed.</value>
		public VertexProcessingType VertexProcessingType
		{
			get { return this.isWindowed ? this.windowedVertexProcessingType : this.fullScreenVertexProcessingType; }
			set { if (this.isWindowed) this.windowedVertexProcessingType = value; else this.fullScreenVertexProcessingType = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="PresentInterval"/>, based on whether or not the application is windowed.
		/// </summary>
		/// <value>The <see cref="PresentInterval"/>, based on whether or not the application is windowed.</value>
		public PresentInterval PresentInterval
		{
			get { return this.isWindowed ? this.windowedPresentInterval : this.fullScreenPresentInterval; }
			set { if (this.isWindowed) this.windowedPresentInterval = value; else this.fullScreenPresentInterval = value; }
		}
		#endregion

		/// <summary>
		/// Gets the <see cref="P:Direct3DSettings.DisplayAdapter"/>'s ordinal.
		/// </summary>
		/// <value>The <see cref="P:Direct3DSettings.DisplayAdapter"/>'s ordinal.</value>
		public Int32 AdapterOrdinal { get { return DisplayAdapter.Ordinal; } }

		/// <summary>
		/// Gets the <see cref="DeviceType"/>.
		/// </summary>
		/// <value>The <see cref="DeviceType"/>.</value>
		public DeviceType DeviceType { get { return DisplayDevice.DeviceType; } }

		/// <summary>
		/// Gets the <see cref="Format"/> for the back buffer.
		/// </summary>
		/// <value>The <see cref="Format"/> for the back buffer.</value>
		public Format BackBufferFormat { get { return DeviceMode.BackBufferFormat; } }

		/// <summary>
		/// Creates a shallow copy of this <see cref="Direct3DSettings"/> instance.
		/// </summary>
		/// <returns>A shallow copy of this <see cref="Direct3DSettings"/> instance.</returns>
		public Direct3DSettings Clone() 
		{
			return (Direct3DSettings)MemberwiseClone();
		}
	}
}