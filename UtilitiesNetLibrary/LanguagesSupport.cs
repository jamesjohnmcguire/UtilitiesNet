/////////////////////////////////////////////////////////////////////////////
// <copyright file="LanguagesSupport.cs" company="James John McGuire">
// Copyright © 2006 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

#nullable enable

namespace DigitalZenWorks.Common.Utilities
{
	using System.Text.RegularExpressions;

	/// <summary>
	/// The LanguagesSupport class support to assist with language-related
	/// operations.
	/// </summary>
	/// <remarks>The LanguagesSupport class provides a collection of static
	/// helper methods designed to assist with language-related operations.
	/// These methods include functionality such as language detection,
	/// translation support, locale validation, and other utility methods for
	/// handling multilingual content. The class is intended to be used as a
	/// utility toolkit for managing language-specific tasks in a consistent
	/// and reusable manner.</remarks>
	public static class LanguagesSupport
	{
		/// <summary>
		/// Determines whether the specified input text contains only English
		/// characters.
		/// </summary>
		/// <param name="input">The text to evaluate. This can be a null or
		/// non-null string.</param>
		/// <returns>
		/// <c>true</c> if the input text contains only English characters
		/// (A-Z, a-z, and common punctuation); otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		/// This method checks for the presence of only English alphabetic
		/// characters (both uppercase and lowercase) and common punctuation
		/// marks. Non-English characters (e.g., accented letters, non-Latin
		/// scripts) will cause the method to return <c>false</c>.
		/// </remarks>
		/// <example>
		/// <code>
		/// // Returns true
		/// bool result = LanguagesSupport.IsEnglishOnly("Hello, world!");
		/// // Returns false
		/// bool result = LanguagesSupport.IsEnglishOnly("Café");
		/// </code>
		/// </example>
		public static bool IsEnglishOnly(string? input)
		{
			// Regular expression to match English letters, numbers, spaces,
			// and common punctuation marks.
			Regex regex = new ("^[a-zA-Z0-9\\s\\p{P}]+$");

			bool isMatch = regex.IsMatch(input ?? string.Empty);

			return isMatch;
		}

		/// <summary>
		/// Determines whether the specified input text contains only Japanese
		/// characters.
		/// </summary>
		/// <param name="input">The text to evaluate. This can be a null or
		/// non-null string.</param>
		/// <returns>
		/// <c>true</c> if the input text contains only Japanese characters
		/// (Hiragana, Katakana, Kanji, and extended Kanji); otherwise,
		/// <c>false</c>. Returns <c>false</c> if the input is null or empty.
		/// </returns>
		/// <remarks>
		/// This method uses a regular expression to check if the input text
		/// consists solely of characters used in the Japanese writing system.
		/// The allowed character ranges include:
		/// <list type="bullet">
		///   <item>Hiragana: \u3040-\u309F</item>
		///   <item>Katakana: \u30A0-\u30FF</item>
		///   <item>Kanji (common): \u4E00-\u9FAF</item>
		///   <item>Kanji (extended): \u3400-\u4DBF</item>
		/// </list>
		/// Any non-Japanese characters (e.g., Latin letters, Cyrillic, or
		/// other scripts) will cause the method to return <c>false</c>.
		/// </remarks>
		/// <example>
		/// <code>
		/// // Returns true (Hiragana)
		/// bool result = LanguagesSupport.IsJapaneseOnly("こんにちは");
		/// // Returns true (Katakana)
		/// bool result = LanguagesSupport.IsJapaneseOnly("カタカナ");
		/// // Returns true (Kanji)
		/// bool result = LanguagesSupport.IsJapaneseOnly("漢字");
		/// // Returns false
		/// bool result = LanguagesSupport.IsJapaneseOnly("Hello");
		/// // Returns false
		/// bool result = LanguagesSupport.IsJapaneseOnly(null);
		/// </code>
		/// </example>
		public static bool IsJapaneseOnly(string? input)
		{
			// Regular expression to match Hiragana, Katakana, and Kanji.
			Regex regex = new (
				"^[\u3040-\u309F\u30A0-\u30FF\u4E00-\u9FAF\u3400-\u4DBF]+$");

			bool isMatch = regex.IsMatch(input ?? string.Empty);

			return isMatch;
		}

		/// <summary>
		/// Determines whether the specified input text contains only
		/// characters commonly used in Western European languages.
		/// </summary>
		/// <param name="input">The text to evaluate. This can be a null or
		/// non-null string.</param>
		/// <returns>
		/// <c>true</c> if the input text contains only characters from the
		/// Western European character set (including English letters,
		/// accented characters, numbers, spaces, and common punctuation
		/// marks); otherwise, <c>false</c>. Returns <c>false</c> if the
		/// input is null or empty.
		/// </returns>
		/// <remarks>
		/// This method uses a regular expression to check if the input text
		/// consists solely of characters commonly used in Western European
		/// languages. The allowed characters include:
		/// <list type="bullet">
		///   <item>English letters (A-Z, a-z)</item>
		///   <item>Accented characters (À-ÿ)</item>
		///   <item>Numbers (0-9)</item>
		///   <item>Spaces</item>
		///   <item>Common punctuation marks (e.g., periods, commas,
		///   exclamation marks, etc.)</item>
		/// </list>
		/// Non-Western European characters (e.g., Cyrillic, Arabic, or
		/// Asian scripts) will cause the method to return <c>false</c>.
		/// </remarks>
		/// <example>
		/// <code>
		/// // Returns true
		/// bool result = LanguagesSupport.IsWesternEuropean("Café au lait");
		/// // Returns false
		/// bool result = LanguagesSupport.IsWesternEuropean("Привет");
		/// // Returns false
		/// bool result = LanguagesSupport.IsWesternEuropean(null);
		/// </code>
		/// </example>
		public static bool IsWesternEuropean(string? input)
		{
			// Regular expression to match English letters, Western European
			// characters, numbers, spaces, and common punctuation marks.
			Regex regex = new ("^[a-zA-ZÀ-ÿ0-9\\s\\p{P}]+$");

			bool isMatch = regex.IsMatch(input ?? string.Empty);

			return isMatch;
		}
	}
}
