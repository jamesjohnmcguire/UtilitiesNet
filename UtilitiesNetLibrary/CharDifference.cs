/////////////////////////////////////////////////////////////////////////////
// <copyright file="CharDifference.cs" company="James John McGuire">
// Copyright © 2006 - 2026 James John McGuire. All Rights Reserved.
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
		/// <value>The current position within the sequence.</value>
		public int Position { get; set; }

		/// <summary>
		/// Gets or sets the original string value.
		/// </summary>
		/// <value>The original string value.</value>
		public string? Original { get; set; }

		/// <summary>
		/// Gets or sets the normalized representation of the string value.
		/// </summary>
		/// <value>The normalized representation of the string value.</value>
		public string? Normalized { get; set; }
	}
}
