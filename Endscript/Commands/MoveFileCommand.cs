using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'move_file [movetype] [pathfromtype] [pathtotype] [pathfrom] [pathto]'.
	/// </summary>
	public class MoveFileCommand : BaseCommand
	{
		private eImportType _import;
		private ePathType _type_from;
		private ePathType _type_to;
		private string _path_from;
		private string _path_to;

		public override eCommandType Type => eCommandType.move_file;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			if (!Enum.TryParse(splits[1], out this._import))
			{

				throw new Exception($"Unable to recognize {splits[1]} serialization type");

			}

			this._type_from = EnumConverter.StringToPathType(splits[2]);
			if (this._type_from == ePathType.Invalid) throw new Exception($"Path type {splits[2]} is an invalid type");

			this._type_to = EnumConverter.StringToPathType(splits[3]);
			if (this._type_to == ePathType.Invalid) throw new Exception($"Path type {splits[3]} is an invalid type");

			this._path_from = splits[4];
			this._path_to = splits[5];
		}

		public override void Execute(CollectionMap map)
		{
			this._path_from = this._type_from == ePathType.Relative
				? Path.Combine(map.Directory, this._path_from)
				: Path.Combine(map.Profile.Directory, this._path_from);

			this._path_to = this._type_to == ePathType.Relative
				? Path.Combine(map.Directory, this._path_to)
				: Path.Combine(map.Profile.Directory, this._path_to);

			if (!File.Exists(this._path_from))
			{

				throw new FileNotFoundException($"File with path {this._path_from} does not exist");

			}

			if (this._import == eImportType.negate && File.Exists(this._path_to)) return;

			var directory = Path.GetDirectoryName(this._path_to);
			Directory.CreateDirectory(directory);
			File.Copy(this._path_from, this._path_to, true);
		}
	}
}
