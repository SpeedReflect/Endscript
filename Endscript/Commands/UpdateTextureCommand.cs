using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Reflection.Enum;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'update_texture [filename] [manager] [tpkblock] [key] [property] [value]'.
	/// </summary>
	public class UpdateTextureCommand : BaseCommand, ISingleParsable
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private uint _texture;
		private string _property;
		private string _value;

		public override eCommandType Type => eCommandType.update_texture;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 7) throw new InvalidArgsNumberException(splits.Length, 7);

			this._filename = splits[1];
			this._manager = splits[2];
			this._tpk = splits[3];
			this._property = splits[5];
			this._value = splits[6];

			if (splits[4].IsHexString()) this._texture = Convert.ToUInt32(splits[4], 16);
			else throw new Exception($"Value {splits[4]} cannot be converted to a hexadecimal key");
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);
			
			if (collection is TPKBlock tpk)
			{

				var texture = tpk.FindTexture(this._texture, eKeyType.BINKEY);
				
				if (texture is null)
				{

					throw new LookupFailException($"Texture with key 0x{this._texture:X8} does not exist");

				}

				texture.SetValue(this._property, this._value);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}

		public void SingleExecution(BaseProfile profile)
		{
			var collection = this.GetManualCollection(this._filename, this._manager, this._tpk, profile);

			if (collection is TPKBlock tpk)
			{

				var texture = tpk.FindTexture(this._texture, eKeyType.BINKEY);

				if (texture is null)
				{

					throw new LookupFailException($"Texture with key 0x{this._texture:X8} does not exist");

				}

				texture.SetValue(this._property, this._value);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}
	}
}
