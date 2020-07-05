using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'add_collection [filename] [manager] [collection]'.
	/// </summary>
	public class AddCollectionCommand : BaseCommand, ISingleParsable
	{
		private string _filename;
		private string _manager;
		private string _collection;

		public override eCommandType Type => eCommandType.add_collection;

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

			manager.Add(this._collection);
			map.AddCollection(this._filename, this._manager, this._collection, manager[^1]);
		}

		public void SingleExecution(BaseProfile profile)
		{
			var sdb = profile[this._filename];

			if (sdb is null)
			{

				throw new LookupFailException($"File {this._filename} was never loaded");

			}

			var manager = sdb.Database.GetManager(this._manager);

			if (manager is null)
			{

				throw new LookupFailException($"Manager named {this._manager} does not exist");

			}

			manager.Add(this._collection);
		}
	}
}
