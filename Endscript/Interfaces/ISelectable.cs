namespace Endscript.Interfaces
{
	internal interface ISelectable
	{
		int Choice { get; }
		int ParseOption(string option);
	}
}
