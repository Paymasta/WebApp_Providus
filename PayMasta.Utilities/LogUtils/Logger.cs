using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PayMasta.Utilities.LogUtils
{
    public class LogUtils : ILogUtils
    {
        public void WriteTextToFile(string message)
        {
            string filePath = (HttpContext.Current.Server.MapPath("~/FileUpload/APILog.txt"));
            using (var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                stream.Write(newline, 0, newline.Length);
                Byte[] info = new UTF8Encoding(true).GetBytes(message);
                stream.Write(info, 0, info.Length);
                stream.Close();
            }
        }
        public void WriteTextToFileForPush(string message)
        {
            string filePath = (HttpContext.Current.Server.MapPath("~/FileUpload/APILogForPush.txt"));
            using (var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                stream.Write(newline, 0, newline.Length);
                Byte[] info = new UTF8Encoding(true).GetBytes(message);
                stream.Write(info, 0, info.Length);
                stream.Close();
            }
        }
    }
}
