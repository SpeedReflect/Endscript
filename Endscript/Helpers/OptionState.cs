namespace Endscript.Helpers
{
	public class OptionState
	{
		public string Name { get; }
		public int Start { get; set; } = -1;
		public int End { get; set; } = -1;

		public OptionState(string name) => this.Name = name;
	}
}
