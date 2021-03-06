﻿using System;
using System.IO;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Nikki.Utils;
using Nikki.Reflection.Enum;
using Nikki.Support.Shared.Class;
using CoreExtensions.Management;



namespace Endscript.Commands
{
	/// <summary>
	/// Command of type 'bind_textures [type] [filename] [manager] [tpkblock] [path]'.
	/// </summary>
	public class BindTexturesCommand : BaseCommand
	{
		private eImportType _type;
		private string _filename;
		private string _manager;
		private string _tpk;
		private string _path;

		public override eCommandType Type => eCommandType.bind_textures;

		public override void Prepare(string[] splits)
		{
			if (splits.Length != 6) throw new InvalidArgsNumberException(splits.Length, 6);

			if (!Enum.TryParse(splits[1], out _type))
			{

				throw new Exception($"Unable to recognize {splits[1]} serialization type");

			}

			this._filename = splits[2].ToUpperInvariant();
			this._manager = splits[3];
			this._tpk = splits[4];
			this._path = splits[5];
		}

		public override void Execute(CollectionMap map)
		{
			var collection = map.GetCollection(this._filename, this._manager, this._tpk);

			if (collection is TPKBlock tpk)
			{

				var directory = Path.Combine(map.Directory, this._path);
				
				if (!Directory.Exists(directory))
				{

					throw new DirectoryNotFoundException($"Directory with path {directory} does not exist");

				}

				this.BindTextures(directory, tpk);

			}
			else
			{

				throw new Exception($"Object {this._tpk} is not a TPKBlock");

			}
		}

		private void BindTextures(string directory, TPKBlock tpk)
		{
			string current = String.Empty;

			try
			{

				foreach (var file in Directory.GetFiles(directory))
				{

					current = file;
					var name = Path.GetFileNameWithoutExtension(file);
					var key = name.BinHash();
					var texture = tpk.FindTexture(key, KeyType.BINKEY);

					if (texture is null)
					{

						tpk.AddTexture(name, file);

					}
					else if (this._type == eImportType.synchronize)
					{

						texture.Reload(file);

					}
					else if (this._type == eImportType.@override)
					{

						tpk.RemoveTexture(key, KeyType.BINKEY);
						tpk.AddTexture(name, file);

					}

				}

			}
			catch (Exception e)
			{

				var error = e.GetLowestMessage();
				error = $"Texture {current} : {error}";
				throw new Exception(error);

			}
		}
	}
}
