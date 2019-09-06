using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Mysql
{
    public class MysqlHelp
    {
        public string ConnectionString { get; set; }

        public MysqlHelp(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public object GetAllCountry()
        {
            //连接数据库
            using (MySqlConnection msconnection = GetConnection())
            {
                msconnection.Open();
                //查找数据库里面的表
                MySqlCommand mscommand = new MySqlCommand("select * from user", msconnection);
                using (MySqlDataReader reader = mscommand.ExecuteReader())
                {
                    //读取数据
                    while (reader.Read())
                    {
                        var ss = reader.GetString("a");
                        //list.Add(new Country()
                        //{
                        //    Code = reader.GetString("Code"),
                        //    Name = reader.GetString("Name"),
                        //    Continent = reader.GetString("Continent"),
                        //    Region = reader.GetString("Region")
                        //});
                    }
                }
            }
            return null;
        }
    }
}
