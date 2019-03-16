using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ixf_V2.Models.Service
{
    public class ServiceQuestion
    {
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Content { get; set; }
        public string Reply { get; set; }
        public string Time { get; set; }

        public ServiceQuestion() { }

        public ServiceQuestion(string subject, string userName, string mail, string content, string reply, string time)
        {
            Subject = subject;
            UserName = userName;
            Mail = mail;
            Content = content;
            Reply = reply;
            Time = time;
        }
    }
}