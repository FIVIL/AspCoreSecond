using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seccond
{
    public class counter
    {
        public int count { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendLine();
            sb.Append("count :");
            sb.Append(count);
            sb.AppendLine();
            sb.Append("}");
            return sb.ToString();
        }
    }
}
