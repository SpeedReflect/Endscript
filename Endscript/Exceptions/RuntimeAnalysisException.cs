using System;



namespace Endscript.Exceptions
{
	public class RuntimeAnalysisException : Exception
	{
		public RuntimeAnalysisException() : base() { }

		public RuntimeAnalysisException(string error)
			: base($"Runtime endscript analysis failure: {error}") { }

		public RuntimeAnalysisException(string error, string filename, int index)
			: base($"Runtime endscript analysis failure: {error}; File: {filename}; Line {index}") { }
	}
}
