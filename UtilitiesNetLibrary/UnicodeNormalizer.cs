/////////////////////////////////////////////////////////////////////////////
// <copyright file="UnicodeNormalizer.cs" company="James John McGuire">
// Copyright © 2006 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

#nullable enable

namespace DigitalZenWorks.Common.Utilities
{
	using System;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using System.IO;
	using System.Text;

	/// <summary>
	/// Provides utility methods for working with Unicode text, including
	/// normalization, comparison, and analysis of character differences.
	/// This class is designed to assist with ensuring text conforms to
	/// specific Unicode normalization forms and to facilitate operations on
	/// Unicode strings.
	/// </summary>
	/// <remarks>The <see cref="UnicodeNormalizer"/> class includes methods
	/// for checking and applying Unicode normalization, comparing strings in
	/// a normalization-insensitive manner, extracting Unicode code points,
	/// and converting text to hexadecimal representations. It also provides
	/// functionality for normalizing entire files while tracking changes.
	/// <para>All methods in this class are static and can be used without
	/// instantiating the class.  The normalization form  can be specified
	/// where applicable.</para>
	/// <para> This class is thread-safe as it does not maintain any
	/// internal state.</para></remarks>
	public static class UnicodeNormalizer
	{
		/// <summary>
		/// Checks if a line needs Unicode normalization.
		/// </summary>
		/// <param name="lineNumber">The line number for reference.</param>
		/// <param name="line">The text to check.</param>
		/// <param name="form">The normalization form to use.</param>
		/// <returns>A NormalizationIssue if the line needs normalization,
		/// otherwise null.</returns>
		public static NormalizationIssue? CheckLine(
			int lineNumber,
			string line,
			NormalizationForm form)
		{
			NormalizationIssue? issue = null;

			ArgumentNullException.ThrowIfNull(line);

			bool isNormalized = line.IsNormalized(form);

			if (isNormalized == false)
			{
				string normalized = line.Normalize(form);
				Collection<CharDifference> differences =
					FindDifferences(line, normalized);

				issue = new ();

				issue.LineNumber = lineNumber;
				issue.OriginalLine = line;
				issue.NormalizedLine = normalized;
				issue.Differences = differences;
			}

			return issue;
		}

		/// <summary>
		/// Compares two strings for equality, taking into account
		/// normalization and null values.
		/// </summary>
		/// <remarks>The comparison uses normalization with
		/// <see cref="NormalizationForm.FormKC"/> to ensure that equivalent
		/// strings with different Unicode representations are treated as
		/// equal. The comparison is case-sensitive and
		/// culture-invariant.</remarks>
		/// <param name="string1">The first string to compare.</param>
		/// <param name="string2">The second string to compare.</param>
		/// <param name="form">The normalization form to use.</param>
		/// <returns><see langword="true"/> if the strings are equal,
		/// including when both are <see langword="null"/>; otherwise, <see
		/// langword="false"/>.</returns>
		public static bool CompareStrings(
			string string1,
			string string2,
			NormalizationForm form)
		{
			bool isEqual = false;

			ArgumentNullException.ThrowIfNull(string1);
			ArgumentNullException.ThrowIfNull(string2);

			string? normalizedString1 = string1!.Normalize(form);
			string? normalizedString2 = string2!.Normalize(form);

			if (normalizedString1.Equals(
				normalizedString2, StringComparison.Ordinal))
			{
				isEqual = true;
			}

			return isEqual;
		}

		/// <summary>
		/// Converts the characters in the specified string to their
		/// corresponding Unicode code points.
		/// </summary>
		/// <remarks>This method iterates through each character in the input
		/// string and converts it to its Unicode code point value.</remarks>
		/// <param name="text">The input string to process.</param>
		/// <returns>A collection of integers representing the Unicode code
		/// points of the characters in the input string. The collection will
		/// be empty if the input string is empty.</returns>
		public static Collection<int> GetCodePoints(string text)
		{
			Collection<int> codes = [];

			ArgumentNullException.ThrowIfNull(text);

			foreach (char character in text)
			{
				int bits = (int)character;
				codes.Add(bits);
			}

			return codes;
		}

		/// <summary>
		/// Converts the specified text into a hexadecimal string
		/// representation.
		/// </summary>
		/// <remarks>This method processes the input text and generates a
		/// space-separated string of hexadecimal codes representing the
		/// characters in the text. If the input text is empty, the method
		/// returns an empty string.</remarks>
		/// <param name="text">The input text to be converted. Cannot be
		/// <see langword="null"/>.</param>
		/// <returns>A string containing the hexadecimal representation of the
		/// input text, with each hexadecimal code separated by a
		/// space. Returns <see langword="null"/> if the input text is
		/// <see langword="null"/>.</returns>
		public static string? GetHexadecimalString(string text)
		{
			ArgumentNullException.ThrowIfNull(text);

			Collection<string> hexadecimalTextCodes =
				GetHexadecimalTextCodes(text);

			string? hexadecimalString = string.Join(" ", hexadecimalTextCodes);

			return hexadecimalString;
		}

