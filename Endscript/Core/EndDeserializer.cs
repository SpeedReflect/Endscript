using System;
using System.IO;
using System.Linq;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Commands;
using Endscript.Profiles;
using Endscript.Exceptions;
using CoreExtensions.Text;



namespace Endscript.Core
{
	public class EndDeserializer
	{
		private BaseProfile _profile;
		private readonly string _directory;
		private readonly string _mainfile;

		private string _sdb_dir;
		private string _game;
		private int _count;

		public EndDeserializer(string filename)
		{
			this._mainfile = filename;
			this._directory = Path.GetDirectoryName(filename);
		}

		public BaseProfile Deserialize()
		{
			if (!File.Exists(this._mainfile))
			{

				throw new FileNotFoundException($"File {this._mainfile} does not exist");

			}

			using var sr = new StreamReader(File.Open(this._mainfile, FileMode.Open, FileAccess.Read));

			if (sr.ReadLine() != "[VERSN4]")
			{

				throw new InvalidVersionException(4);

			}

			while (!sr.EndOfStream)
			{

				var line = sr.ReadLine();
				if (String.IsNullOrWhiteSpace(line) || line.StartsWith("//") || line.StartsWith('#')) continue;

				var splits = line.SmartSplitString().ToArray();
				if (splits.Length < 1) continue;

				if (!Enum.TryParse(splits[0], out eCommandType command))
				{

					throw new Exception($"Command {splits[0]} cannot be recognized");

				}

				switch (command)
				{
					case eCommandType.game:
						this.SetGame(splits);
						break;

					case eCommandType.directory:
						this.SetDirectory(splits);
						break;

					case eCommandType.filecount:
						this.SetFileCount(splits);
						break;

					case eCommandType.generate:
						this.GenerateProfile();
						break;

					case eCommandType.capacity:
						this.SetManagerCapacity(splits);
						break;

					case eCommandType.@new:
						this.MakeNewSDB(splits);
						break;

					case eCommandType.import:
						this.ImportCollection(splits);
						break;

					default:
						throw new Exception($"Command {command} is not supported in a version 4 endscript");

				}

			}

			return this._profile;
		}

		private void SetGame(string[] splits)
		{
			if (splits.Length != 2)
			{

				throw new InvalidArgsNumberException(splits.Length, 2);

			}

			this._game = splits[1];
		}

		private void SetDirectory(string[] splits)
		{
			if (splits.Length != 2)
			{

				throw new InvalidArgsNumberException(splits.Length, 2);

			}

			this._sdb_dir = splits[1];
		}
	
		private void SetFileCount(string[] splits)
		{
			if (splits.Length != 2)
			{

				throw new InvalidArgsNumberException(splits.Length, 2);

			}

			this._count = Int32.Parse(splits[1]);
		}
	
		private void GenerateProfile()
		{
			this._profile = BaseProfile.NewProfile(this._game, this._sdb_dir);
			this._profile.Capacity += this._count;
		}
	
		private void SetManagerCapacity(string[] splits)
		{
			if (splits.Length != 4)
			{

				throw new InvalidArgsNumberException(splits.Length, 4);

			}

			var sdb = this._profile[splits[1]];
			if (sdb is null) throw new Exception($"File {splits[1]} was never loaded");

			var manager = sdb.Database.GetManager(splits[2]);
			if (manager is null) throw new Exception($"Manager named {splits[2]} does not exist");

			manager.Capacity += Int32.Parse(splits[3]);
		}

		private void MakeNewSDB(string[] splits)
		{
			var command = new NewCommand();
			command.Prepare(splits);
			command.SingleExecution(this._profile);
		}

		private void ImportCollection(string[] splits)
		{
			if (splits.Length != 4)
			{

				throw new InvalidArgsNumberException(splits.Length, 4);

			}

			if (!Enum.TryParse(splits[1], out eImportType import))
			{

				throw new Exception($"Import type {splits[1]} cannot be recognized");

			}

			var type = EnumConverter.ImportToSerialize(import);

			var sdb = this._profile[splits[2]];
			if (sdb is null) throw new Exception($"File {splits[2]} was never loaded");

			var filename = Path.Combine(this._directory, splits[3]);
			
			if (!File.Exists(filename))
			{

				throw new FileNotFoundException($"File with path {splits[3]} does not exist");

			}

			using var br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
			sdb.Database.Import(type, br);
		}
	}
}
