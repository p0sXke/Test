using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Booking.Domain.Utils
{
	public static class EnumHelper
	{
		public static string GetDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0)
			{
				return attributes[0].Description;
			}

			return value.ToString();
		}

		public static IEnumerable<string> GetValuesFor(Type type)
		{
			return Enum.GetNames(type).ToList();
		}
	}
}
