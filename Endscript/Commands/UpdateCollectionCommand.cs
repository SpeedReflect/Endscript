using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'update_collection [filename] [manager] [collection] ([node] [subpart]) [property] [value]'.
	/// </summary>
	public class UpdateCollectionCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _collection;
		private string _expand;
		private string _subpart;
		private string _property;
		private string _value;

		public override eCommandType Type => eCommandType.update_collection;

		public override void Prepare(string[] splits)
		{
			switch (splits.Length)
			{
				case 6:
					this._filename = splits[1];
					this._manager = splits[2];
					this._collection = splits[3];
					this._property = splits[4];
					this._value = splits[5];
					return;

				case 8:
					this._filename = splits[1];
					this._manager = splits[2];
					this._collection = splits[3];
					this._expand = splits[4];
					this._subpart = splits[5];
					this._property = splits[6];
					this._value = splits[7];
					return;

				default: // invalid
					throw new InvalidArgsNumberException(splits.Length, 6, 8);

			}
		}

		public override void Execute(CollectionMap map)
		{
			if (this._expand is null) this.UpdateCollection(map);
			else this.UpdateSubPart(map); // quick way to check
		}

		private void UpdateCollection(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._collection);
			collection.SetValue(this._property, this._value);
		}

		private void UpdateSubPart(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._collection);
			var part = collection.GetSubPart(this._subpart, this._expand);
			part.SetValue(this._property, this._value);
		}
	}
}
