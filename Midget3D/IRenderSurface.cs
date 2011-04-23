using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Midget
{
	/// <summary>
	/// Possible drawing modes
	/// </summary>
	public enum DrawMode {wireFrame, solid, point};

	/// <summary>
	/// Interface that must implemented by any control that is going to rendered
	/// </summary>
	public interface IRenderSurface
	{
		/// <summary>
		/// Reference to the render surface's swapcahin
		/// </summary>
		SwapChain SwapChain { get; set; }
	
		/// <summary>
		/// The camera that the render surface is currently using
		/// </summary>
		Camera Camera { get; set; }

		/// <summary>
		/// The drawmode the render surface currently is in
		/// </summary>
		DrawMode DrawMode { get; set; }

		/// <summary>
		/// Whether or no the render surface is too display the camera name
		/// </summary>
		bool NameVisible { get; set; }

		/// <summary>
		/// The background colour of the render surface
		/// </summary>
		Color BackColor {get; set;}

	}
}