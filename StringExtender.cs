/////////////////////////////////////////////////////////////////////////////
// $Id$
// <copyright file="StringExtender.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utilities.Extensions
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

		public static string Reverse(this string input)
		{
			if (!string.IsNullOrWhiteSpace(input))
			{
				char[] charArray = input.ToCharArray();
				Array.Reverse(charArray);
				input = new string(charArray);
			}

			return input;
		}

		// Convert the string to camel case.
		public static string ToCamelCase(this string input)
		{
			// If there are 0 or 1 characters, just return the string.
			if (input == null || input.Length < 2)
			{
				return input;
			}

			// Split the string into words.
			char[] separators = Array.Empty<char>();
			string[] words =
				input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

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
			return digitsOnly.Replace(input, string.Empty);
		}

		// Convert the string to Pascal case.
		public static string ToPascalCase(this string input)
		{
			// If there are 0 or 1 characters, just return the string.
			if (input == null)
			{
				return input;
			}

			if (input.Length < 2)
			{
				return input.ToUpper(CultureInfo.CurrentCulture);
			}

			// Split the string into words.
			char[] separators = Array.Empty<char>();
			string[] words =
				input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

			// Combine the words.
			string result = string.Empty;
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