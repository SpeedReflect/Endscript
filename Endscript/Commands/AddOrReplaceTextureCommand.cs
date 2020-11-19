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
	/// Command of type 'add_or_replace_texture [filename] [manager] [tpkblock] [key] [cname] [path]'.
	/// </summary>
	public class AddOrReplaceTextureCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private string _key;
		private string _cname;
		private string _path;

		public override eCommandType Type => eCommandType.add_or_replace_texture;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 7) throw new InvalidArgsNumberException(splits.Length, 7);

			this._filename = splits[1].ToUpperInvariant();
			this._manager = splits[2];
			this._tpk = splits[3];
			this._key = splits[4];
			this._cname = splits[5];
			this._path = splits[6];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);

			if (collection is TPKBlock tpk)
			{

				var path = Path.Combine(map.Directory, this._path);

				try
				{

					tpk.AddTexture(this._cname, path);

				}
				catch
				{

					var key = this._key.IsHexString()
						? Convert.ToUInt32(this._key, 16)
						: this._key.BinHash();

					// Since was caught, texture does exist
					tpk.ReplaceTexture(key, KeyType.BINKEY, path);

				}

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}
	}
}
