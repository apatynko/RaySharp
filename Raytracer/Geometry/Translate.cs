using System;
using Raytracer.Geometry;
using Raytracer.BaseClasses;

namespace Raytracer.Geometry
{
    public class Translate : Hitable
    {
        private Hitable _hitable;
        private Vec3 _offset;

        public Translate(Hitable h, Vec3 offset)
        {
            _hitable = h;
            _offset = offset;
        }

        
        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            Ray movedRay = new Ray(r.Origin() - _offset, r.Direction(), r.Time());
            if (_hitable.Hit(movedRay, tMin, tMax, out rec))
            {
                rec.P += _offset;
                return true;
            }

            rec.P = null;
            rec.T = 0;
            rec.U = 0;
            rec.V = 0;
            rec.Material = null;
            return false;
        } 

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox box)
        {
            if (_hitable.BoundingBox(t0, t1, out box))
            {
                return true;
            }

            box = null;
            return false;
        }
    }
}
