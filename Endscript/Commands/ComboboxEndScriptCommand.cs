using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	public sealed class ComboboxEndScriptCommand : BaseEndScriptCommand, ISelectable
	{
		private string[] _options;
		private string _description = String.Empty;
		private int _level;

		public override eCommandType Type => eCommandType.combobox;
		public int Selection { get; set; }
		public string[] Options => this._options;
		public string Description => this._description;
		public int Choice
		{
			get
			{
				if (this.Selection < 0) return 0;
				else if (this.Selection >= this._options.Length) return this._options.Length - 1;
				return this.Selection;
			}
		}

		public override void Prepare(string[] splits)
		{
			this._level = splits.Length;

			if (this._level > 3)
			{

				this._options = new string[this._level - 2];

				for (int i = 1; i < this._level - 1; ++i)
				{

					this._options[i - 1] = splits[i];

				}

				this._description = splits[^1];

			}
		}

		public override void Execute(CollectionMap map)
		{
			if (this._level <= 3 && ThrowError)
			{

				throw new Exception($"Invalid number of arguments passed: expected at least 4, got {this._level}");

			}
		}
	
		public int ParseOption(string option)
		{
			for (int i = 0; i < this._options.Length; ++i)
			{

				if (option == this._options[i]) return i;

			}

			return -1;
		}
	}
}
