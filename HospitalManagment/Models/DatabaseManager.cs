using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagment.Models
{
    public static class DatabaseManager
    {
        private static string connectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog=db_hospital_management; integrated security=SSPI;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
