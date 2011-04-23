using System;
using System.Collections;

namespace Midget.Command
{
	/// <summary>
	/// Command Manager class to keep track of the commands
	/// </summary>
	public class CommandContainer
	{
		private  ArrayList commandHistory;
		private  int position;
		
		public CommandContainer() 
		{
			commandHistory = new ArrayList();
			position = -1;
		}

		public  void Add(ICommand command)
		{
			try
			{
				command.Execute();
			}
			catch (System.Exception e)
			{
				System.Windows.Forms.MessageBox.Show(e.Message);
			}

			
			// if not at end of history list, cut off end
			if (!AtEnd && !Empty)
			{
				if(position == -1)
					commandHistory.Clear();
				else	
					commandHistory.RemoveRange(position + 1,commandHistory.Count - position - 1);
			}

			commandHistory.Add(command);
			position++;

		}

		public  void Undo()
		{
			if(UndoAvailable)
			{
				// undo command
				((ICommand)commandHistory[position]).UnExecute();
				
				// move backward in the list
				--position;
			}
		}

		public  void Redo()
		{
			if(RedoAvailable)
			{
				// move forward in the list
				++position;

				// redo command
				((ICommand)commandHistory[position]).Execute();
			}
		}

		public  bool UndoAvailable
		{
			get { return !this.BeforeStart; }
		}

		public  bool RedoAvailable
		{
			get { return ( !this.Empty && !this.AtEnd); }
		}
		
		private bool AtEnd
		{
			get { return ((position + 1) == commandHistory.Count); }
		}
		
		private bool Empty
		{
			get { return (commandHistory.Count == 0); }
		}

		private bool BeforeStart
		{
			get { return (position < 0); }
		}
	}
}
