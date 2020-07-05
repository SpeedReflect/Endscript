using System;
using System.Collections.Generic;
using System.Text;
using Endscript.Enums;
using Nikki.Reflection.Enum;



namespace Endscript.Helpers
{
	internal static class EnumConverter
	{
		public static eSerializeType ImportToSerialize(eImportType type)
		{
			return type switch
			{
				eImportType.negate => eSerializeType.Negate,
				eImportType.synchronize => eSerializeType.Synchronize,
				_ => eSerializeType.Override
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
