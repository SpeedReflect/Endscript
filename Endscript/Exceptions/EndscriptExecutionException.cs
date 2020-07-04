using System;



namespace Endscript.Exceptions
{
	public sealed class EndscriptExecutionException : Exception
	{
		public EndscriptExecutionException() : base() { }

		public EndscriptExecutionException(string message) : base(message) { }
	}
}
