using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ixf_V2.Models.CompanyModel
{
    public class Company
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Province { get; set; }
        public string Grade { get; set; }
        public string Scope { get; set; }
        public string Scale { get; set; }


        #region 构造函数
        public Company() { }

        public Company(string name, string date, string address, string tel, string scope)
        {
            Name = name;
            Date = date;
            Address = address;
            Tel = tel;
            Scope = scope;
        }

        public Company(string name, string date, string address, string tel, string email, string webSite, string province, string scope, string scale)
        {
            Name = name;
            Date = date;
            Address = address;
            Tel = tel;
            Email = email;
            WebSite = webSite;
            Province = province;
            Scope = scope;
            Scale = scale;
        }

        public Company(string type, string name, string date, string address, string tel, string email, string webSite, string province, string grade, string scope, string scale)
        {
            Type = type;
            Name = name;
            Date = date;
            Address = address;
            Tel = tel;
            Email = email;
            WebSite = webSite;
            Province = province;
            Grade = grade;
            Scope = scope;
            Scale = scale;
        }
        
        #endregion
    }
}