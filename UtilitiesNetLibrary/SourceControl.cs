/////////////////////////////////////////////////////////////////////////////
// <copyright file="SourceControl.cs" company="James John McGuire">
// Copyright © 2006 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// Source version control class.
	/// </summary>
	public static class SourceControl
	{
		/// <summary>
		/// Split out a specific project from a SVN repository.
		/// </summary>
		/// <remarks>The status of this functionality is
		/// very uncertain.</remarks>
		/// <param name="originalRepository">The original repository.</param>
		/// <param name="originalProject">The original project.</param>
		/// <param name="newRepository">The new repository.</param>
		public static void SvnSplit(
			string originalRepository,
			string originalProject,
			string newRepository)
		{
			if (!string.IsNullOrWhiteSpace(originalRepository) &&
				!string.IsNullOrWhiteSpace(originalProject))
			{
				string arguments = "dump " + originalRepository;
				byte[]? output = GeneralUtilities.Execute(
					"svnadmin.exe", arguments, null);

				// Filter out project
				arguments = "include " + originalProject;
				output = GeneralUtilities.Execute(
					"svndumpfilter.exe", arguments, output);

				if (output != null)
				{
					byte[] replacedOutput =
						RemoveProject(originalProject, output);

					replacedOutput =
						ReplaceProject(originalProject, replacedOutput);

					if (!string.IsNullOrWhiteSpace(newRepository))
					{
						// Create new repository
						arguments = "create " + newRepository;
						GeneralUtilities.Execute(
							"svnadmin.exe", arguments, null);

						// Load project
						arguments = "load " + newRepository;
						GeneralUtilities.Execute(
							"svnadmin.exe", arguments, replacedOutput);
					}
				}
			}
		}

		private static byte[] RemoveProject(
			string originalProject, byte[] output)
		{
			// Edit output to remove 'project' path as it will be
			// the root in the new repository
			string nodePath = "Node-path: " + originalProject + "/";
			byte[] nodePathBytes = BitBytes.StringToByteArray(nodePath);

			nodePath = "Node-path: ";
			byte[] nodePathBytesAgain = BitBytes.StringToByteArray(nodePath);

			byte[] replacedOutput = BitBytes.ReplaceInByteArray(
				output, nodePathBytes, nodePathBytesAgain);

			return replacedOutput;
		}

		private static byte[] ReplaceProject(
			string originalProject, byte[] output)
		{
			string nodeCopy = "Node-copyfrom-path: " + originalProject + "/";
			byte[] nodeCopyBytes = BitBytes.StringToByteArray(nodeCopy);

			nodeCopy = "Node-copyfrom-path: ";
			byte[] nodeCopyBytesAgain = BitBytes.StringToByteArray(nodeCopy);

			byte[] replacedOutput = BitBytes.ReplaceInByteArray(
				output, nodeCopyBytes, nodeCopyBytesAgain);

			return replacedOutput;
		}
	}
}

