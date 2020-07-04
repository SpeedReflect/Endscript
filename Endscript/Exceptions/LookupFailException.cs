using System;



namespace Endscript.Exceptions
{
	public class LookupFailException : Exception
	{
		public LookupFailException() : base() { }

		public LookupFailException(string message) : base(message) { }
	}
}
