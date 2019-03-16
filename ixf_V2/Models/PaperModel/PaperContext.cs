using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ixf_V2.Models.PaperModel
{
    public enum PaperType
    {
        LWK = 1, ZJFB, XJS, FLFG, GJBZ, HYBZ, JSGF
    }

    public class PaperContext : ListContext<Paper>
    {
        private static string[] TypeArray = { "", "lwk", "zjfb", "xjs", "flfg", "gjbz", "hybz", "jsgf" };
        public static string[] TypeArrayName = { "", "论文库", "专家发表", "新技术", "法律法规", "国家标准", "行业标准", "技术规范", };
        public PaperType Type { get; set; }

        public string PageName
        {
            get
            {
                return TypeArrayName[(int)Type];
            }
        }

        #region 构造函数
        public PaperContext() : base() { }
        #endregion

        protected override void InitResultHandler(MySqlDataReader re)
        {
            while(re.Read())
            {
                Paper paper = new Paper()
                {
                    Name = (string)GetObjectFromReader("name" ,re),
                    Location = (string)GetObjectFromReader("location", re),
                    Date = (string)GetObjectFromReader("date", re),
                };

                ListContent.Add(paper);
            }
        }

        protected override void InitSQLHandle(MySqlCommand command)
        {
            string sql = "SELECT * FROM upload WHERE type = @Type LIMIT @L, @OnePageNum";
            MySqlParameter type = new MySqlParameter("@Type", MySqlDbType.String) { Value = TypeArray[(int)Type] };
            MySqlParameter l = new MySqlParameter("@L", MySqlDbType.Int32) { Value = (NowPage - 1) * OnePageNum };
            MySqlParameter onePagerNum = new MySqlParameter("@OnePageNum", MySqlDbType.Int32) { Value = OnePageNum };

            command.CommandText = sql;
            command.Parameters.Add(type);
            command.Parameters.Add(l);
            command.Parameters.Add(onePagerNum);
        }

        protected override void PageNumSQLHandler(MySqlCommand command)
        {
            string sql = "SELECT COUNT(*) FROM upload WHERE type = @Type";
            MySqlParameter type = new MySqlParameter("@Type", MySqlDbType.String) { Value = TypeArray[(int)Type] };
            
            command.CommandText = sql;
            command.Parameters.Add(type);
        }
    }
}