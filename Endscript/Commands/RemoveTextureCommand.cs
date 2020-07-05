using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Nikki.Reflection.Enum;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'remove_texture [filename] [manager] [tpkblock] [key]'.
	/// </summary>
	public class RemoveTextureCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private uint _texture;

		public override eCommandType Type => eCommandType.remove_texture;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 5) throw new InvalidArgsNumberException(splits.Length, 5);

			this._filename = splits[1];
			this._manager = splits[2];
			this._tpk = splits[3];

			if (splits[4].IsHexString()) this._texture = Convert.ToUInt32(splits[4], 16);
			else throw new Exception($"Value {splits[4]} cannot be converted to a hexadecimal key");
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);

			if (collection is TPKBlock tpk)
			{

				tpk.RemoveTexture(this._texture, eKeyType.BINKEY);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}
	}
}
