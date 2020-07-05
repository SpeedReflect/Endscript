using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// An optional command with unknown <see cref="eCommandType"/> type.
	/// </summary>
	public class OptionalCommand : BaseCommand
	{
		private string _option;

		public override eCommandType Type => eCommandType.invalid;
		public string Option => this._option;
		public override void Prepare(string[] splits)
		{
			this._option = splits[0];
		}
		public override void Execute(CollectionMap map)
		{
			throw new EndscriptExecutionException($"Command {this._option} cannot be recognized");
		}
	}
}
