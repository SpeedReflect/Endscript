using System;
using System.Collections.Generic;
using System.Text;
using Endscript.Enums;
using Nikki.Reflection.Enum;



namespace Endscript.Helpers
{
	internal static class EnumConverter
	{
		public static SerializeType ImportToSerialize(eImportType type)
		{
			return type switch
			{
				eImportType.negate => SerializeType.Negate,
				eImportType.synchronize => SerializeType.Synchronize,
				_ => SerializeType.Override
			};
		}
	
		public static ePathType StringToPathType(string str)
		{
			return str switch
			{
				"relative" => ePathType.Relative,
				"absolute" => ePathType.Absolute,
				_ => ePathType.Invalid
			};
		}
	}
}
