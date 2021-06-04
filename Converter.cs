/////////////////////////////////////////////////////////////////////////////
// <copyright file="Converter.cs" company="James John McGuire">
// Copyright © 2006 - 2021 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// Converter class.
	/// </summary>
	public static class Converter
	{
		/// <summary>
		/// Converts a string to a byte array.
		/// </summary>
		/// <param name="original">The string to be converted.</param>
		/// <returns>The byte array.</returns>
		public static byte[] StringToByteArray(
			string original)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] byteArrary = encoding.GetBytes(original);
			return byteArrary;
		}
	}
}
