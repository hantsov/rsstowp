using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RssUptime.Models
{
    public class RssReaderVM
    {

        public RssReaderVM()
        {       
        }

        public RssReaderVM(List<RssWithUrl> list)
        {
            this.Elements = list;
        }

        public IEnumerable<RssWithUrl> Elements { get; set; }

        public RssWithUrl Selected { get; set; }

        public int DeviceSpecificRows { get; set; }
    }
}