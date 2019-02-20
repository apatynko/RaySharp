using System;
using Raytracer.Geometry;

namespace Raytracer.BaseClasses
{
    public class FlipNormals : Hitable
    {
        private Hitable _hitable;

        public FlipNormals(Hitable h)
        {
            _hitable = h;
        }

        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            if (_hitable.Hit(r, tMin, tMax, out rec))
            {
                rec.Normal = -rec.Normal;
                return true;
            }
            else
            {
                rec.T = 0.0;
                rec.U = 0.0;
                rec.V = 0.0;
                rec.Normal = null;
                rec.P = null;
                return false;
            }
        }

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox box)
        {
            return _hitable.BoundingBox(t0, t1, out box);
        }
    }
}
