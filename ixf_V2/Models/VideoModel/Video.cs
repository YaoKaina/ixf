using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ixf_V2.Models.VideoModel
{
    public class Video
    {
        public string Location { get; set; }
        public string VideoType { get; set; }
        public string Date { get; set; }
        public string ImgFile { get; set; }
        public string VideoName { get; set; }
        public string TimeLen { get; set; }

        public Video() { }

        public Video(string location, string type, string date, string imgType, string videoName, string timeLen)
        {
            Location = location;
            VideoType = type;
            Date = date;
            ImgFile = imgType;
            VideoName = videoName;
            TimeLen = timeLen;
        }
    }
}