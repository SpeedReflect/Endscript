using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'speedreflect [all/one]'.
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
			var speedfrom = Path.Combine(map.Directory, "SpeedReflect.dll");
			var speedto = Path.Combine(map.Profile.Directory, @"scripts\SpeedReflect.asi");

			if (String.Compare(this._type, "all", StringComparison.OrdinalIgnoreCase) == 0)
			{

				var dinfrom = Path.Combine(map.Directory, "dinput8.dll");
				var dinto = Path.Combine(map.Profile.Directory, "dinput8.dll");
				File.Copy(speedfrom, speedto, true);
				File.Copy(dinfrom, dinto, true);

			}
			else if (String.Compare(this._type, "one", StringComparison.OrdinalIgnoreCase) == 0)
			{

				File.Copy(speedfrom, speedto, true);

			}
			else
			{

				throw new Exception($"Invalid argument passed named {this._type}");

			}
		}
	}
}
