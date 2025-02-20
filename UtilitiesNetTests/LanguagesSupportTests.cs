using NUnit.Framework;

namespace DigitalZenWorks.Common.Utilities.LanguagesSupportTests
{
	/// <summary>
	/// The languages support tests class.
	/// </summary>
	[TestFixture]
	internal class LanguagesSupportTests
	{
		// Test cases for IsEnglishOnly.
		[Test]
		public void IsEnglishOnlyReturnsTrueForEnglishText()
		{
			bool result = LanguagesSupport.IsEnglishOnly("Hello, world!");
			Assert.That(result, Is.True);
		}

		[Test]
		public void IsEnglishOnlyReturnsFalseForNonEnglishText()
		{
			bool result = LanguagesSupport.IsEnglishOnly("Café");
			Assert.That(result, Is.False);
		}

		[Test]
		public void IsEnglishOnlyReturnsFalseForNullInput()
		{
			bool result = LanguagesSupport.IsEnglishOnly(null);
			Assert.That(result, Is.False);
		}

		[Test]
		public void IsEnglishOnlyReturnsFalseForEmptyString()
		{
			bool result = LanguagesSupport.IsEnglishOnly("");
			Assert.That(result, Is.False);
		}

		// Test cases for IsWesternEuropean
		[Test]
		public void IsWesternEuropeanReturnsTrueForWesternEuropeanText()
		{
			bool result = LanguagesSupport.IsWesternEuropean("Café au lait");
			Assert.That(result, Is.True);
		}

		[Test]
		public void IsWesternEuropeanReturnsTrueForEnglishTextWithPunctuation()
		{
			bool result =
				LanguagesSupport.IsWesternEuropean("123! Hello, world!");
			Assert.That(result, Is.True);
		}

		[Test]
		public void IsWesternEuropeanReturnsFalseForNonWesternEuropeanText()
		{
			bool result = LanguagesSupport.IsWesternEuropean("Привет");
			Assert.That(result, Is.False);
		}

		[Test]
		public void IsWesternEuropeanReturnsFalseForNullInput()
		{
			bool result = LanguagesSupport.IsWesternEuropean(null);
			Assert.That(result, Is.False);
		}

		[Test]
		public void IsWesternEuropeanReturnsFalseForEmptyString()
		{
			bool result = LanguagesSupport.IsWesternEuropean("");
			Assert.That(result, Is.False);
		}

		// Test cases for IsJapaneseOnly
		[Test]
		public void IsJapaneseOnlyReturnsTrueForHiraganaText()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("こんにちは");
			Assert.That(result, Is.True);
		}

		[Test]
		public void IsJapaneseOnlyReturnsTrueForKatakanaText()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("カタカナ");
			Assert.That(result, Is.True);
		}

		[Test]
		public void IsJapaneseOnlyReturnsTrueForKanjiText()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("漢字");
			Assert.That(result, Is.True);
		}

		[Test]
		public void IsJapaneseOnlyReturnsFalseForNonJapaneseText()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("Hello");
			Assert.That(result, Is.False);
		}

		[Test]
		public void IsJapaneseOnlyReturnsFalseForNullInput()
		{
			bool result = LanguagesSupport.IsJapaneseOnly(null);
			Assert.That(result, Is.False);
		}

		[Test]
		public void IsJapaneseOnlyReturnsFalseForEmptyString()
		{
			bool result = LanguagesSupport.IsJapaneseOnly("");
			Assert.That(result, Is.False);
		}
	}
}
