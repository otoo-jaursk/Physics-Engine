using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealPhysics
{
    public class GameObject
    {
        private Vector velocity;
        private Vector acceleration;
        private RectangleF rekt;
        private double mass;
        private bool elastic;
        private bool flight = false;
        private double startingHeight = -1;
        public List<Vector> forces = new List<Vector>();
        string name;

        public GameObject(Vector v, Vector xcelration, double catholicChurchService, bool elasticity, RectangleF hitbox, string llamo)
        {
            name = llamo;
            rekt = hitbox;
            velocity = v;
            acceleration = xcelration;
            mass = catholicChurchService;
            elastic = elasticity;
        }

        public double getMass()
        {
            return mass;
        }

        public string getName()
        {
            return name;
        }

        public void addForce(Vector addedForce)
        {
            foreach (Vector force in forces)
            {
                if (force.getName().Equals(addedForce.getName()))
                {
                    return;
                }
            }
            forces.Add(addedForce);
        }

        public void removeForce(string name)
        {
            if(name.Equals("speed up"))
            {
                double[] comps = velocity.getComponent();
                velocity = velocity.resultantVector(new Vector(comps[1], 3 * Math.PI / 2, ""));
            }
            for(int x = 0; x < forces.Count; x++)
            {
                Vector v = forces[x];
                if (v.getName().Equals(name))
                {
                    forces.RemoveAt(x);
                    if (v.getName().Equals("gravity"))
                    {
                        double[] comps = velocity.getComponent();
                        velocity = velocity.resultantVector(new Vector(-comps[1], Math.PI / 2, ""));
                    }
                }
            }
        }

        public void removeAllForces(string name)
        {
            for (int x = 0; x < forces.Count; x++)
            {
                Vector v = forces[x];
                if (v.getName().Contains(name))
                {
                    forces.RemoveAt(x);
                }
            }
        }

        public void accelerate(int fps)
        {
            double mag = acceleration.getMagnitude() / fps;
            Vector temp = new Vector(mag, acceleration.getDirection(), "acceleration");
            velocity = velocity.resultantVector(temp);
        }

        public void addAcceleration(Vector xceleration)
        {
            acceleration = acceleration.resultantVector(xceleration);
        }

        public void ground()
        {
            flight = false;
            startingHeight = -1;
        }

        private void determineAcceleration()
        {
            Vector fnet = new Vector(0, 0, "resultantForce");
            foreach (Vector v in forces)
            {
                fnet = fnet.resultantVector(v);
            }
            acceleration = new Vector(fnet.getMagnitude() / mass, fnet.getDirection(), "acceleration");
        }

        public void collision(GameObject other)
        {
            double[] components = velocity.getComponent();
            double[] theirComponents = other.velocity.getComponent();
            double momentumX = components[0] * mass + theirComponents[0] * other.mass;
            double momentumY = components[1] * mass + theirComponents[1] * other.mass;
            double ourXVelocity, ourYVelocity, theirXVelocity, theirYVelocity;
            if (other.elastic || elastic)
            {
                //kinetic energy when only taking into account velocity in the x direction
                double xJoules = .5 * Math.Pow(components[0], 2) * mass + .5 * Math.Pow(theirComponents[0], 2) * other.mass;
                //formula I figured out
                double[] theirPossibleVxs = AdditionalMath.quadraticFormula(other.mass * mass + Math.Pow(other.mass, 2), 2 * other.mass * momentumX, -(2 * mass * xJoules - Math.Pow(momentumX, 2)));
                if (theirPossibleVxs[0] == other.velocity.getMagnitude())
                {
                    theirXVelocity = theirPossibleVxs[1];
                }
                else
                {
                    theirXVelocity = theirPossibleVxs[0];
                }
                double yJoules = .5 * Math.Pow(components[1], 2) * mass + .5 * Math.Pow(theirComponents[1], 2) * other.mass;
                //formula I figured out don't question it
                double[] theirPossibleVys = AdditionalMath.quadraticFormula(other.mass * mass + Math.Pow(other.mass, 2), 2 * other.mass * momentumX, -(2 * mass * yJoules - Math.Pow(momentumX, 2)));
                if (theirPossibleVys[0] == other.velocity.getMagnitude())
                {
                    theirYVelocity = theirPossibleVys[1];
                }
                else
                {
                    theirYVelocity = theirPossibleVys[0];
                }
                ourYVelocity = (momentumY - theirYVelocity * other.mass) / mass;
                ourXVelocity = (momentumX - theirXVelocity * other.mass) / mass;
            }
            else
            {
                ourXVelocity = theirXVelocity = momentumX / (mass + other.mass);
                ourYVelocity = theirYVelocity = momentumY / (mass + other.mass);
                
            }
            double velocityMag = Math.Sqrt(Math.Pow(ourXVelocity, 2) + Math.Pow(ourYVelocity, 2));
            double otherVelocityMag = Math.Sqrt(Math.Pow(theirXVelocity, 2) + Math.Pow(theirYVelocity, 2));
            double otherDirection = Math.Atan2(theirYVelocity, theirXVelocity);
            double direction = Math.Atan2(ourYVelocity, ourXVelocity);
            velocity = new Vector(velocityMag, direction, "velocity");
            other.velocity = new Vector(otherVelocityMag, otherDirection, "velocity");
        }

        public double getVelocity()
        {
            return velocity.getMagnitude();
        }

        public double getVelocityDirection()
        {
            return velocity.getDirection();
        }

        public Vector getVelocity(string whatever)
        {
            return velocity;
        }

        public void determineMovement(int fps, double g)
        {
            determineAcceleration();
            accelerate(fps);
            /*if(flight){
                double ke = joules - mass * g * (rekt.Y - startingHeight);
                double velocityMagnitude = Math.Sqrt((ke * 2) / mass);
                velocity = new Vector(velocityMagnitude, velocity.getDirection());
            }*/
            if (velocity.getMagnitude() < .3)
            {
                velocity = new Vector(0, 0, "velocity");
            }
            double[] comps = velocity.getComponent();
            float x = rekt.X;
            if(Math.Abs(comps[0]) > 0)
                x += (float)(comps[0] / fps);
            float y = rekt.Y;
            if(Math.Abs(comps[1]) > 0)
                y += (float)(comps[1] / fps);
            rekt = new RectangleF(x, y, rekt.Width, rekt.Height);
        }

        public RectangleF getRekt()
        {
            return rekt;
        }

        public void setRekt(RectangleF daRect)
        {
            rekt = daRect;
        }

        public bool getFlight()
        {
            return flight;
        }

        public void setFlight(bool toSet)
        {
            flight = toSet;
        }

        public double getStartingHeight()
        {
            return startingHeight;
        }

        public void setStartingHeight(double newStart)
        {
            startingHeight = newStart;
        }
    }
}
