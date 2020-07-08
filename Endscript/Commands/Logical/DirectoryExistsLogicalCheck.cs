using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Exceptions;
using Endscript.Interfaces;



namespace Endscript.Commands.Logical
{
	public class DirectoryExistsLogicalCheck : ILogical
	{
		private ePathType _type;
		private string _path;

		public eLogicType LogicType => eLogicType.directory_exists;

		public void Parse(string[] splits)
		{
			// Considering first 2 tokens are 'if' and 'directory_exists'

			if (splits.Length != 4) throw new InvalidArgsNumberException(splits.Length, 4);

			this._type = EnumConverter.StringToPathType(splits[2]);
			if (this._type == ePathType.Invalid) throw new Exception($"Path type {splits[2]} is an invalid type");

			this._path = splits[3];
		}

		public bool Evaluate(CollectionMap map)
		{
			try
			{

				this._path = this._type == ePathType.Relative
					? Path.Combine(map.Directory, this._path)
					: Path.Combine(map.Profile.Directory, this._path);

				return Directory.Exists(this._path);

			}
			catch { return false; }
		}
	}
}
