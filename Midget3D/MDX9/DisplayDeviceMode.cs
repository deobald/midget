using System;
using System.Collections;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// A combination of <see cref="AdapterFormat"/>, <see cref="BackBufferFormat"/>, and windowed/fullscreen that is compatible with a particular D3D device (and the app)
	/// </summary>
	public class DisplayDeviceMode
	{
		private readonly DisplayDevice displayDevice;
		private readonly Format adapterFormat;
		private readonly Format backBufferFormat;
		private readonly Boolean isWindowed;
		private readonly ArrayList depthStencilFormats = new ArrayList(); // List of D3DFORMATs
		private readonly ArrayList multiSampleTypes = new ArrayList(); // List of D3DMULTISAMPLE_TYPEs
		private readonly ArrayList multiSampleQualityLevels = new ArrayList(); // List of ints (maxQuality per multisample type)
		private readonly ArrayList depthStencilMultiSampleConflicts = new ArrayList(); // List of DepthStencilMultiSampleConflicts
		private readonly ArrayList vertexProcessingTypes = new ArrayList(); // List of VertexProcessingTypes
		private readonly ArrayList presentIntervals= new ArrayList(); // List of D3DPRESENT_INTERVALs

		/// <summary>
		/// Gets or sets the current <see cref="DisplayDevice"/>.
		/// </summary>
		/// <value>The current <see cref="DisplayDevice"/>.</value>
		public DisplayDevice Device { get { return displayDevice; } }

		/// <summary>
		/// Gets or sets the current <see cref="DisplayAdapter"/>.
		/// </summary>
		/// <value>The current <see cref="DisplayAdapter"/>.</value>
		public DisplayAdapter Adapter { get { return displayDevice.Adapter; } }

		/// <summary>
		/// Gets or sets the current <see cref="DeviceType"/>.
		/// </summary>
		/// <value>The current <see cref="DeviceType"/>.</value>
		public DeviceType DeviceType {	 get { return displayDevice.DeviceType; } }

		/// <summary>
		/// Gets or sets the current <see cref="Format"/> for the adapter.
		/// </summary>
		/// <value>The current <see cref="Format"/> for the adapter.</value>
		public Format AdapterFormat { get { return this.adapterFormat; } }

		/// <summary>
		/// Gets or sets the current <see cref="Format"/> for the back buffer.
		/// </summary>
		/// <value>The current <see cref="Format"/> for the back buffer.</value>
		public Format BackBufferFormat { get { return this.backBufferFormat; } }

		public Boolean IsWindowed { get { return this.isWindowed; } }

		public ArrayList DepthStencilFormats { get { return this.depthStencilFormats; } }

		public ArrayList MultiSampleTypes { get { return this.multiSampleTypes; } }

		public ArrayList MultiSampleQualityLevels { get { return this.multiSampleQualityLevels; } }

		public ArrayList DepthStencilMultiSampleConflicts { get { return this.depthStencilMultiSampleConflicts; } }

		public ArrayList VertexProcessingTypes { get { return this.vertexProcessingTypes; } }

		public ArrayList PresentIntervals { get { return this.presentIntervals; } }
	
		public DisplayDeviceMode(DisplayDevice displayDevice, Format adapterFormat,
			Format backBufferFormat, Boolean isWindowed)
		{
			this.displayDevice = displayDevice;
			this.adapterFormat = adapterFormat;
			this.backBufferFormat = backBufferFormat;
			this.isWindowed = isWindowed;

			this.RefreshDepthStencilFormats();
			this.RefreshMultiSampleTypes();
			this.RefreshDepthStencilMultiSampleConflicts();
			this.RefreshVertexProcessingTypes();
			this.RefreshPresentIntervals();
		}

		/// <summary>
		/// Adds all depth/stencil formats that are compatible with the device and app to the given deviceCombo.
		/// </summary>
		public void RefreshDepthStencilFormats()
		{
			this.depthStencilFormats.Clear();
			foreach (DepthFormat stencilFormat in Enum.GetValues(typeof(DepthFormat)))
			{
				//				if (GraphicsUtility.GetDepthBits(depthStencilFmt) < AppMinDepthBits)
				//					continue;
				//				if (GraphicsUtility.GetStencilBits(depthStencilFmt) < AppMinStencilBits)
				//					continue;
				if (Manager.CheckDeviceFormat(displayDevice.Adapter.Ordinal, displayDevice.DeviceType, 
					this.adapterFormat, Usage.DepthStencil, ResourceType.Surface, stencilFormat))
				{
					if (Manager.CheckDepthStencilMatch(displayDevice.Adapter.Ordinal, displayDevice.DeviceType,
						this.adapterFormat, this.backBufferFormat, stencilFormat))
					{
						this.depthStencilFormats.Add(stencilFormat);
					}
				}
			}
		}

		/// <summary>
		/// Adds all multisample types that are compatible with the device and app to the given deviceCombo.
		/// </summary>
		public void RefreshMultiSampleTypes()
		{
			this.multiSampleTypes.Clear();
			foreach (MultiSampleType msType in Enum.GetValues(typeof(MultiSampleType)))
			{
				int result;
				int qualityLevels = 0;
				if (Manager.CheckDeviceMultiSampleType(this.displayDevice.Adapter.Ordinal, this.displayDevice.DeviceType, 
					this.backBufferFormat, this.isWindowed, msType, out result, ref qualityLevels))
				{
					this.multiSampleTypes.Add(msType);
					this.multiSampleQualityLevels.Add(qualityLevels);
				}
			}
		}

		/// <summary>
		/// Finds any depthstencil formats that are incompatible with multisample types and builds a list of them.
		/// </summary>
		public void RefreshDepthStencilMultiSampleConflicts()
		{
			this.depthStencilMultiSampleConflicts.Clear();

			foreach (DepthFormat depthFormat in this.depthStencilFormats)
			{
				foreach (MultiSampleType msType in this.multiSampleTypes)
				{
					if (!Manager.CheckDeviceMultiSampleType(displayDevice.Adapter.Ordinal,
						displayDevice.DeviceType, (Format)depthFormat, this.isWindowed, msType))
					{
						this.depthStencilMultiSampleConflicts.Add(new DepthStencilMultiSampleConflict(depthFormat, msType));
					}
				}
			}
		
		}

		/// <summary>
		/// Adds all vertex processing types that are compatible with the device and app to the given deviceCombo.
		/// </summary>
		public void RefreshVertexProcessingTypes()
		{

			//	HACK: check support for ConfirmDeviceCallback
			this.vertexProcessingTypes.Clear();
			if (displayDevice.Caps.DeviceCaps.SupportsHardwareTransformAndLight)
			{
				if (displayDevice.Caps.DeviceCaps.SupportsPureDevice)
				{
					//if (ConfirmDeviceCallback == null ||
					//	ConfirmDeviceCallback(deviceInfo.Caps, VertexProcessingType.PureHardware, deviceCombo.BackBufferFormat))
					//{
					this.vertexProcessingTypes.Add(VertexProcessingType.PureHardware);
					//}
				}
				//				if (ConfirmDeviceCallback == null ||
				//					ConfirmDeviceCallback(deviceInfo.Caps, VertexProcessingType.Hardware, deviceCombo.BackBufferFormat))
				//				{
				this.vertexProcessingTypes.Add(VertexProcessingType.Hardware);
				//				}
				//				if (AppUsesMixedVP && (ConfirmDeviceCallback == null ||
				//					ConfirmDeviceCallback(deviceInfo.Caps, VertexProcessingType.Mixed, deviceCombo.BackBufferFormat)))
				//				{
				//VertexProcessingTypes.Add(VertexProcessingType.Mixed);
				//				}
			}
			//			if (ConfirmDeviceCallback == null ||
			//				ConfirmDeviceCallback(deviceInfo.Caps, VertexProcessingType.Software, deviceCombo.BackBufferFormat))
			//			{
			this.vertexProcessingTypes.Add(VertexProcessingType.Software);
			//			}
		}

		/// <summary>
		/// Adds all present intervals that are compatible with the device and app to the given deviceCombo.
		/// </summary>
		public void RefreshPresentIntervals()
		{
			foreach (PresentInterval interval in Enum.GetValues(typeof(PresentInterval)))
			{
				if (this.isWindowed)
				{
					if (interval == PresentInterval.Two ||
						interval == PresentInterval.Three ||
						interval == PresentInterval.Four)
					{
						// These intervals are not supported in windowed mode
						continue;
					}
				}
				// Note that PresentInterval.Default is zero, so you
				// can't do a caps check for it -- it is always available.
				if (interval == PresentInterval.Default ||
					(this.displayDevice.Caps.PresentationIntervals & interval) != 0)
				{
					this.presentIntervals.Add(interval);
				}
			}
		}
	}
}