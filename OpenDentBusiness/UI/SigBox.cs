using CodeBase;
using DataConnectionBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.UI
{
    public class SigBox
	{
		/// <summary>
		/// When we fetch webforms, they are not yet formated as our SheetField objects, so pass in as a datarow.
		/// </summary>
		public static string GetSignatureKeySheets(List<DataRow> sheetRows)
		{
			var sheetFields = sheetRows.Select(
				dataRow =>
					new SheetField
					{
						FieldValue = SIn.String(dataRow["FieldValue"].ToString()),
						FieldType = SIn.Enum<SheetFieldType>(dataRow["FieldType"].ToString())
					});

			return GetSignatureKeySheets(sheetFields);
		}

		/// <summary>
		/// Get the key used to encrypt the signature in a sheet.
		/// The key is made up of all the field values in the sheet in the order they were inserted into the db.
		/// This method assumes the list of sheet fields was already sorted.
		/// </summary>
		public static string GetSignatureKeySheets(IEnumerable<SheetField> sheetFields)
		{
			var stringBuilder = new StringBuilder();

			foreach (var sheetField in sheetFields)
            {
				if (string.IsNullOrEmpty(sheetField.FieldValue))
					continue;

				if (sheetField.FieldType == SheetFieldType.SigBox &&
					sheetField.FieldType == SheetFieldType.SigBoxPractice)
					continue;

				stringBuilder.Append(sheetField.FieldValue);
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Encrypts signature text and returns a base 64 string so that it can go directly into the database.
		/// Takes in a hashed key, and a string describing the signature using semi-colon separated points (i.e. "x1,y1;x2,y2;").
		/// </summary>
		public static string EncryptSignatureString(byte[] key, string signatureString)
		{
			if (string.IsNullOrWhiteSpace(signatureString))
			{
				return "";
			}

			var signatureData = Encoding.UTF8.GetBytes(signatureString);

            using var memoryStream = new MemoryStream();
            using var rijndael = Rijndael.Create();

            rijndael.KeySize = 128;
            rijndael.Key = key;
            rijndael.IV = new byte[16];

            using var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);

            cryptoStream.Write(signatureData, 0, signatureData.Length);
            cryptoStream.FlushFinalBlock();

            byte[] encryptedBytes = new byte[memoryStream.Length];

            memoryStream.Position = 0;
            memoryStream.Read(encryptedBytes, 0, (int)memoryStream.Length);

            return Convert.ToBase64String(encryptedBytes);
        }
	}
}
