/////////////////////////////////////////////////////////////////////////////
// <copyright file="SourceControl.cs" company="James John McGuire">
// Copyright © 2006 - 2022 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// Source version control class.
	/// </summary>
	public static class SourceControl
	{
		/////////////////////////////////////////////////////////////////////
		// SvnSplit
		/// <summary>
		/// Split out a specific project from a SVN repository.
		/// </summary>
		/// <remarks>The status of this functionality is
		/// very uncertain.</remarks>
		/// <param name="originalRepository">The original repository.</param>
		/// <param name="originalProject">The original project.</param>
		/// <param name="newRepository">The new repository.</param>
		/////////////////////////////////////////////////////////////////////
		public static void SvnSplit(
			string originalRepository,
			string originalProject,
			string newRepository)
		{
			if (!string.IsNullOrWhiteSpace(originalRepository))
			{
				// Dump original repository
				// byte[] Output = GeneralUtilities.Execute(
				// "C:\Program Files\CollabNet Subversion Server\svnadmin.exe",
				// "dump " + arguments[0], null);
				byte[] output = GeneralUtilities.Execute(
					@"svnadmin.exe", "dump " + originalRepository, null);

				if (!string.IsNullOrWhiteSpace(originalProject))
				{
					// Filter out project
					output = GeneralUtilities.Execute(
						"svndumpfilter.exe", "include " + originalProject, output);

					// Edit output to remove 'project' path as it will be
					// the root in the new repository
					byte[] replacedOutput = GeneralUtilities.ReplaceInByteArray(
						output,
						Converter.StringToByteArray(
							"Node-path: " + originalProject + "/"),
						Converter.StringToByteArray("Node-path: "));

					replacedOutput = GeneralUtilities.ReplaceInByteArray(
							replacedOutput,
							Converter.StringToByteArray(
								"Node-copyfrom-path: " + originalProject + "/"),
							Converter.StringToByteArray(
								"Node-copyfrom-path: "));

					if (!string.IsNullOrWhiteSpace(newRepository))
					{
						// Create new repository
						GeneralUtilities.Execute(
							"svnadmin.exe", "create " + newRepository, null);

						// Load project
						GeneralUtilities.Execute(
							"svnadmin.exe",
							"load " + newRepository,
							replacedOutput);
					}
				}
			}
		}
	}
}
