using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'checkbox [description]' with 'enabled/disabled' options.
	/// </summary>
	public class CheckboxCommand : BaseCommand, ISelectable
	{
		private OptionState[] _options;
		private string _description = String.Empty;

		public override eCommandType Type => eCommandType.checkbox;
		public OptionState[] Options => this._options;
		public string Description => this._description;
		public int Choice { get; set; }
		public int LastCommand { get; set; }

		public CheckboxCommand()
		{
			this._options = new OptionState[]
			{
				new OptionState("disabled"),
				new OptionState("enabled"),
			};
		}

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2) throw new InvalidArgsNumberException(splits.Length, 2);
			this._description = splits[1];
		}

		public override void Execute(CollectionMap map)
		{
		}
	
		public int ParseOption(string option)
		{
			return option switch
			{
				"disabled" => 0,
				"enabled" => 1,
				_ => -1
			};
		}
	
		public bool Contains(string option)
		{
			return option == this._options[0].Name || option == this._options[1].Name;
		}
	}
}
