using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command with any invalid <see cref="eCommandType"/> passed.
	/// </summary>
	public sealed class InvalidEndScriptCommand : BaseEndScriptCommand
	{
		private string _command;

		public override void Prepare(string[] splits) { this._command = splits[0]; }
		public override void Execute(CollectionMap map)
		{
			if (ThrowError)
			{

				throw new EndscriptExecutionException($"Command {this._command} cannot be recognized");

			}
		}
	}
}
