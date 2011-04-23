using System;
using Microsoft.DirectX.Direct3D;

namespace MDX9Lib.Direct3D
{
	/// <summary>
	/// Information about a depth/stencil buffer format that is incompatible with a multisample type.
	/// </summary>
	public class DepthStencilMultiSampleConflict
	{
		private readonly DepthFormat depthStencilFormat;
		private readonly MultiSampleType multiSampleType;

		/// <summary>
		/// Creates a new <see cref="DepthStencilMultiSampleConflict"/> instance with defined field values.
		/// </summary>
		/// <param name="depthStencilFormat">The <see cref="DepthFormat"/> involved in the conflict.</param>
		/// <param name="multiSampleType">The <see cref="MultiSampleType"/> involved in the conflict.</param>
		public DepthStencilMultiSampleConflict(DepthFormat depthStencilFormat, MultiSampleType multiSampleType)
		{
			this.depthStencilFormat = depthStencilFormat;
			this.multiSampleType = multiSampleType;
		}

		/// <summary>
		/// Gets the <see cref="DepthFormat"/> involved in the conflict.
		/// </summary>
		/// <value>The <see cref="DepthFormat"/> involved in the conflict.</value>
		public DepthFormat DepthStencilFormat { get { return this.depthStencilFormat; }  }
		
		/// <summary>
		/// Gets the <see cref="MultiSampleType"/> involved in the conflict.
		/// </summary>
		/// <value>The <see cref="MultiSampleType"/> involved in the conflict.</value>
		public MultiSampleType MultiSampleType { get { return this.multiSampleType; }  }
	}
}