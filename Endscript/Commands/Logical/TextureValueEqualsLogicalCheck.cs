using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Utils;
using Nikki.Reflection.Enum;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands.Logical
{
	public class TextureValueEqualsLogicalCheck : ILogical
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private string _texture;
		private string _property;
		private string _value;

		public eLogicType LogicType => eLogicType.texture_value_equals;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'texture_value_equals'

			if (splits.Length != 8) throw new InvalidArgsNumberException(splits.Length, 8);

			this._filename = splits[2].ToUpperInvariant();
			this._manager = splits[3];
			this._tpk = splits[4];
			this._texture = splits[5];
			this._property = splits[6];
			this._value = splits[7];
		}

		public bool Evaluate(CollectionMap map)
		{
			try
			{

				var key = this._texture.IsHexString()
					? Convert.ToUInt32(this._texture, 16)
					: this._texture.BinHash();

				var collection = map.GetCollection(this._filename, this._manager, this._tpk);
				var tpk = collection as TPKBlock;
				var check = tpk.FindTexture(key, KeyType.BINKEY).GetValue(this._property);
				return String.Compare(check, this._value, StringComparison.Ordinal) == 0;

			}
			catch { return false; }
		}
	}
}
