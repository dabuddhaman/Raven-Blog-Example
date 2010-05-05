using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven.Sample.SimpleClient
{
    class TagCloud
    {
        public string Id { get; set; }
        public string Tag { get; set; }
        public long Count { get; set; }

        public override string ToString()
        {
            return string.Format(@"""{0}"" -> {1}", Tag, Count);
        }
    }
}
