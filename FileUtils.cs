/////////////////////////////////////////////////////////////////////////////
// $Id$
//
// Copyright © 2006-2016 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// Namespace includes
/////////////////////////////////////////////////////////////////////////////
using Common.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Security;

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
		private static readonly ResourceManager stringTable = new
			ResourceManager("DigitalZenWorks.Common.Utils.Resources",
			Assembly.GetExecutingAssembly());

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public static bool CreateFileFromEmbeddedResource(string resourceName,
			string filePath)
		{
			bool success = false;
			Stream templateObjectStream = null;
			FileStream fileStream = null;

			try
			{
				Assembly thisAssembly = Assembly.GetCallingAssembly();

				templateObjectStream =
					thisAssembly.GetManifestResourceStream(resourceName);

				if (null == templateObjectStream)
				{
					log.Error(CultureInfo.InvariantCulture, m => m(
						"Failed to manifest resource stream"));
				}
				else
				{
					using (FileStream file = new FileStream(filePath,
						FileMode.Create, FileAccess.Write))
					{
						templateObjectStream.CopyTo(file);
					}

					success = true;
				}
			}
			catch (Exception exception) when
				(exception is ArgumentNullException ||
				exception is ArgumentException ||
				exception is FileLoadException ||
				exception is FileNotFoundException ||
				exception is BadImageFormatException ||
				exception is NotImplementedException ||
				exception is ArgumentOutOfRangeException ||
				exception is IOException ||
				exception is NotSupportedException ||
				exception is ObjectDisposedException ||
				exception is SecurityException ||
				exception is DirectoryNotFoundException ||
				exception is PathTooLongException)
			{
				log.Error(CultureInfo.InvariantCulture, m => m(
					stringTable.GetString("EXCEPTION") + exception));
			}
			catch
			{
				throw;
			}
			finally
			{
				if (null != fileStream)
				{
					fileStream.Close();
				}
			}

			return success;
		}

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
			catch (Exception exception)
			{
				log.Error(CultureInfo.InvariantCulture, m => m(
					exception.ToString()));
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
		/// Determines if a directory is empty or not
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static bool IsDirectoryEmpty(string path)
		{
			return !Directory.EnumerateFileSystemEntries(path).Any();
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
			catch (System.Exception exception)
			{
				log.Error(CultureInfo.InvariantCulture, m => m(
					exception.ToString()));
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
			if (File.Exists(filePath))
			{
				using (FileStream file = File.Open(filePath,
					FileMode.Open, FileAccess.ReadWrite, FileShare.None))
				{
					using (StreamReader reader = new StreamReader(file))
					{
						string contents = reader.ReadToEnd();

						string newContents =
							contents.Replace(oldValue, newValue);

						using (StreamWriter writer = new StreamWriter(file))
						{
							writer.Write(newContents);

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
				catch (Exception exception)
				{
					log.Error(CultureInfo.InvariantCulture,
						m => m(exception.ToString()));

					throw;
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
		/// Touch
		/// </summary>
		/// <param name="path"></param>
		/// <param name="time"></param>
		/////////////////////////////////////////////////////////////////////
		public static void Touch(string path, string time)
		{
			string[] format = { "yyyyMMddHHmmss" };
			DateTime dateTime;

			if (DateTime.TryParseExact(time, format,
				System.Globalization.CultureInfo.InvariantCulture,
				System.Globalization.DateTimeStyles.None, out dateTime))
			{
				FileAttributes attributes = File.GetAttributes(path);

				if (attributes.HasFlag(FileAttributes.Directory))
				{
					Directory.SetCreationTime(path, dateTime);
					Directory.SetLastAccessTime(path, dateTime);
					Directory.SetLastWriteTime(path, dateTime);
				}
				else
				{
					File.SetCreationTime(path, dateTime);
					File.SetLastAccessTime(path, dateTime);
					File.SetLastWriteTime(path, dateTime);
				}
			}
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
			"CA2202:Do not dispose objects multiple times")]
		public static void WriteExtractedFile(string fileName, byte[] contents)
		{
			using (FileStream fileStream =
				File.Open(fileName, FileMode.Create))
			{
				using (BinaryWriter writer = new BinaryWriter(fileStream))
				{
					writer.Write(contents);
				}
			}
		}
	} // End Class
} // End Namespace