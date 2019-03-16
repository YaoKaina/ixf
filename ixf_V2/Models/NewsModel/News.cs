using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ixf_V2.Models
{
    public class News
    {
        public string Content { get; set; }
        public string Url { get; set; }
        public string Style { get; set; }
        public string Type { get; set; }
        public string IssueDate { get; set; }
        public string Remark { get; set; }

        #region 构造函数
        public News() { }

        public News(string content, string url)
        {
            Content = content;
            Url = url;
        }

        public News(string content, string url, string style, string type, string issueDate, string remark)
        {
            Content = content;
            Url = url;
            Style = style;
            Type = type;
            IssueDate = issueDate;
            Remark = remark;
        }
        #endregion
    }
}