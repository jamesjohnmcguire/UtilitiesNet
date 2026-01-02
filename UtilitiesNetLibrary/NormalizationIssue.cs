/////////////////////////////////////////////////////////////////////////////
// <copyright file="NormalizationIssue.cs" company="James John McGuire">
// Copyright © 2006 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
{
	using System.Collections.ObjectModel;

	/// <summary>
	/// Represents an issue encountered during the normalization of a text
	/// line,  including details about the original and normalized lines,
	/// as well as any differences.
	/// </summary>
	/// <remarks>This class provides information about a specific line of
	/// text that was processed during normalization.  It includes the line
	/// number, the original and normalized versions of the line, and a
	/// collection of character-level differences between the two.</remarks>
	public class NormalizationIssue
	{
		/// <summary>
		/// Gets or sets the collection of character differences between two
		/// compared strings.
		/// </summary>
		/// <value>The collection of character differences between two
		/// compared strings.</value>
#pragma warning disable CA2227
		public Collection<CharDifference>? Differences { get; set; }
#pragma warning restore CA2227

		/// <summary>
		/// Gets or sets the line number associated with the current instance.
		/// </summary>
		/// <value>The line number associated with the current instance.</value>
		public int LineNumber { get; set; }

		/// <summary>
		/// Gets or sets the normalized version of the line.
		/// </summary>
		/// <value>The normalized version of the line.</value>
		public string? NormalizedLine { get; set; }

		/// <summary>
		/// Gets or sets the original line of text associated with
		/// this instance.
		/// </summary>
		/// <value>The original line of text associated with
		/// this instance.</value>
		public string? OriginalLine { get; set; }
	}
}
