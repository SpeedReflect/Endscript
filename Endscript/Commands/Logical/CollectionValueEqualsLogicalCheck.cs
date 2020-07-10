using System;

using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands.Logical
{
	public class CollectionValueEqualsLogicalCheck : ILogical
	{
		private string _filename;
		private string _manager;
		private string _collection;
		private string _expand;
		private string _subpart;
		private string _property;
		private string _value;

		public eLogicType LogicType => eLogicType.collection_value_equals;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'collection_value_equals'

			switch (splits.Length)
			{
				case 7:
					this._filename = splits[2].ToUpperInvariant();
					this._manager = splits[3];
					this._collection = splits[4];
					this._property = splits[5];
					this._value = splits[6];
					return;

				case 9:
					this._filename = splits[2].ToUpperInvariant();
					this._manager = splits[3];
					this._collection = splits[4];
					this._expand = splits[5];
					this._subpart = splits[6];
					this._property = splits[7];
					this._value = splits[8];
					return;

				default: // invalid
					throw new InvalidArgsNumberException(splits.Length, 7, 9);

			}
		}

		public bool Evaluate(CollectionMap map)
		{
			try
			{

				var collection = map.GetCollection(this._filename, this._manager, this._collection);

				if (this._expand is null)
				{

					var check = collection.GetValue(this._property);
					return String.Compare(check, this._value, StringComparison.Ordinal) == 0;

				}
				else
				{

					var part = collection.GetSubPart(this._subpart, this._expand);
					var check = part.GetValue(this._property);
					return String.Compare(check, this._value, StringComparison.Ordinal) == 0;

				}
			
			}
			catch { return false; }
		}
	}
}
