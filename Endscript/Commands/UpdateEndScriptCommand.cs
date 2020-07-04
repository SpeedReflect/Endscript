using System;
using Endscript.Core;
using Endscript.Exceptions;

namespace Endscript.Commands
{
	class UpdateEndScriptCommand : BaseEndScriptCommand
	{
		private int _level;
		private string _filename;
		private string _manager;
		private string _collection;
		private string _expand;
		private string _subpart;
		private string _property;
		private string _value;

		public override void Prepare(string[] splits)
		{
			this._level = splits.Length;

			switch (this._level)
			{
				case 6:
					this._filename = splits[1];
					this._manager = splits[2];
					this._collection = splits[3];
					this._property = splits[4];
					this._value = splits[5];
					return;

				case 7:
					this._filename = splits[1];
					this._manager = splits[2];
					this._collection = splits[3];
					this._expand = splits[4];
					this._property = splits[5];
					this._value = splits[6];
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
					return;

			}
		}

		public override void Execute(CollectionMap map)
		{
			switch (this._level)
			{
				case 6: this.UpdateCollection(map); return;
				case 7: this.UpdateSTR_TPK(map); return;
				case 8: this.UpdateSubPart(map); return;
				default:
					if (!ThrowError) return;
					throw new InvalidArgsNumberException(this._level, 6, 7, 8);
			}
		}

		private void UpdateCollection(CollectionMap map)
		{

		}

		private void UpdateSTR_TPK(CollectionMap map)
		{

		}

		private void UpdateSubPart(CollectionMap map)
		{

		}
	}
}
