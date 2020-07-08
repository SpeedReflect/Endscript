using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Endscript.Enums;
using Endscript.Commands;
using Endscript.Profiles;
using Endscript.Exceptions;
using Endscript.Interfaces;
using CoreExtensions.Text;



namespace Endscript.Core
{
	public class EndScriptParser
	{
		private readonly string _filename;
		private string _xml_description = String.Empty;
		private const string VERSN2 = "[VERSN2]";
		private const string VERSN3 = "[VERSN3]";

		/// <summary>
		/// Directory of the launcher endscript passed.
		/// </summary>
		public string Directory => Path.GetDirectoryName(this._filename);

		/// <summary>
		/// XML description menu as a string value.
		/// </summary>
		public string XMLDescription => this._xml_description;
		
		public EndScriptParser(string filename)
		{
			this._filename = filename;
		}

		~EndScriptParser()
		{
			#if DEBUG
			Console.WriteLine("EndScriptParser destroyed");
			#endif
		}

		public BaseCommand[] Read()
		{
			return this.RecursiveRead(this._filename).ToArray();
		}

		private List<BaseCommand> RecursiveRead(string filename)
		{
			// Always expect Version 2 endscript to be passed

			if (!File.Exists(filename))
			{

				throw new FileNotFoundException($"File with path {filename} does not exist");

			}

			using (var sr = new StreamReader(filename))
			{

				var read = sr.ReadLine();
				if (read == VERSN3)
				{

					this._xml_description = sr.ReadToEnd();
					return null;

				}
				else if (read != VERSN2)
				{

					throw new InvalidVersionException(2);

				}

			}

			var relative = filename.Substring(this.Directory.Length + 1);
			var lines = File.ReadAllLines(filename);
			var list = new List<BaseCommand>(lines.Length);

			// Start with line 1 b/c line 0 is VERSN line
			for (int i = 1; i < lines.Length; ++i)
			{

				var line = lines[i];
				if (String.IsNullOrWhiteSpace(line) || line.StartsWith("//") || line.StartsWith('#')) continue;

				var splits = line.SmartSplitString().ToArray();

				if (!Enum.TryParse(splits[0], out eCommandType type)) type = eCommandType.invalid;

				// Flatten all endscripts into one by merging them together via append commands
				if (type == eCommandType.append)
				{

					if (splits.Length != 2)
					{

						throw new InvalidArgsNumberException(splits.Length, 2);

					}

					var path = Path.Combine(this.Directory, splits[1]);
					var addon = this.RecursiveRead(path);
					if (addon != null) list.AddRange(addon);
					continue;

				}

				// Get command type, try preparing
				var command = GetCommandFromType(type);
				command.Prepare(splits);

				// If command is correct, add it to list
				command.Filename = relative;
				command.Line = line;
				command.Index = i;
				list.Add(command);

			}

			return list;
		}
	
		public static eCommandType ExecuteSingleCommand(string line, BaseProfile profile)
		{
			if (String.IsNullOrWhiteSpace(line) || line.StartsWith("//") || line.StartsWith('#'))
			{

				return eCommandType.empty;

			}

			var splits = line.SmartSplitString().ToArray();

			if (!Enum.TryParse(splits[0], out eCommandType type))
			{

				throw new Exception($"Unrecognizable command named {splits[0]}");

			}

			var command = GetCommandFromType(type);
			
			if (command is ISingleParsable single)
			{

				command.Line = line;
				command.Prepare(splits);
				single.SingleExecution(profile);
				return type;

			}
			else
			{

				throw new Exception($"Command of type {type} cannot be executed in a single-command mode");

			}

		}

		private static BaseCommand GetCommandFromType(eCommandType type)
		{
			return type switch
			{
				eCommandType.add_collection => new AddCollectionCommand(),
				eCommandType.add_string => new AddStringCommand(),
				eCommandType.add_texture => new AddTextureCommand(),
				eCommandType.checkbox => new CheckboxCommand(),
				eCommandType.combobox => new ComboboxCommand(),
				eCommandType.copy_collection => new CopyCollectionCommand(),
				eCommandType.copy_texture => new CopyTextureCommand(),
				eCommandType.create_file => new CreateFileCommand(),
				eCommandType.create_folder => new CreateFolderCommand(),
				eCommandType.delete => new DeleteCommand(),
				eCommandType.end => new EndCommand(),
				eCommandType.erase_file => new EraseFileCommand(),
				eCommandType.erase_folder => new EraseFolderCommand(),
				eCommandType.@if => new IfStatementCommand(),
				eCommandType.import => new ImportCommand(),
				eCommandType.move_file => new MoveFileCommand(),
				eCommandType.@new => new NewCommand(),
				eCommandType.remove_collection => new RemoveCollectionCommand(),
				eCommandType.remove_string => new RemoveStringCommand(),
				eCommandType.remove_texture => new RemoveTextureCommand(),
				eCommandType.replace_texture => new ReplaceTextureCommand(),
				eCommandType.@static => new StaticCommand(),
				eCommandType.update_collection => new UpdateCollectionCommand(),
				eCommandType.update_string => new UpdateStringCommand(),
				eCommandType.update_texture => new UpdateTextureCommand(),
				eCommandType.version => new VersionCommand(),
				eCommandType.watermark => new WatermarkCommand(),
				_ => new OptionalCommand()
			};
		}
	}
}
