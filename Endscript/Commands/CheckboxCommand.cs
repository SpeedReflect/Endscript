using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	public class CheckboxCommand : BaseCommand, ISelectable
	{
		private string[] _options;
		private string _description = String.Empty;

		public override eCommandType Type => eCommandType.checkbox;
		public string[] Options => this._options;
		public string Description => this._description;
		public int Choice { get; set; }

		public CheckboxCommand() => this._options = new string[] { "disabled", "enabled" };

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
			return option == this._options[0] || option == this._options[1];
		}
	
		public void Evaluate()
		{

		}
	}
}
