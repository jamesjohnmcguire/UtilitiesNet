/////////////////////////////////////////////////////////////////////////////
// <copyright file="VersionUtilities.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utilities
{
	public static class VersionUtilities
	{
		public static int GetBuildNumber()
		{
			int buildNumber;

			Assembly thisAssembly = Assembly.GetExecutingAssembly();

			AssemblyName name = thisAssembly.GetName();
			Version version = name.Version;

			buildNumber = version.Revision;

			return buildNumber;
		}

		public static string GetVersion()
		{
			string version;

			Assembly thisAssembly = Assembly.GetExecutingAssembly();

			AssemblyName name = thisAssembly.GetName();
			Version versionNumber = name.Version;

			version = versionNumber.ToString();

			return version;
		}
	}
}