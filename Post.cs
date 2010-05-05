using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven.Sample.SimpleClient
{
    class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        //public string Content { get; set; }
        public DateTime PostedAt { get; set; }
        public List<string> Tags { get; set; }
        //public List<Comment> Comments  { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0,10}: {1} on {2} - {3} - {{{4}}}", 
                Id, PostedAt.ToShortTimeString(), PostedAt.ToShortDateString(), Title, string.Join(", ", Tags));
        }
    }
}
