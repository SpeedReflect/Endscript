using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Helpers;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'import [type] [filename] [path]'.
	/// </summary>
	public class ImportCommand : BaseCommand
	{
		private eImportType _type;
		private string _filename;
		private string _path;

		public override eCommandType Type => eCommandType.import;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 4) throw new InvalidArgsNumberException(splits.Length, 4);

			if (!Enum.TryParse(splits[1], out _type))
			{

				throw new Exception($"Unable to recognize {splits[1]} serialization type");

			}

			this._filename = splits[2];
			this._path = splits[3];
		}

		public override void Execute(CollectionMap map)
		{
			var sdb = map.Profile[this._filename];

			if (sdb is null)
			{

				throw new LookupFailException($"File {this._filename} was never loaded");

			}

			var path = Path.Combine(map.Directory, this._path);

			if (!File.Exists(path))
			{

				throw new FileNotFoundException($"File with path {path} does not exist");

			}

			using var br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
			sdb.Database.Import(EnumConverter.ImportToSerialize(this._type), br);
		}
	}
}
