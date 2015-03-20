/////////////////////////////////////////////////////////////////////////////
// $Id:$
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

namespace Zenware.Common.UtilsNet
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
		/// <param name="PathOfFileToRead"></param>
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
		/// <param name="PathOfFileToRead"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static string GetFileContents(
			string pathOfFileToRead)
		{
			string FileContents = null;

			if (File.Exists(pathOfFileToRead))
			{
				StreamReader StreamReaderObject = new StreamReader(pathOfFileToRead);

				if (null != StreamReaderObject)
				{
					FileContents = StreamReaderObject.ReadToEnd();
					StreamReaderObject.Close();
				}
			}

			return FileContents;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a writable stream object.
		/// </summary>
		/// <param name="FilePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static StreamWriter GetWriteStreamObject(
			string filePath)
		{
			StreamWriter StreamWriterObject = null;
			if (File.Exists(filePath))
			{
				//set up a filestream
				FileStream FileStreamObject = new FileStream(filePath,
															FileMode.Open,
															FileAccess.ReadWrite);

				if (null != FileStreamObject)
				{
					//set up a streamwriter for adding text
					StreamWriterObject = new StreamWriter(FileStreamObject);
				}
			}

			return StreamWriterObject;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void RecursiveRemove(
			string initialPath,
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
									log.Info(CultureInfo.InvariantCulture, m => m(
										"Deleting: " + Path));
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
				log.Info(CultureInfo.InvariantCulture, m => m(ex.Message));
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// RecursiveUtf8WinFiles
		/// </summary>
		/// <param name="initialPath"></param>
		/////////////////////////////////////////////////////////////////////
		public static void RecursiveUtf8WinFiles(
			string initialPath)
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
						UpdateFileUnixToCrlf(f);
					}
				}
				foreach (string f in Directory.GetFiles(initialPath, "*.php"))
				{
					log.Info(CultureInfo.InvariantCulture, m => m(
						"Checking Directory: " + f));
					UpdateFileUnixToCrlf(f);
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
		/// <param name="oldString"></param>
		/// <param name="newString"></param>
		/////////////////////////////////////////////////////////////////////
		public static void RegexStringInFile(string filePath, string oldValue,
			string newValue)
		{
			string Contents;

			if (File.Exists(filePath))
			{
				StreamReader sr = new StreamReader(filePath);

				if (sr != null)
				{
					Contents = sr.ReadToEnd();
					sr.Close();

					FileStream fs = new FileStream(filePath, FileMode.Open,
						FileAccess.ReadWrite);

					StreamWriter sw = new StreamWriter(fs);

					Contents = Regex.Replace(Contents, oldValue, newValue);

					sw.Write(Contents);

					sw.Close();
					fs.Close();
				}
			}
		}

		public static void ReplaceStringInFile(string filePath,
			string oldValue, string newValue)
		{
			string contents;
			string newContents;

			if (File.Exists(filePath))
			{
				StreamReader sr = new StreamReader(filePath);

				if (sr != null)
				{
					contents = sr.ReadToEnd();
					sr.Close();

					FileStream fs = new FileStream(filePath, FileMode.Open,
						FileAccess.ReadWrite);

					//set up a streamwriter for adding text
					StreamWriter sw = new StreamWriter(fs);

					newContents = contents.Replace(oldValue, newValue);

					sw.Write(newContents);

					// close file
					sw.Close();
					fs.Close();
				}
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="FileContents"></param>
		/// <param name="FilePathName"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static bool SaveFile(
			string fileContents,
			string filePathName)
		{
			FileStream fileStreamObject = new FileStream(filePathName,
														FileMode.Create,
														FileAccess.ReadWrite);

			StreamWriter streamWriterObject = new StreamWriter(fileStreamObject);
			streamWriterObject.Write(fileContents);
			streamWriterObject.Close();
			fileStreamObject.Close();

			return true;
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
			bool successCode = false;
			if (!string.IsNullOrEmpty(fileContents))
			{
				FileStream fileStreamObject = new FileStream(filePathName,
															FileMode.Create,
															FileAccess.ReadWrite);

				fileContents = fileContents.Replace("\n", Environment.NewLine);
				StreamWriter streamWriterObject = new StreamWriter(fileStreamObject);
				streamWriterObject.Write(fileContents);
				streamWriterObject.Close();
				fileStreamObject.Close();

				successCode = true;
			}

			return successCode;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Update the given file so that all lines end with CRLF
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public static void UpdateFileMacToCrlf(string filePath)
		{
			RegexStringInFile(filePath, "\r\n|\r|\n", "\r\n");
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Update the given file so that all lines end with CRLF
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public static void UpdateFileUnixToCrlf(string filePath)
		{
			RegexStringInFile(filePath, "\r\n|\r|\n", "\r\n");
		}
	} // End Class
} // End Namespace