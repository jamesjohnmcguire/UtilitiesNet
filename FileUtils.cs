/////////////////////////////////////////////////////////////////////////////
// $Id$
//
// Copyright (c) 2006-2015 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// Namespace includes
/////////////////////////////////////////////////////////////////////////////
using Common.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utils
{
	/////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Represents a FileUtils object
	/// </summary>
	/////////////////////////////////////////////////////////////////////////
	public static class FileUtils
	{
		private static readonly ILog log = LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Compares two files to see if they are they same
		/// </summary>
		/// <param name="path1"></param>
		/// <param name="path2"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static bool FileEquals(string path1, string path2)
		{
			bool filesSame = false;
			if (path1 == path2)
			{
				filesSame = true;
			}
			else
			{
				byte[] file1 = File.ReadAllBytes(path1);
				byte[] file2 = File.ReadAllBytes(path2);
				if (file1.Length == file2.Length)
				{
					bool stillSame = true;
					for (int i = 0; i < file1.Length; i++)
					{
						if (file1[i] != file2[i])
						{
							stillSame = false;
							break;
						}
					}

					if (true == stillSame)
					{
						filesSame = true;
					}
				}
			}

			return filesSame;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reads a text file contents into a string
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static string GetFileContents(string filePath)
		{
			string fileContents = null;

			if (File.Exists(filePath))
			{
				using (StreamReader StreamReaderObject = new
					StreamReader(filePath))
				{
					if (null != StreamReaderObject)
					{
						fileContents = StreamReaderObject.ReadToEnd();
					}
				}
			}

			return fileContents;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a writable stream object.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public static StreamWriter GetWriteStreamObject(string filePath)
		{
			StreamWriter streamWriter = null;
			FileStream fileStream = null;

			try
			{
				if (File.Exists(filePath))
				{
					fileStream = new FileStream(filePath, FileMode.Open,
						FileAccess.ReadWrite);

					if (null != fileStream)
					{
						//set up a streamwriter for adding text
						streamWriter = new StreamWriter(fileStream);
					}
				}
			}
			catch (Exception ex)
			{
				log.Error(CultureInfo.InvariantCulture, m => m(ex.Message));
			}
			finally
			{
				if (null != fileStream)
				{
					fileStream.Close();
				}
			}

			return streamWriter;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Safe recursive removal. Directories with a "DevClean Active" file
		/// not deleted.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void RecursiveRemove(string initialPath,
			string[] pathsToRemove)
		{
			try
			{
				foreach (string d in Directory.GetDirectories(initialPath))
				{
					if ((pathsToRemove != null) && (pathsToRemove.Length > 0))
					{
						foreach (string Path in pathsToRemove)
						{
							if (d.EndsWith("\\" + Path,
								StringComparison.Ordinal))
							{
								if (!File.Exists("DevClean.active"))
								{
									log.Info(CultureInfo.InvariantCulture,
										m => m("Deleting: " + Path));
									Directory.Delete(d, true);
								}
							}
							else
							{
								RecursiveRemove(d, pathsToRemove);
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				log.Error(CultureInfo.InvariantCulture, m => m(ex.Message));
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// RecursiveUtf8WinFiles
		/// </summary>
		/// <param name="initialPath"></param>
		/////////////////////////////////////////////////////////////////////
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void RecursiveUtf8WinFiles(string initialPath)
		{
			try
			{
				foreach (string d in Directory.GetDirectories(initialPath))
				{
					RecursiveUtf8WinFiles(d);

					foreach (string f in Directory.GetFiles(d, "*.php"))
					{
						log.Info(CultureInfo.InvariantCulture, m => m(
							"Checking File: " + f));
						UpdateFileLineEndingUnixToWindows(f);
					}
				}
				foreach (string f in Directory.GetFiles(initialPath, "*.php"))
				{
					log.Info(CultureInfo.InvariantCulture, m => m(
						"Checking Directory: " + f));
					UpdateFileLineEndingUnixToWindows(f);
				}
			}
			catch (System.Exception excpt)
			{
				Console.WriteLine(excpt.Message);
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// RegexStringInFile
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		/////////////////////////////////////////////////////////////////////
		public static void RegexStringInFile(string filePath, string oldValue,
			string newValue)
		{
			string contents;

			if (File.Exists(filePath))
			{
				using (StreamReader sr = new StreamReader(filePath))
				{
					if (sr != null)
					{
						contents = sr.ReadToEnd();

						using (FileStream fs = new FileStream(filePath,
							FileMode.Open, FileAccess.ReadWrite))
						{
							StreamWriter sw = new StreamWriter(fs);

							contents = Regex.Replace(contents, oldValue,
								newValue);

							sw.Write(contents);

							//sw.Close();
						}
					}
				}
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Replace a string in a file
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		/////////////////////////////////////////////////////////////////////
		public static void ReplaceStringInFile(string filePath,
			string oldValue, string newValue)
		{
			string contents;
			string newContents;

			if (File.Exists(filePath))
			{
				using (StreamReader sr = new StreamReader(filePath))
				{
					if (sr != null)
					{
						contents = sr.ReadToEnd();

						using (FileStream fs = new FileStream(filePath,
							FileMode.Open, FileAccess.ReadWrite))
						{
							//set up a stream writer for adding text
							StreamWriter sw = new StreamWriter(fs);

							newContents = contents.Replace(oldValue, newValue);

							sw.Write(newContents);

							// close file
							//sw.Close();
						}
					}
				}
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="fileContents"></param>
		/// <param name="filePathName"></param>
		/// <param name="insureWindowsLineEndings"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1026:DefaultParametersShouldNotBeUsed"),
		System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
			"CA2202:Do not dispose objects multiple times"),
		System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public static bool SaveFile(string fileContents, string filePathName,
			bool insureWindowsLineEndings = false)
		{
			bool successCode = false;

			if (!string.IsNullOrWhiteSpace(fileContents))
			{
				FileStream fileStream = null;
				StreamWriter streamWriter = null;
				try
				{
					fileStream = new FileStream(filePathName,
						FileMode.Create, FileAccess.ReadWrite);

					if (true == insureWindowsLineEndings)
					{
						fileContents = fileContents.Replace("\n",
							Environment.NewLine);
					}

					streamWriter = new StreamWriter(fileStream);
					streamWriter.Write(fileContents);

					successCode = true;
				}
				catch (Exception ex)
				{
					log.Error(CultureInfo.InvariantCulture,
						m => m(ex.Message));
				}
				finally
				{
					if (streamWriter != null)
					{
						streamWriter.Close();
					}
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
			}
			return successCode;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="fileContents"></param>
		/// <param name="filePathName"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static bool SaveFileWin(
			string fileContents,
			string filePathName)
		{
			return SaveFile(fileContents, filePathName, true);
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Update the given file so that all lines end with CRLF
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public static void UpdateFileMacToWindows(string filePath)
		{
			RegexStringInFile(filePath, "\r\n|\r|\n", "\r\n");
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Update the given file so that all lines end with CRLF
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public static void UpdateFileLineEndingUnixToWindows(string filePath)
		{
			RegexStringInFile(filePath, "\r\n|\r|\n", "\r\n");
		}
	} // End Class
} // End Namespace