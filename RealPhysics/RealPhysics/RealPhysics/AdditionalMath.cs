using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealPhysics
{
    public static class AdditionalMath
    {
        public static double[] quadraticFormula(double a, double b, double c)
        {
            double[] returnable = new double[2];
            returnable[0] = (-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            returnable[1] = (-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            return returnable;
        }

        public static bool contains(RectangleF container, RectangleF containee)
        {
            bool x = container.X < containee.X && container.X + container.Width > containee.X + containee.Width;
            bool y = container.Y > containee.Y && container.Y - container.Height < containee.Y - containee.Height;
            return x && y;
        }

    }
}
