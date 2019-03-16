using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Timers;
using System.Data;
using System.Configuration;

namespace ixf_V2.DbSupport
{
    class DbPool
    {
        //最后的检测时间
        private long lastCheckOut;

        //正在使用的数据库连接对象
        private static Dictionary<MySqlConnection, long> locked;

        //未使用的数据库连接对象
        private static Dictionary<MySqlConnection, long> unlocked;

        //数据库连接语句
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["Ixf_V2"].ConnectionString;

        public static readonly DbPool Instance = new DbPool();

        //对象的有效时间
        internal static long GARBAGE_INTERVAL = 90 * 10000; //90 seconds
        static DbPool()
        {
            locked = new Dictionary<MySqlConnection, long>();
            unlocked = new Dictionary<MySqlConnection, long>();
        }

        internal DbPool()
        {
            lastCheckOut = DateTime.Now.Ticks;

            //创建一个跟踪过期对象进行清理的时间对象
            Timer aTimer = new Timer
            {
                Enabled = true,
                Interval = 90 * 1000
            };
            aTimer.Elapsed += new ElapsedEventHandler(CollectGarbage);
        }

        //创建一个新的对象
        protected MySqlConnection Create()
        {
            MySqlConnection conn = new MySqlConnection
            {
                ConnectionString = connectionString
            };
            conn.Open();
            return conn;
        }

        //检验该对象是否过期
        protected bool Validate(MySqlConnection o)
        {
            try
            {
                long now = DateTime.Now.Ticks;
                if ((now - unlocked[o]) > GARBAGE_INTERVAL)
                {
                    return false;
                }
                return true;
            }
            catch(Exception e)
            {
                throw new Exception("验证数据库连接时出错：" + e.ToString());
            }
        }

        //清除过期的对象
        protected void Expire(MySqlConnection o)
        {
            try
            {
                if (o.State.Equals(ConnectionState.Open))
                {
                    o.Close();
                }
                o.Dispose();
            }
            catch(Exception e)
            {
                throw new Exception("销毁数据库连接对象时出现错误：" + e.ToString());
            }
        }

        /// <summary>
        /// 从连接池得到一个已经打开的数据库连接对象
        /// </summary>
        /// <returns></returns>
        internal MySqlConnection GetConnectionFromPool()
        {
            long now = DateTime.Now.Ticks;
            lastCheckOut = now;
            MySqlConnection o = null;

            lock (this)
            {
                try
                {
                    Dictionary<MySqlConnection, long>.KeyCollection keys = unlocked.Keys;
                    MySqlConnection[] connArray = new MySqlConnection[keys.Count];
                    keys.CopyTo(connArray, 0);
                    int length = connArray.Length;
                    foreach (MySqlConnection c in connArray)
                    {
                        if(Validate(c))
                        {
                            unlocked.Remove(c);
                            OpenConnection(c);
                            locked.Add(c, now);
                            return c;
                        }
                        else
                        {
                            unlocked.Remove(c);
                            Expire(c);
                        }
                    }
                }
                catch(Exception e)
                {
                    throw new Exception("得到数据库连接时出错：" + e.Message);
                }
                o = Create();
                OpenConnection(o);
                locked.Add(o, now);
            }
            return o;
        }

        /// <summary>
        /// 将数据库连接对象返回给连接池
        /// </summary>
        /// <param name="o"></param>
        internal void ReturnObjectToPool(MySqlConnection o)
        {
            if (o != null)
            {
                lock (this)
                {
                    if (o.State.Equals(ConnectionState.Open))
                        o.Close();
                    locked.Remove(o);
                    unlocked.Add(o, DateTime.Now.Ticks);
                }
            }
        }

        private void OpenConnection(MySqlConnection o)
        {
            if (o.State.Equals(ConnectionState.Closed))
                o.Open();
        }

        private void CollectGarbage(object sender, ElapsedEventArgs ea)
        {
            lock (this)
            {
                long now = DateTime.Now.Ticks;
                try
                {
                    Dictionary<MySqlConnection, long>.KeyCollection keys = unlocked.Keys;
                    MySqlConnection[] connArray = new MySqlConnection[keys.Count];
                    keys.CopyTo(connArray, 0);
                    foreach(MySqlConnection c in connArray)
                    {
                        if((now - unlocked[c]) > GARBAGE_INTERVAL)
                        {
                            unlocked.Remove(c);
                            Expire(c);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("线程池清理线程时出现错误："+e.ToString());
                }
            }
        }
    }
}
