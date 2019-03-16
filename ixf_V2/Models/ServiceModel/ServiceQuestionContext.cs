using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ixf_V2.Models.Service
{
    public class ServiceQuestionContext : ListContext<ServiceQuestion>
    {
        public ServiceQuestionContext() : base() { }

        protected override void InitResultHandler(MySqlDataReader re)
        {
            while(re.Read())
            {
                ServiceQuestion question = new ServiceQuestion()
                {
                    Subject = (string)GetObjectFromReader("subject", re),
                    UserName = (string)GetObjectFromReader("user_name", re),
                    Mail = (string)GetObjectFromReader("mail", re),
                    Content = (string)GetObjectFromReader("content", re),
                    Reply = (string)GetObjectFromReader("reply", re),
                    Time = (string)GetObjectFromReader("date", re),
                };

                ListContent.Add(question);
            }
        }

        protected override void InitSQLHandle(MySqlCommand command)
        {
            command.CommandText = "SELECT subject, user_name, mail, content, reply, date FROM services_question LIMIT @L, @ONEPAGENUM ";
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
            command.CommandText = "SELECT COUNT(*) FROM services_question";
        }
    }
}