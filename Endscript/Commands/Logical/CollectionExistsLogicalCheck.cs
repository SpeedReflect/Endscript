using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands.Logical
{
	public class CollectionExistsLogicalCheck : ILogical
	{
		private string _filename;
		private string _manager;
		private string _collection;

		public eLogicType LogicType => eLogicType.collection_exists;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'collection_exists'

			if (splits.Length != 5) throw new InvalidArgsNumberException(splits.Length, 5);

			this._filename = splits[2].ToUpperInvariant();
			this._manager = splits[3];
			this._collection = splits[4];
		}

		public bool Evaluate(CollectionMap map)
		{
			try { map.GetCollection(this._filename, this._manager, this._collection); return true; }
			catch { return false; }

		}
	}
}
