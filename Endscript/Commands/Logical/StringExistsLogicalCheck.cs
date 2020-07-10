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
	public class StringExistsLogicalCheck : ILogical
	{
		private string _filename;
		private string _manager;
		private string _str;
		private string _record;

		public eLogicType LogicType => eLogicType.string_exists;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'string_exists'

			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			this._filename = splits[2].ToUpperInvariant();
			this._manager = splits[3];
			this._str = splits[4];
			this._record = splits[5];
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

				return !(str.GetRecord(key) is null);

			}
			catch { return false; }
		}
	}
}
