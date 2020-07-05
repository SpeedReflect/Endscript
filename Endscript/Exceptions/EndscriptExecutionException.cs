using System;



namespace Endscript.Exceptions
{
	public class EndscriptExecutionException : Exception
	{
		public EndscriptExecutionException() : base() { }

		public EndscriptExecutionException(string message) : base(message) { }
	}
}
