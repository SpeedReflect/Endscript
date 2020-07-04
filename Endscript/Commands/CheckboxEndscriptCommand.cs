using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	public class CheckboxEndscriptCommand : BaseEndScriptCommand, ISelectable
	{
		private string _description = String.Empty;
		private int _level;

		public override eCommandType Type => eCommandType.checkbox;
		public bool Enabled { get; set; }
		public string Description => this._description;
		public int Choice => this.Enabled ? 1 : 0;

		public override void Prepare(string[] splits)
		{
			this._level = splits.Length;
			if (this._level == 2) this._description = splits[1];
		}

		public override void Execute(CollectionMap map)
		{
			if (this._level != 2 && ThrowError)
			{

				throw new InvalidArgsNumberException(this._level, 2);

			}
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
	}
}
