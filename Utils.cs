using System;
using System.IO;

namespace Zenware.Common.UtilsNet
{
	public static class Utils
	{
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

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validates command line parameters
		/// </summary>
		/// <param name="FilePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		private static bool CheckCommandLineParameters(
			string[] Parameters)
		{
			bool IsValid = false;

			// Ensure we have a valid file name
			if (Parameters.Length < 1)
			{
				Console.WriteLine("usage: ");
			}
			else
			{
				IsValid = true;
			}

			return IsValid;
		}

		public static bool IsDate(Object obj)
		{
			string strDate = obj.ToString();
			try
			{
				DateTime dt;
				DateTime.TryParse(strDate, out dt);
				if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
					return true;
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="FileContents"></param>
		/// <param name="FilePathName"></param>
		/// <returns></returns>
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

		public static string ToProperCase(
			string UnformattedString)
		{
			string formattedText = null;

			if (null != UnformattedString)
			{
				formattedText = new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(UnformattedString.ToLower());
			}

			return formattedText;
		}
	} // End Class
} // End Namespace