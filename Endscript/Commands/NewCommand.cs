using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'new [type] [filename]'.
	/// </summary>
	public class NewCommand : BaseCommand, ISingleParsable
	{
		private eImportType _type;
		private string _filename;

		public override eCommandType Type => eCommandType.@new;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 3) throw new InvalidArgsNumberException(splits.Length, 3);

			if (!Enum.TryParse(splits[1], out _type))
			{

				throw new Exception($"Unable to recognize {splits[1]} serialization type");

			}

			this._filename = splits[2].ToUpperInvariant();
		}

		public override void Execute(CollectionMap map)
		{
			map.Profile.New(this._type, this._filename);
			map.LoadMapFromProfile(true);
		}

		public void SingleExecution(BaseProfile profile)
		{
			profile.New(this._type, this._filename);
		}
	}
}
