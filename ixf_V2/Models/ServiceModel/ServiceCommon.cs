using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ixf_V2.Models.Service
{
    public class ServiceCommon
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string Time { get; set; }

        public ServiceCommon() { }

        public ServiceCommon(string title, string content, string remark, string time)
        {
            Title = title;
            Content = content;
            Remark = remark;
            Time = time;
        }
    }
}