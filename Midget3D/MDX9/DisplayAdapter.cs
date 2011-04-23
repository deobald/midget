using System;
using System.Collections;
using Microsoft.DirectX.Direct3D;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// Info about a display adapter.
	/// </summary>
	public class DisplayAdapter
	{
		private readonly Int32 ordinal;
		private readonly AdapterDetails details;
		private readonly ArrayList displayModes = new ArrayList(); // List of D3DDISPLAYMODEs
		private readonly ArrayList displayDevices = new ArrayList(); // List of D3DDeviceInfos

		public Int32 Ordinal { get { return this.ordinal; } }

		public AdapterDetails Details { get { return this.details; } }

		public ArrayList DisplayModes { get { return this.displayModes; } }

		public ArrayList DisplayDevices { get { return this.displayDevices; } }

		public override string ToString() { return this.details.Description; }

		public DisplayAdapter(Microsoft.DirectX.Direct3D.AdapterInformation adapterInfo)
		{
			this.ordinal = adapterInfo.Adapter;
			this.details = adapterInfo.Information;

			// Get list of all display modes on this adapter.  
			// Also build a temporary list of all display adapter formats.
			foreach (DisplayMode displayMode in adapterInfo.SupportedDisplayModes)
				this.displayModes.Add(displayMode);

			DisplayModeComparer dmc = new DisplayModeComparer();
			this.displayModes.Sort(dmc);

			RefreshDisplayDevices();
		}

		public void RefreshDisplayDevices()
		{
			this.displayDevices.Clear();

			foreach(DeviceType devType in Enum.GetValues(typeof(DeviceType)))
			{
				DisplayDevice displayDevice = new DisplayDevice(this, devType);

				//	add the display device if we have any valid device modes
				if (displayDevice.DeviceModes.Count >= 0)
					this.displayDevices.Add(displayDevice);
			}		
		}
	}
}