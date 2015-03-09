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
        private HashSet<Vector> hashedForces;
        public List<Vector> forces = new List<Vector>();
        string name;

        public GameObject(Vector v, Vector xcelration, double catholicChurchService, bool elasticity, RectangleF hitbox, string llamo)
        {
            VectorEqualityComparer vectorComparer = new VectorEqualityComparer();
            hashedForces = new HashSet<Vector>(vectorComparer);
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
            hashedForces.Add(addedForce);
        }

        public void removeForce(string name)
        {
            hashedForces.Remove(new Vector(0, 0, VectorType.FORCE, name));
        }

        public void removeAllForces(VectorType type)
        {
            hashedForces.RemoveWhere(x => x.getVectorType() == type);
            /*
            for (int x = 0; x < forces.Count; x++)
            {
                Vector v = forces[x];
                if (v.getName().Contains(name))
                {
                    forces.RemoveAt(x);
                }
            }*/
        }

        public void accelerate(int fps)
        {
            double mag = acceleration.getMagnitude() / fps;
            Vector temp = new Vector(mag, acceleration.getDirection(), VectorType.ACCELERATION, "acceleration");
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
            Vector fnet = new Vector(0, 0, VectorType.FORCE, "resultantForce");
            foreach (Vector v in hashedForces)
            {
                fnet = fnet.resultantVector(v);
            }
            acceleration = new Vector(fnet.getMagnitude() / mass, fnet.getDirection(), VectorType.ACCELERATION, "acceleration");
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
            velocity = new Vector(velocityMag, direction, VectorType.VELOCITY, "velocity");
            other.velocity = new Vector(otherVelocityMag, otherDirection, VectorType.VELOCITY, "velocity");
            if (rekt.X < other.rekt.X)
            {
                float change = ((rekt.X + rekt.Width) - other.rekt.X) / 2;
                rekt = new RectangleF(rekt.X - change, rekt.Y, rekt.Width, rekt.Height);
                other.rekt = new RectangleF(other.rekt.X + change, other.rekt.Y, other.rekt.Width, other.rekt.Height);
            }
            if (rekt.X > other.rekt.X)
            {
                float change = ((other.rekt.X  + other.rekt.Width) - rekt.X) / 2;
                other.rekt = new RectangleF(other.rekt.X - change, other.rekt.Y, other.rekt.Width, other.rekt.Height);
                rekt = new RectangleF(rekt.X + change, rekt.Y, rekt.Width, rekt.Height); 
            }

        }

        public double getVelocity()
        {
            return velocity.getMagnitude();
        }


        public void setVelocity(Vector v)
        {
            velocity = v;
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
                velocity = new Vector(0, 0, VectorType.VELOCITY, "velocity");
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
