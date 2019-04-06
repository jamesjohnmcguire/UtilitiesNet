/////////////////////////////////////////////////////////////////////////////
// $Id$
// <copyright file="Converter.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalZenWorks.Common.Utilities
{
	public static class Converter
	{
		public static byte[] StringToByteArray(
			string original)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] byteArrary = encoding.GetBytes(original);
			return byteArrary;
		}
	}
}
