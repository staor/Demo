using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class UtilesHelper
    {
        #region NLog
        static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Info(object msg)
        {
            logger.Info(msg);
        }
        public static void Trace(object msg)
        {
            logger.Trace(msg);
        }
        public static void Debug(object msg)
        {
            logger.Debug(msg);
        }
        public static void Error(object msg)
        {
            logger.Error(msg);
        }

        public static void CWshow(string msg)
        {
            
            Console.WriteLine(DateTime.Now.ToString("HH: mm:ss: fff")  + msg);
        }
        #endregion
    }
}
