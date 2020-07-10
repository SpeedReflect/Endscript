using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Utils;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands.Logical
{
	public class StringValueEqualsLogicalCheck : ILogical
	{
		private string _filename;
		private string _manager;
		private string _str;
		private string _record;
		private string _property;
		private string _value;

		public eLogicType LogicType => eLogicType.string_value_equals;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'string_value_equals'

			if (splits.Length != 8) throw new InvalidArgsNumberException(splits.Length, 8);

			this._filename = splits[2].ToUpperInvariant();
			this._manager = splits[3];
			this._str = splits[4];
			this._record = splits[5];
			this._property = splits[6];
			this._value = splits[7];
		}

		public bool Evaluate(CollectionMap map)
		{
			try
			{

				var collection = map.GetCollection(this._filename, this._manager, this._str);
				var str = collection as STRBlock;

				var key = this._record.IsHexString()
					? Convert.ToUInt32(this._record, 16)
					: this._record.BinHash();

				var check = str.GetRecord(key).GetValue(this._property);
				return String.Compare(check, this._value, StringComparison.Ordinal) == 0;

			}
			catch { return false; }
		}
	}
}
