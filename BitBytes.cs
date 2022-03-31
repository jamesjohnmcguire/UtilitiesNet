/////////////////////////////////////////////////////////////////////////////
// <copyright file="BitBytes.cs" company="James John McGuire">
// Copyright © 2006 - 2022 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// BitBytes class.
	/// </summary>
	public static class BitBytes
	{
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
	}
}
