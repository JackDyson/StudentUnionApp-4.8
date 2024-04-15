using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentUnionApp
{
    public static class StringHelper
    {
        /// <summary>
        ///     Replaces new line characters with HTML break tags 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceNewLineWithBr(this string value)
        {
            return value.Replace("\n", "<br>");
        }
    }
}
