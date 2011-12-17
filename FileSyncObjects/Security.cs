using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Defines actions that are untertaken to ensure security of the operations.
	/// </summary>
	[DataContract]
	public class Security {

		private Security() { }

		/// <summary>
		/// Computes a hash of a given arbitrary string
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ComputeHash(string input) {

			byte[] hashBytes = new MD5CryptoServiceProvider()
				.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));

			StringBuilder hash = new StringBuilder();
			foreach (byte b in hashBytes) {
				char c = (char)b;
				hash.Append(c);
			}

			return hash.ToString();
		}

	}
}
