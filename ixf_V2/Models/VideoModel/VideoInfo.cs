using System;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ixf_V2.Models.VideoModel
{
    public class VideoInfo
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Time { get; set; }
        public string Content { get; set; }
        public int PeopleNum { get; set; } = 0;
        public HttpPostedFileBase[] Img { get; set; }
        public string Suggest { get; set; }

        #region 构造函数
        public VideoInfo() { }

        public VideoInfo(string name, string address, string time, string content, int peopleNum)
        {
            Name = name;
            Address = address;
            Time = time;
            Content = content;
            PeopleNum = peopleNum;
        }

        public VideoInfo(string name, string address, string time, string content, int peopleNum, HttpPostedFileBase[] img, string suggest)
        {
            Name = name;
            Address = address;
            Time = time;
            Content = content;
            PeopleNum = peopleNum;
            Img = img;
            Suggest = suggest;
        }
        #endregion

        private bool IsCheck()
        {
            if (string.Empty.Equals(Name) || string.Empty.Equals(Address) || string.Empty.Equals(Content) || string.Empty.Equals(Time) || PeopleNum == 0)
                return true;
            return false;
        }
        
        private int CreatSQL(MySqlCommand command)
        {
            string sql = "id, name, address, time, content, peoplenum";
            string value = "@ID, @Name, @Address, @Time, @Content, @PeopleNum";
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (!string.Empty.Equals(Suggest))
            {
                sql += ", suggest";
                value += ", @Suggest";
                MySqlParameter suggest = new MySqlParameter("@Suggest", MySqlDbType.String)
                {
                    Value = Suggest,
                };
                parameters.Add(suggest);
            }
            if(Img != null)
            {
                sql += ", img";
                value += ", @Img";
                string imgPath = "";
                foreach(HttpPostedFileBase p in Img)
                {
                    imgPath += p.FileName + "@";
                }
                MySqlParameter img = new MySqlParameter("@Img", MySqlDbType.String)
                {
                    Value = imgPath,
                };
                parameters.Add(img);
            }

            MySqlParameter id = new MySqlParameter("@ID", MySqlDbType.Int32)
            {
                Value = ID,
            };
            MySqlParameter name = new MySqlParameter("@Name", MySqlDbType.String)
            {
                Value = Name,
            };
            MySqlParameter address = new MySqlParameter("@Address", MySqlDbType.String)
            {
                Value = Address,
            };
            MySqlParameter time = new MySqlParameter("@Time", MySqlDbType.String)
            {
                Value = Time,
            };
            MySqlParameter content = new MySqlParameter("@Content", MySqlDbType.String)
            {
                Value = Content,
            };
            MySqlParameter peoplenum = new MySqlParameter("@PeopleNum", MySqlDbType.Int32)
            {
                Value = PeopleNum,
            };

            parameters.Add(id);
            parameters.Add(name);
            parameters.Add(address);
            parameters.Add(time);
            parameters.Add(content);
            parameters.Add(peoplenum);

            command.CommandText = string.Format("INSERT INTO videoinfo ({0}) VALUES {1}", sql, value);
            return command.ExecuteNonQuery();
        }

        public long SaveToSQL()
        {
            ID = DateTime.Now.Ticks;

            if (IsCheck())
                return ID;

            try
            {
                MySqlConnection conn = DbSupport.DbPool.Instance.GetConnectionFromPool();
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                CreatSQL(conn.CreateCommand());

                DbSupport.DbPool.Instance.ReturnObjectToPool(conn);
            }
            catch(Exception e)
            {

            }

            return ID;
        }
    }
}