using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Nikki.Support.Shared.Class;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'add_incareer [filename] [manager] [gcareer] [root] [collection]'.
	/// </summary>
	public class AddInCareerCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _gcareer;
		private string _root;
		private string _collection;

		public override eCommandType Type => eCommandType.add_incareer;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			this._filename = splits[1].ToUpperInvariant();
			this._manager = splits[2];
			this._gcareer = splits[3];
			this._root = splits[4];
			this._collection = splits[5];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._gcareer);

			if (collection is GCareer gcareer)
			{

				gcareer.AddCollection(this._collection, this._root);

			}
			else
			{

				throw new Exception($"Object {this._gcareer} is not a GCareer");

			}
		}
	}
}
