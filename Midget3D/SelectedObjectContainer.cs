using System;
using System.Collections;

using Midget;

namespace Midget
{
	/// <summary>
	/// Summary description for SelectedObjectContainer.
	/// </summary>
	public class SelectedObjectContainer
	{
		private ArrayList selectedObjects;

		public SelectedObjectContainer()
		{
			selectedObjects = new ArrayList();

		}

		public ArrayList SelectedObjects
		{
			get { return selectedObjects; }
		}

	}
}
