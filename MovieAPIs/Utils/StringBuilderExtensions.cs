using System.Text;

namespace MovieAPIs.Utils
{
    static internal class StringBuilderExtensions
    {
        static internal string ToStringWithoutLastChar(this StringBuilder sb)
        {
            return sb.Remove(sb.Length - 1, 1).ToString();
        }
    }
}
