using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Support.Shared.Class;



namespace Endscript.Commands.Logical
{
	public class InCareerValueEqualsLogicalCheck : ILogical
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

		public eLogicType LogicType => eLogicType.incareer_value_equals;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'incareer_value_equals'

			switch (splits.Length)
			{
				case 9:
					this._filename = splits[2].ToUpperInvariant();
					this._manager = splits[3];
					this._gcareer = splits[4];
					this._root = splits[5];
					this._collection = splits[6];
					this._property = splits[7];
					this._value = splits[8];
					return;

				case 11:
					this._filename = splits[2].ToUpperInvariant();
					this._manager = splits[3];
					this._gcareer = splits[4];
					this._root = splits[5];
					this._collection = splits[6];
					this._expand = splits[7];
					this._subpart = splits[8];
					this._property = splits[9];
					this._value = splits[10];
					return;

				default: // invalid
					throw new InvalidArgsNumberException(splits.Length, 9, 11);

			}
		}

		public bool Evaluate(CollectionMap map)
		{
			try
			{

				var gcareer = map.GetCollection(this._filename, this._manager, this._gcareer) as GCareer;
				var collection = gcareer.GetCollection(this._collection, this._root);

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
