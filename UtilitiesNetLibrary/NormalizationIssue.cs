/////////////////////////////////////////////////////////////////////////////
// <copyright file="NormalizationIssue.cs" company="James John McGuire">
// Copyright © 2006 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

#nullable enable

namespace DigitalZenWorks.Common.Utilities
{
	using System.Collections.Generic;

	public class NormalizationIssue
	{
		public int LineNumber { get; set; }
		public string? OriginalLine { get; set; }
		public string? NormalizedLine { get; set; }
		public List<CharDifference>? Differences { get; set; }
	}
}
