using System;
using System.IO;
using Nikki.Utils;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'unlock_memory [all/file]'.
	/// </summary>
	public class UnlockMemoryCommand : BaseCommand
	{
		private string _file;

		public override eCommandType Type => eCommandType.unlock_memory;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2) throw new InvalidArgsNumberException(splits.Length, 2);

			this._file = splits[1];
		}

		public override void Execute(CollectionMap map)
		{
			if (String.Compare(this._file, "all", StringComparison.OrdinalIgnoreCase) == 0)
			{

				MemoryUnlock.FastUnlock(map.Profile.Directory + @"\GLOBAL\CARHEADERSMEMORYFILE.BIN");
				MemoryUnlock.FastUnlock(map.Profile.Directory + @"\GLOBAL\FRONTENDMEMORYFILE.BIN");
				MemoryUnlock.FastUnlock(map.Profile.Directory + @"\GLOBAL\INGAMEMEMORYFILE.BIN");
				MemoryUnlock.FastUnlock(map.Profile.Directory + @"\GLOBAL\PERMANENTMEMORYFILE.BIN");
				MemoryUnlock.LongUnlock(map.Profile.Directory + @"\GLOBAL\GLOBALMEMORYFILE.BIN");

			}
			else
			{

				if (String.Compare(this._file, @"GLOBAL\GLOBALMEMORYFILE.BIN", StringComparison.OrdinalIgnoreCase) == 0)
				{

					MemoryUnlock.LongUnlock(Path.Combine(map.Profile.Directory, this._file));

				}
				else
				{

					MemoryUnlock.FastUnlock(Path.Combine(map.Profile.Directory, this._file));

				}

			}
		}
	}
}
