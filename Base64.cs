/////////////////////////////////////////////////////////////////////////////
// <copyright file="Base64.cs" company="James John McGuire">
// Copyright © 2006 - 2022 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// This class implements base64 encoder/decoder.  Defined in RFC 4648.
	/// </summary>
	public static class Base64
	{
		private static readonly ResourceManager StringTable = new
			ResourceManager(
				"DigitalZenWorks.Common.Utilities.Resources",
				Assembly.GetExecutingAssembly());

		private static readonly short[] Base64DecodeTable = new short[]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 0 -    9
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 10 -   19
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 20 -   29
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 30 -   39
			-1, -1, -1, 62, -1, -1, -1, 63, 52, 53, // 40 -   49
			54, 55, 56, 57, 58, 59, 60, 61, -1, -1, // 50 -   59
			-1, -1, -1, -1, -1, 0, 1, 2, 3, 4,      // 60 -   69
			5, 6, 7, 8, 9, 10, 11, 12, 13, 14,      // 70 -   79
			15, 16, 17, 18, 19, 20, 21, 22, 23, 24, // 80 -   89
			25, -1, -1, -1, -1, -1, -1, 26, 27, 28, // 90 -   99
			29, 30, 31, 32, 33, 34, 35, 36, 37, 38, // 100 - 109
			39, 40, 41, 42, 43, 44, 45, 46, 47, 48, // 110 - 119
			49, 50, 51, -1, -1, -1, -1, -1 // 120 - 127
		};

		/// <summary>
		/// Encodes bytes.
		/// </summary>
		/// <returns>Returns encoded data.</returns>
		public static byte[] Encode()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Decodes specified base64 string.
		/// </summary>
		/// <param name="value">Base64 string.</param>
		/// <param name="ignoreNonBase64Chars">If true all invalid base64
		/// chars ignored. If false, FormatException is raised.</param>
		/// <returns>Returns decoded data.</returns>
		/// <exception cref="ArgumentNullException">Is raised when
		/// <b>value</b> is null reference.</exception>
		/// <exception cref="FormatException">Is raised when <b>value</b>
		/// contains invalid base64 data.</exception>
		public static byte[] Decode(string value, bool ignoreNonBase64Chars)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			byte[] encBuffer = Encoding.ASCII.GetBytes(value);
			byte[] buffer = new byte[encBuffer.Length];

			int decodedCount = Decode(encBuffer, 0, encBuffer.Length, buffer, 0, ignoreNonBase64Chars);
			byte[] retVal = new byte[decodedCount];
			Array.Copy(buffer, retVal, decodedCount);

			return retVal;
		}

		/// <summary>
		/// Decodes specified base64 data.
		/// </summary>
		/// <param name="data">Base64 encoded data buffer.</param>
		/// <param name="offset">Offset in the buffer.</param>
		/// <param name="count">Number of bytes available in the buffer.</param>
		/// <param name="ignoreNonBase64Chars">If true all invalid base64 chars ignored. If false, FormatException is raised.</param>
		/// <returns>Returns decoded data.</returns>
		/// <exception cref="ArgumentNullException">Is raised when <b>data</b> is null reference.</exception>
		/// <exception cref="FormatException">Is raised when <b>value</b> contains invalid base64 data.</exception>
		public static byte[] Decode(byte[] data, int offset, int count, bool ignoreNonBase64Chars)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			byte[] buffer = new byte[data.Length];

			int decodedCount = Decode(data, offset, count, buffer, 0, ignoreNonBase64Chars);
			byte[] retVal = new byte[decodedCount];
			Array.Copy(buffer, retVal, decodedCount);

			return retVal;
		}

		/// <summary>
		/// Decodes base64 encoded bytes.
		/// </summary>
		/// <param name="encodeBuffer">Base64 encoded data buffer.</param>
		/// <param name="encodeOffset">Offset in the encodeBuffer.</param>
		/// <param name="encodeCount">Number of bytes available in the encodeBuffer.</param>
		/// <param name="buffer">Buffer where to decode data.</param>
		/// <param name="offset">Offset int the buffer.</param>
		/// <param name="ignoreNonBase64Chars">If true all invalid base64 chars ignored. If false, FormatException is raised.</param>
		/// <returns>Returns number of bytes decoded.</returns>
		/// <exception cref="ArgumentNullException">Is raised when <b>encodeBuffer</b> or <b>encodeBuffer</b> is null reference.</exception>
		/* RFC 4648.
			Base64 is processed from left to right by 4 6-bit byte block, 4 6-bit byte block
			are converted to 3 8-bit bytes.
			If base64 4 byte block doesn't have 3 8-bit bytes, missing bytes are marked with =.

			Value Encoding  Value Encoding  Value Encoding  Value Encoding
				0 A            17 R            34 i            51 z
				1 B            18 S            35 j            52 0
				2 C            19 T            36 k            53 1
				3 D            20 U            37 l            54 2
				4 E            21 V            38 m            55 3
				5 F            22 W            39 n            56 4
				6 G            23 X            40 o            57 5
				7 H            24 Y            41 p            58 6
				8 I            25 Z            42 q            59 7
				9 J            26 a            43 r            60 8
				10 K           27 b            44 s            61 9
				11 L           28 c            45 t            62 +
				12 M           29 d            46 u            63 /
				13 N           30 e            47 v
				14 O           31 f            48 w         (pad) =
				15 P           32 g            49 x
				16 Q           33 h            50 y

			NOTE: 4 base64 6-bit bytes = 3 8-bit bytes
				// |    6-bit    |    6-bit    |    6-bit    |    6-bit    |
				// | 1 2 3 4 5 6 | 1 2 3 4 5 6 | 1 2 3 4 5 6 | 1 2 3 4 5 6 |
				// |    8-bit         |    8-bit        |    8-bit         |
		*/
		public static int Decode(
			byte[] encodeBuffer,
			int encodeOffset,
			int encodeCount,
			byte[] buffer,
			int offset,
			bool ignoreNonBase64Chars)
		{
			int decodedOffset = 0;

			if (encodeBuffer == null)
			{
				throw new ArgumentNullException(nameof(encodeBuffer));
			}

			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			if ((encodeOffset >= 0) && (encodeCount >= 0) &&
				(encodeOffset + encodeCount <= encodeBuffer.Length) &&
				(offset >= 0 || offset < buffer.Length))
			{
				int decodeOffset = encodeOffset;
				byte[] base64Block = new byte[4];

				// Decode while we have data.
				while ((decodeOffset - encodeOffset) < encodeCount)
				{
					// Read 4-byte base64 block.
					int offsetInBlock = 0;
					while (offsetInBlock < 4)
					{
						// Check that we won't exceed buffer data.
						if ((decodeOffset - encodeOffset) >= encodeCount)
						{
							if (offsetInBlock == 0)
							{
								break;
							}
							else
							{
								// Incomplete 4-byte base64 data block.
								string message = StringTable.GetString(
									"INCOMPLETE_BASE64_BLOCK",
									CultureInfo.InstalledUICulture);

								throw new FormatException(message);
							}
						}

						// Read byte.
						short currentByte = encodeBuffer[decodeOffset++];

						// Pad char.
						if (currentByte == '=')
						{
							// Padding may appear only in last two chars of 4-char block.
							// ab==
							// abc=
							if (offsetInBlock < 2)
							{
								string message = StringTable.GetString(
									"INVALID_BASE64_PADDING",
									CultureInfo.InstalledUICulture);

								throw new FormatException(message);
							}

							// Skip next padding char.
							if (offsetInBlock == 2)
							{
								decodeOffset++;
							}

							break;
						}
						else if (currentByte > 127 || Base64DecodeTable[currentByte] == -1)
						{
							// Non-base64 char.
							if (!ignoreNonBase64Chars)
							{
								string message = string.Format(
									CultureInfo.InvariantCulture,
									"Invalid base64 char '{0}'.",
									currentByte.ToString(
										CultureInfo.InvariantCulture));

								throw new FormatException(message);
							}
						}
						else
						{
							base64Block[offsetInBlock++] =
								(byte)Base64DecodeTable[currentByte];
						}
					}

					// Decode base64 block.
					if (offsetInBlock > 1)
					{
						buffer[decodedOffset++] =
							(byte)((base64Block[0] << 2) | (base64Block[1] >> 4));
					}

					if (offsetInBlock > 2)
					{
						buffer[decodedOffset++] =
							(byte)(((base64Block[1] & 0xF) << 4) |
							(base64Block[2] >> 2));
					}

					if (offsetInBlock > 3)
					{
						buffer[decodedOffset++] =
							(byte)(((base64Block[2] & 0x3) << 6) | base64Block[3]);
					}
				}
			}

			return decodedOffset;
		}
	}
}
