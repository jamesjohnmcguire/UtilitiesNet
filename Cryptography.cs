/////////////////////////////////////////////////////////////////////////////
// <copyright file="Cryptography.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// The <see cref="System.Security.Cryptography.Crypto"/> provides
	/// an easy way encrypt and decrypt data using a simple password.
	/// </summary>
	/// <remarks>
	/// Code based on the book "C# 3.0 in a nutshell
	/// by Joseph Albahari" (pages 630-632)
	/// and from this StackOverflow post by somebody called Brett
	/// http://stackoverflow.com/questions/202011/encrypt-decrypt-string-in-net/2791259#2791259.
	/// </remarks>
	internal static class Cryptography
	{
		// Define the secret salt value for encrypting data
		private static readonly byte[] Salt =
			Encoding.ASCII.GetBytes("Xamarin.iOS Version: 7.0.6.168");

		/// <summary>
		/// Takes the given text and encrypts it using the given password.
		/// </summary>
		/// <param name="textToEncrypt">Text to encrypt.</param>
		/// <param name="encryptionPassword">Encryption password.</param>
		/// <returns>The encrypted text.</returns>
		internal static string Encrypt(
			string textToEncrypt, string encryptionPassword)
		{
			string encryptedString = null;

			// Anything to process?
			if (!string.IsNullOrEmpty(textToEncrypt))
			{
				using (RijndaelManaged algorithm =
					GetAlgorithm(encryptionPassword))
				{
					byte[] encryptedBytes;
					using (ICryptoTransform encryptor =
						algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
					{
						byte[] bytesToEncrypt =
							Encoding.UTF8.GetBytes(textToEncrypt);
						encryptedBytes =
							InMemoryCrypt(bytesToEncrypt, encryptor);
					}

					encryptedString = Convert.ToBase64String(encryptedBytes);
				}
			}

			return encryptedString;
		}

		/// <summary>
		/// Takes the given encrypted text and decrypts it
		/// using the given password.
		/// </summary>
		/// <param name="encryptedText">Encrypted text.</param>
		/// <param name="encryptionPassword">Encryption password.</param>
		/// <returns>The decrypted text.</returns>
		internal static string Decrypt(
			string encryptedText, string encryptionPassword)
		{
			string decryptedString = null;
			if ((!string.IsNullOrWhiteSpace(encryptedText)) &&
				IsBase64String(encryptedText))
			{
				using (RijndaelManaged algorithm =
					GetAlgorithm(encryptionPassword))
				{
					byte[] descryptedBytes;
					using (ICryptoTransform decryptor =
						algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
					{
						byte[] encryptedBytes =
							Convert.FromBase64String(encryptedText);
						descryptedBytes =
							InMemoryCrypt(encryptedBytes, decryptor);
					}

					decryptedString = Encoding.UTF8.GetString(descryptedBytes);
				}
			}

			return decryptedString;
		}

		/// <summary>
		/// Performs an in-memory encrypt/decrypt
		/// transformation on a byte array.
		/// </summary>
		/// <returns>The memory crypt.</returns>
		/// <param name="data">Data.</param>
		/// <param name="transform">Transform.</param>
		private static byte[] InMemoryCrypt(
			byte[] data, ICryptoTransform transform)
		{
			MemoryStream memory = new MemoryStream();
			using (Stream stream =
				new CryptoStream(memory, transform, CryptoStreamMode.Write))
			{
				stream.Write(data, 0, data.Length);
			}

			return memory.ToArray();
		}

		/// <summary>
		/// Defines a RijndaelManaged algorithm and sets its key and
		/// Initialization Vector (IV)
		/// values based on the encryptionPassword received.
		/// </summary>
		/// <returns>The algorithm.</returns>
		/// <param name="encryptionPassword">Encryption password.</param>
		private static RijndaelManaged GetAlgorithm(string encryptionPassword)
		{
			RijndaelManaged algorithm = new RijndaelManaged();

			using (Rfc2898DeriveBytes key =
				new Rfc2898DeriveBytes(encryptionPassword, Salt))
			{
				// Declare that we are going to use the Rijndael algorithm
				// with the key that we've just got.
				int bytesForKey = algorithm.KeySize / 8;
				int bytesForIV = algorithm.BlockSize / 8;
				algorithm.Key = key.GetBytes(bytesForKey);
				algorithm.IV = key.GetBytes(bytesForIV);
			}

			return algorithm;
		}

		private static bool IsBase64String(string test)
		{
			bool valid = false;

			test = test.Trim();
			if ((test.Length % 4 == 0) &&
				Regex.IsMatch(
					test,
					@"^[a-zA-Z0-9\+/]*={0,3}$",
					RegexOptions.None))
			{
				valid = true;
			}

			return valid;
		}
	}
}
