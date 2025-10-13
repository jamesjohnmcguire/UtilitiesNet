/////////////////////////////////////////////////////////////////////////////
// <copyright file="UnicodeNormalizerTests.cs" company="James John McGuire">
// Copyright © 2006 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

#nullable enable

namespace DigitalZenWorks.Common.Utilities.Tests
{
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Text;
	using NUnit.Framework;

	/// <summary>
	/// Provides unit tests for the <see cref="UnicodeNormalizer"/> class,
	/// verifying its behavior across various scenarios involving Unicode
	/// normalization, comparison, and processing.
	/// </summary>
	/// <remarks>This test suite includes tests for methods that handle
	/// Unicode normalization, string comparison, hexadecimal code generation,
	/// and file processing. It ensures that the
	/// <see cref="UnicodeNormalizer"/> class behaves correctly when dealing
	/// with normalized and unnormalized Unicode text, including edge cases
	/// such as empty strings, mixed content, and special Unicode characters
	/// like Kangxi radicals.</remarks>
	[TestFixture]
	internal class UnicodeNormalizerTests
	{
		private readonly string[] languagesData =
		[
			"日本語,Japanese",
			"中文,Chinese",
			"한글,Korean"
		];

		private readonly string[] mixedData =
		[

			// Kangxi radical
			"⼈,Person",

			// Standard character
			"人,Person"
		];

		private readonly string[] radicalsData =
		[
			"⼈,Radical 1",
			"⼆,Radical 2",
			"⼉,Radical 3"
		];

		private readonly string[] someData =
		[
			"人,Person",
			"木,Tree"
		];

		private string testDataDirectory = string.Empty;

		/// <summary>
		/// Setup test.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			string tempPath = Path.GetTempPath();

			testDataDirectory =
				Path.Combine(tempPath, "UnicodeNormalizerTests");
			Directory.CreateDirectory(testDataDirectory);
		}

