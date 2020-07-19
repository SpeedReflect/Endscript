using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Nikki.Utils;
using Nikki.Reflection.Enum;
using Nikki.Support.Shared.Class;
using CoreExtensions.Text;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'replace_texture [filename] [manager] [tpkblock] [key] [path]'.
	/// </summary>
	public class ReplaceTextureCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private string _texture;
		private string _path;

		public override eCommandType Type => eCommandType.replace_texture;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			this._filename = splits[1].ToUpperInvariant();
			this._manager = splits[2];
			this._tpk = splits[3];
			this._texture = splits[4];
			this._path = splits[5];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);

			if (collection is TPKBlock tpk)
			{

				var key = this._texture.IsHexString()
					? Convert.ToUInt32(this._texture, 16)
					: this._texture.BinHash();

				var path = Path.Combine(map.Directory, this._path);
				tpk.ReplaceTexture(key, KeyType.BINKEY, path);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}
	}
}
