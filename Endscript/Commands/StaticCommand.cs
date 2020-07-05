using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'static [filename] [manager] [property] [value]'.
	/// </summary>
	public class StaticCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _property;
		private string _value;

		public override eCommandType Type => eCommandType.@static;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 4) throw new InvalidArgsNumberException(splits.Length, 4);

			this._filename = splits[1];
			this._manager = splits[2];
			this._property = splits[3];
			this._value = splits[4];
		}

		public override void Execute(CollectionMap map)
		{
			var sdb = map.Profile[this._filename];

			if (sdb is null)
			{

				throw new LookupFailException($"File {this._filename} was never loaded");

			}

			var manager = sdb.Database.GetManager(this._manager);

			if (manager is null)
			{

				throw new LookupFailException($"Manager named {this._manager} does not exist");

			}

			manager.Static(this._property, this._value);
		}
	}
}
