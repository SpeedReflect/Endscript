using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Support.Shared.Class;



namespace Endscript.Commands.Logical
{
	public class InCareerExistsLogicalCheck : ILogical
	{
		private string _filename;
		private string _manager;
		private string _gcareer;
		private string _root;
		private string _collection;

		public eLogicType LogicType => eLogicType.incareer_exists;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'incareer_exists'

			if (splits.Length != 7) throw new InvalidArgsNumberException(splits.Length, 7);

			this._filename = splits[2].ToUpperInvariant();
			this._manager = splits[3];
			this._gcareer = splits[4];
			this._root = splits[5];
			this._collection = splits[6];
		}

		public bool Evaluate(CollectionMap map)
		{
			try
			{

				var collection = map.GetCollection(this._filename, this._manager, this._gcareer);
				var gcareer = collection as GCareer;

				var node = gcareer.GetCollection(this._collection, this._root);
				return !(node is null);

			}
			catch { return false; }
		}
	}
}
