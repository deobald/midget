using System;
using Microsoft.DirectX.Direct3D;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// Used to sort <see cref="DisplayMode"/> objects.
	/// </summary>
	public class DisplayModeComparer : System.Collections.IComparer
	{
		/// <summary>
		/// Compare two <see cref="DisplayMode"/> objects.
		/// </summary>
		/// <param name="x">The left-side object to compare.</param>
		/// <param name="y">The right-side object to compare.</param>
		/// <exception cref="InvalidCastException">Either X or Y is not a <see cref="DisplayMode"/>.</exception>
		public Int32 Compare(Object x, Object y)
		{
			DisplayMode dx = (DisplayMode)x;
			DisplayMode dy = (DisplayMode)y;

			if (dx.Width > dy.Width)
				return 1;
			if (dx.Width < dy.Width)
				return -1;
			if (dx.Height > dy.Height)
				return 1;
			if (dx.Height < dy.Height)
				return -1;
			if (dx.Format > dy.Format)
				return 1;
			if (dx.Format < dy.Format)
				return -1;
			if (dx.RefreshRate > dy.RefreshRate)
				return 1;
			if (dx.RefreshRate < dy.RefreshRate)
				return -1;
			return 0;
		}
	}
}