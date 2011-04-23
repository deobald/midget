using System;
using System.Collections;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// Information about a D3D device, including a list of DeviceCombos (see below) that work with the device
	/// </summary>
	public class DisplayDevice
	{
		private readonly DisplayAdapter adapter;
		private readonly DeviceType deviceType;
		private readonly Caps caps;
		private readonly ArrayList deviceModes = new ArrayList();

		/// <summary>
		/// Gets the <see cref="DisplayAdapter"/> being used.
		/// </summary>
		/// <value>The <see cref="DisplayAdapter"/> being used.</value>
		public DisplayAdapter Adapter { get { return this.adapter; } }

		/// <summary>
		/// Gets the <see cref="DeviceType"/> being used.
		/// </summary>
		/// <value>The <see cref="DeviceType"/> being used.</value>
		public DeviceType DeviceType { get { return this.deviceType; } }

		/// <summary>
		/// Returns the <see cref="Caps"/> for the Direct3D device.
		/// </summary>
		public Caps Caps { get { return this.caps; } }

		/// <summary>
		/// Contains a list of device modes supported by the graphics hardware.
		/// </summary>
		public ArrayList DeviceModes { get { return this.deviceModes; } }

		/// <summary>
		/// Calls the <see cref="DeviceType"/>'s ToString method.
		/// </summary>
		public override string ToString() { return this.deviceType.ToString(); }

		/// <summary>
		/// Creates a new <see cref="DisplayDevice"/> using a provided <see cref="DisplayAdapter"/> and <see cref="DeviceType"/>.
		/// </summary>
		/// <param name="displayAdapter">The display adapter you wish to use.</param>
		/// <param name="deviceType">The type of device you wish to use.</param>
		public DisplayDevice(DisplayAdapter displayAdapter, DeviceType deviceType)
		{
			this.adapter = displayAdapter;
			this.deviceType = deviceType;

			try
			{
				this.caps = Manager.GetDeviceCaps(adapter.Ordinal, deviceType);
			}
			catch (DirectXException)
			{
				//	do nothing, this is expected
			}

			this.RefreshDeviceModes();
		}

		/// <summary>
		/// Supported formats.  Loaded once and reused from then on.
		/// </summary>
		private static readonly Format[] backBufferFormats = new Format[] 
			{
				Format.R8G8B8, Format.A8R8G8B8, Format.X8R8G8B8, 
				Format.R5G6B5, Format.A1R5G5B5, Format.X1R5G5B5,
				Format.R3G3B2, Format.A8R3G3B2,
				Format.X4R4G4B4, Format.A4R4G4B4,
				Format.A2B10G10R10 
			};

		/// <summary>
		/// True and False.  Loaded once and reused from then on. 
		/// </summary>
		private static readonly bool[] isWindowedArray = new bool[] { false, true };

		/// <summary>
		/// Regenerates the internal deviceModes list.
		/// </summary>
		private void RefreshDeviceModes()
		{
			this.deviceModes.Clear();

			// See which adapter formats are supported by this device
			foreach (DisplayMode displayMode in this.adapter.DisplayModes)
			{
				foreach (Format backBufferFormat in DisplayDevice.backBufferFormats)
				{
					//	HACK: replace this line
					//					if (GraphicsUtility.GetAlphaChannelBits(backBufferFormat) < AppMinAlphaChannelBits)
					//						continue;

					foreach (bool isWindowed in DisplayDevice.isWindowedArray)
					{
						if (!Manager.CheckDeviceType(this.adapter.Ordinal, this.deviceType, 
							displayMode.Format, backBufferFormat, isWindowed))
							continue;

						DisplayDeviceMode newDeviceMode = new DisplayDeviceMode(this,
							displayMode.Format, backBufferFormat, isWindowed);
						
						//	add the device mode if it is valid for actual use
						if ((newDeviceMode.MultiSampleTypes.Count > 0) &&
							(newDeviceMode.VertexProcessingTypes.Count > 0) &&
							(newDeviceMode.PresentIntervals.Count > 0))
							this.deviceModes.Add(newDeviceMode);
					}
				}
			}
		}
	}
}