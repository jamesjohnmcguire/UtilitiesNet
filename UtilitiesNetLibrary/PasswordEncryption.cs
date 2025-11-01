/////////////////////////////////////////////////////////////////////////////
// <copyright file="PasswordEncryption.cs" company="James John McGuire">
// Copyright © 2006 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// Simple encryption / decryption methods.
	/// </summary>
	/// <remarks>
	/// Changed to use the work of James Tuley's AESGCM.cs, as it is more
	/// modern and vetted by code reviews.
	/// </remarks>
	public static class PasswordEncryption
	{
		/// <summary>
		/// Takes the given text and encrypts it using the given password.
		/// </summary>
		/// <param name="textToEncrypt">Text to encrypt.</param>
		/// <param name="encryptionPassword">Encryption password.</param>
		/// <returns>The encrypted text.</returns>
		public static string Encrypt(
			string textToEncrypt, string encryptionPassword)
		{
			return AESGCM.SimpleEncryptWithPassword(
				textToEncrypt, encryptionPassword);
		}

		/// <summary>
		/// Takes the given encrypted text and decrypts it
		/// using the given password.
		/// </summary>
		/// <param name="encryptedText">Encrypted text.</param>
		/// <param name="encryptionPassword">Encryption password.</param>
		/// <returns>The decrypted text.</returns>
		public static string? Decrypt(
			string encryptedText, string encryptionPassword)
		{
			string? result = AESGCM.SimpleDecryptWithPassword(
				encryptedText, encryptionPassword);

			return result;
		}
	}
}
