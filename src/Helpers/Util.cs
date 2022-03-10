using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.Helpers
{
    internal class Util
    {
        public static int MulDiv(int number, int numerator, int denominator)
        {
            return (int)(((long)number * numerator + (denominator >> 1)) / denominator);
        }
    }
}
