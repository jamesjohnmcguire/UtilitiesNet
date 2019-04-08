/////////////////////////////////////////////////////////////////////////////
// $Id$
// <copyright file="SourceControl.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalZenWorks.Common.Utilities
{
	public static class SourceControl
	{
		/////////////////////////////////////////////////////////////////////
		// SvnSplit
		/// <summary>
		/// Split out a specific project from a SVN repository.
		/// </summary>
		/// <remarks>The status of this functionality is
		/// very uncertain.</remarks>
		/// <param name="arguments">A set of command line arguments</param>
		/////////////////////////////////////////////////////////////////////
		public static void SvnSplit(string[] arguments)
		{
			if (arguments != null)
			{
				// Dump original repository
				// byte[] Output = GeneralUtilities.Execute(
				// "C:\Program Files\CollabNet Subversion Server\svnadmin.exe",
				// "dump " + arguments[0], null);
				byte[] output = GeneralUtilities.Execute(
					@"svnadmin.exe", "dump " + arguments[0], null);

				// Filter out project
				output = GeneralUtilities.Execute(
					"svndumpfilter.exe", "include " + arguments[1], output);

				// Edit output to remove 'project' path as it will be 
				// the root in the new repository
				byte[] replacedOutput = GeneralUtilities.ReplaceInByteArray(
					output,
					Converter.StringToByteArray(
						"Node-path: " + arguments[1] + "/"),
					Converter.StringToByteArray("Node-path: "));

				replacedOutput = GeneralUtilities.ReplaceInByteArray(
						replacedOutput,
						Converter.StringToByteArray(
							"Node-copyfrom-path: " + arguments[1] + "/"),
						Converter.StringToByteArray("Node-copyfrom-path: "));

				// Create new repository
				GeneralUtilities.Execute(
					"svnadmin.exe", "create " + arguments[2], null);

				// Load project
				GeneralUtilities.Execute(
					"svnadmin.exe", "load " + arguments[2], replacedOutput);
			}
		}
	}
}
