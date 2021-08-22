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
	/// Command of type 'import_all [type] [filename] [manager] [path]'.
	/// </summary>
	public class ImportAllCommand : BaseCommand
	{
		private eImportType _type;
		private string _filename;
		private string _manager;
		private string _path;

		public override eCommandType Type => eCommandType.import_all;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 5) throw new InvalidArgsNumberException(splits.Length, 5);

			this._filename = splits[2].ToUpperInvariant();
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

			if (!Directory.Exists(path))
			{

				throw new DirectoryNotFoundException($"Directory with path {path} does not exist");

			}

			foreach (var file in Directory.GetFiles(path))
			{

				var count = manager.Count;
				using var br = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.Read));
				manager.Import(EnumConverter.ImportToSerialize(this._type), br);

				if (manager.Count == count) continue; // if was override/negated/synchronized
				var collection = manager[^1] as Collectable; // if entirely new, add to the map
				map.AddCollection(this._filename, this._manager, collection.CollectionName, collection);

			}
		}
	}
}
