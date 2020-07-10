using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using Nikki.Utils;
using Nikki.Reflection.Enum;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'remove_texture [filename] [manager] [tpkblock] [key]'.
	/// </summary>
	public class RemoveTextureCommand : BaseCommand, ISingleParsable
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private string _texture;

		public override eCommandType Type => eCommandType.remove_texture;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 5) throw new InvalidArgsNumberException(splits.Length, 5);

			this._filename = splits[1].ToUpperInvariant();
			this._manager = splits[2];
			this._tpk = splits[3];
			this._texture = splits[4];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);

			if (collection is TPKBlock tpk)
			{

				var key = this._texture.IsHexString()
					? Convert.ToUInt32(this._texture, 16)
					: this._texture.BinHash();

				tpk.RemoveTexture(key, eKeyType.BINKEY);

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

				var key = this._texture.IsHexString()
					? Convert.ToUInt32(this._texture, 16)
					: this._texture.BinHash();

				tpk.RemoveTexture(key, eKeyType.BINKEY);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}
	}
}
