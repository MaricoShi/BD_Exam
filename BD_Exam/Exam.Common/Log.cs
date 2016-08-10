using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading;


namespace Exam.Common
{
    public class Log
    {
        public static string LogFilePath = AppDomain.CurrentDomain.BaseDirectory + "WebLog\\";

        private static object lockobj = new object();

        public static void WriteLogFile(string message, Exception ex = null)
        {
            DeleteBackMessge();
            string Date = DateTime.Now.Date.ToShortDateString().Replace('/', '_');
            if (!Directory.Exists(LogFilePath))
                Directory.CreateDirectory(LogFilePath);

            string fileName = LogFilePath + "\\" + Date + ".txt";
            FileInfo fileInfo = new FileInfo(fileName);

            lock (lockobj)
            {
               
                if (!fileInfo.Exists)
                {
                    CreateFileAndLog(fileInfo, message, ex);
                    return;
                }

                using (FileStream fileStream = fileInfo.OpenWrite())
                {
                    WriteContent(fileStream, message, ex);
                    return;
                }
            }


        }

        private static void WriteContent(FileStream fileStream, string mes, Exception ex)
        {
            try
            {
                StreamWriter write = new StreamWriter(fileStream);
                write.BaseStream.Seek(0, SeekOrigin.End);
                string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-" + mes;
                if (ex != null)
                    msg += "  ExceptionMsg:" + ex.Message;
                if (ex != null && ex.InnerException != null)
                    msg += "  InnerMsg:" + ex.InnerException.Message;
                write.WriteLine(msg);
                write.Flush();
                write.Close();
            }
            catch (Exception)
            {
            }
        }

        private static void CreateFileAndLog(FileInfo fileInfo, string mes, Exception ex)
        {
            using (FileStream fileStream = fileInfo.Create()) { WriteContent(fileStream, mes, ex); }
        }

        //大于30M时删除15日前的备份
        private static void DeleteBackMessge()
        {
            try
            {
                string dirPath = LogFilePath;
                long len = 0;
                //判断该路径是否存在（是否为文件夹）
                if (Directory.Exists(dirPath))
                {
                    //定义一个DirectoryInfo对象
                    DirectoryInfo di = new DirectoryInfo(dirPath);

                    //通过GetFiles方法，获取di目录中的所有文件的大小
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        len += fi.Length;
                    }
                }
                if (len >= 30 * 1024 * 1024)
                {
                    //定义一个DirectoryInfo对象
                    DirectoryInfo di = new DirectoryInfo(dirPath);

                    //通过GetFiles方法，获取di目录中的所有文件的大小
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        if (fi.CreationTime < System.DateTime.Now.AddDays(-15))
                        {
                            fi.Delete();
                        }
                    }
                }
            }
            catch (Exception )
            {
                
            }
        }

    }
}
