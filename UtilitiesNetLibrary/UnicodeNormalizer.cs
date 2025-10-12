/////////////////////////////////////////////////////////////////////////////
// <copyright file="UnicodeNormalizer.cs" company="James John McGuire">
// Copyright © 2006 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

#nullable enable

namespace DigitalZenWorks.Common.Utilities
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;

	public class UnicodeNormalizer
	{
		/// <summary>
		/// Checks if a line needs Unicode normalization to NFKC form.
		/// </summary>
		/// <param name="lineNumber">The line number for reference.</param>
		/// <param name="line">The text to check.</param>
		/// <param name="form">The normalization form to use.</param>
		/// <returns>A NormalizationIssue if the line needs normalization,
		/// otherwise null.</returns>
		public static NormalizationIssue? CheckLine(
			int lineNumber,
			string line,
			NormalizationForm form = NormalizationForm.FormKC)
		{
			NormalizationIssue? issue = null;

			if (!line.IsNormalized(form))
			{
				string normalized = line.Normalize(form);
				var differences = FindDifferences(line, normalized);

				issue = new();

				issue.LineNumber = lineNumber;
				issue.OriginalLine = line;
				issue.NormalizedLine = normalized;
				issue.Differences = differences;
			}

			return issue;
		}

		public static bool CompareStrings(string? string1, string? string2)
		{
			bool isEqual = false;

			if (string1 == null || string2 == null)
			{
				if (string1 == string2)
				{
					isEqual = true;
				}
			}
			else
			{
				string? normalizedString1 =
					string1!.Normalize(NormalizationForm.FormKC);
				string? normalizedString2 =
					string2!.Normalize(NormalizationForm.FormKC);

				if (normalizedString1.Equals(
					normalizedString2, StringComparison.Ordinal))
				{
					isEqual = true;
				}
			}

			return isEqual;
		}

		public static List<int> GetCodePoints(string text)
		{
			List<int> codes = [];

			foreach (char character in text)
			{
				int bits = (int)character;
				codes.Add(bits);
			}

			return codes;
		}

		public static string? GetHexadecimalString(string text)
		{
			List<string> hexadecimalTextCodes = GetHexadecimalTextCodes(text);

			string? hexadecimalString = string.Join(" ", hexadecimalTextCodes);

			return hexadecimalString;
		}

		public static List<string> GetHexadecimalTextCodes(string text)
		{
			List<string> codes = [];

			foreach (char character in text)
			{
				int bits = (int)character;
				string bitsText = $"{bits:X4}";

				codes.Add(bitsText);
			}

			return codes;
		}

		public static int NormalizeFile(
			string inputPath, string outputPath, out int linesProcessed)
		{
			int linesChanged = -1;
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

				using StreamReader reader = new (inputPath, Encoding.UTF8);
				using StreamWriter writer =
					new (outputPath, false, Encoding.UTF8);

				string? line;

				do
				{
					line = reader.ReadLine();

					if (line != null)
					{
						string normalized =
							line.Normalize(NormalizationForm.FormKC);

						writer.WriteLine(normalized);
						linesProcessed++;

						if (line != normalized)
						{
							linesChanged++;
						}
					}
				}
				while (line != null);
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

				difference.Position = index;
				difference.Original = originalString;
				difference.Normalized = normalizedString;
			}

			return difference;
		}

		private static List<CharDifference> FindDifferences(
			string original, string normalized)
		{
			var differences = new List<CharDifference>();

			int minLength = Math.Min(original.Length, normalized.Length);

			for (int index = 0; index < minLength; index++)
			{
				char originalChar = original[index];
				char normalizedChar = normalized[index];

				CharDifference? difference =
					CheckDifferences(index, originalChar, normalizedChar);

				if (difference != null)
				{
					differences.Add(difference);
				}
			}

			return differences;
		}

	}
}
