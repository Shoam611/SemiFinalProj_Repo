using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tWpfMashUp_v0._0._1.Extentions
{
    public static class ValidationExtention
    {
        
        /// <param name="str">string to check</param>
        /// <returns>true if the provided string is Empty, Null or contains white space</returns>
        public static bool IsEmptyNullOrWhiteSpace(this string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return true;
            return false;
        }
    }
}
