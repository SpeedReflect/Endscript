using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Interfaces;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'combobox param[options] [description]' with 'param' options.
	/// </summary>
	public class ComboboxCommand : BaseCommand, ISelectable
	{
		private OptionState[] _options;
		private string _description = String.Empty;

		public override eCommandType Type => eCommandType.combobox;
		public OptionState[] Options => this._options;
		public string Description => this._description;
		public int Choice { get; set; }
		public int LastCommand { get; set; }

		public override void Prepare(string[] splits)
		{
			if (splits.Length < 4) throw new Exception($"Expected at least 4 arguments, got {splits.Length}");

			this._options = new OptionState[splits.Length - 2];

			for (int i = 1; i < splits.Length - 1; ++i)
			{

				this._options[i - 1] = new OptionState(splits[i]);

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

				if (option == this._options[i].Name) return i;

			}

			return -1;
		}

		public bool Contains(string option)
		{
			for (int i = 0; i < this._options.Length; ++i)
			{

				if (option == this._options[i].Name) return true;

			}

			return false;
		}
	}
}
