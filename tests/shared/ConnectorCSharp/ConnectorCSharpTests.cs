using System;
using System.Collections.Generic;
using System.Text;

using SuperFastDB.Data;

namespace ConnectorCSharp
{
    public static class ConnectorCSharpTests
    {
        public static void SelectQueryAndReturnDataTests()
        {
            string connectionString = "Source=localhost;user=admin;pass=12345";

            using (FastDBConnection con = new FastDBConnection(connectionString))
            {
                con.Open();

                using (FastDBCommand com = new FastDBCommand("", con))
                {
                    com.CommandText = "SELET * FROM banco.tabela";

                    using (FastDBDataReader dr = (FastDBDataReader)com.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                string columnName = dr.GetName(i);
                                object value = dr.GetValue(i);
                            }
                        }
                    }
                }
            }
        }
    }
}
