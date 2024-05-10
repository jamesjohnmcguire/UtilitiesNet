/////////////////////////////////////////////////////////////////////////////
// <copyright file="FileUtils.cs" company="James John McGuire">
// Copyright © 2006 - 2024 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;

namespace DigitalZenWorks.Common.Utilities
{
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
