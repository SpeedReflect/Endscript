using System;
using Endscript.Core;
using Endscript.Enums;



namespace Endscript.Commands
{
	public abstract class BaseCommand
	{
		public abstract eCommandType Type { get; }
		public string Line { get; set; }

		public abstract void Prepare(string[] splits);
		public abstract void Execute(CollectionMap map);
	}
}
