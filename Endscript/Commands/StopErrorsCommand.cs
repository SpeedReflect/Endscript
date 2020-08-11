using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'stop_errors [true/false]'.
	/// </summary>
	public class StopErrorsCommand : BaseCommand
	{
		private string _enable = String.Empty;
		public override eCommandType Type => eCommandType.stop_errors;
		public bool Enable { get; private set; }

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2) throw new InvalidArgsNumberException(splits.Length, 2);

			this._enable = splits[1];
		}

		public override void Execute(CollectionMap map)
		{
			if (String.Compare(this._enable, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{

				this.Enable = true;

			}
			else if (String.Compare(this._enable, "false", StringComparison.OrdinalIgnoreCase) == 0)
			{

				this.Enable = false;

			}
			else
			{

				throw new Exception("Invalid parameter passed to 'stop_errors' command");

			}
		}
	}
}
