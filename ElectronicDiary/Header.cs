using Microsoft.Data.SqlClient;

namespace ElectronicDiary
{
    internal class Header
    {
        public static SqlConnection SqlConnection { get; } = new SqlConnection("Data Source=desktop-neuqaj1\\sqlexpress;Initial Catalog=\"electronic diary\";Integrated Security=True;Trust Server Certificate=True");
    }
}