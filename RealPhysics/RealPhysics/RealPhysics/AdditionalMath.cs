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

        public static bool contains(PointF point, RectangleF container)
        {
            bool x = point.X > container.X && point.X < container.X + container.Width;
            bool y = point.Y < container.Y && point.Y > container.Y - container.Height;
            return x && y;
        }

        public static bool intersects(RectangleF rekt1, RectangleF rekt2)
        {
            bool x = rekt1.X < rekt2.X && rekt1.X + rekt1.Width > rekt2.X + rekt2.Width || rekt2.X < rekt1.X && rekt2.X + rekt2.Width > rekt1.X + rekt1.Width;
            bool y = rekt1.Y > rekt2.Y && rekt1.Y - rekt1.Height < rekt2.Y - rekt2.Height || rekt2.Y > rekt1.Y && rekt2.Y - rekt2.Height < rekt1.Y - rekt1.Height;
            return x || y;
        }

    }
}
