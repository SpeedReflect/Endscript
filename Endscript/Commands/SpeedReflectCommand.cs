using System;
using System.Diagnostics;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'speedreflect [auto/dir]'.
	/// </summary>
	public class SpeedReflectCommand : BaseCommand
	{
		private string _type;

		public override eCommandType Type => eCommandType.speedreflect;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2) throw new InvalidArgsNumberException(splits.Length, 2);

			this._type = splits[1];
		}

		public override void Execute(CollectionMap map)
		{
			var dir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
			var speedfrom = Path.Combine(dir, "SpeedReflect.asi");

			if (!File.Exists(speedfrom))
			{

				throw new FileNotFoundException("SpeedReflect.asi was not found");

			}

			if (String.Compare(this._type, "auto", StringComparison.OrdinalIgnoreCase) == 0)
			{

				var speedto = Path.Combine(map.Profile.Directory, @"scripts\SpeedReflect.asi");
				Directory.CreateDirectory(Path.Combine(map.Profile.Directory, "scripts"));
				File.Copy(speedfrom, speedto, true);

			}
			else // accounts as a directory
			{

				var speedto = Path.Combine(map.Profile.Directory, this._type);
				Directory.CreateDirectory(Path.Combine(map.Profile.Directory, Path.GetDirectoryName(this._type)));
				File.Copy(speedfrom, speedto, true);

			}
		}
	}
}
