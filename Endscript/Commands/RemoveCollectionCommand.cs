using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'remove_collection [filename] [manager] [collection]'.
	/// </summary>
	public sealed class RemoveCollectionCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _collection;

		public override eCommandType Type => eCommandType.remove_collection;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 4) throw new InvalidArgsNumberException(splits.Length, 4);

			this._filename = splits[1];
			this._manager = splits[2];
			this._collection = splits[3];
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

			manager.Remove(this._collection);
			map.RemoveCollection(this._filename, this._manager, this._collection);
		}
	}
}
