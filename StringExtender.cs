﻿/////////////////////////////////////////////////////////////////////////////
// $Id:$
//
// Copyright (c) 2006-2012 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Text.RegularExpressions;

namespace Zenware.Common.UtilsNet
{
	public static class StringExtender
	{
		public static bool CompareMultiple(
			this string data,
			StringComparison compareType,
			params string[] compareValues)
		{
			foreach (string s in compareValues)
			{
				if (data.Equals(s, compareType))
				{
					return true;
				}
			}

			return false;
		}

		public static string ToDigitsOnly(this string input)
		{
			Regex digitsOnly = new Regex(@"[^\d]");
			return digitsOnly.Replace(input, "");
		}

		public static string ToProperCase(
			string UnformattedString)
		{
			string formattedText = null;

			if (null != UnformattedString)
			{
				formattedText = new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(UnformattedString.ToLower());
			}

			return formattedText;
		}
	}
}