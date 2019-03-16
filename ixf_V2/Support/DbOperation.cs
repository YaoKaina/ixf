using ixf_V2.DbSupport;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ixf_V2.Support
{
    public class DbOperation
    {
        #region 受保护的方法
        /// <summary>
        /// 执行对应的数据库操作，并对返回集进处理
        /// </summary>
        /// <param name="sqlHandle">一个定义具体的数据库操作的委托类型</param>
        /// <param name="resultHandle">一个对返回集进行处理的委托类型</param>
        protected void SQLExectueReader(SQLEventHandler sqlHandle, ResultEventHandler resultHandle)
        {
            /**
             * 使用了委托的方式，可以使得该方法可以处理不同的数据库操作，使得程序代码变得更加简洁
             * 
             * SQLEventHandler ：一个定义具体的数据库操作的委托类型
             * ResultEventHandler  ：一个定义对返回集进行处理的委托类型
             * 
             * 当相对数据库进行操作，并要返回一个 MySqlDataResult 类型的结果集时，
             * 你可以在你的方法中调用该方法（前提是你的类必须集成自该类），然后你
             * 可以将数据库的操作与对结果集的处理作为新的方法去定义，然后使用委托
             * 将具体的操作传入该方法之中，已完成对数据库的操作
             */
            try
            {
                MySqlConnection conn = DbPool.Instance.GetConnectionFromPool();
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                MySqlCommand command = conn.CreateCommand();
                sqlHandle(command);

                MySqlDataReader re = command.ExecuteReader();
                resultHandle(re);

                command.Dispose();
                re.Close();
                re.Dispose();

                DbPool.Instance.ReturnObjectToPool(conn);
            }
            catch (Exception e)
            {

            }
        }


        /// <summary>
        /// 从一个 MySqlDataReader 对象中根据指定的列名的对象
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        protected object GetObjectFromReader(string filedName, MySqlDataReader re)
        {
            int filedNum = re.GetOrdinal(filedName);
            if(!re.IsDBNull(filedNum))
            {
                return re[filedNum];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 委托
        /// <summary>
        /// 定义具体的数据库操作的委托类型
        /// </summary>
        /// <param name="command"></param>
        protected delegate void SQLEventHandler(MySqlCommand command);

        /// <summary>
        /// 对返回集进行处理的委托类型
        /// </summary>
        /// <param name="re"></param>
        protected delegate void ResultEventHandler(MySqlDataReader re);
        #endregion
    }
}