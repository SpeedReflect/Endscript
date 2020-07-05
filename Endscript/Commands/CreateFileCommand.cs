using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'create_file [pathtype] [path]'.
	/// </summary>
	public class CreateFileCommand : BaseCommand
	{
		private ePathType _type;
		private string _path;

		public override eCommandType Type => eCommandType.create_file;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 3) throw new InvalidArgsNumberException(splits.Length, 3);

			this._type = EnumConverter.StringToPathType(splits[1]);
			if (this._type == ePathType.Invalid) throw new Exception($"Path type {splits[1]} is an invalid type");

			this._path = splits[2];
		}

		public override void Execute(CollectionMap map)
		{
			if (this._type == ePathType.Relative)
			{

				this._path = Path.Combine(map.Directory, this._path);
				var directory = Path.GetDirectoryName(this._path);
				Directory.CreateDirectory(directory);
				File.Create(this._path);

			}
			else
			{

				this._path = Path.Combine(map.Profile.Directory, this._path);
				var directory = Path.GetDirectoryName(this._path);
				Directory.CreateDirectory(directory);
				File.Create(this._path);

			}
		}
	}
}
