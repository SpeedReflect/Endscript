using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Nikki.Support.Shared.Class;



namespace Endscript.Commands
{
	public sealed class AddTextureCommand : BaseCommand
	{
		private string _filename;
		private string _manager;
		private string _tpk;
		private string _cname;
		private string _path;

		public override eCommandType Type => eCommandType.add_texture;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			this._filename = splits[1];
			this._manager = splits[2];
			this._tpk = splits[3];
			this._cname = splits[4];
			this._path = splits[5];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);

			if (collection is TPKBlock tpk)
			{

				var path = Path.Combine(map.Directory, this._path);
				tpk.AddTexture(this._cname, path);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}
	}
}
