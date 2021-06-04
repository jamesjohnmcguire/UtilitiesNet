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
	/// <summary>
	/// Version Utilities.
	/// </summary>
	public static class VersionUtilities
	{
		/// <summary>
		/// Gets the build number.
		/// </summary>
		/// <returns>The build number.</returns>
		public static int GetBuildNumber()
		{
			int buildNumber;

			Assembly assembly = Assembly.GetCallingAssembly();

			AssemblyName name = assembly.GetName();
			Version version = name.Version;

			buildNumber = version.Revision;

			return buildNumber;
		}

		/// <summary>
		/// Gets the package version.
		/// </summary>
		/// <param name="packageId">The identifier for the package.</param>
		/// <returns>The package version.</returns>
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

		/// <summary>
		/// Get the version.
		/// </summary>
		/// <returns>The version.</returns>
		public static string GetVersion()
		{
			string version;

			Assembly assembly = Assembly.GetCallingAssembly();

			AssemblyName name = assembly.GetName();
			Version versionNumber = name.Version;

			version = versionNumber.ToString();

			return version;
		}

		/// <summary>
		/// Updates the version tag inside the given file.
		/// </summary>
		/// <param name="fileName">The file containing the version tag.</param>
		/// <returns>The updated version build number.</returns>
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

		private static string AssemblyInfoTagUpdate(
			string contents, string tag, out string version)
		{
			version = null;

			string pattern = tag + "\\(\"(?<major>\\d+)\\.(?<minor>\\d+)\\." +
				"(?<revision>\\d+)\\.(?<build>\\d+)\"\\)";
			string replacementFormat = tag + "(\"{0}.{1}.{2}.{3}\")";

			contents = VersionTagUpdate(
				contents, pattern, replacementFormat, out version);

			return contents;
		}

		private static string AssemblyInfoUpdate(string fileName)
		{
			string version = null;

			if (File.Exists(fileName))
			{
				string contents = File.ReadAllText(fileName);

				if (!string.IsNullOrWhiteSpace(contents))
				{
					string tag = "AssemblyVersion";
					contents = AssemblyInfoTagUpdate(contents, tag, out version);

					tag = "AssemblyFileVersion";
					contents = AssemblyInfoTagUpdate(contents, tag, out version);

					File.WriteAllText(fileName, contents);
				}
			}

			return version;
		}

		private static string VersionTagUpdate(
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
