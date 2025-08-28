using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary
{
    internal class Header
    {
        public static SqlConnection Conn { get; } = new SqlConnection("Data Source=desktop-neuqaj1\\sqlexpress;Initial Catalog=\"electronic diary\";Integrated Security=True;Trust Server Certificate=True");
    }
}