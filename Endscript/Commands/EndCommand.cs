using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'end'. Used to specify end of <see cref="ISelectable"/> statements.
	/// </summary>
	public class EndCommand : BaseCommand
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
