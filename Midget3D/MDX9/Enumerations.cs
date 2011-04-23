namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// All the different types of Direct3D vertex processing
	/// </summary>
	public enum VertexProcessingType
	{
		/// <summary>
		/// All vertex processing is done by the CPU.
		/// </summary>
		Software,
		/// <summary>
		/// Vertex processing is done with a mix of CPU and hardware acceleration.
		/// </summary>
		Mixed,
		/// <summary>
		/// Most vertex processing is done using hardware acceleration.
		/// </summary>
		Hardware,
		/// <summary>
		/// All vertex processing is done using hardware acceleration.
		/// </summary>
		PureHardware
	}

	/// <summary>
	/// Defines different behaviors for Direct3D event loop processing.
	/// </summary>
	public enum ProcessingBehavior
	{
		/// <summary>
		/// Throttles processing to a target framerate of around 100 FPS. This is the default.
		/// <remarks>Direct3DHost will throttle down CPU utilization by its event loop depending on the application's ability to meet the target frame rate. 
		/// If application load picks up, the throttling steps back to add more CPU horsepower as needed. 
		/// There is little if any performance difference evident under high load, but under lighter loads, ConserveCpu utilizes much less CPU for essentially the same visible results.</remarks>
		/// </summary>
		ConserveCpu,
		/// <summary>
		/// Processing is unrestricted and will use up all available CPU resources.
		/// </summary>
		/// <remarks>This mode should be unnecessary in the majority of situations, as ConserveCpu provides virtually the same performance under high stress situatons.
		/// Also keep in mind that FPS is limited by the monitor refresh rate.  Windows and DirectX default to 60 and most people don't increase it.  Most monitors (and all LCDs) can't go above 100Hz anyway.</remarks>
		FullThrottle
	}
}