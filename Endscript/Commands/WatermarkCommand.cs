using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'watermark [description]'.
	/// </summary>
	public class WatermarkCommand : BaseCommand, ISingleParsable
	{
		private string _watermark;

		public override eCommandType Type => eCommandType.watermark;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2) throw new InvalidArgsNumberException(splits.Length, 2);

			this._watermark = splits[1];
		}

		public override void Execute(CollectionMap map)
		{
			SynchronizedDatabase.Watermark = this._watermark;
		}

		public void SingleExecution(BaseProfile profile)
		{
			SynchronizedDatabase.Watermark = this._watermark;
		}
	}
}
