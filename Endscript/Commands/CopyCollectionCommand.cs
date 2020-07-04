using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	public sealed class CopyCollectionCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _from;
		private string _to;

		public override eCommandType Type => eCommandType.copy_collection;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 5) throw new InvalidArgsNumberException(splits.Length, 5);

			this._filename = splits[1];
			this._manager = splits[2];
			this._from = splits[3];
			this._to = splits[4];
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

			manager.Clone(this._to, this._from);
			map.AddCollection(this._filename, this._manager, this._to, manager[^1]);
		}
	}
}
