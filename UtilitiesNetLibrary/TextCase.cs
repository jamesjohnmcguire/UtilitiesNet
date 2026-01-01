/////////////////////////////////////////////////////////////////////////////
// <copyright file="TextCase.cs" company="James John McGuire">
// Copyright © 2006 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
{
	using System;
	using System.Text;
	using DigitalZenWorks.Common.Utilities.Extensions;

	/// <summary>
	/// Text casing support class.
	/// </summary>
	public static class TextCase
	{
		/// <summary>
		/// Gets the name in camel case.
		/// </summary>
		/// <param name="knrName">A string in knr format.</param>
		/// <returns>A name in camel case.</returns>
		public static string? ConvertToCamelCaseFromKnr(string? knrName)
		{
			string? output = null;

			if (!string.IsNullOrEmpty(knrName))
			{
				output = ConvertFromKnrText(knrName, true);
			}

			return output;
		}

		/// <summary>
		/// Gets the name in camel case.
		/// </summary>
		/// <param name="snakeCase">A string in snake case.</param>
		/// <returns>A name in camel case.</returns>
		public static string? ConvertToCamelCaseFromSnakeCase(
			string? snakeCase)
		{
			return ConvertToCamelCaseFromKnr(snakeCase);
		}

		/// <summary>
		/// Gets the name in pascal case.
		/// </summary>
		/// <param name="knrName">The name of variable in knr form.</param>
		/// <returns>A variable name in Pascal case form.</returns>
		public static string? ConvertToPascalCaseFromKnr(string? knrName)
		{
			string? output = null;

			if (!string.IsNullOrEmpty(knrName))
			{
				output = ConvertFromKnrText(knrName, false);
			}

			return output;
		}

		/// <summary>
		/// Converts to snake case from pascal case.
		/// </summary>
		/// <param name="pascalCase">The pascal case.</param>
		/// <returns>The text in snake case.</returns>
		/// <exception cref="System.ArgumentNullException">Exception
		/// text.</exception>
		public static string? ConvertToSnakeCaseFromPascalCase(
			string? pascalCase)
		{
			string? output = null;

			if (pascalCase == null)
			{
				throw new ArgumentNullException(nameof(pascalCase));
			}
			else if (!string.IsNullOrWhiteSpace(pascalCase))
			{
				StringBuilder builder = new();

				char item = char.ToLowerInvariant(pascalCase[0]);
				builder.Append(item);

				for (int i = 1; i < pascalCase.Length; ++i)
				{
					item = pascalCase[i];

					if (char.IsUpper(item))
					{
						builder.Append('_');
					}

					item = char.ToLowerInvariant(item);
					builder.Append(item);
				}

				output = builder.ToString();
			}

			return output;
		}

		/// <summary>
		/// Gets the case types.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <returns>The set of case types.</returns>
		public static CaseTypes GetCaseTypes(string variable)
		{
			CaseTypes caseTypes = default;

			bool isCase = IsCamelCase(variable);

			if (isCase == true)
			{
				caseTypes |= CaseTypes.CamelCase;
			}

			isCase = IsKabobCase(variable);

			if (isCase == true)
			{
				caseTypes |= CaseTypes.KabobCase;
			}

			isCase = IsPascalCase(variable);

			if (isCase == true)
			{
				caseTypes |= CaseTypes.PascalCase;
			}

			isCase = IsSnakeCase(variable);

			if (isCase == true)
			{
				caseTypes |= CaseTypes.SnakeCase;
			}

			return caseTypes;
		}

		/// <summary>
		/// Determines whether the specified variable is in camel case.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <returns><c>true</c> if [is camel case] [the specified variable];
		/// otherwise, <c>false</c>.</returns>
		public static bool IsCamelCase(string variable)
		{
			bool isCamelCase = false;

			if (!string.IsNullOrWhiteSpace(variable))
			{
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
				bool hasUnderScore = variable.Contains(
				'_', StringComparison.OrdinalIgnoreCase);
				bool hasDash = variable.Contains(
					'-', StringComparison.OrdinalIgnoreCase);
#else
				bool hasUnderScore = variable.Contains("_");
				bool hasDash = variable.Contains("-");
#endif

				char first = variable[0];
				bool firstLower = char.IsLower(first);

				if (firstLower == true && hasUnderScore == false &&
					hasDash == false)
				{
					isCamelCase = true;
				}
			}

			return isCamelCase;
		}

		/// <summary>
		/// Determines whether the specified variable is in kabob case.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <returns><c>true</c> if [is kabob case] [the specified variable];
		/// otherwise, <c>false</c>.</returns>
		public static bool IsKabobCase(string variable)
		{
			bool isKabobCase = false;

			if (!string.IsNullOrWhiteSpace(variable))
			{
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
				bool hasUnderScore = variable.Contains(
					'_', StringComparison.OrdinalIgnoreCase);
#else
				bool hasUnderScore = variable.Contains("_");
#endif

				if (hasUnderScore == false)
				{
					bool hasUppers = false;

					for (int index = 0; index < variable.Length; index++)
					{
						char item = variable[index];

						hasUppers = char.IsUpper(item);

						if (hasUppers == true)
						{
							break;
						}
					}

					if (hasUppers == false)
					{
						isKabobCase = true;
					}
				}
			}

			return isKabobCase;
		}

		/// <summary>
		/// Determines whether the specified variable is in pascal case.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <returns><c>true</c> if [is pascal case] [the specified variable];
		/// otherwise, <c>false</c>.</returns>
		public static bool IsPascalCase(string variable)
		{
			bool isPascalCase = false;

			if (!string.IsNullOrWhiteSpace(variable))
			{
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
				bool hasUnderScore = variable.Contains(
					'_', StringComparison.OrdinalIgnoreCase);
				bool hasDash = variable.Contains(
					'-', StringComparison.OrdinalIgnoreCase);
#else
				bool hasUnderScore = variable.Contains("_");
				bool hasDash = variable.Contains("-");
#endif

				char first = variable[0];
				bool firstUpper = char.IsUpper(first);

				char second = variable[1];
				bool secondLower = char.IsLower(second);

				if (firstUpper == true && secondLower == true &&
					hasUnderScore == false && hasDash == false)
				{
					isPascalCase = true;
				}
			}

			return isPascalCase;
		}

		/// <summary>
		/// Determines whether the specified variable is in snake case.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <returns><c>true</c> if [is snake case] [the specified variable];
		/// otherwise, <c>false</c>.</returns>
		public static bool IsSnakeCase(string variable)
		{
			bool isSnakeCase = false;

			if (!string.IsNullOrWhiteSpace(variable))
			{
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
				bool hasDash = variable.Contains(
					'-', StringComparison.OrdinalIgnoreCase);
#else
				bool hasDash = variable.Contains("-");
#endif

				if (hasDash == false)
				{
					bool hasUppers = false;

					for (int index = 0; index < variable.Length; index++)
					{
						char item = variable[index];

						hasUppers = char.IsUpper(item);

						if (hasUppers == true)
						{
							break;
						}
					}

					if (hasUppers == false)
					{
						isSnakeCase = true;
					}
				}
			}

			return isSnakeCase;
		}

		private static string? ConvertFromKnrText(
			string? knrName, bool setToCamelCase)
		{
			string? newCase = string.Empty;

			if (knrName != null)
			{
				// split at underscores
#if NET6_0_OR_GREATER
				char[] splitters = ['_'];
#else
				char[] splitters = new char[] { '_' };
#endif
				string[] parts = knrName.Split(
					splitters,
					StringSplitOptions.RemoveEmptyEntries);
				bool first = true;

				// remove underscores, set parts in intended case
				foreach (string part in parts)
				{
					if ((setToCamelCase == true) && (first == true))
					{
#pragma warning disable CA1308
						newCase += part.ToLowerInvariant();
#pragma warning restore CA1308
					}
					else
					{
						newCase += part.ToProperCase();
					}

					first = false;
				}
			}

			return newCase;
		}
	}
}

