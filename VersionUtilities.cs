/////////////////////////////////////////////////////////////////////////////
// <copyright file="VersionUtilities.cs" company="James John McGuire">
// Copyright © 2006 - 2021 James John McGuire. All Rights Reserved.
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

			Assembly assembly = Assembly.GetCallingAssembly();

			AssemblyName name = assembly.GetName();
			Version version = name.Version;

			buildNumber = version.Revision;

			return buildNumber;
		}

		public static string GetPackageVersion(string packageId)
		{
			string version = string.Empty;

			if (!string.IsNullOrWhiteSpace(packageId))
			{
				// get the package file, based on current directory
				string contents = FileUtils.GetFileContents("packages.config");

				if (!string.IsNullOrWhiteSpace(contents))
				{
					int index = contents.IndexOf(
						packageId, StringComparison.OrdinalIgnoreCase) +
						packageId.Length + 1;
					string substring = contents.Substring(index);
					version =
						Regex.Match(substring, "\"([^\"]*)\"").Groups[1].Value;
				}
			}

			return version;
		}

		public static string GetVersion()
		{
			string version;

			Assembly assembly = Assembly.GetCallingAssembly();

			AssemblyName name = assembly.GetName();
			Version versionNumber = name.Version;

			version = versionNumber.ToString();

			return version;
		}
	}
}