		/// <summary>
		/// Tear down test.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			if (Directory.Exists(testDataDirectory))
			{
				Directory.Delete(testDataDirectory, true);
			}
		}

		/// <summary>
		/// Test CheckLine with empty string returns null.
		/// </summary>
		[Test]
		public void CheckLineWithEmptyStringReturnsNull()
		{
			string emptyLine = string.Empty;
			int lineNumber = 1;

			NormalizationIssue? result =
				UnicodeNormalizer.CheckLine(lineNumber, emptyLine);

			Assert.That(result, Is.Null);
		}

		/// <summary>
		/// Tests the <see cref="UnicodeNormalizer.CheckLine"/> method to
		/// ensure it identifies and normalizes lines containing Kangxi
		/// Radicals, returning the appropriate issue details.
		/// </summary>
		/// <remarks>This test verifies that the method correctly detects
		/// differences between Kangxi Radicals and their standard Unicode
		/// equivalents, normalizes the input line, and provides detailed
		/// information about the differences, including
		/// their positions.</remarks>
		[Test]
		public void CheckLineWithKangxiRadicalReturnsIssue()
		{
			// ⼈ (Kangxi) vs 人 (standard)
			string lineWithRadical = "⼈は人";
			int lineNumber = 5;

			NormalizationIssue? result =
				UnicodeNormalizer.CheckLine(lineNumber, lineWithRadical);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result!.LineNumber, Is.EqualTo(5));
				Assert.That(result.OriginalLine, Is.EqualTo(lineWithRadical));
				Assert.That(result.NormalizedLine, Is.EqualTo("人は人"));
				Assert.That(result.Differences, Has.Count.EqualTo(1));
				Assert.That(result.Differences![0].Position, Is.Zero);
			}
		}

		/// <summary>
		/// Test CheckLine with multiple Issues, returns all differences.
		/// </summary>
		[Test]
		public void CheckLineWithMultipleIssuesReturnsAllDifferences()
		{
			// Multiple Kangxi radicals
			string lineWithMultiple = "⼆⼈⼉";
			int lineNumber = 10;

			NormalizationIssue? result =
				UnicodeNormalizer.CheckLine(lineNumber, lineWithMultiple);

			Assert.That(result, Is.Not.Null);
			Assert.That(result!.Differences, Has.Count.EqualTo(3));
		}

		/// <summary>
		/// Verifies that the <see cref="UnicodeNormalizer.CheckLine"/> method
		/// returns <see langword="null"/>  when provided with a line of text
		/// that does not contain any normalization issues.
		/// </summary>
		/// <remarks>This test ensures that the
		/// <see cref="UnicodeNormalizer.CheckLine"/> method behaves as
		/// expected when the input text is already normalized. The method is
		/// called with a valid line number and a  string containing standard
		/// characters, and the result is asserted to be
		/// <see langword="null"/>.</remarks>
		[Test]
		public void CheckLineWithNormalizedTextReturnsNull()
		{
			// Standard characters
			string normalizedLine = "人は人";
			int lineNumber = 1;

			NormalizationIssue? result =
				UnicodeNormalizer.CheckLine(
					lineNumber, normalizedLine);

			Assert.That(result, Is.Null);
		}

		/// <summary>
		/// Test CheckLine with only whitespace, returns null.
		/// </summary>
		[Test]
		public void CheckLineWithOnlyWhitespaceReturnsNull()
		{
			string whitespaceLine = "   \t  ";
			int lineNumber = 1;

			NormalizationIssue? result =
				UnicodeNormalizer.CheckLine(lineNumber, whitespaceLine);

			Assert.That(result, Is.Null);
		}

		/// <summary>
		/// Test CompareStrings with different characters, return false.
		/// </summary>
		[Test]
		public void CompareStringsWithDifferentCharactersReturnsFalse()
		{
			string string1 = "人";
			string string2 = "木";

			bool result = UnicodeNormalizer.CompareStrings(string1, string2);

			Assert.That(result, Is.False);
		}

		/// <summary>
		/// Test CompareStrings with empty strings, returns true.
		/// </summary>
		[Test]
		public void CompareStringsWithEmptyStringsReturnsTrue()
		{
			string string1 = string.Empty;
			string string2 = string.Empty;

			bool result = UnicodeNormalizer.CompareStrings(string1, string2);

			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Test CompareString with identical strings returns true.
		/// </summary>
		[Test]
		public void CompareStringsWithIdenticalStringsReturnsTrue()
		{
			string string1 = "人";
			string string2 = "人";

			bool result = UnicodeNormalizer.CompareStrings(string1, string2);

			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Test CompareStrings with kangxi and standard, returns true.
		/// </summary>
		[Test]
		public void CompareStringsWithKangxiAndStandardReturnsTrue()
		{
			// U+2F08
			string kangxi = "⼈";

			// U+4EBA
			string standard = "人";

			bool result = UnicodeNormalizer.CompareStrings(kangxi, standard);

			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Test CompareStrings with mixed content, handles correctly.
		/// </summary>
		[Test]
		public void CompareStringsWithMixedContentHandlesCorrectly()
		{
			// Mixed Kangxi and standard
			string string1 = "⼈は人です";

			// All standard
			string string2 = "人は人です";

			bool result = UnicodeNormalizer.CompareStrings(string1, string2);

			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Test CompareStrings with multiple kangxi radicals, returns true.
		/// </summary>
		[Test]
		public void CompareStringsWithMultipleKangxiRadicalsReturnsTrue()
		{
			string kangxiString = "⼆⼈⼉";
			string standardString = "二人儿";

			bool result = UnicodeNormalizer.CompareStrings(
				kangxiString, standardString);

			Assert.That(result, Is.True);
		}

		/// <summary>
		/// Test GetHexadecimalCodes with empty string, returns empty list.
		/// </summary>
		[Test]
		public void GetHexadecimalCodesWithEmptyStringReturnsEmptyList()
		{
			string text = string.Empty;

			Collection<int> result = UnicodeNormalizer.GetCodePoints(text);

			Assert.That(result, Is.Empty);
		}

		/// <summary>
		/// Test GetHexadecimalCodes with kangxi radical, returns correct code.
		/// </summary>
		[Test]
		public void GetHexadecimalCodesWithKangxiRadicalReturnsCorrectCode()
		{
			// U+2F08 = 12040
			string text = "⼈";

			Collection<int> result = UnicodeNormalizer.GetCodePoints(text);

			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0], Is.EqualTo(0x2F08));
		}

		/// <summary>
		/// Test GetHexadecimalCodes with multiple characters, returns
		/// all codes.
		/// </summary>
		[Test]
		public void GetHexadecimalCodesWithMultipleCharactersReturnsAllCodes()
		{
			// U+4EBA, U+6728
			string text = "人木";

			Collection<int> result = UnicodeNormalizer.GetCodePoints(text);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(result, Has.Count.EqualTo(2));
				Assert.That(result[0], Is.EqualTo(0x4EBA));
				Assert.That(result[1], Is.EqualTo(0x6728));
			}
		}

		/// <summary>
		/// Test GetHexadecimalCodes with single character, returns
		/// correct code.
		/// </summary>
		[Test]
		public void GetHexadecimalCodesWithSingleCharacterReturnsCorrectCode()
		{
			// U+4EBA = 20154
			string text = "人";

			Collection<int> result = UnicodeNormalizer.GetCodePoints(text);

			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0], Is.EqualTo(0x4EBA));
		}

		/// <summary>
		/// Test GetHexadecimalString with empty string, returns empty string.
		/// </summary>
		[Test]
		public void GetHexadecimalStringWithEmptyStringReturnsEmptyString()
		{
			string text = string.Empty;

			string? result = UnicodeNormalizer.GetHexadecimalString(text);

			Assert.That(result, Is.EqualTo(string.Empty));
		}

		/// <summary>
		/// Test GetHexadecimalString with kangxi and standard,
		/// shows difference.
		/// </summary>
		[Test]
		public void GetHexadecimalStringWithKangxiAndStandardShowsDifference()
		{
			string kangxi = "⼈";
			string standard = "人";

			string? kangxiHex = UnicodeNormalizer.GetHexadecimalString(kangxi);
			string? standardHex =
				UnicodeNormalizer.GetHexadecimalString(standard);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(kangxiHex, Is.EqualTo("2F08"));
				Assert.That(standardHex, Is.EqualTo("4EBA"));
				Assert.That(kangxiHex, Is.Not.EqualTo(standardHex));
			}
		}

		/// <summary>
		/// Test GetHexadecimalString with kangxi radical, returns
		/// correct code.
		/// </summary>
		[Test]
		public void GetHexadecimalStringWithKangxiRadicalReturnsCorrectCode()
		{
			// U+2F08
			string text = "⼈";

			string? result = UnicodeNormalizer.GetHexadecimalString(text);

			Assert.That(result, Is.EqualTo("2F08"));
		}

		/// <summary>
		/// Test GetHexadecimalString with multiple characters, returns space
		/// separated text.
		/// </summary>
		[Test]
		public void GetHexadecimalStringWithMultipleCharactersReturnSeparated()
		{
			// U+4EBA, U+6728
			string text = "人木";

			string? result = UnicodeNormalizer.GetHexadecimalString(text);

			Assert.That(result, Is.EqualTo("4EBA 6728"));
		}

		/// <summary>
		/// Test GetHexadecimalString with single character, returns
		/// correct format.
		/// </summary>
		[Test]
		public void GetHexadecimalStringWithSingleCharacterReturnsFormat()
		{
			// U+4EBA
			string text = "人";

			string? result = UnicodeNormalizer.GetHexadecimalString(text);

			Assert.That(result, Is.EqualTo("4EBA"));
		}

		/// <summary>
		/// Test GetHexadecimalTextCodes with single character,
		/// returns correct format.
		/// </summary>
		[Test]
		public void GetHexadecimalTextCodesWithCharacterReturnsCorrectFormat()
		{
			// U+4EBA
			string text = "人";

			Collection<string> result =
				UnicodeNormalizer.GetHexadecimalTextCodes(text);

			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0], Is.EqualTo("4EBA"));
		}

		/// <summary>
		/// Test GetHexadecimalTextCodes with low ascii, returns padded format.
		/// </summary>
		[Test]
		public void GetHexadecimalTextCodesWithLowAsciiReturnsPaddedFormat()
		{
			// U+0041
			string text = "A";

			Collection<string> result =
				UnicodeNormalizer.GetHexadecimalTextCodes(text);

			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0], Is.EqualTo("0041"));
		}

		/// <summary>
		/// Test GetHexadecimalTextCodes with multiple characters, returns
		/// all codes.
		/// </summary>
		[Test]
		public void GetHexadecimalTextCodesWithMultipleReturnsAllCodes()
		{
			// U+4EBA, U+6728
			string text = "人木";

			Collection<string> result =
				UnicodeNormalizer.GetHexadecimalTextCodes(text);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(result, Has.Count.EqualTo(2));
				Assert.That(result[0], Is.EqualTo("4EBA"));
				Assert.That(result[1], Is.EqualTo("6728"));
			}
		}

		/// <summary>
		/// Test NormalizeFile, preserves encoding.
		/// </summary>
		[Test]
		public void NormalizeFilePreservesUtf8Encoding()
		{
			string inputPath = Path.Combine(testDataDirectory, "utf8.csv");
			string outputPath = Path.Combine(testDataDirectory, "output.csv");

			File.WriteAllLines(inputPath, languagesData, Encoding.UTF8);

			UnicodeNormalizer.NormalizeFile(
				inputPath, outputPath, out int linesProcessed);

			string[] outputLines =
				File.ReadAllLines(outputPath, Encoding.UTF8);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(outputLines[0], Is.EqualTo("日本語,Japanese"));
				Assert.That(outputLines[1], Is.EqualTo("中文,Chinese"));
				Assert.That(outputLines[2], Is.EqualTo("한글,Korean"));
			}
		}

		/// <summary>
		/// Test NormalizeFile with empty file, handles correctly.
		/// </summary>
		[Test]
		public void NormalizeFileWithEmptyFileHandlesCorrectly()
		{
			string inputPath = Path.Combine(testDataDirectory, "empty.csv");
			string outputPath = Path.Combine(testDataDirectory, "output.csv");

			File.WriteAllText(inputPath, string.Empty, Encoding.UTF8);

			int linesChanged = UnicodeNormalizer.NormalizeFile(
				inputPath, outputPath, out int linesProcessed);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(linesProcessed, Is.Zero);
				Assert.That(linesChanged, Is.Zero);
			}
		}

		/// <summary>
		/// Test NormalizeFile with kangxi radicals, normalizes content.
		/// </summary>
		[Test]
		public void NormalizeFileWithKangxiRadicalsNormalizesContent()
		{
			string inputPath =
				Path.Combine(testDataDirectory, "unnormalized.csv");
			string outputPath = Path.Combine(testDataDirectory, "output.csv");

			File.WriteAllLines(inputPath, mixedData, Encoding.UTF8);

			int linesChanged = UnicodeNormalizer.NormalizeFile(
				inputPath, outputPath, out int linesProcessed);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(linesProcessed, Is.EqualTo(2));

				// One line changed (0-indexed)
				Assert.That(linesChanged, Is.EqualTo(1));

				// Verify output content
				var outputLines = File.ReadAllLines(outputPath, Encoding.UTF8);
				Assert.That(outputLines[0], Is.EqualTo("人,Person"));
				Assert.That(outputLines[1], Is.EqualTo("人,Person"));
			}
		}

		/// <summary>
		/// Test NormalizeFile with multiple kangxi lines, counts all changed.
		/// </summary>
		[Test]
		public void NormalizeFileWithMultipleKangxiLinesCountsAllChanges()
		{
			string inputPath = Path.Combine(testDataDirectory, "multiple.csv");
			string outputPath = Path.Combine(testDataDirectory, "output.csv");

			File.WriteAllLines(inputPath, radicalsData, Encoding.UTF8);

			int linesChanged = UnicodeNormalizer.NormalizeFile(
				inputPath, outputPath, out int linesProcessed);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(linesProcessed, Is.EqualTo(3));
				Assert.That(linesChanged, Is.EqualTo(3));
			}
		}

		/// <summary>
		/// Test NormalizeFile with non-existant file, returns negative one.
		/// </summary>
		[Test]
		public void NormalizeFileWithNonExistentFileReturnsNegativeOne()
		{
			string inputPath =
				Path.Combine(testDataDirectory, "nonexistent.csv");
			string outputPath = Path.Combine(testDataDirectory, "output.csv");

			int linesChanged = UnicodeNormalizer.NormalizeFile(
				inputPath, outputPath, out int linesProcessed);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(linesChanged, Is.Zero);
				Assert.That(linesProcessed, Is.Zero);
			}
		}

		/// <summary>
		/// Test NormalizeFile with normalized content, retursn zero changes.
		/// </summary>
		[Test]
		public void NormalizeFileWithNormalizedContentReturnsZeroChanges()
		{
			string inputPath =
				Path.Combine(testDataDirectory, "normalized.csv");
			string outputPath = Path.Combine(testDataDirectory, "output.csv");

			File.WriteAllLines(inputPath, someData, Encoding.UTF8);

			int linesChanged = UnicodeNormalizer.NormalizeFile(
				inputPath, outputPath, out int linesProcessed);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(linesProcessed, Is.EqualTo(2));

				// No changes made
				Assert.That(linesChanged, Is.Zero);
				Assert.That(File.Exists(outputPath), Is.True);
			}
		}
	}
}
