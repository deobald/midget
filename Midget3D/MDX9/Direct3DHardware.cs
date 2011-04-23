/*
 *	Direct3DHardwareInfo.cs
 *	gathers information about Direct3D hardware (adapters, supported modes, etc)
 * 
 *	based on D3DEnumeration.cs from the DXSDK
 */

using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// Gathers information about <see cref="DisplayAdapter"/>s.
	/// </summary>
	public class Direct3DHardware
	{
		private readonly ArrayList displayAdapters = new ArrayList();

		public ArrayList DisplayAdapters { get { return this.displayAdapters; } }

		public Direct3DHardware()
		{
		}

		public void Load()
		{
			this.displayAdapters.Clear();
			foreach (AdapterInformation ai in Manager.Adapters)
			{
				this.displayAdapters.Add(new DisplayAdapter(ai));
			}
		}
	}
}