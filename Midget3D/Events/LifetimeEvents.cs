using System;
using System.Collections;

namespace Midget.Events.Object.Lifetime
{
	
	public delegate void CreateObjectEventHandler(object sender, SingleObjectEventArgs e);
	public delegate void CreateObjectRequestEventHandler (object sender, CreateObjectRequestEventArgs e);
	
	
	public delegate void DeleteObjectEventHandler(object sender, MultiObjectEventArgs e);
	public delegate void DeleteObjectRequestEventHandler(object sender, MultiObjectEventArgs e);


	public delegate void DuplicateObjectEventHandler (object sender, MultiObjectEventArgs e);
	public delegate void DuplicateObjectRequestEventHandler (object sender, DuplicateObjectRequestEventArgs e);


	public class CreateObjectRequestEventArgs : ObjectEventArgs
	{
		private readonly ObjectFactory.ObjectTypes type;
		private readonly string parameters;

		public CreateObjectRequestEventArgs( ObjectFactory.ObjectTypes objectType, string options)
		{
			type = objectType;
			parameters = options;
		}

		public ObjectFactory.ObjectTypes Type
		{
			get { return type; }
		}

		public string Parameters
		{
			get { return parameters;}
		}
	}


	public class DuplicateObjectRequestEventArgs : MultiObjectEventArgs
	{	
		private readonly int number;

		public DuplicateObjectRequestEventArgs(ArrayList initObjects, int initNumber) : base(initObjects) 
		{
			number = initNumber;
		}

		public int Number
		{
			get { return number; }
		}
	}
}
