using System;

namespace Midget.Command
{
	/// <summary>
	/// The Command Interface
	/// </summary>
	public interface ICommand
	{
		void Execute();
		void UnExecute();
	}
}
