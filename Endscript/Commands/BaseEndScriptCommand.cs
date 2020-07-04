using Endscript.Core;
using Endscript.Enums;



namespace Endscript.Commands
{
	public abstract class BaseEndScriptCommand
	{
		public abstract eCommandType Type { get; }
		public static bool ThrowError { get; set; }
		public string Line { get; set; }

		public abstract void Prepare(string[] splits);
		public abstract void Execute(CollectionMap map);
	}
}
