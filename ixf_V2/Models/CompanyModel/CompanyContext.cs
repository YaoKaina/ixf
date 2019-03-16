using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using ixf_V2.DbSupport;

namespace ixf_V2.Models.CompanyModel
{
    public enum CompanyType
    {
        Engineering = 1,
        Project = 2,
    }

    public class CompanyContext : ListContext<Company>
    {
        public CompanyType Type { get; set; }
        public string PageName = "企业考察";

        #region 构造函数
        public CompanyContext() : base() { }
        #endregion

        protected override void InitSQLHandle(MySqlCommand command)
        {
            string sql = "SELECT * FROM qiye WHERE type = @Type LIMIT @L, @OnePageNum";
            MySqlParameter type = new MySqlParameter("@Type", MySqlDbType.String);
            if (Type == CompanyType.Engineering)
            {
                type.Value = "工程型";
            }
            else if (Type == CompanyType.Project)
            {
                type.Value = "生产型";
            }
            MySqlParameter L = new MySqlParameter("@L", MySqlDbType.Int32)
            {
                Value = (NowPage - 1) * OnePageNum
            };

            MySqlParameter onePageNum = new MySqlParameter("@OnePageNum", MySqlDbType.Int32)
            {
                Value = OnePageNum
            };

            command.CommandText = sql;
            command.Parameters.Add(type);
            command.Parameters.Add(L);
            command.Parameters.Add(onePageNum);
        }

        protected override void InitResultHandler(MySqlDataReader re)
        {
            while (re.Read())
            {
                Company item = new Company()
                {
                    Type = (string)GetObjectFromReader("Type", re),
                    Name = (string)GetObjectFromReader("Name", re),
                    Date = (string)GetObjectFromReader("Date", re),
                    Address = (string)GetObjectFromReader("Address", re),
                    Tel = (string)GetObjectFromReader("Tel", re),
                    Email = (string)GetObjectFromReader("Email", re),
                    WebSite = (string)GetObjectFromReader("WebSite", re),
                    Province = (string)GetObjectFromReader("Province", re),
                    Grade = (string)GetObjectFromReader("Grade", re),
                    Scope = (string)GetObjectFromReader("Scope", re),
                    Scale = (string)GetObjectFromReader("Scale", re)
                };
                ListContent.Add(item);
            }
        }

        protected override void PageNumSQLHandler(MySqlCommand command)
        {
            string sql = "SELECT COUNT(*) FROM qiye WHERE type = @Type";
            MySqlParameter type = new MySqlParameter("@Type", MySqlDbType.String);
            if (Type == CompanyType.Engineering)
            {
                type.Value = "工程型";
            }
            else if (Type == CompanyType.Project)
            {
                type.Value = "生产型";
            }

            command.CommandText = sql;
            command.Parameters.Add(type);
        }
    }
}