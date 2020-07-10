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
	/// Command of type 'copy_texture [filename] [manager] [tpkblock] [from] [to]'.
	/// </summary>
	public class CopyTextureCommand : BaseCommand, ISingleParsable
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private string _from;
		private string _to;

		public override eCommandType Type => eCommandType.copy_texture;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			this._filename = splits[1].ToUpperInvariant();
			this._manager = splits[2];
			this._tpk = splits[3];
			this._from = splits[4];
			this._to = splits[5];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);

			if (collection is TPKBlock tpk)
			{

				var key = this._from.IsHexString()
					? Convert.ToUInt32(this._from, 16)
					: this._from.BinHash();

				tpk.CloneTexture(this._to, key, eKeyType.BINKEY);

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

				var key = this._from.IsHexString()
					? Convert.ToUInt32(this._from, 16)
					: this._from.BinHash();

				tpk.CloneTexture(this._to, key, eKeyType.BINKEY);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}
	}
}
