using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'create_file [creationtype] [pathtype] [path]'.
	/// </summary>
	public class CreateFileCommand : BaseCommand
	{
		private eImportType _import;
		private ePathType _type;
		private string _path;

		public override eCommandType Type => eCommandType.create_file;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 4) throw new InvalidArgsNumberException(splits.Length, 4);

			if (!Enum.TryParse(splits[1], out this._import))
			{

				throw new Exception($"Unable to recognize {splits[1]} serialization type");

			}

			this._type = EnumConverter.StringToPathType(splits[2]);
			if (this._type == ePathType.Invalid) throw new Exception($"Path type {splits[2]} is an invalid type");

			this._path = splits[3];
		}

		public override void Execute(CollectionMap map)
		{
			if (this._type == ePathType.Relative)
			{

				this._path = Path.Combine(map.Directory, this._path);

				if (File.Exists(this._path) && this._import == eImportType.negate) return;

				var directory = Path.GetDirectoryName(this._path);
				Directory.CreateDirectory(directory);
				File.Create(this._path);

			}
			else
			{

				this._path = Path.Combine(map.Profile.Directory, this._path);

				if (File.Exists(this._path) && this._import == eImportType.negate) return;

				var directory = Path.GetDirectoryName(this._path);
				Directory.CreateDirectory(directory);
				File.Create(this._path);

			}
		}
	}
}
