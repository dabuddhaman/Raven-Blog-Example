using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven.Sample.SimpleClient
{
    class Comment
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostedAt { get; set; }
    }
}
