using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Interfaces;
using Endscript.Commands.Logical;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'if [logictype] [statement]' with 'do/else' options.
	/// </summary>
	public class IfStatementCommand : BaseCommand, ISelectable
	{
		private OptionState[] _options;
		private ILogical _logical;

		public override eCommandType Type => eCommandType.@if;
		public OptionState[] Options => this._options;
		public string Description => "IfStatement";
		public int Choice { get; set; }
		public int LastCommand { get; set; } = -1;

		public IfStatementCommand()
		{
			this._options = new OptionState[2]
			{
				new OptionState("else"),
				new OptionState("do"),
			};
		}

		public OptionState this[string name]
		{
			get
			{

				if (this._options[0].Name == name) return this._options[0];
				else if (this._options[1].Name == name) return this._options[1];
				return null;

			}
		}

		public override void Prepare(string[] splits)
		{
			if (splits.Length < 2) throw new Exception("Expected at least 2 tokens in an if-statement");

			if (!Enum.TryParse(splits[1], out eLogicType logic))
			{

				throw new Exception($"Unable to recognize logical check type named {splits[1]}");

			}

			this._logical = logic switch
			{
				eLogicType.incareer_exists => new InCareerExistsLogicalCheck(),
				eLogicType.texture_exists => new TextureExistsLogicalCheck(),
				eLogicType.string_exists => new StringExistsLogicalCheck(),
				eLogicType.file_exists => new FileExistsLogicalCheck(),
				eLogicType.directory_exists => new DirectoryExistsLogicalCheck(),
				eLogicType.collection_value_equals => new CollectionValueEqualsLogicalCheck(),
				eLogicType.incareer_value_equals => new InCareerValueEqualsLogicalCheck(),
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
			return option == this._options[0].Name || option == this._options[1].Name;
		}
	}
}
