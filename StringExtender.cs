/////////////////////////////////////////////////////////////////////////////
// <copyright file="StringExtender.cs" company="James John McGuire">
// Copyright © 2006 - 2022 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utilities.Extensions
{
	/// <summary>
	/// String extender class.
	/// </summary>
	public static class StringExtender
	{
		/// <summary>
		/// Compare multiple strings.
		/// </summary>
		/// <remarks>Not quite sure what the intent of this
		/// message is.</remarks>
		/// <param name="data">The base data to compare.</param>
		/// <param name="compareType">The compare type.</param>
		/// <param name="compareValues">A set of values to compare.</param>
		/// <returns>The result of the compare.</returns>
		public static bool CompareMultiple(
			this string data,
			StringComparison compareType,
			params string[] compareValues)
		{
			bool result = false;

			if (!string.IsNullOrEmpty(data) && compareValues != null)
			{
				foreach (string compare in compareValues)
				{
					if (data.Equals(compare, compareType))
					{
						result = true;
						break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Reverse a string.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The reversed string.</returns>
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

		/// <summary>
		/// Convert the string to camel case.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The camel case string.</returns>
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

		/// <summary>
		/// Converts a string to digits only.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The digits only string.</returns>
		public static string ToDigitsOnly(this string input)
		{
			Regex digitsOnly = new Regex(@"[^\d]");
			return digitsOnly.Replace(input, string.Empty);
		}

		/// <summary>
		/// Convert the string to Pascal case.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The Pascal case string.</returns>
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

		/// <summary>
		/// Convert the string to proper case.
		/// </summary>
		/// <param name="unformattedText">The input string.</param>
		/// <returns>The proper case string.</returns>
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