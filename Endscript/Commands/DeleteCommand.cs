using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'delete [filename]'.
	/// </summary>
	public class DeleteCommand : BaseCommand
	{
		private string _filename;

		public override eCommandType Type => eCommandType.delete;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2) throw new InvalidArgsNumberException(splits.Length, 2);

			this._filename = splits[1];
		}

		public override void Execute(CollectionMap map)
		{
			map.Profile.Delete(this._filename);
			map.LoadMapFromProfile(true);
		}
	}
}
