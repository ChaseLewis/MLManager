using System.Text;

namespace MLManager.Database
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string str)
        {
            StringBuilder builder = new StringBuilder(str.Length+4);

            for(int i = 0;i < str.Length;i++)
            { 
                if(char.IsUpper(str[i]) && i > 0 && char.IsLower(str[i-1]))
                {
                    builder.Append('_');
                }

                builder.Append(char.ToLower(str[i]));
            }

            return builder.ToString();
        }
    }
}