using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace PlatformManagement.Common
{
    public class MailHelp
    {
        private static readonly string ADDRESS;
        private readonly static string USER_NAME;
        private readonly static string PASSWORD;
        private readonly static string MAILTEMP;
        private readonly static string TITLE;

        public static void Send(string address,string mailContext)
        {
            new Mail().SendMessage(ADDRESS, PASSWORD, USER_NAME, TITLE, address, mailContext, address);
        }
    }  

    public class MailInfo
    {
        public Dictionary<string, string> smtpServer;

        public Dictionary<string, string> popServer;

        public MailInfo()
        {
            IniSmtpServer();
            IniPopServer();
        }
        public string server { get; set; }//服务器 
        public string user { get; set; }//用户名 
        public string pwd { get; set; }//密码 
        /// <summary> 
        /// 初始化常用smtpServer，用于绑定下拉选择菜单 
        /// </summary> 
        private void IniSmtpServer()
        {
            smtpServer = new Dictionary<string, string>();
            smtpServer.Add("网易163邮箱", "smtp.163.com");
            smtpServer.Add("网易vip.163邮箱", "smtp.vip.163.com");
            smtpServer.Add("网易126邮箱", "smtp.126.com");
            smtpServer.Add("网易188邮箱", "smtp.188.com");
            smtpServer.Add("新浪邮箱", "smtp.sina.com");
            smtpServer.Add("雅虎邮箱", "smtp.mail.yahoo.com");
            smtpServer.Add("搜狐邮箱", "smtp.sohu.com");
            smtpServer.Add("TOM邮箱", "smtp.tom.com");
            smtpServer.Add("Gmail邮箱", "smtp.gmail.com");
            smtpServer.Add("QQ邮箱", "smtp.qq.com");
            smtpServer.Add("QQ企业邮箱", "smtp.biz.mail.qq.com");
            smtpServer.Add("139邮箱", "smtp.139.com");
            smtpServer.Add("263邮箱", "smtp.263.com");
        }

   
        private void IniPopServer()
        {
            popServer = new Dictionary<string, string>();
            popServer.Add("网易163邮箱", "pop3.163.com");
            popServer.Add("网易vip.163邮箱", "pop3.vip.163.com");
            popServer.Add("网易126邮箱", "pop3.126.com");
            popServer.Add("网易188邮箱", "pop3.188.com");
            popServer.Add("新浪邮箱", "pop3.sina.com");
            popServer.Add("雅虎邮箱", "pop3.mail.yahoo.com");
            popServer.Add("搜狐邮箱", "pop3.sohu.com");
            popServer.Add("TOM邮箱", "pop.tom.com");
            popServer.Add("Gmail邮箱", "pop.gmail.com");
            popServer.Add("QQ邮箱", "pop.qq.com");
            popServer.Add("QQ企业邮箱", "pop.biz.mail.qq.com");
            popServer.Add("139邮箱", "pop.139.com");
            popServer.Add("263邮箱", "pop.263.com");
        }
    }

  
    public class Mail
    {
        public bool SendMessage(string fromEmail, string password, string user, string title, string toEmail, string email, string smtpServer)
        {
            try
            {
                SmtpClient smtp = new SmtpClient(); //实例化一个SmtpClient 
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //将smtp的出站方式设为 Network 
                smtp.EnableSsl = false;//smtp服务器是否启用SSL加密 
                smtp.Host = smtpServer;//指定 smtp 服务器                    
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                MailMessage mm = new MailMessage(); //实例化一个邮件类 
                mm.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可               
                mm.From = new MailAddress(fromEmail, user, Encoding.GetEncoding(936));
                mm.CC.Add(new MailAddress(toEmail, "", Encoding.GetEncoding(936)));
                mm.Subject = title; //邮件标题 
                mm.SubjectEncoding = Encoding.GetEncoding(936);
                mm.IsBodyHtml = true; //邮件正文是否是HTML格式mm.BodyEncoding = Encoding.GetEncoding(936); 
                mm.Body = email;
                smtp.Send(mm);
                return true;
            }
            catch
            {
                return false;
            }
        }
   
        delegate bool MyDelegate(object checkEmailInfo);

        public bool CheckUser(string server, string user, string pwd)
        {
            MyDelegate myDelegate = new MyDelegate(CheckUser);
            MailInfo checkEmailInfo = new MailInfo();
            checkEmailInfo.server = server;
            checkEmailInfo.user = user;
            checkEmailInfo.pwd = pwd;
            IAsyncResult result = myDelegate.BeginInvoke(checkEmailInfo, null, null);
            Thread.Sleep(1000);//主线程1秒后检查异步线程是否运行完毕 
            if (result.IsCompleted)
            { return myDelegate.EndInvoke(result); }//如果错误的邮箱和密码，函数将会运行很慢 
            else
            { return false; }
        }

 
        private bool CheckUser(object checkEmailInfo)
        {
            MailInfo checkInfo = (MailInfo)checkEmailInfo;
            TcpClient sender = new TcpClient(checkInfo.server, 110);//pop协议使用TCP的110端口 
            Byte[] outbytes;
            NetworkStream ns;
            StreamReader sr;
            string input;
            string readuser = string.Empty;
            string readpwd = string.Empty;
            try
            {
                ns = sender.GetStream();
                sr = new StreamReader(ns);
                sr.ReadLine();
                //检查用户名和密码 
                input = "user " + checkInfo.user + "\r\n";
                outbytes = Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                readuser = sr.ReadLine();
                input = "pass " + checkInfo.pwd + "\r\n";
                outbytes = Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                readpwd = sr.ReadLine();
                if (readuser.Substring(0, 3) == "+OK" && readpwd.Substring(0, 3) == "+OK")
                { return true; }
                else
                { return false; }
            }
            catch
            {
                return false;
            }
        }

        public bool IsEmail(string email)
        {
            string paterner = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            if (!Regex.IsMatch(email, paterner))
            { return false; }
            else
            { return true; }
        }
    }
}