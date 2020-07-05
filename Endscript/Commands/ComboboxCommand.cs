using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'combobox param[options] [description]' with 'param' options.
	/// </summary>
	public class ComboboxCommand : BaseCommand, ISelectable
	{
		private string[] _options;
		private string _description = String.Empty;
		private int _level;

		public override eCommandType Type => eCommandType.combobox;
		public string[] Options => this._options;
		public string Description => this._description;
		public int Choice { get; set; }

		public override void Prepare(string[] splits)
		{
			if (splits.Length < 4) throw new Exception($"Expected at least 4 arguments, got {splits.Length}");

			this._options = new string[this._level - 2];

			for (int i = 1; i < this._level - 1; ++i)
			{

				this._options[i - 1] = splits[i];

			}

			this._description = splits[^1];
		}

		public override void Execute(CollectionMap map)
		{
		}
	
		public int ParseOption(string option)
		{
			for (int i = 0; i < this._options.Length; ++i)
			{

				if (option == this._options[i]) return i;

			}

			return -1;
		}

		public bool Contains(string option)
		{
			for (int i = 0; i < this._options.Length; ++i)
			{

				if (option == this._options[i]) return true;

			}

			return false;
		}

		public void Evaluate()
		{

		}
	}
}
