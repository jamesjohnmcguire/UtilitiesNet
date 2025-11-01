/////////////////////////////////////////////////////////////////////////////
// <copyright file="CharDifference.cs" company="James John McGuire">
// Copyright © 2006 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// Char difference class.
	/// </summary>
	public class CharDifference
	{
		/// <summary>
		/// Gets or sets the current position within the sequence.
		/// </summary>
		public int Position { get; set; }

		/// <summary>
		/// Gets or sets the original string value.
		/// </summary>
		public string? Original { get; set; }

		/// <summary>
		/// Gets or sets the normalized representation of the string value.
		/// </summary>
		public string? Normalized { get; set; }
	}
}
