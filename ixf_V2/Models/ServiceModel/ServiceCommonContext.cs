using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ixf_V2.Models.Service
{
    public class ServiceCommonContext : ListContext<ServiceCommon>
    {
        protected override void InitResultHandler(MySqlDataReader re)
        {
            while(re.Read())
            {
                ServiceCommon v = new ServiceCommon()
                {
                    Title = (string)GetObjectFromReader("title", re),
                    Content = (string)GetObjectFromReader("content", re),
                    Remark = (string)GetObjectFromReader("Remark", re),
                    Time = (string)GetObjectFromReader("date", re),
                };

                ListContent.Add(v);
            }
        }

        protected override void InitSQLHandle(MySqlCommand command)
        {
            command.CommandText = "SELECT * FROM services_knack LIMIT @L, @ONEPAGENUM";
            MySqlParameter L = new MySqlParameter("@L", MySqlDbType.Int32)
            {
                Value = OnePageNum * (NowPage - 1),
            };
            MySqlParameter ONEPAGENUM = new MySqlParameter("@ONEPAGENUM", MySqlDbType.Int32)
            {
                Value = OnePageNum,
            };
            command.Parameters.Add(L);
            command.Parameters.Add(ONEPAGENUM);
        }

        protected override void PageNumSQLHandler(MySqlCommand command)
        {
            command.CommandText = "SELECT COUNT(*) FROM services_knack";
        }
    }
}