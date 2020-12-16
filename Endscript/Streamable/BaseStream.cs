using System;
using System.Collections.Generic;
using System.Text;
using Endscript.Exceptions;
using Nikki.Core;



namespace Endscript.Streamable
{
	public abstract class BaseStream
	{
		public abstract GameINT GameINT { get; }
		public abstract string GameSTR { get; }
		public string LXRY { get; }
		public string STREAMLXRY { get; }

		protected BaseStream(string lxry, string streamlxry)
		{
			this.LXRY = lxry;
			this.STREAMLXRY = streamlxry;
		}

		public abstract void Load(string directory, string dest);
		public abstract void Save(string directory, string src);

		public static BaseStream GetStreamer(GameINT game, string lxry, string streamlxry)
		{
			return game switch
			{
				GameINT.Carbon => new CarbonStream(lxry, streamlxry),
				GameINT.MostWanted => new MostWantedStream(lxry, streamlxry),
				GameINT.Underground2 => new Underground2Stream(lxry, streamlxry),
				_ => throw new InvalidGameException(game),
			};
		}
	}

	public interface IStreamingSection
	{
		string GetName();
	}
}
