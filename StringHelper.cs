using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentUnionApp
{
    public static class StringHelper
    {
        // function to replace \n with <br> in the message
        public static string ReplaceNewLineWithBr(this string value)
        {
            return value.Replace("\n", "<br>");
        }
    }
}
