using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    public class DBConnnection
    {
        private static readonly string con = @"Data Source=LAPTOP-Q286H62N; Initial Catalog=Biblioteka; Integrated Security=True";
        public static string getConnection() => con;
    }
}
