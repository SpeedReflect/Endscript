using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'update_collection [filename] [manager] [collection] ([node] [subpart]) [property] [value]'.
	/// </summary>
	public class UpdateCollectionCommand : BaseCommand, ISingleParsable
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
			var collection = map.GetCollection(this._filename, this._manager, this._collection);

			if (this._expand is null)
			{

				collection.SetValue(this._property, this._value);

				if (this._property == "CollectionName")
				{

					map.RemoveCollection(this._filename, this._manager, this._collection);
					map.AddCollection(this._filename, this._manager, this._value, collection);

				}

			}
			else
			{

				var part = collection.GetSubPart(this._subpart, this._expand);
				if (part == null) throw new Exception($"SubPart named {this._subpart} in node {this._expand} does not exist");
				part.SetValue(this._property, this._value);

			}
		}

		public void SingleExecution(BaseProfile profile)
		{
			var collection = this.GetManualCollection(this._filename, this._manager, this._collection, profile);

			if (this._expand is null)
			{

				collection.SetValue(this._property, this._value);

			}
			else
			{

				var part = collection.GetSubPart(this._subpart, this._expand);
				part.SetValue(this._property, this._value);

			}
		}
	}
}
