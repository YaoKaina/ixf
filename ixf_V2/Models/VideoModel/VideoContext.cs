using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ixf_V2.Models.VideoModel
{
    public class VideoContext : ListContext<Video>
    {
        protected override void InitResultHandler(MySqlDataReader re)
        {
            while(re.Read())
            {
                Video v = new Video
                {
                    Location = (string)GetObjectFromReader("location", re),
                    VideoType = (string)GetObjectFromReader("type", re),
                    Date = (string)GetObjectFromReader("date", re),
                    ImgFile = (string)GetObjectFromReader("imgFile", re),
                    VideoName = (string)GetObjectFromReader("videoName", re),
                    TimeLen = (string)GetObjectFromReader("time_len", re),
                };
                ListContent.Add(v);
            }
        }

        protected override void InitSQLHandle(MySqlCommand command)
        {
            string sql = "SELECT location,type,date,imgFile,videoName,time_len FROM video LIMIT @L, @OnePageNum";
            MySqlParameter l = new MySqlParameter("@L", MySqlDbType.Int32) { Value = (NowPage - 1) * OnePageNum };
            MySqlParameter onePageNum = new MySqlParameter("@OnePageNum", MySqlDbType.Int32) { Value = OnePageNum };
            command.CommandText = sql;
            command.Parameters.Add(l);
            command.Parameters.Add(onePageNum);
        }

        protected override void PageNumSQLHandler(MySqlCommand command)
        {
            command.CommandText = "SELECT COUNT(*) FROM video";
        }
    }
}