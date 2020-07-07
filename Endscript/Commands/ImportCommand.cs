using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Exceptions;
using Nikki.Reflection.Abstract;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'import [type] [manager] [filename] [path]'.
	/// </summary>
	public class ImportCommand : BaseCommand
	{
		private eImportType _type;
		private string _filename;
		private string _manager;
		private string _path;

		public override eCommandType Type => eCommandType.import;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 5) throw new InvalidArgsNumberException(splits.Length, 5);

			this._filename = splits[2];
			this._manager = splits[3];
			this._path = splits[4];

			if (!Enum.TryParse(splits[1], out this._type))
			{

				throw new Exception($"Unable to recognize {splits[1]} serialization type");

			}
		}

		public override void Execute(CollectionMap map)
		{
			var sdb = map.Profile[this._filename];

			if (sdb is null)
			{

				throw new LookupFailException($"File {this._filename} was never loaded");

			}

			var manager = sdb.Database.GetManager(this._manager);

			if (manager is null)
			{

				throw new LookupFailException($"Manager named {this._manager} does not exist");

			}

			var path = Path.Combine(map.Directory, this._path);

			if (!File.Exists(path))
			{

				throw new FileNotFoundException($"File with path {path} does not exist");

			}

			using var br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
			manager.Import(EnumConverter.ImportToSerialize(this._type), br);
			var collection = manager[^1] as Collectable; // considering it should be the last one
			map.AddCollection(this._filename, this._manager, collection.CollectionName, collection);
		}
	}
}
