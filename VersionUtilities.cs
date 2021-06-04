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

		public static string VersionUpdate(string fileName)
		{
			string version = null;

			try
			{
				if (File.Exists(fileName))
				{
					string contents = File.ReadAllText(fileName);

					if (!string.IsNullOrWhiteSpace(contents))
					{
						string pattern = "AssemblyVersion\\(\"" +
							"(?<major>\\d+)\\.(?<minor>\\d+)\\." +
							"(?<revision>\\d+)\\.(?<build>\\d+)\"\\)";
						string replacementFormat =
							"AssemblyVersion(\"{0}.{1}.{2}.{3}\")";

						contents = VersionTagUpdate(
							contents, pattern, replacementFormat, out version);

						File.WriteAllText(fileName, contents);
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

		public static string VersionTagUpdate(
			string contents,
			string pattern,
			string replacementFormat,
			out string version)
		{
			version = null;
			Regex regex = new Regex(pattern);
			MatchCollection matches = regex.Matches(contents);

			if (matches.Count > 0)
			{
				int build;
				string major = matches[0].Groups["major"].Value;
				string minor = matches[0].Groups["minor"].Value;
				string revision = matches[0].Groups["revision"].Value;

				build = Convert.ToInt32(matches[0].Groups["build"].Value);
				build++;

				version = build.ToString(CultureInfo.InvariantCulture);

				string replacement = string.Format(
					CultureInfo.InvariantCulture,
					replacementFormat,
					major,
					minor,
					revision,
					version);

				contents = Regex.Replace(
					contents, pattern, replacement);
			}

			return contents;
		}
	}
}
