using System;

using Endscript.Commands.Logical;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'if [logictype] [statement]' with 'do/else' options.
	/// </summary>
	public class IfStatementCommand : BaseCommand, ISelectable
	{
		private string[] _options;
		private ILogical _logical;

		public override eCommandType Type => eCommandType.@if;
		public string[] Options => this._options;
		public string Description => "IfStatement";
		public int Choice { get; set; }

		public IfStatementCommand() => this._options = new string[2] { "do", "else" };

		public override void Prepare(string[] splits)
		{
			if (splits.Length < 2) throw new Exception("Expected at least 2 tokens in an if-statement");

			if (!Enum.TryParse(splits[1], out eLogicType logic))
			{

				throw new Exception($"Unable to recognize logical check type named {splits[1]}");

			}

			this._logical = logic switch
			{
				eLogicType.texture_exists => new TextureExistsLogicalCheck(),
				eLogicType.string_exists => new StringExistsLogicalCheck(),
				eLogicType.file_exists => new FileExistsLogicalCheck(),
				eLogicType.directory_exists => new DirectoryExistsLogicalCheck(),
				eLogicType.collection_value_equals => new CollectionValueEqualsLogicalCheck(),
				eLogicType.texture_value_equals => new TextureValueEqualsLogicalCheck(),
				eLogicType.string_value_equals => new StringValueEqualsLogicalCheck(),
				_ => new CollectionExistsLogicalCheck(),
			};

			this._logical.Parse(splits);
		}

		public override void Execute(CollectionMap map)
		{
			this.Choice = this._logical.Evaluate(map) ? 1 : 0;
		}

		public int ParseOption(string option)
		{
			return option switch
			{
				"else" => 0,
				"do" => 1,
				_ => -1
			};
		}

		public bool Contains(string option)
		{
			return option == this._options[0] || option == this._options[1];
		}
	}
}
