using System;



namespace Endscript.Exceptions
{
	public class DatabaseExistenceException : Exception
	{
		public DatabaseExistenceException() : base() { }

		public DatabaseExistenceException(string filename)
			: base($"File {filename} is already loaded in the database") { }
	}
}
