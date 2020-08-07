using Endscript.Helpers;



namespace Endscript.Interfaces
{
	public interface ISelectable
	{
		public int Choice { get; set; }
		public int LastCommand { get; set; }
		public string Description { get; }
		public OptionState[] Options { get; }
		public int ParseOption(string option);
		public bool Contains(string option);
		public OptionState this[string name] { get; }
	}
}
