﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealPhysics
{
    
   public class Platform
        {
            private RectangleF hitbox;
            private double staticFriction;
            private double kineticFriction;
            private string name;

            public Platform(float x, float y, float w, float h, double mewS, double mewK, string llamo)
            {
                name = llamo;
                hitbox = new RectangleF(x, y, w, h);
                staticFriction = mewS;
                kineticFriction = mewK;
            }

            public string getName()
            {
                return name;
            }

            public double getKineticFriction()
            {
                return kineticFriction;
            }

            //hopefully this will not be necessary
            public RectangleF gravityZone(float height)
            {
                return new RectangleF(hitbox.X, hitbox.Y + height, hitbox.Width, height + hitbox.Height);
            }

            public RectangleF underside(float height)
            {
                return new RectangleF(hitbox.X, hitbox.Y, hitbox.Width, height + hitbox.Height); 
            }

            public RectangleF getHitbox()
            {
                return hitbox;
            }
        }
    
}
