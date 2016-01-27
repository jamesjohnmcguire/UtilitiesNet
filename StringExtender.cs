/////////////////////////////////////////////////////////////////////////////
// $Id$
//
// Copyright © 2011-2016 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utils.Extensions
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

		// Convert the string to camel case.
		public static string ToCamelCase(this string input)
		{
			// If there are 0 or 1 characters, just return the string.
			if (input == null || input.Length < 2)
				return input;

			// Split the string into words.
			string[] words = input.Split(
				new char[] { },
				StringSplitOptions.RemoveEmptyEntries);

			// Combine the words.
			string result = words[0].Substring(0, 1).ToLower(
				CultureInfo.CurrentCulture) + words[0].Substring(1);

			for (int i = 1; i < words.Length; i++)
			{
				result +=
					words[i].Substring(0, 1).ToUpper(
					CultureInfo.CurrentCulture) + words[i].Substring(1);
			}

			return result;
		}

		public static string ToDigitsOnly(this string input)
		{
			Regex digitsOnly = new Regex(@"[^\d]");
			return digitsOnly.Replace(input, "");
		}

		// Convert the string to Pascal case.
		public static string ToPascalCase(this string input)
		{
			// If there are 0 or 1 characters, just return the string.
			if (input == null) return input;
			if (input.Length < 2)
			{
				return input.ToUpper(CultureInfo.CurrentCulture);
			}

			// Split the string into words.
			string[] words = input.Split(
				new char[] { },
				StringSplitOptions.RemoveEmptyEntries);

			// Combine the words.
			string result = "";
			foreach (string word in words)
			{
				result +=
					word.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) +
					word.Substring(1);
			}

			return result;
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