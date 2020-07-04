namespace Endscript.Interfaces
{
	public interface ISelectable
	{
		public int Choice { get; set; }
		public string Description { get; }
		public string[] Options { get; }
		public int ParseOption(string option);
		public bool Contains(string option);
		public void Evaluate();
	}
}
