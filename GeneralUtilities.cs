/////////////////////////////////////////////////////////////////////////////
// <copyright file="GeneralUtilities.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using Common.Logging;
using DigitalZenWorks.Common.Utilities.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utilities
{
	public static class GeneralUtilities
	{
		private static readonly ILog Log = LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static string CallingMethod()
		{
			StackFrame stackFrame = new StackFrame(1);
			MethodBase methodBase = stackFrame.GetMethod();

			string methodName = methodBase.Name;
			if (methodBase.Name.StartsWith(
				"<", StringComparison.Ordinal))
			{
				methodName = methodBase.Name.Substring(1);
				int index = methodName.IndexOf('>');

				if (index > 0)
				{
					methodName = methodName.Substring(0, index);
				}
			}

			return methodName;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validates command line parameters.
		/// </summary>
		/// <param name="parameters">The command line parameters.</param>
		/// <returns>Returns true if there is at least one parameter,
		/// otherwise false.</returns>
		/////////////////////////////////////////////////////////////////////
		public static bool CheckCommandLineParameters(string[] parameters)
		{
			bool isValid = false;

			// Ensure we have a valid file name
			if ((null != parameters) && (parameters.Length < 1))
			{
				// Console.WriteLine("usage: ");
			}
			else
			{
				isValid = true;
			}

			return isValid;
		}

		/// <summary>
		/// Gets the name in camel case.
		/// </summary>
		/// <param name="knrName">A string in knr format.</param>
		/// <returns>A name in camel case.</returns>
		public static string ConvertToCamelCaseFromKnr(string knrName)
		{
			return ConvertFromKnrText(knrName, true);
		}

		/// <summary>
		/// Gets the name in pascal case.
		/// </summary>
		/// <param name="knrName">The name of variable in knr form.</param>
		/// <returns>A variable name in Pascal case form.</returns>
		public static string ConvertToPascalCaseFromKnr(string knrName)
		{
			return ConvertFromKnrText(knrName, false);
		}

		public static DateTime DateFromString(string dateText)
		{
			DateTime date = DateTime.MinValue;
			DateTime testDate = DateTime.MinValue;

			if (DateTime.TryParse(dateText, out testDate))
			{
				date = testDate;
			}

			return date;
		}

		public static byte[] Execute(
			string filename, string arguments, byte[] standardInput)
		{
			Process externalProgram = new Process();

			externalProgram.StartInfo.UseShellExecute = false;
			externalProgram.StartInfo.CreateNoWindow = true;
			externalProgram.StartInfo.ErrorDialog = false;

			if (null != standardInput)
			{
				externalProgram.StartInfo.RedirectStandardInput = true;
			}

			externalProgram.StartInfo.RedirectStandardOutput = true;

			externalProgram.StartInfo.FileName = filename;
			externalProgram.StartInfo.Arguments = arguments;
			externalProgram.Start();

			if (standardInput != null)
			{
				// Prepare incoming binary stream to stdin
				// hook up stdin
				StreamWriter input = externalProgram.StandardInput;

				// write bytes
				input.BaseStream.Write(standardInput, 0, standardInput.Length);
			}

			Stream outputStream = externalProgram.StandardOutput.BaseStream;
			byte[] outputBytes = FileUtils.ReadBinaryOutput(outputStream);

			return outputBytes;
		}

		public static int FindInByteArray(byte[] haystack, byte[] needle)
		{
			int foundPosition = -1;
			int mayHaveFoundIt = -1;
			int miniCounter = 0;

			for (int counter = 0; counter < haystack.Length; counter++)
			{
				if (haystack[counter] == needle[miniCounter])
				{
					if (miniCounter == 0)
					{
						mayHaveFoundIt = counter;
					}

					if (miniCounter == needle.Length - 1)
					{
						return mayHaveFoundIt;
					}

					miniCounter++;
				}
				else
				{
					mayHaveFoundIt = -1;
					miniCounter = 0;
				}
			}

			return foundPosition;
		}

		/// <summary>
		/// Decodes specified base64 data.
		/// </summary>
		/// <param name="data">Base64 string.</param>
		/// <returns>Returns decoded data.</returns>
		/// <exception cref="ArgumentNullException">Is raised when <b>data</b> is null reference.</exception>
		public static byte[] FromBase64(string data)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			return Base64.Decode(data, true);
		}

		/// <summary>
		/// Decodes specified base64 data.
		/// </summary>
		/// <param name="data">Base64 data.</param>
		/// <returns>Returns decoded data.</returns>
		/// <exception cref="ArgumentNullException">Is raised when <b>data</b> is null reference.</exception>
		public static byte[] FromBase64(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			return Base64.Decode(data, 0, data.Length, true);
		}

		/// <summary>
		/// Converts hex byte data to normal byte data. Hex data must be in
		/// two bytes pairs, for example: 0F,FF,A3,... .
		/// </summary>
		/// <param name="hexData">Hex data.</param>
		/// <returns>Returns decoded data.</returns>
		public static byte[] FromHex(byte[] hexData)
		{
			byte[] data = null;

			if (hexData == null)
			{
				throw new ArgumentNullException(nameof(hexData));
			}

			if (hexData.Length < 2 ||
				(hexData.Length / 2D != Math.Floor(hexData.Length / 2D)))
			{
				throw new ArgumentException("Illegal hex data, hex data " +
					"must be in two bytes pairs, for example: 0F,FF,A3,... .");
			}

			using (MemoryStream retVal = new MemoryStream(hexData.Length / 2))
			{
				data = GetHexPair(hexData, retVal);
			}

			return data;
		}

		public static string GetPackageVersion(string packageId)
		{
			string version = string.Empty;

			if (!string.IsNullOrWhiteSpace(packageId))
			{
				// get the package file, based on current directory
				string contents = FileUtils.GetFileContents("packages.config");

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
		/// Gets if the specified string is ASCII string.
		/// </summary>
		/// <param name="value">String value.</param>
		/// <returns>Returns true if specified string is ASCII string, otherwise false.</returns>
		public static bool IsAscii(string value)
		{
			bool isAscii = true;

			if (string.IsNullOrWhiteSpace(value))
			{
				isAscii = false;
			}
			else
			{
				foreach (char c in value)
				{
					if ((int)c > 127)
					{
						isAscii = false;
						break;
					}
				}
			}

			return isAscii;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Is object a date.
		/// </summary>
		/// <param name="value">The object to check.</param>
		/// <returns>Returns true if the object is a date,
		/// otherwise false.</returns>
		/////////////////////////////////////////////////////////////////////
		public static bool IsDate(object value)
		{
			bool successCode = false;

			if (value != null)
			{
				string dateString = value.ToString();

				if (DateTime.TryParse(dateString, out DateTime date))
				{
					if (date != DateTime.MinValue &&
						date != DateTime.MaxValue)
					{
						successCode = true;
					}
				}
			}

			return successCode;
		}

		/// <summary>
		/// Checks if specified string is integer(int/long).
		/// </summary>
		/// <param name="value">The item to test.</param>
		/// <returns>Returns true if specified string is integer.</returns>
		public static bool IsInteger(string value)
		{
			bool isInteger = false;

			if (!string.IsNullOrWhiteSpace(value))
			{
				isInteger = long.TryParse(value, out long l);
			}

			return isInteger;
		}

		/// <summary>
		/// Checks to see if the given email address is valid.
		/// </summary>
		/// <param name="emailAddress">The email address to check.</param>
		/// <returns>Returns true if the email address is valid,
		/// otherwise false.</returns>
		public static bool IsValidEmailAddress(string emailAddress)
		{
			bool valid = false;

			// validEmailRegEx = "/^([a-zA-Z0-9])+([a-zA-Z0-9\._-])" +
			// "*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$/";

			// string validEmailRegEx = @"/^([a-z0-9])(([-a-z0-9._])" +
			// "*([a-z0-9]))*\@([a-z0-9])(([a-z0-9-])*([a-z0-9]))" +
			// "+(\.([a-z0-9])([-a-z0-9_-])?([a-z0-9])+)+$/i";
			string validEmailRegEx =
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
				@"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
				@"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
				@"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

			// checks proper syntax
			Match match = Regex.Match(emailAddress, validEmailRegEx);

			if (true == match.Success)
			{
				valid = true;
			}

			return valid;
		}

		/// <summary>
		/// quoted-printable decoder. Defined in RFC 2045 6.7.
		/// </summary>
		/// <param name="data">Data which to encode.</param>
		/// <returns>Returns decoded data.</returns>
		/// <exception cref="ArgumentNullException">Is raised when <b>data</b> is null reference.</exception>
		public static byte[] QuotedPrintableDecode(byte[] data)
		{
			byte[] returnData = null;
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			/* RFC 2045 6.7. Quoted-Printable Content-Transfer-Encoding

				(1) (General 8bit representation) Any octet, except a CR or
					LF that is part of a CRLF line break of the canonical
					(standard) form of the data being encoded, may be
					represented by an "=" followed by a two digit
					hexadecimal representation of the octet's value.  The
					digits of the hexadecimal alphabet, for this purpose,
					are "0123456789ABCDEF".  Uppercase letters must be
					used; lowercase letters are not allowed.

				(2) (Literal representation) Octets with decimal values of
					33 through 60 inclusive, and 62 through 126, inclusive,
					MAY be represented as the US-ASCII characters which
					correspond to those octets (EXCLAMATION POINT through
					LESS THAN, and GREATER THAN through TILDE, respectively).

				(3) (White Space) Octets with values of 9 and 32 MAY be
					represented as US-ASCII TAB (HT) and SPACE characters,
					respectively, but MUST NOT be so represented at the end
					of an encoded line.  Any TAB (HT) or SPACE characters
					on an encoded line MUST thus be followed on that line
					by a printable character.  In particular, an "=" at the
					end of an encoded line, indicating a soft line break
					(see rule #5) may follow one or more TAB (HT) or SPACE
					characters.  It follows that an octet with decimal
					value 9 or 32 appearing at the end of an encoded line
					must be represented according to Rule #1.  This rule is
					necessary because some MTAs (Message Transport Agents,
					programs which transport messages from one user to
					another, or perform a portion of such transfers) are
					known to pad lines of text with SPACEs, and others are
					known to remove "white space" characters from the end
					of a line.  Therefore, when decoding a Quoted-Printable
					body, any trailing white space on a line must be
					deleted, as it will necessarily have been added by
					intermediate transport agents.

				(4) (Line Breaks) A line break in a text body, represented
					as a CRLF sequence in the text canonical form, must be
					represented by a (RFC 822) line break, which is also a
					CRLF sequence, in the Quoted-Printable encoding.  Since
					the canonical representation of media types other than
					text do not generally include the representation of
					line breaks as CRLF sequences, no hard line breaks
					(i.e. line breaks that are intended to be meaningful
					and to be displayed to the user) can occur in the
					quoted-printable encoding of such types.  Sequences
					like "=0D", "=0A", "=0A=0D" and "=0D=0A" will routinely
					appear in non-text data represented in quoted-
					printable, of course.

				(5) (Soft Line Breaks) The Quoted-Printable encoding
					REQUIRES that encoded lines be no more than 76
					characters long.  If longer lines are to be encoded
					with the Quoted-Printable encoding, "soft" line breaks
			*/

			using (MemoryStream destination = new MemoryStream())
			{
				using (MemoryStream sourceStream = new MemoryStream(data))
				{
					int b = sourceStream.ReadByte();
					while (b > -1)
					{
						// Encoded 8-bit byte(=XX) or soft line break(=CRLF)
						if (b == '=')
						{
							byte[] buffer = new byte[2];
							int bufferCount = sourceStream.Read(buffer, 0, 2);
							if (bufferCount == 2)
							{
								// Soft line break, line spitted, just skip CRLF
								if (buffer[0] == '\r' && buffer[1] == '\n')
								{
								}
								else
								{
									// This must be encoded 8-bit byte
									try
									{
										destination.Write(FromHex(buffer), 0, 1);
									}
									catch (Exception exception) when
										(exception is ArgumentException ||
										exception is
										ArgumentOutOfRangeException)
									{
										Log.Error(
											CultureInfo.InvariantCulture,
											m => m(exception.ToString()));

										// Illegal value after =,
										// just leave it as is
										destination.WriteByte((byte)'=');
										destination.Write(buffer, 0, 2);
									}
								}
							}
							else
							{
								// Illegal =, just leave as it is
								destination.Write(buffer, 0, bufferCount);
							}
						}
						else
						{
							// Just write back all other bytes
							destination.WriteByte((byte)b);
						}

						// Read next byte
						b = sourceStream.ReadByte();
					}
				}

				returnData = destination.ToArray();
			}

			return returnData;
		}

		public static byte[] ReplaceInByteArray(
			byte[] originalArray, byte[] find, byte[] replace)
		{
			byte[] returnValue = originalArray;

			if (System.Array.BinarySearch(returnValue, find) > -1)
			{
				byte[] newReturnValue;
				int foundPosition;
				int currentPosition;
				int currentOriginalPosition;
				while (FindInByteArray(returnValue, find) > -1)
				{
					newReturnValue = new byte[returnValue.Length + replace.Length - find.Length];
					foundPosition = FindInByteArray(returnValue, find);
					currentPosition = 0;
					currentOriginalPosition = 0;

					for (int x = 0; x < foundPosition; x++)
					{
						newReturnValue[x] = returnValue[x];
						currentPosition++;
						currentOriginalPosition++;
					}

					for (int y = 0; y < replace.Length; y++)
					{
						newReturnValue[currentPosition] = replace[y];
						currentPosition++;
					}

					currentOriginalPosition = currentOriginalPosition + find.Length;

					while (currentPosition < newReturnValue.Length)
					{
						newReturnValue[currentPosition] = returnValue[currentOriginalPosition];
						currentPosition++;
						currentOriginalPosition++;
					}

					returnValue = newReturnValue;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Converts specified data to HEX string.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <returns>Returns hex string.</returns>
		public static string ToHex(byte[] data)
		{
			string hexString = string.Empty;
			if (null != data)
			{
				hexString = BitConverter.ToString(data).
					ToLower(CultureInfo.CurrentCulture).Replace("-", string.Empty);
			}

			return hexString;
		}

		/// <summary>
		/// Converts specified string to HEX string.
		/// </summary>
		/// <param name="text">String to convert.</param>
		/// <returns>Returns hex string.</returns>
		public static string ToHex(string text)
		{
			string hexString = string.Empty;
			if (!string.IsNullOrWhiteSpace(text))
			{
				hexString = BitConverter.ToString(Encoding.Default.
				GetBytes(text)).ToLower(CultureInfo.CurrentCulture).
				Replace("-", string.Empty);
			}

			return hexString;
		}

		private static string ConvertFromKnrText(
			string knrName, bool setToCamelCase)
		{
			string newCase = string.Empty;

			// split at underscores
			string[] parts = knrName.Split(
				new char[] { '_' },
				StringSplitOptions.RemoveEmptyEntries);
			bool first = true;

			// remove underscores, set parts in intended case
			foreach (string part in parts)
			{
				if ((setToCamelCase == true) && (first == true))
				{
					// Idiots!! I need the lower case text!
#pragma warning disable CA1308 // Normalize strings to uppercase
					newCase += part.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
				}
				else
				{
					newCase += part.ToProperCase();
				}

				first = false;
			}

			return newCase;
		}

		private static byte[] GetHexPair(byte[] hexData, MemoryStream retVal)
		{
			// Loop hex value pairs
			for (int i = 0; i < hexData.Length; i += 2)
			{
				byte[] hexPairInDecimal = new byte[2];

				// We need to convert hex char to decimal number,
				// for example F = 15
				for (int h = 0; h < 2; h++)
				{
					if (((char)hexData[i + h]) == '0')
					{
						hexPairInDecimal[h] = 0;
					}
					else if (((char)hexData[i + h]) == '1')
					{
						hexPairInDecimal[h] = 1;
					}
					else if (((char)hexData[i + h]) == '2')
					{
						hexPairInDecimal[h] = 2;
					}
					else if (((char)hexData[i + h]) == '3')
					{
						hexPairInDecimal[h] = 3;
					}
					else if (((char)hexData[i + h]) == '4')
					{
						hexPairInDecimal[h] = 4;
					}
					else if (((char)hexData[i + h]) == '5')
					{
						hexPairInDecimal[h] = 5;
					}
					else if (((char)hexData[i + h]) == '6')
					{
						hexPairInDecimal[h] = 6;
					}
					else if (((char)hexData[i + h]) == '7')
					{
						hexPairInDecimal[h] = 7;
					}
					else if (((char)hexData[i + h]) == '8')
					{
						hexPairInDecimal[h] = 8;
					}
					else if (((char)hexData[i + h]) == '9')
					{
						hexPairInDecimal[h] = 9;
					}
					else if (((char)hexData[i + h]) == 'A' || ((char)hexData[i + h]) == 'a')
					{
						hexPairInDecimal[h] = 10;
					}
					else if (((char)hexData[i + h]) == 'B' || ((char)hexData[i + h]) == 'b')
					{
						hexPairInDecimal[h] = 11;
					}
					else if (((char)hexData[i + h]) == 'C' || ((char)hexData[i + h]) == 'c')
					{
						hexPairInDecimal[h] = 12;
					}
					else if (((char)hexData[i + h]) == 'D' || ((char)hexData[i + h]) == 'd')
					{
						hexPairInDecimal[h] = 13;
					}
					else if (((char)hexData[i + h]) == 'E' || ((char)hexData[i + h]) == 'e')
					{
						hexPairInDecimal[h] = 14;
					}
					else if (((char)hexData[i + h]) == 'F' || ((char)hexData[i + h]) == 'f')
					{
						hexPairInDecimal[h] = 15;
					}
				}

				// Join hex 4 bit(left hex char) + 4bit(right hex char) in bytes 8 it
				retVal.WriteByte((byte)((hexPairInDecimal[0] << 4) | hexPairInDecimal[1]));
			}

			return retVal.ToArray();
		}
	} // End Class
} // End Namespace
