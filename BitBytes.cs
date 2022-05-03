/////////////////////////////////////////////////////////////////////////////
// <copyright file="BitBytes.cs" company="James John McGuire">
// Copyright © 2006 - 2022 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using Common.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// BitBytes class.
	/// </summary>
	public static class BitBytes
	{
		private static readonly ILog Log = LogManager.GetLogger(
			MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Copies the array if existent.
		/// </summary>
		/// <param name="source">The source buffer.</param>
		/// <param name="destination">The destination buffer.</param>
		/// <param name="index">The destination index.</param>
		/// <returns>The updated index.</returns>
		public static long ArrayCopyConditional(
			byte[] source, ref byte[] destination, long index)
		{
			if (source != null)
			{
				Array.Copy(
					source,
					0,
					destination,
					index,
					source.LongLength);
				index += source.LongLength;
			}

			return index;
		}

		/// <summary>
		/// Copy an integer into a byte array.
		/// </summary>
		/// <param name="bytes">The byte array.</param>
		/// <param name="index">The index to copy at.</param>
		/// <param name="value">The integer value.</param>
		/// <returns>The updated byte array.</returns>
		public static byte[] CopyIntToByteArray(
			byte[] bytes, long index, int value)
		{
			byte byteValue1 = (byte)value;
			byte byteValue2 = (byte)(value >> 8);
			byte byteValue3 = (byte)(value >> 0x10);
			byte byteValue4 = (byte)(value >> 0x18);

			if (bytes != null)
			{
				bytes[index] = byteValue1;
				index++;
				bytes[index] = byteValue2;
				index++;
				bytes[index] = byteValue3;
				index++;
				bytes[index] = byteValue4;
			}

			return bytes;
		}

		/// <summary>
		/// Copy an unsigned short into a byte array.
		/// </summary>
		/// <param name="bytes">The byte array.</param>
		/// <param name="index">The index to copy at.</param>
		/// <param name="value">The unsigned short value.</param>
		/// <returns>The updated byte array.</returns>
		public static byte[] CopyUshortToByteArray(
			byte[] bytes, long index, ushort value)
		{
			byte byteValue1 = (byte)value;
			byte byteValue2 = (byte)(value >> 8);

			if (bytes != null)
			{
				bytes[index] = byteValue1;
				index++;
				bytes[index] = byteValue2;
			}

			return bytes;
		}

		/// <summary>
		/// Finds an item in a byte array.
		/// </summary>
		/// <param name="haystack">The byte array haystack.</param>
		/// <param name="needle">The needle byte.</param>
		/// <returns>The index of the bytes found.</returns>
		public static int FindInByteArray(byte[] haystack, byte[] needle)
		{
			int foundPosition = -1;
			int mayHaveFoundIt = -1;
			int miniCounter = 0;

			if ((haystack != null) && (needle != null))
			{
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
			}

			return foundPosition;
		}

		/// <summary>
		/// Get bit value, as boolean.
		/// </summary>
		/// <param name="rawValue">The byte with the bit to check.</param>
		/// <param name="bitNumber">The 0 based index of the bit
		/// to retrieve.</param>
		/// <returns>The bit value, as boolean.</returns>
		public static bool GetBit(byte rawValue, byte bitNumber)
		{
			bool bitValue = false;

			// 0 based
			byte shifter2 = (byte)(1 << bitNumber);
			byte bit = (byte)(rawValue & shifter2);

			if (bit != 0)
			{
				bitValue = true;
			}

			return bitValue;
		}

		/// <summary>
		/// Replace in byte array.
		/// </summary>
		/// <param name="originalArray">The byte array.</param>
		/// <param name="find">The bytes to replace.</param>
		/// <param name="replace">The replacement bytes.</param>
		/// <returns>The new byte array.</returns>
		public static byte[] ReplaceInByteArray(
			byte[] originalArray, byte[] find, byte[] replace)
		{
			byte[] returnValue = originalArray;

			if ((originalArray != null) && (find != null) && (replace != null))
			{
				if (System.Array.BinarySearch(returnValue, find) > -1)
				{
					byte[] newReturnValue;
					int foundPosition;
					int currentPosition;
					int currentOriginalPosition;
					while (FindInByteArray(returnValue, find) > -1)
					{
						int length =
							returnValue.Length + replace.Length - find.Length;
						newReturnValue = new byte[length];

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

						currentOriginalPosition += find.Length;

						while (currentPosition < newReturnValue.Length)
						{
							newReturnValue[currentPosition] =
								returnValue[currentOriginalPosition];
							currentPosition++;
							currentOriginalPosition++;
						}

						returnValue = newReturnValue;
					}
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Set a specific bit.
		/// </summary>
		/// <param name="holder">The byte holding the bit to set.</param>
		/// <param name="bitIndex">The bit index.</param>
		/// <param name="value">The value to set.</param>
		/// <returns>The updated byte holding the bit.</returns>
		public static byte SetBit(byte holder, byte bitIndex, bool value)
		{
			int intValue = Convert.ToInt32(value);

			// 0 based
			int shifter = intValue << bitIndex;
			int intHolder = holder | shifter;
			holder = (byte)intHolder;

			return holder;
		}

		/// <summary>
		/// Set a specific bit.
		/// </summary>
		/// <param name="holder">The byte holding the bit to set.</param>
		/// <param name="bitIndex">The bit index.</param>
		/// <param name="value">The value to set.</param>
		/// <returns>The updated byte holding the bit.</returns>
		public static ushort SetBit(ushort holder, byte bitIndex, bool value)
		{
			int intValue = Convert.ToInt32(value);

			// 0 based
			int shifter = intValue << bitIndex;
			int intHolder = holder | shifter;
			holder = (ushort)intHolder;

			return holder;
		}

		/// <summary>
		/// Converts a string to a byte array.
		/// </summary>
		/// <param name="original">The string to be converted.</param>
		/// <returns>The byte array.</returns>
		public static byte[] StringToByteArray(
			string original)
		{
			Encoding encoding = Encoding.UTF8;
			byte[] byteArrary = encoding.GetBytes(original);
			return byteArrary;
		}

		/// <summary>
		/// To integer method.
		/// </summary>
		/// <param name="bytes">The source bytes.</param>
		/// <param name="index">The index with in the bytes to copy.</param>
		/// <returns>An integer of the bytes values.</returns>
		public static uint ToInteger(byte[] bytes, uint index)
		{
			uint result = ToIntegerLimit(bytes, index, 4);

			return result;
		}

		/// <summary>
		/// To integer limit method.
		/// </summary>
		/// <param name="bytes">The source bytes.</param>
		/// <param name="index">The index with in the bytes to copy.</param>
		/// <param name="limit">The amount of bytes to copy.</param>
		/// <returns>An integer of the bytes values.</returns>
		public static uint ToIntegerLimit(byte[] bytes, uint index, int limit)
		{
			uint result;

			// The converter still needs 4 bytes to act on.
			byte[] testBytes = new byte[4];
			Array.Copy(bytes, index, testBytes, 0, limit);

			// Dbx files are apprentely stored as little endian.
			if (BitConverter.IsLittleEndian == false)
			{
				Array.Reverse(testBytes);
			}

			result = BitConverter.ToUInt32(testBytes, 0);

			return result;
		}

		/// <summary>
		/// To integer array method.
		/// </summary>
		/// <param name="bytes">The source bytes.</param>
		/// <returns>An integer array of the bytes values.</returns>
		public static uint[] ToIntegerArray(byte[] bytes)
		{
			uint[] integerArray = null;

			if (bytes != null)
			{
				int size = bytes.Length / sizeof(uint);
				integerArray = new uint[size];
				Buffer.BlockCopy(bytes, 0, integerArray, 0, bytes.Length);
			}

			return integerArray;
		}

		/// <summary>
		/// To integer method.
		/// </summary>
		/// <param name="bytes">The source bytes.</param>
		/// <param name="index">The index with in the bytes to copy.</param>
		/// <returns>An integer of the bytes values.</returns>
		public static ulong ToLong(byte[] bytes, uint index)
		{
			ulong result;

			// The converter still needs 8 bytes to act on.
			byte[] testBytes = new byte[8];
			Array.Copy(bytes, index, testBytes, 0, 8);

			// Dbx files are apprentely stored as little endian.
			if (BitConverter.IsLittleEndian == false)
			{
				Array.Reverse(testBytes);
			}

			result = BitConverter.ToUInt64(testBytes, 0);

			return result;
		}

		/// <summary>
		/// To short method.
		/// </summary>
		/// <param name="bytes">The source bytes.</param>
		/// <param name="index">The index with in the bytes to copy.</param>
		/// <returns>An integer of the bytes values.</returns>
		public static ushort ToShort(byte[] bytes, int index)
		{
			ushort result;
			byte[] testBytes = new byte[2];
			Array.Copy(bytes, index, testBytes, 0, 2);

			// Dbx files are apprentely stored as little endian.
			if (BitConverter.IsLittleEndian == false)
			{
				Array.Reverse(testBytes);
			}

			result = BitConverter.ToUInt16(testBytes, 0);

			return result;
		}
	}
}
