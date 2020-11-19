using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Utils;
using Nikki.Support.Shared.Class;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'add_or_update_string [filename] [manager] [strblock] [key] [label] [value]'.
	/// </summary>
	public class AddOrUpdateStringCommand : BaseCommand, ISingleParsable
	{
		private string _filename;
		private string _manager;
		private string _str;
		private string _key;
		private string _label;
		private string _value;

		public override eCommandType Type => eCommandType.add_or_update_string;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 7) throw new InvalidArgsNumberException(splits.Length, 7);

			this._filename = splits[1].ToUpperInvariant();
			this._manager = splits[2];
			this._str = splits[3];
			this._key = splits[4];
			this._label = splits[5];
			this._value = splits[6];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._str);

			if (collection is STRBlock str)
			{

				try
				{

					str.AddRecord(this._key, this._label, this._value);

				}
				catch
				{

					// Prioritize label to key
					// We can be confident that record is not null b/c of exception
					var record = str.GetRecord(this._label.BinHash());
					record.SetValue("Text", this._value);

				}

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

				try
				{

					str.AddRecord(this._key, this._label, this._value);

				}
				catch
				{

					// Prioritize label to key
					// We can be confident that record is not null b/c of exception
					var record = str.GetRecord(this._label.BinHash());
					record.SetValue("Text", this._value);

				}

			}
			else
			{

				throw new Exception($"Object {this._str} is not a STRBlock");

			}
		}
	}
}
