using System;
using System.Collections.Generic;
using System.Text;
using Raytracer.Geometry;

namespace Raytracer.Textures
{
    public class CheckerTexture : Texture
    {
        private Texture _even;
        private Texture _odd;

        #region Constructors
        public CheckerTexture()
        {

        }

        public CheckerTexture(Texture t0, Texture t1)
        {
            _even = t0;
            _odd = t1;
        }
        #endregion

        public override Vec3 Value(double u, double v, Vec3 p)
        {
            double sines = Math.Sin(10 * p.X()) * Math.Sin(10 * p.Y()) * Math.Sin(10 * p.Z());
            if (sines < 0)
            {
                return _odd.Value(u, v, p);
            }
            else
            {
                return _even.Value(u, v, p);
            }
        }
    }
}
