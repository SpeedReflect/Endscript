using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	public sealed class EndCommand : BaseCommand
	{
		public override eCommandType Type => eCommandType.end;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 1) throw new InvalidArgsNumberException(splits.Length, 1);
		}

		public override void Execute(CollectionMap map)
		{
		}
	}
}
