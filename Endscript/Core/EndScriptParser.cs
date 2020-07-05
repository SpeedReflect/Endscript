﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Endscript.Enums;
using Endscript.Commands;
using Endscript.Exceptions;
using CoreExtensions.Text;



namespace Endscript.Core
{
	public class EndScriptParser
	{
		private readonly string _filename;
		private string _xml_description = String.Empty;
		private const string VERSN2 = "[VERSN2]";
		private const string VERSN3 = "[VERSN3]";

		private string Directory => Path.GetDirectoryName(this._filename);
		public string XMLDescription => this._xml_description;
		
		public EndScriptParser(string filename)
		{
			this._filename = filename;
		}

		public IEnumerable<BaseCommand> Read()
		{
			return this.RecursiveRead(this._filename);
		}

		private List<BaseCommand> RecursiveRead(string filename)
		{
			// Always expect Version 2 endscript to be passed

			if (!File.Exists(filename))
			{

				throw new FileNotFoundException($"File with path {filename} does not exist");

			}

			using (var sr = new StreamReader(this._filename))
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

			var lines = File.ReadAllLines(this._filename);
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

				// Get command type, parse it and add
				BaseCommand command = type switch
				{
					eCommandType.update_collection => new UpdateCollectionCommand(),
					eCommandType.update_string => new UpdateStringCommand(),
					eCommandType.update_texture => new UpdateTextureCommand(),
					eCommandType.add_collection => new AddCollectionCommand(),
					eCommandType.add_string => new AddStringCommand(),
					eCommandType.add_texture => new AddTextureCommand(),
					eCommandType.remove_collection => new RemoveCollectionCommand(),
					eCommandType.remove_string => new RemoveStringCommand(),
					eCommandType.remove_texture => new RemoveTextureCommand(),
					eCommandType.copy_collection => new CopyCollectionCommand(),
					eCommandType.copy_texture => new CopyTextureCommand(),
					eCommandType.replace_texture => new ReplaceTextureCommand(),
					eCommandType.@static => new StaticCommand(),
					eCommandType.import => new ImportCommand(),
					eCommandType.@new => new NewCommand(),
					eCommandType.delete => new DeleteCommand(),
					eCommandType.checkbox => new CheckboxCommand(),
					eCommandType.combobox => new ComboboxCommand(),
					eCommandType.end => new EndCommand(),
					_ => new OptionalCommand()
				};

				command.Line = line;
				command.Prepare(splits);
				list.Add(command);

			}

			return list;
		}
	}
}