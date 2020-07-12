using Nikki.Core;



namespace Endscript.Profiles
{
	public class Underground2Profile : BaseProfile
	{
		public override GameINT GameINT => GameINT.Underground2;

		public override string GameSTR => GameINT.Underground2.ToString();

		public override string Directory { get; }

		public Underground2Profile(string directory) : base() { this.Directory = directory; }
	}
}
