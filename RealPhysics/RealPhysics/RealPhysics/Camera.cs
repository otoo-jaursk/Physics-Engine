using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace RealPhysics
{
    public class Camera
    {
        private double meters_to_pixels;
        private Universe universe;
        private double XAXIS = SystemParameters.PrimaryScreenWidth;
        private double YAXIS = SystemParameters.PrimaryScreenHeight;

        public Camera(double metersPerScreen, Universe world)
        {
            meters_to_pixels = metersPerScreen / SystemParameters.PrimaryScreenWidth;
            universe = world;
        }

        public List<Rectangle> snapshot(GameObject focus)
        {
            List<Rectangle> returnable = new List<Rectangle>();
            RectangleF player = universe.getPlayer().getRekt();
            int height = (int)(player.Height / meters_to_pixels);
            int width = (int)(player.Width / meters_to_pixels);
            int x = (int)(player.X / meters_to_pixels);
            int y = (int)YAXIS - (int)(player.Y / meters_to_pixels);
            returnable.Add(new Rectangle(x, y, width, height));
            List<GameObject> objects = universe.getObjects();
            List<Platform> plats = universe.getPlatforms();
            foreach (GameObject obj in objects)
            {
                RectangleF rect = obj.getRekt();
                height = (int)(rect.Height / meters_to_pixels);
                width = (int)(rect.Width / meters_to_pixels);
                x = (int)(rect.X / meters_to_pixels);
                y = (int)YAXIS - (int)(rect.Y / meters_to_pixels);
                returnable.Add(new Rectangle(x, y, width, height));
            }
            foreach (Platform obj in plats)
            {
                RectangleF rect = obj.getHitbox();
                height = (int)(rect.Height / meters_to_pixels);
                width = (int)(rect.Width / meters_to_pixels);
                x = (int)(rect.X / meters_to_pixels);
                y = (int)YAXIS - (int)(rect.Y / meters_to_pixels);
                returnable.Add(new Rectangle(x, y, width, height));
            }
            return returnable;
        }


    }
}
