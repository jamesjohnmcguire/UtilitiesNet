/////////////////////////////////////////////////////////////////////////////
// <copyright file="CaseTypes.cs" company="Digital Zen Works">
// Copyright © 2006 - 2026 Digital Zen Works.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
{
	using System;

	/// <summary>
	/// Provides an enumeration for types of variable cases.
	/// </summary>
	[Flags]
	public enum CaseTypes
	{
		/// <summary>
		/// Other or unknown case type.
		/// </summary>
#pragma warning disable CA1008
		Other = 0,
#pragma warning restore CA1008

		/// <summary>
		/// Camel case type.
		/// </summary>
		CamelCase = 1,

		/// <summary>
		/// Kabob case type.
		/// </summary>
		KabobCase = 2,

		/// <summary>
		/// Pascal csae type.
		/// </summary>
		PascalCase = 4,

		/// <summary>
		/// Snake case type.
		/// </summary>
		SnakeCase = 8
	}
}
