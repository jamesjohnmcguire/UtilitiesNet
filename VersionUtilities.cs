/////////////////////////////////////////////////////////////////////////////
// <copyright file="VersionUtilities.cs" company="James John McGuire">
// Copyright © 2006 - 2021 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.IO;
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
				string contents = File.ReadAllText("packages.config");

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

		public static string UpdateVersion(string fileName)
		{
			string version = null;

			try
			{
				if (File.Exists(fileName))
				{
					string contents = File.ReadAllText(fileName);

					if (!string.IsNullOrWhiteSpace(contents))
					{
						int major = 0;
						int minor = 0;
						int build = 0;
						int revision = 0;
						string pattern = "AssemblyVersion\\(\"(?<major>\\d+)\\." +
							"(?<minor>\\d+)\\.(?<revision>\\d+)\\.(?<build>\\d+)\"\\)";

						Regex regex = new Regex(pattern);
						MatchCollection matches = regex.Matches(contents);

						if (matches.Count > 0)
						{
							major = Convert.ToInt32(matches[0].Groups["major"].Value);
							minor = Convert.ToInt32(matches[0].Groups["minor"].Value);
							revision = Convert.ToInt32(matches[0].Groups["revision"].Value);
							build = Convert.ToInt32(matches[0].Groups["build"].Value) + 1;

							version = build.ToString(CultureInfo.InvariantCulture);
						}
					}
				}
			}
			catch (Exception exception) when
				(exception is ArgumentException ||
				exception is ArgumentNullException ||
				exception is ArgumentOutOfRangeException ||
				exception is DirectoryNotFoundException ||
				exception is FileNotFoundException ||
				exception is FormatException ||
				exception is IOException ||
				exception is NotSupportedException ||
				exception is OverflowException ||
				exception is PathTooLongException ||
				exception is System.Security.SecurityException ||
				exception is UnauthorizedAccessException)
			{
			}

			return version;
		}
	}
}
