using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Utility
{
    internal class Utils
    {
        public static string GetEnumString(Type enumClass, int intValue) => Enum.GetName(enumClass, intValue);

    }
}
