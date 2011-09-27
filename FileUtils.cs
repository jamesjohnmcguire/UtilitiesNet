/////////////////////////////////////////////////////////////////////////////
// $Id: $
//
// Represents a set of file utilities
//
// Copyright (c) 2007-2010 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// Namespace includes
/////////////////////////////////////////////////////////////////////////////
using System;
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
				FileStream fs1 = new FileStream(path1, FileMode.Open);
				FileStream fs2 = new FileStream(path2, FileMode.Open);

				if (fs1.Length != fs2.Length)
				{
					fs1.Close();
					fs2.Close();
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
			string PathOfFileToRead)
		{
			string FileContents = null;

			if (File.Exists(PathOfFileToRead))
			{
				StreamReader StreamReaderObject = new StreamReader(PathOfFileToRead);

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
			string FilePath)
		{
			StreamWriter StreamWriterObject = null;
			if (File.Exists(FilePath))
			{
				//set up a filestream
				FileStream FileStreamObject = new FileStream(FilePath,
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

		public static void RecursiveRemove(
			string InitialPath,
			string[] PathsToRemove)
		{
			try
			{
				foreach (string d in Directory.GetDirectories(InitialPath))
				{
					foreach (string Path in PathsToRemove)
					{
						if (d.EndsWith("\\" + Path))
						{
							if (!File.Exists("DevClean.active"))
							{
								Console.WriteLine("Deleting: " + Path);
								Directory.Delete(d, true);
							}
						}
						else
						{
							RecursiveRemove(d, PathsToRemove);
						}
					}
				}
			}
			catch (System.Exception excpt)
			{
				Console.WriteLine(excpt.Message);
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// RecursiveUtf8WinFiles
		/// </summary>
		/// <param name="InitialPath"></param>
		/////////////////////////////////////////////////////////////////////
		public static void RecursiveUtf8WinFiles(
			string InitialPath)
		{
			try
			{
				foreach (string d in Directory.GetDirectories(InitialPath))
				{
					RecursiveUtf8WinFiles(d);

					foreach (string f in Directory.GetFiles(d, "*.php"))
					{
						Console.WriteLine("Checking File: " + f);
						UpdateFileUnixToCRLF(f);
					}
				}
				foreach (string f in Directory.GetFiles(InitialPath, "*.php"))
				{
					Console.WriteLine("Checking File: " + f);
					UpdateFileUnixToCRLF(f);
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
		/// <param name="FilePath"></param>
		/// <param name="OldString"></param>
		/// <param name="NewString"></param>
		/////////////////////////////////////////////////////////////////////
		public static void RegexStringInFile(
			string FilePath,
			string OldString,
			string NewString)
		{
			string Contents;

			if (File.Exists(FilePath))
			{
				StreamReader sr = new StreamReader(FilePath);

				if (sr != null)
				{
					Contents = sr.ReadToEnd();
					sr.Close();

					FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);

					StreamWriter sw = new StreamWriter(fs);

					Contents = Regex.Replace(Contents, OldString, NewString);

					sw.Write(Contents);

					sw.Close();
					fs.Close();
				}
			}
		}

		public static void ReplaceStringInFile(string sFilePath, string sOldString, string sNewString)
		{
			string sContents;
			string sNewContents;

			if (File.Exists(sFilePath))
			{
				StreamReader sr = new StreamReader(sFilePath);

				if (sr != null)
				{
					sContents = sr.ReadToEnd();
					sr.Close();

					FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.ReadWrite);

					//set up a streamwriter for adding text
					StreamWriter sw = new StreamWriter(fs);

					sNewContents = sContents.Replace(sOldString, sNewString);

					sw.Write(sNewContents);

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
			string FileContents,
			string FilePathName)
		{
			FileStream FileStreamObject = new FileStream(FilePathName,
														FileMode.Create,
														FileAccess.ReadWrite);

			StreamWriter StreamWriterObject = new StreamWriter(FileStreamObject);
			StreamWriterObject.Write(FileContents);
			StreamWriterObject.Close();
			FileStreamObject.Close();

			return true;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="FileContents"></param>
		/// <param name="FilePathName"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static bool SaveFileWin(
			string FileContents,
			string FilePathName)
		{
			FileStream FileStreamObject = new FileStream(FilePathName,
														FileMode.Create,
														FileAccess.ReadWrite);

			FileContents = FileContents.Replace("\n", Environment.NewLine);
			StreamWriter StreamWriterObject = new StreamWriter(FileStreamObject);
			StreamWriterObject.Write(FileContents);
			StreamWriterObject.Close();
			FileStreamObject.Close();

			return true;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Update the given file so that all lines end with CRLF
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public static void UpdateFileMacToCRLF(string sFilePath)
		{
			RegexStringInFile(sFilePath, "\r\n|\r|\n", "\r\n");
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Update the given file so that all lines end with CRLF
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public static void UpdateFileUnixToCRLF(string sFilePath)
		{
			RegexStringInFile(sFilePath, "\r\n|\r|\n", "\r\n");
		}
	} // End Class
} // End Namespace