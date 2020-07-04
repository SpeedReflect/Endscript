using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'remove_string [filename] [manager] [strblock] [key]'.
	/// </summary>
	public sealed class RemoveStringCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _str;
		private uint _record;

		public override eCommandType Type => eCommandType.remove_string;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 5) throw new InvalidArgsNumberException(splits.Length, 5);

			this._filename = splits[1];
			this._manager = splits[2];
			this._str = splits[3];

			if (splits[4].IsHexString()) this._record = Convert.ToUInt32(splits[4], 16);
			else throw new Exception($"Value {splits[4]} cannot be converted to a hexadecimal key");
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._str);

			if (collection is STRBlock str)
			{

				str.RemoveRecord(this._record);

			}
			else
			{

				throw new Exception($"Object {this._str} is not a STRBlock");

			}
		}
	}
}