		/// <summary>
		/// Converts each character in the specified text to its corresponding
		/// hexadecimal Unicode representation.
		/// </summary>
		/// <remarks>Each character in the input text is converted to a
		/// 4-digit uppercase hexadecimal string, representing its Unicode
		/// code point. For example, the character 'A' is converted
		/// to "0041".</remarks>
		/// <param name="text">The input text to be converted. If
		/// <paramref name="text"/> is <see langword="null"/>, an empty
		/// collection is returned.</param>
		/// <returns>A collection of strings where each string represents the
		/// hexadecimal Unicode value of a character in the
		/// input text.</returns>
		public static Collection<string> GetHexadecimalTextCodes(string text)
		{
			Collection<string> codes = [];

			ArgumentNullException.ThrowIfNull(text);

			foreach (char character in text)
			{
				int bits = (int)character;
				string bitsText = $"{bits:X4}";

				codes.Add(bitsText);
			}

			return codes;
		}

		/// <summary>
		/// Normalizes the text content of a file by applying Unicode
		/// normalization and writes the result to a new file.
		/// </summary>
		/// <remarks>This method reads the content of the file specified by
		/// <paramref name="inputPath"/>, applies Unicode normalization using
		/// the <see cref="NormalizationForm.FormKC"/> form, and writes the
		/// normalized content to the file specified by
		/// <paramref name="outputPath"/>. The method also tracks the number
		/// of lines processed and the number of lines that were modified
		/// during normalization.</remarks>
		/// <param name="inputPath">The path to the input file to be
		/// normalized. Cannot be null, empty, or whitespace.</param>
		/// <param name="outputPath">The path to the output file where the
		/// normalized content will be written. Cannot be null, empty,
		/// or whitespace.</param>
		/// <param name="linesProcessed">When this method returns, contains
		/// the total number of lines processed from the input file. This
		/// parameter is passed uninitialized.</param>
		/// <param name="form">The normalization form to use.</param>
		/// <returns>The number of lines that were changed during
		/// normalization.</returns>
		/// <exception cref="ArgumentException">Thrown if
		/// <paramref name="inputPath"/> or <paramref name="outputPath"/> is
		/// null, empty, or consists only of whitespace.</exception>
		/// <exception cref="FileNotFoundException">Thrown if the file
		/// specified by <paramref name="inputPath"/> does
		/// not exist.</exception>
		public static int NormalizeFile(
			string inputPath,
			string outputPath,
			out int linesProcessed,
			NormalizationForm form)
		{
			int linesChanged;
			linesProcessed = 0;
			string message;
			string name;

			if (string.IsNullOrWhiteSpace(inputPath))
			{
				message = "Input path cannot be null or empty";
				name = nameof(inputPath);
				throw new ArgumentException(message, name);
			}

			if (string.IsNullOrWhiteSpace(outputPath))
			{
				message = "Output path cannot be null or empty";
				name = nameof(outputPath);
				throw new ArgumentException(message, name);
			}

			if (!File.Exists(inputPath))
			{
				message = "Input file not found";
				throw new FileNotFoundException(message, inputPath);
			}
			else
			{
				linesChanged = 0;

				using StreamReader reader = new(inputPath, Encoding.UTF8);
				using StreamWriter writer =
					new(outputPath, false, Encoding.UTF8);

				string? line = reader.ReadLine();

				while (line != null)
				{
					string normalized = line.Normalize(form);

					writer.WriteLine(normalized);
					linesProcessed++;

					if (line != normalized)
					{
						linesChanged++;
					}

					line = reader.ReadLine();
				}
			}

			return linesChanged;
		}

		private static CharDifference? CheckDifferences(
			int index, char original, char normalized)
		{
			CharDifference? difference = null;

			if (original != normalized)
			{
				string originalString = original.ToString();
				string normalizedString = normalized.ToString();

				difference = new ();

				index++;
				difference.Position = index;
				difference.Original = originalString;
				difference.Normalized = normalizedString;
			}

			return difference;
		}

		private static Collection<CharDifference> FindDifferences(
			string original, string normalized)
		{
			Collection<CharDifference> differences = new ();

			TextElementEnumerator originalElements =
				StringInfo.GetTextElementEnumerator(original);
			TextElementEnumerator normalizedElements =
				StringInfo.GetTextElementEnumerator(normalized);

			int position = 0;

			while (originalElements.MoveNext() &&
				normalizedElements.MoveNext())
			{
				string originalElement = originalElements.GetTextElement();
				string normalizedElement = normalizedElements.GetTextElement();

				if (originalElement != normalizedElement)
				{
					CharDifference difference = new ()
					{
						Position = position,
						Original = originalElement,
						Normalized = normalizedElement
					};

					differences.Add(difference);
				}

				position++;
			}

			return differences;
		}
	}
}
