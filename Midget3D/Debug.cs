using System;
using System.IO;

using System.Windows.Forms;
using Midget;

namespace Midget
{
	/// <summary>
	/// Just a static method to aid in debugging, so you don't have to use MessageBoxes
	/// to test anything. Just use Debug.Write("debug message here")
	/// </summary>
	public class Debug
	{
		//string filename = "debug.log";
		public Debug()
		{
		}

		public static void Write (string message)
		{
			StreamWriter writer = File.AppendText(Application.StartupPath + "\\debug.log");
			//writer.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToShortDateString());
			//writer.WriteLine("  :");
			//writer.WriteLine("  :{0}", message);
			writer.WriteLine("|\t" + message);
			writer.WriteLine("|----------------------------------------");
			writer.Flush();
			writer.Close();

		}

		public static void Clear()
		{
			StreamWriter writer = File.CreateText(Application.StartupPath + "\\debug.log");
			writer.WriteLine("Midget Debug Log\n===================================");
			writer.Close();
		}
		
	}
			
	
}
