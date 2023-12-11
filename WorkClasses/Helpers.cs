using System.Text;

namespace GCCode.Net.WorkClasses
{
    public class Helpers
    {

        /// <summary>
        /// Кодирует строки, содержащие сложые символы
        /// </summary>
        /// <param name="n_ascii"></param>
        /// <returns></returns>
        public static string EncodeNonAscIItring(string n_ascii)
        {

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(n_ascii));
        }

        /// <summary>
        /// Декожирует обратно строку после преобразования
        /// </summary>
        /// <param name="coded_str"></param>
        /// <returns></returns>
        public static string DecodeNonAscIItring (string coded_str)
        {

            return Encoding.UTF8.GetString(Convert.FromBase64String(coded_str));
        }


    }
}
