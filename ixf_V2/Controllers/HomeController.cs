using ixf_V2.Models.PaperModel;
using ixf_V2.Models.NewsModel;
using ixf_V2.Models.VideoModel;
using ixf_V2.Models.CompanyModel;
using ixf_V2.Models.Service;
using System.Web.Mvc;
using System.IO;
using ixf_V2.Support;
using System.Web;

namespace ixf_V2.Controllers
{
    public class HomeController : Controller
    {
        #region 首页
        // GET: Home
        [HttpGet]
        [OutputCache(Duration = 3600)]
        public ActionResult Index()
        {
            return View();
        }
        #endregion 

        #region 新闻页面
        [HttpGet]
        [OutputCache(Duration = 3600)]
        public ActionResult News(NewsType type = NewsType.GNXW, int nowPageNum = 1)
        {
            if (nowPageNum == 0)
                nowPageNum = 1;

            NewsContext context = new NewsContext
            {
                Type = type,
                NowPage = nowPageNum
            };
            context.Init();
            return View(context);
        }
        #endregion

        #region 公司信息页面
        [HttpGet]
        [OutputCache(Duration = 3600)]
        public ActionResult Company(CompanyType type = CompanyType.Engineering, int nowPageNum = 1)
        {
            CompanyContext context = new CompanyContext
            {
                Type = type,
                NowPage = nowPageNum
            };
            context.Init();
            return View(context);
        }
        #endregion

        #region 论文页面
        [HttpGet]
        [OutputCache(Duration = 3600)]
        public ActionResult Paper(PaperType type = PaperType.LWK, int nowpageNum = 1)
        {
            PaperContext context = new PaperContext()
            {
                Type = type,
                NowPage = nowpageNum
            };
            context.Init();
            return View(context);
        }

        [HttpGet]
        [OutputCache(Duration = 3600)]
        public ActionResult Laws(PaperType type = PaperType.LWK, int nowpageNum = 1)
        {
            ViewBag.Title = PaperContext.TypeArrayName[(int)type];

            PaperContext context = new PaperContext()
            {
                Type = type,
                NowPage = nowpageNum
            };
            context.Init();
            return View(context);
        }

        [MyActionNameSelecter]
        [MyPdfActionFilter]
        [OutputCache(Duration = 3600, VaryByParam = "filePath")]
        public ActionResult OpenFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            ViewBag.title = fileName;
            return File(filePath, "application/pdf");
        }
        #endregion
        
        #region 后台维护页面
        public ActionResult Maintain()
        {
            return View();
        }
        #endregion

        #region 视频播放

        //public ActionResult Photo()
        //{
        //    return View();
        //}

        //public string UpLoadFile(HttpPostedFileBase file)
        //{
        //    if(file.FileName.Equals(string.Empty))
        //    {
        //        return "this is a empty file";
        //    }
        //    else
        //    {
        //        return "the file name = " + file.FileName;
        //    }
        //}

        
        public ActionResult VideoList(int nowPageNum = 1)
        {
            VideoContext context = new VideoContext()
            {
                NowPage = nowPageNum,
            };
            context.Init();
            return View(model:context);
        }
       
        #endregion

        #region 增值服务

        public ActionResult Service()
        {
            return View();
        }

        public ActionResult ServiceQuestion(int nowPageNum = 1)
        {
            ServiceQuestionContext context = new ServiceQuestionContext()
            {
                NowPage = nowPageNum,
            };
            context.Init();
            return View(model:context);
        }

        public ActionResult ServiceCommon(int nowPageNum = 1)
        {
            ServiceCommonContext context = new ServiceCommonContext()
            {
                NowPage = nowPageNum,
            };
            context.Init();
            return View(model:context);
        }
        #endregion

    }
}