using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'update_string [filename] [manager] [strblock] [key] [property] [value]'.
	/// </summary>
	public class UpdateStringCommand : BaseCommand, ISingleParsable
	{
		private string _filename;
		private string _manager;
		private string _str;
		private uint _record;
		private string _property;
		private string _value;

		public override eCommandType Type => eCommandType.update_string;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 7) throw new InvalidArgsNumberException(splits.Length, 7);

			this._filename = splits[1];
			this._manager = splits[2];
			this._str = splits[3];
			this._property = splits[5];
			this._value = splits[6];

			if (splits[4].IsHexString()) this._record = Convert.ToUInt32(splits[4], 16);
			else throw new Exception($"Value {splits[4]} cannot be converted to a hexadecimal key");
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._str);
			
			if (collection is STRBlock str)
			{

				var record = str.GetRecord(this._record);
				
				if (record is null)
				{

					throw new LookupFailException($"String with key 0x{this._record:X8} does not exist");

				}

				record.SetValue(this._property, this._value);

			}
			else
			{

				throw new Exception($"Object {this._str} is not a STRBlock");

			}
		}

		public void SingleExecution(BaseProfile profile)
		{
			var collection = this.GetManualCollection(this._filename, this._manager, this._str, profile);

			if (collection is STRBlock str)
			{

				var record = str.GetRecord(this._record);

				if (record is null)
				{

					throw new LookupFailException($"String with key 0x{this._record:X8} does not exist");

				}

				record.SetValue(this._property, this._value);

			}
			else
			{

				throw new Exception($"Object {this._str} is not a STRBlock");

			}
		}
	}
}
