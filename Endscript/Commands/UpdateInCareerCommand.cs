using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Support.Shared.Class;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'update_incareer [filename] [manager] [gcareer] [root] [collection] ([node] [subpart]) [property] [value]'.
	/// </summary>
	public class UpdateInCareerCommand : BaseCommand, ISingleParsable
	{
		private string _filename;
		private string _manager;
		private string _gcareer;
		private string _root;
		private string _collection;
		private string _expand;
		private string _subpart;
		private string _property;
		private string _value;

		public override eCommandType Type => eCommandType.update_incareer;

		public override void Prepare(string[] splits)
		{
			switch (splits.Length)
			{
				case 8:
					this._filename = splits[1].ToUpperInvariant();
					this._manager = splits[2];
					this._gcareer = splits[3];
					this._root = splits[4];
					this._collection = splits[5];
					this._property = splits[6];
					this._value = splits[7];
					return;

				case 10:
					this._filename = splits[1].ToUpperInvariant();
					this._manager = splits[2];
					this._gcareer = splits[3];
					this._root = splits[4];
					this._collection = splits[5];
					this._expand = splits[6];
					this._subpart = splits[7];
					this._property = splits[8];
					this._value = splits[9];
					return;

				default: // invalid
					throw new InvalidArgsNumberException(splits.Length, 8, 10);

			}
		}

		public override void Execute(CollectionMap map)
		{
			var temp = map.GetCollection(this._filename, this._manager, this._gcareer);

			if (temp is GCareer gcareer)
			{

				var collection = gcareer.GetCollection(this._collection, this._root);

				if (collection is null)
				{

					throw new LookupFailException($"Collection named {this._collection} does not exist");

				}

				if (this._expand is null)
				{

					collection.SetValue(this._property, this._value);

				}
				else
				{

					var part = collection.GetSubPart(this._subpart, this._expand);
					if (part == null) throw new Exception($"SubPart named {this._subpart} in node {this._expand} does not exist");
					part.SetValue(this._property, this._value);

				}

			}
			else
			{

				throw new Exception($"Object {this._gcareer} is not a GCareer");

			}
		}

		public void SingleExecution(BaseProfile profile)
		{
			var temp = this.GetManualCollection(this._filename, this._manager, this._gcareer, profile);

			if (temp is GCareer gcareer)
			{

				var collection = gcareer.GetCollection(this._collection, this._root);

				if (collection is null)
				{

					throw new LookupFailException($"Collection named {this._collection} does not exist");

				}

				if (this._expand is null)
				{

					collection.SetValue(this._property, this._value);

				}
				else
				{

					var part = collection.GetSubPart(this._subpart, this._expand);
					if (part == null) throw new Exception($"SubPart named {this._subpart} in node {this._expand} does not exist");
					part.SetValue(this._property, this._value);

				}

			}
			else
			{

				throw new Exception($"Object {this._gcareer} is not a GCareer");

			}
		}
	}
}
