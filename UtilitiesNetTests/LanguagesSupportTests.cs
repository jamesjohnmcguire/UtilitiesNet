/////////////////////////////////////////////////////////////////////////////
// <copyright file="LanguagesSupportTests.cs" company="James John McGuire">
// Copyright © 2006 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities.Tests
{
	using NUnit.Framework;

	/// <summary>
	/// The languages support tests class.
	/// </summary>
	[TestFixture]
	internal sealed class LanguagesSupportTests
	{
		/// <summary>
		/// Is english only returns true for english text.
		/// </summary>
		[Test]
		public void IsEnglishOnlyReturnsTrueForEnglishText()
		{
			bool result = LanguagesSupport.IsEnglishOnly("Hello, world!");
			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Is english only returns false for non-english text.
		/// </summary>
		[Test]
		public void IsEnglishOnlyReturnsFalseForNonEnglishText()
		{
			bool result = LanguagesSupport.IsEnglishOnly("Café");
			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Is english only returns false for null input.
		/// </summary>
		[Test]
		public void IsEnglishOnlyReturnsFalseForNullInput()
		{
			bool result = LanguagesSupport.IsEnglishOnly(null);
			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Is english only returns false for empty string.
		/// </summary>
		[Test]
		public void IsEnglishOnlyReturnsFalseForEmptyString()
		{
			bool result = LanguagesSupport.IsEnglishOnly(string.Empty);
			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Is western european returns true for western european text.
		/// </summary>
		[Test]
		public void IsWesternEuropeanReturnsTrueForWesternEuropeanText()
		{
			bool result = LanguagesSupport.IsWesternEuropean("Café au lait");
			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Is western european returns true for english text with punctuation.
		/// </summary>
		[Test]
		public void IsWesternEuropeanReturnsTrueForEnglishTextWithPunctuation()
		{
			bool result =
				LanguagesSupport.IsWesternEuropean("123! Hello, world!");
			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Is western european returns false for non-western european text.
		/// </summary>
		[Test]
		public void IsWesternEuropeanReturnsFalseForNonWesternEuropeanText()
		{
			bool result = LanguagesSupport.IsWesternEuropean("Привет");
			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Is western european returns false for null input.
		/// </summary>
		[Test]
		public void IsWesternEuropeanReturnsFalseForNullInput()
		{
			bool result = LanguagesSupport.IsWesternEuropean(null);
			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Is western european returns false for empty string.
		/// </summary>
		[Test]
		public void IsWesternEuropeanReturnsFalseForEmptyString()
		{
			bool result = LanguagesSupport.IsWesternEuropean(string.Empty);
			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Is japanese only returns true for hiragana text.
		/// </summary>
		[Test]
		public void IsJapaneseOnlyReturnsTrueForHiraganaText()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("こんにちは");
			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Is japanese only returns true for katakana text.
		/// </summary>
		[Test]
		public void IsJapaneseOnlyReturnsTrueForKatakanaText()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("カタカナ");
			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Is japanese only returns true for kanji text.
		/// </summary>
		[Test]
		public void IsJapaneseOnlyReturnsTrueForKanjiText()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("漢字");
			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Is japanese only returns false for non-japanese text.
		/// </summary>
		[Test]
		public void IsJapaneseOnlyReturnsFalseForNonJapaneseText()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("Hello");
			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Is japanese only returns false for null input.
		/// </summary>
		[Test]
		public void IsJapaneseOnlyReturnsFalseForNullInput()
		{
			bool result = LanguagesSupport.IsJapaneseOnly(null);
			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Is japanese only returns false for empty string.
		/// </summary>
		[Test]
		public void IsJapaneseOnlyReturnsFalseForEmptyString()
		{
			bool result = LanguagesSupport.IsJapaneseOnly(string.Empty);
			Assert.That(result, Is.False);
		}
	}
}
