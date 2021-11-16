using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public static class Utilities
    {

        public static string CreateMD5(string input)
        {
            //simple code to generate md5Hash for Marvel's api

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            return value == null ? "" : value.ToString().OrganizeEnumString();
        }

        private static string OrganizeEnumString(this string valor)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (valor != null)
                {
                    char[] arrayChar = valor.Normalize(NormalizationForm.FormD).ToCharArray();

                    for (int i = 0; i < arrayChar.Length; i++)
                    {
                        if (char.IsUpper(arrayChar[i]) || arrayChar[i] == '_')
                        {
                            sb.Append($" {arrayChar[i]}");
                        }
                        else
                        {
                            sb.Append($"{arrayChar[i]}");
                        }
                    }
                }

                return sb.ToString().Trim();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
