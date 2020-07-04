using System;
using System.Collections.Generic;
using System.Text;

using Endscript.Core;

namespace Endscript.Commands
{
	public sealed class AddEndScriptCommand : BaseEndScriptCommand
	{
		private int _level;
		private string _filename;
		private string _manager;
		private string _collection;

		public override void Execute(CollectionMap map) => throw new NotImplementedException();
		public override void Prepare(string[] splits) => throw new NotImplementedException();
	}
}
