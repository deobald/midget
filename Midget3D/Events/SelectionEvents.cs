using System;

namespace Midget.Events.Object.Selection
{
	public delegate void SelectObjectEventHandler(object sender, SingleObjectEventArgs e);
	public delegate void SelectObjectRequestEventHandler(object sender, SingleObjectEventArgs e);

	public delegate void SelectAdditionalObjectEventHandler(object sender, SingleObjectEventArgs e);
	public delegate void SelectAdditionalObjectRequestEventHandler(object sender, SingleObjectEventArgs e);

	public delegate void DeselectObjectEventHandler(object sender, SingleObjectEventArgs e);
	public delegate void DeselectObjectEventRequestHandler(object sender, SingleObjectEventArgs e);

	public delegate void DeselectAllObjectsEventHandler(object sender, MultiObjectEventArgs e);
	public delegate void DeselectAllObjectsEventRequestHandler(object sender, MultiObjectEventArgs e);
}
