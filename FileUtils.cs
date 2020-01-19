/////////////////////////////////////////////////////////////////////////////
// <copyright file="FileUtils.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
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
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utilities
{
	/////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Represents a FileUtils object.
	/// </summary>
	/////////////////////////////////////////////////////////////////////////
	public static class FileUtils
	{
		private static readonly ILog Log = LogManager.GetLogger(
			MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly ResourceManager StringTable = new
			ResourceManager(
				"DigitalZenWorks.Common.Utilities.Resources",
				Assembly.GetExecutingAssembly());

		public static bool CreateFileFromEmbeddedResource(
			string resourceName, string filePath)
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
					Log.Error(CultureInfo.InvariantCulture, m => m(
						"Failed to manifest resource stream"));
				}
				else
				{
					using (FileStream file = new FileStream(
						filePath, FileMode.Create, FileAccess.Write))
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
				Log.Error(CultureInfo.InvariantCulture, m => m(
					StringTable.GetString(
						"EXCEPTION",
						CultureInfo.InstalledUICulture) + exception));
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

		public static void Flatten(string directory)
		{
			foreach (string subDirectory in Directory.GetDirectories(directory))
			{
				Flatten(subDirectory);
				Directory.Delete(directory);
			}

			foreach (string fileName in Directory.GetFiles(directory))
			{
				string newFileName = fileName.Replace("\\", "_");
				File.Move(fileName, newFileName);
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Compares two files to see if they are they same.
		/// </summary>
		/// <param name="path1">The path of the first file to check.</param>
		/// <param name="path2">The path of the second file to check.</param>
		/// <returns>Return true if the file contents are the same,
		/// otherwise returns false.</returns>
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
		/// Reads a text file contents into a string.
		/// </summary>
		/// <param name="filePath">The path of the file.</param>
		/// <returns>The contents of file upon success,
		/// otherwise returns null.</returns>
		/////////////////////////////////////////////////////////////////////
		public static string GetFileContents(string filePath)
		{
			string fileContents = null;

			if (File.Exists(filePath))
			{
				using (StreamReader streamReaderObject = new
					StreamReader(filePath))
				{
					if (null != streamReaderObject)
					{
						fileContents = streamReaderObject.ReadToEnd();
					}
				}
			}

			return fileContents;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a writable stream object.
		/// </summary>
		/// <param name="filePath">The file path to file.</param>
		/// <returns>A StreamWriter object upon success,
		/// otherwise null.</returns>
		/////////////////////////////////////////////////////////////////////
		public static StreamWriter GetWriteStreamObject(string filePath)
		{
			StreamWriter streamWriter = null;
			FileStream fileStream = null;

			try
			{
				if (File.Exists(filePath))
				{
					fileStream = new FileStream(
						filePath, FileMode.Open, FileAccess.ReadWrite);

					if (null != fileStream)
					{
						// set up a streamwriter for adding text
						streamWriter = new StreamWriter(fileStream);
					}
				}
			}
			catch (Exception exception) when
				(exception is ArgumentException ||
				exception is ArgumentNullException ||
				exception is IOException ||
				exception is SecurityException ||
				exception is UnauthorizedAccessException)
			{
				Log.Error(CultureInfo.InvariantCulture, m => m(
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
		/// Determines if a directory is empty or not.
		/// </summary>
		/// <param name="path">The path of the directory.</param>
		/// <returns>Returns true if the directory is empty,
		/// otherwise false.</returns>
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
		/// <param name="initialPath">The initial path to start from.</param>
		/// <param name="pathsToRemove">A list of paths to remove.</param>
		/////////////////////////////////////////////////////////////////////
		public static void RecursiveRemove(
			string initialPath, string[] pathsToRemove)
		{
			foreach (string d in Directory.GetDirectories(initialPath))
			{
				if ((pathsToRemove != null) && (pathsToRemove.Length > 0))
				{
					foreach (string path in pathsToRemove)
					{
						if (d.EndsWith(
							"\\" + path, StringComparison.Ordinal))
						{
							if (!File.Exists("DevClean.active"))
							{
								Log.Info(
									CultureInfo.InvariantCulture,
									m => m("Deleting: " + path));
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

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// RecursiveUtf8WinFiles.
		/// </summary>
		/// <param name="initialPath">The initial path to begin
		/// recursion into.</param>
		/////////////////////////////////////////////////////////////////////
		public static void RecursiveUtf8WinFiles(string initialPath)
		{
			try
			{
				foreach (string d in Directory.GetDirectories(initialPath))
				{
					RecursiveUtf8WinFiles(d);

					foreach (string f in Directory.GetFiles(d, "*.php"))
					{
						Log.Info(CultureInfo.InvariantCulture, m => m(
							"Checking File: " + f));
						UpdateFileLineEndingUnixToWindows(f);
					}
				}

				foreach (string f in Directory.GetFiles(initialPath, "*.php"))
				{
					Log.Info(CultureInfo.InvariantCulture, m => m(
						"Checking Directory: " + f));
					UpdateFileLineEndingUnixToWindows(f);
				}
			}
			catch (Exception exception) when (exception is ArgumentException ||
				exception is ArgumentNullException ||
				exception is ArgumentOutOfRangeException)
			{
				Log.Error(exception.ToString());
			}
			catch (Exception exception)
			{
				Log.Error(exception.ToString());

				throw;
			}
		}

		public static byte[] ReadBinaryOutput(Stream output)
		{
			byte[] outputBytes = null;

			if (output != null)
			{
				using (Stream writeStream = new MemoryStream())
				{
					using (BinaryWriter binaryOutput =
						new BinaryWriter(writeStream))
					{
						int currentByte = 0;

						while (-1 != currentByte)
						{
							currentByte = output.ReadByte();

							if (-1 != currentByte)
							{
								byte writeByte = Convert.ToByte(currentByte);
								binaryOutput.BaseStream.WriteByte(writeByte);
							}
						}

						outputBytes = new byte[binaryOutput.BaseStream.Length];
						binaryOutput.BaseStream.Write(
							outputBytes, 0, (int)binaryOutput.BaseStream.Length);
					}
				}
			}

			return outputBytes;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Replace string in file by regular expression.
		/// </summary>
		/// <param name="filePath">The path of the file.</param>
		/// <param name="oldValue">The value to be replaced.</param>
		/// <param name="newValue">The value of the replacement.</param>
		/////////////////////////////////////////////////////////////////////
		public static void RegexStringInFile(
			string filePath, string oldValue, string newValue)
		{
			string contents;

			if (File.Exists(filePath))
			{
				using (StreamReader sr = new StreamReader(filePath))
				{
					if (sr != null)
					{
						contents = sr.ReadToEnd();

						using (FileStream stream = new FileStream(
							filePath, FileMode.Open, FileAccess.ReadWrite))
						{
							using (StreamWriter writer =
								new StreamWriter(stream))
							{
								contents = Regex.Replace(
									contents, oldValue, newValue);

								writer.Write(contents);
							}
						}
					}
				}
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Replace a string in a file.
		/// </summary>
		/// <param name="filePath">The path of the file.</param>
		/// <param name="oldValue">The text to replace.</param>
		/// <param name="newValue">The text of replacement.</param>
		/////////////////////////////////////////////////////////////////////
		public static void ReplaceStringInFile(
			string filePath, string oldValue, string newValue)
		{
			if (File.Exists(filePath))
			{
				using (FileStream file = File.Open(
					filePath,
					FileMode.Open,
					FileAccess.ReadWrite,
					FileShare.None))
				{
					using (StreamReader reader = new StreamReader(file))
					{
						string contents = reader.ReadToEnd();

						string newContents =
							contents.Replace(oldValue, newValue);

						using (StreamWriter writer = new StreamWriter(file))
						{
							writer.Write(newContents);

							// sw.Close();
						}
					}
				}
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="fileContents">The contents of the file.</param>
		/// <param name="filePathName">The full path of the file.</param>
		/// <returns>A value indicating success.</returns>
		/////////////////////////////////////////////////////////////////////
		public static bool SaveFile(
			string fileContents, string filePathName)
		{
			return SaveFile(fileContents, filePathName, Encoding.Default);
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="fileContents">The contents of the file.</param>
		/// <param name="filePathName">The full path of the file.</param>
		/// <param name="encoding">The encoding to use to save
		/// the text.</param>
		/// <returns>A value indicating success.</returns>
		/////////////////////////////////////////////////////////////////////
		[System.Diagnostics.CodeAnalysis.SuppressMessage(
			"Microsoft.Usage",
			"CA2202:Do not dispose objects multiple times",
			Justification = "A harmless warning and difficult to get rid of.")]
		public static bool SaveFile(
			string fileContents, string filePathName, Encoding encoding)
		{
			bool successCode = false;

			if (!string.IsNullOrWhiteSpace(filePathName))
			{
				FileStream fileStream = null;
				StreamWriter streamWriter = null;
				try
				{
					fileStream = new FileStream(
						filePathName, FileMode.Create, FileAccess.ReadWrite);

					streamWriter = new StreamWriter(fileStream, encoding);
					streamWriter.Write(fileContents);

					successCode = true;
				}
				catch (Exception exception)
				{
					Log.Error(
						CultureInfo.InvariantCulture,
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
		/// <param name="fileContents">The contents of the file.</param>
		/// <param name="filePathName">The full path of the file.</param>
		/// <returns>A value indicating success.</returns>
		/////////////////////////////////////////////////////////////////////
		public static bool SaveFileUtf8Bom(
			string fileContents, string filePathName)
		{
			UTF8Encoding utf8EmitBom = new UTF8Encoding(true);
			return SaveFile(fileContents, filePathName, utf8EmitBom);
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="fileContents">The contents of the file.</param>
		/// <param name="filePathName">The full path of the file.</param>
		/// <returns>A value indicating success.</returns>
		/////////////////////////////////////////////////////////////////////
		public static bool SaveFileWin(
			string fileContents, string filePathName)
		{
			fileContents =
				Regex.Replace(fileContents, @"\r\n|\n\r|\n|\r", "\r\n");

			return SaveFile(fileContents, filePathName);
		}

		public static void StreamCopy(Stream input, Stream output)
		{
			int i;
			byte b;

			if ((input != null) && (output != null))
			{
				i = input.ReadByte();

				while (i != -1)
				{
					b = (byte)i;
					output.WriteByte(b);

					i = input.ReadByte();
				}
			}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Touch the file path with the time.
		/// </summary>
		/// <param name="path">The path of the file to touch.</param>
		/// <param name="time">The time to set the file time to.</param>
		/////////////////////////////////////////////////////////////////////
		public static void Touch(string path, string time)
		{
			string[] format = { "yyyyMMddHHmmss" };

			if (DateTime.TryParseExact(
				time,
				format,
				CultureInfo.InvariantCulture,
				DateTimeStyles.None,
				out DateTime dateTime))
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
		/// Update the given file so that all lines end with CRLF.
		/// </summary>
		/// <param name="filePath">The path of the file to update.</param>
		/////////////////////////////////////////////////////////////////////
		public static void UpdateFileMacToWindows(string filePath)
		{
			RegexStringInFile(filePath, "\r\n|\r|\n", "\r\n");
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Update the given file so that all lines end with CRLF.
		/// </summary>
		/// <param name="filePath">The path of the file to update.</param>
		/////////////////////////////////////////////////////////////////////
		public static void UpdateFileLineEndingUnixToWindows(string filePath)
		{
			RegexStringInFile(filePath, "\r\n|\r|\n", "\r\n");
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage(
			"Microsoft.Usage",
			"CA2202:Do not dispose objects multiple times",
			Justification = "The warning seems to be a mistake.")]
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