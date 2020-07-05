using System;
using Nikki.Core;



namespace Endscript.Exceptions
{
	public class InvalidGameException : Exception
	{
		public InvalidGameException() : base() { }

		public InvalidGameException(GameINT game)
			: base($"Game {game} is not supported with this endscript") { }

		public InvalidGameException(string game)
			: base($"Game {game} is not a recognizable game type") { }
	}
}
