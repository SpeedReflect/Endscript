using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Endscript.Enums;
using Endscript.Commands;
using Endscript.Exceptions;
using CoreExtensions.Text;



namespace Endscript.Core
{
	public sealed class EndScriptParser
	{
		private readonly string _filename;
		private string _xml_description = String.Empty;
		private const string VERSN2 = "[VERSN2]";
		private const string VERSN3 = "[VERSN3]";
		private BaseEndScriptCommand _last_parsed;

		private string Directory => Path.GetDirectoryName(this._filename);
		public string XMLDescription => this._xml_description;
		
		public EndScriptParser(string filename)
		{
			this._filename = filename;
		}

		public IEnumerable<BaseEndScriptCommand> Read()
		{
			return this.RecursiveRead(this._filename);
		}

		private List<BaseEndScriptCommand> RecursiveRead(string filename)
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
			var list = new List<BaseEndScriptCommand>(lines.Length);

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

				}

				// Get command type, parse it and add
				BaseEndScriptCommand command = type switch
				{
					eCommandType.update => new UpdateEndScriptCommand(),
					eCommandType.checkbox => new CheckboxEndscriptCommand(),
					eCommandType.combobox => new ComboboxEndScriptCommand(),
					_ => new InvalidEndScriptCommand()
				};

				command.Line = line;
				command.Prepare(splits);
				list.Add(command);
				this._last_parsed = command;

			}

			return list;
		}

	}
}
