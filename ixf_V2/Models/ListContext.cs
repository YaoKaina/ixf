using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ixf_V2.DbSupport;
using System;
using ixf_V2.Support;

namespace ixf_V2.Models
{
    abstract public class ListContext<T>:DbOperation
    {
        #region 字段
        /// <summary>
        /// 存放信息的集合
        /// </summary>
        public List<T> ListContent { get; set; }

        /// <summary>
        /// 当前的页数
        /// </summary>
        public int NowPage { get; set; } = 1;

        /// <summary>
        /// 一页中信息的数量
        /// </summary>
        public int OnePageNum { get; set; } = 10;

        /// <summary>
        /// 当前List集合中 T 的数目
        /// </summary>
        public int Count
        {
            get
            {
                if (ListContent != null)
                    return ListContent.Count;
                else
                    return -1;
            }
        }

        /// <summary>
        /// 信息一共可以显示的页数
        /// </summary>
        public int _PageNum { get; set; }
        public int PageNum
        {
            set
            {
                _PageNum = value;
            }

            get
            {
                GetPageNum();
                return _PageNum;
            }
        }
        #endregion

        #region 构造函数
        public ListContext()
        {
            ListContent = new List<T>();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 加载对应页的数据信息
        /// </summary>
        public void Init()
        {
            SQLEventHandler initSQLHandler = new SQLEventHandler(InitSQLHandle);
            ResultEventHandler initResultHandler = new ResultEventHandler(InitResultHandler);

            SQLExectueReader(initSQLHandler, initResultHandler);
        }

        /// <summary>
        /// 得到数据库中所有记录可以展示的页数
        /// </summary>
        public void  GetPageNum()
        {
            SQLEventHandler pageNumSQLHandler = new SQLEventHandler(PageNumSQLHandler);
            ResultEventHandler pageNumResultHandler = new ResultEventHandler(PageNumResultHandler);

            SQLExectueReader(pageNumSQLHandler, pageNumResultHandler);
        }
        #endregion

        #region 私有的方法
        private void PageNumResultHandler(MySqlDataReader re)
        {
            while (re.Read())
            {
                _PageNum = (int)GetObjectFromReader("COUNT(*)", re);
            }
            _PageNum = _PageNum / OnePageNum + 1;
        }
        #endregion

        #region 抽象方法
        abstract protected void InitSQLHandle(MySqlCommand command);
        abstract protected void InitResultHandler(MySqlDataReader re);

        abstract protected void PageNumSQLHandler(MySqlCommand command);
        #endregion
    }
}