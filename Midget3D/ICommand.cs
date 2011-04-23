using System;

namespace MidgetCommand
{
	/// <summary>
	/// The Command Interface
	/// </summary>
	public interface ICommand
	{
		void Execute();
		void UnExectute();
	}
}
