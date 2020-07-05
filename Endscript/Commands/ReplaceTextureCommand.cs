using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
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
		private uint _texture;
		private string _path;

		public override eCommandType Type => eCommandType.replace_texture;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			this._filename = splits[1];
			this._manager = splits[2];
			this._tpk = splits[3];
			this._path = splits[5];

			if (splits[4].IsHexString()) this._texture = Convert.ToUInt32(splits[4], 16);
			else throw new Exception($"Value {splits[4]} cannot be converted to a hexadecimal key");
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);

			if (collection is TPKBlock tpk)
			{

				var path = Path.Combine(map.Directory, this._path);
				tpk.ReplaceTexture(this._texture, eKeyType.BINKEY, path);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}
	}
}
