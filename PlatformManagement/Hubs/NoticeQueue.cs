using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Hubs
{
    public class NoticeQueue
    {
        private static readonly NoticeQueue _noticeQueue = new NoticeQueue();

        private readonly List<INotice> _notice = new List<INotice>();
        private NoticeQueue()
        {
            
        }
        public string Notify(Message message)
        {
            string log = string.Empty;
            _notice.ForEach(t =>
            {
                try
                {
                    t.Receive(message).Start();
                }
                catch (Exception e)
                {
                    log = string.Format("{0}:{1}-----/n", log, e.Message);
                }
            });
            return log;
        }

        public void Add(params INotice[] iNotice)
        {
            _notice.AddRange(iNotice);
        }

        public static NoticeQueue Get
        {
            get
            { 
                return _noticeQueue;
            }
        }
    }
}