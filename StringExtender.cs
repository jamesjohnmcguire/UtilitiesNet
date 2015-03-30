/////////////////////////////////////////////////////////////////////////////
// $Id:$
//
// Copyright (c) 2006-2015 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Zenware.Common.UtilsNet.Extensions
{
	public static class StringExtender
	{
		public static bool CompareMultiple(
			this string data,
			StringComparison compareType,
			params string[] compareValues)
		{
			if ((compareValues != null) && (!string.IsNullOrEmpty(data)))
			{
			foreach (string s in compareValues)
			{
				if (data.Equals(s, compareType))
				{
					return true;
				}
			}
			}

			return false;
		}

		public static string ToDigitsOnly(this string input)
		{
			Regex digitsOnly = new Regex(@"[^\d]");
			return digitsOnly.Replace(input, "");
		}

		public static string ToProperCase(this string unformattedText)
		{
			string formattedText = null;

			if (null != unformattedText)
			{
				formattedText = new CultureInfo("en").TextInfo.
					ToTitleCase(unformattedText.ToLower(
					CultureInfo.CurrentCulture));
			}

			return formattedText;
		}
	}
}