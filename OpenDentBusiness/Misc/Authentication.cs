using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OpenDentBusiness
{
    public static class Password
    {
		/// <summary>
		/// Hashes a password using the specified algorithm.
		/// </summary>
		/// <param name="password">The password.</param>
		/// <param name="algo">The hashing algorithm to use.</param>
		/// <returns>The hashed password.</returns>
		public static string Hash(string password, PasswordAlgorithm algo = PasswordAlgorithm.Default)
        {
			return algo switch
            {
                PasswordAlgorithm.SHA512 => HashSHA512(password),

                _ => throw new NotSupportedException("The specified password algorithm is not supported.")
            };
        }

		/// <summary>
		/// Generates a salt of the specified <paramref name="length"/>.
		/// </summary>
		/// <param name="length">The lenth of the salt.</param>
		/// <returns>The generated salt.</returns>
		private static byte[] GenerateSalt(int length)
		{
            using var randomNumberGenerator = new RNGCryptoServiceProvider();

            byte[] buffer = new byte[length];
            randomNumberGenerator.GetBytes(buffer);

            return buffer;
        }

		/// <summary>
		/// Computes the SHA-512 hash of the specified <paramref name="password"/> using the specified <paramref name="salt"/>.
		/// </summary>
		/// <param name="password">The password.</param>
		/// <param name="salt">The salt.</param>
		/// <returns>The password hash.</returns>
		private static byte[] ComputeSHA512Hash(string password, byte[] salt)
        {
			using var algorithm = new SHA512Managed();

			var data = Encoding.UTF8.GetBytes(password);
			var buffer = new byte[data.Length + salt.Length];

			Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
			Buffer.BlockCopy(salt, 0, buffer, data.Length, salt.Length);

			return algorithm.ComputeHash(buffer);
		}

		/// <summary>
		/// Generates a SHA-512 of the specified password with a random salt.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <returns></returns>
		private static string HashSHA512(string password)
        {
			var salt = GenerateSalt(60);

			return PasswordAlgorithm.SHA512.ToString() + '$' +
				Convert.ToBase64String(ComputeSHA512Hash(password, salt)) + '$' + 
				Convert.ToBase64String(salt);
		}

		/// <summary>
		/// Verifies the specified <paramref name="password"/> against the specified <paramref name="hash"/>.
		/// </summary>
		/// <param name="password">The password.</param>
		/// <param name="hash">The password hash.</param>
		/// <returns>True if the password matches the hash; otherwise, false.</returns>
		public static bool Verify(string password, string hash)
		{
			if (string.IsNullOrEmpty(password))
            {
				if (string.IsNullOrEmpty(hash))
                {
					return true;
                }

				return false;
            }

			var i = hash.IndexOf('$');

			if (i == -1)
            {
				return false;
            }

			if (Enum.TryParse<PasswordAlgorithm>(hash.Substring(0, i), out var result))
			{
				return false;
			}

			return result switch
			{
				PasswordAlgorithm.SHA512 => VerifySHA512(password, hash.Substring(i + 1)),

				_ => false
			};
		}

		/// <summary>
		/// Verifies the specified <paramref name="password"/> against the specified <paramref name="hash"/>.
		/// </summary>
		/// <param name="password">The password.</param>
		/// <param name="hash">The password hash.</param>
		/// <returns>True if the password matches the hash; otherwise, false.</returns>
		private static bool VerifySHA512(string password, string hash)
        {
			var i = hash.IndexOf('$');
			if (i == -1)
            {
				return false;
            }

			try
			{
				var salt = Convert.FromBase64String(hash.Substring(i + 1));

				var x = ComputeSHA512Hash(password, salt);
				var y = Convert.FromBase64String(hash.Substring(0, i));

				if (x.Length != y.Length)
				{
					return false;
				}

				for (i = 0; i < x.Length; i++)
				{
					if (x[i] != y[i])
					{
						return false;
					}
				}
			}
            catch
            {
				return false;
            }

			return true;
		}
	}

	/// <summary>
	/// Identifies the hashing algorithm for a password.
	/// </summary>
	public enum PasswordAlgorithm
    {
		SHA512,

		Default = SHA512
	}
}
