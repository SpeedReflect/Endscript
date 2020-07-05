using System;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'version [#.#.#.#]'.
	/// </summary>
	public class VersionCommand : BaseCommand
	{
		private System.Version _version;

		public override eCommandType Type => eCommandType.version;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 2) throw new InvalidArgsNumberException(splits.Length, 2);

			this.CheckValidVersion(splits[1]);

			if (Version.Value.CompareTo(this._version) >= 0) return;
			else throw new Exception($"Endscript version {this._version} is higher than executable {Version.Value}");
		}

		public override void Execute(CollectionMap map)
		{
		}

		private void CheckValidVersion(string version)
		{
			try
			{

				this._version = new System.Version(version);

			}
			catch
			{

				throw new Exception($"Version stated is of invalid format");

			}
		}
	}
}
