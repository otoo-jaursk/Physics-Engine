using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealPhysics
{
    public class Vector
    {
        private double magnitude;
        private double direction;
        private double componentX;
        private double componentY;
        private string name;

        public Vector(double mag, double theta, string forceName)
        {
            magnitude = mag;
            direction = theta;
            componentX = magnitude * Math.Cos(direction);
            componentY = magnitude * Math.Sin(direction);
            name = forceName;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string newName)
        {
            name = newName;
        }

        public double getMagnitude()
        {
            return magnitude;
        }

        public double getDirection()
        {
            return direction;
        }

        public double[] getComponent()
        {
            double[] array = {componentX, componentY};
            return array;
        }
        public Vector resultantVector(Vector other)
        {
            double newCompX = componentX + other.componentX;
            double newCompY = componentY + other.componentY;
            return new Vector(Math.Sqrt(Math.Pow(newCompX, 2) + Math.Pow(newCompY, 2)), Math.Atan2(newCompY, newCompX), "resultant of " + name + " " + other.name);
        }



    }
}
