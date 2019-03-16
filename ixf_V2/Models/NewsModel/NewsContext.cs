using System;
using ixf_V2.DbSupport;
using System.Data;
using MySql.Data.MySqlClient;

namespace ixf_V2.Models.NewsModel
{
    public enum NewsType
    {
        GNXW = 1, GJXW, TPXW, BTXW
    }

    public class NewsContext : ListContext<News>
    {
        public static string[] TypeArrayName = { "", "国内新闻", "国际新闻", "图片新闻", "标题新闻" };
        public NewsType Type { get; set; }
        

        #region 构造函数
        public NewsContext() : base() { }
        #endregion

        protected override void InitResultHandler(MySqlDataReader re)
        {
            while (re.Read())
            {
                News item = new News
                {
                    Content = (string)GetObjectFromReader("Content", re),
                    Url = (string)GetObjectFromReader("Url", re),
                    Style = (string)GetObjectFromReader("Style", re),
                    Type = (string)GetObjectFromReader("Type", re),
                    IssueDate = (string)GetObjectFromReader("IssueDate", re),
                    Remark = (string)GetObjectFromReader("remark", re)
                };
                ListContent.Add(item);
            }
        }

        protected override void InitSQLHandle(MySqlCommand command)
        {
            string sql = "SELECT * FROM tb_news WHERE Type = @Type LIMIT @L, @OnePageNum";
            MySqlParameter type = new MySqlParameter("@Type", MySqlDbType.String)
            {
                Value = TypeArrayName[(int)Type]
            };

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

        protected override void PageNumSQLHandler(MySqlCommand command)
        {
            command.CommandText = "SELECT COUNT(*) FROM tb_news WHERE Type = @Type";
            MySqlParameter type = new MySqlParameter("@Type", MySqlDbType.String)
            {
                Value = TypeArrayName[(int)Type]
            };
            command.Parameters.Add(type);
        }
    }
}