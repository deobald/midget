using System;

namespace Midget
{
	/// <summary>
	/// Class that generates new render to file threads
	/// </summary>
	public class FileRenderThreadFactory
	{

		DeviceManager dm;
		SceneManager sm;

		public FileRenderThreadFactory(DeviceManager initDM, SceneManager initSM)
		{
			this.dm = initDM;
			this.sm = initSM;
		}
		
		public FileRenderThread NewFileRender(FileRenderSettings renderSettings)
		{
			return new FileRenderThread(sm.Scene,dm,renderSettings);
		}
	}
}