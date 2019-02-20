using System;
using Raytracer.BaseClasses;
using Raytracer.Materials;

namespace Raytracer.Geometry
{
    public class YZRect : Hitable
    {
       private double _y0;
       private double _y1;
       private double _z0;
       private double _z1;
       private double _k;
       private Material _mat;

        public YZRect()
        {
        }

        public YZRect(double y0, double y1, double z0, double z1, double k, Material mat)
        {
            _y0 = y0;
            _y1 = y1;
            _z0 = z0;
            _z1 = z1;
            _k = k;
            _mat = mat;
        }

        public override bool Hit(Ray r, double t0, double t1, out HitRecord rec)
        {
            double t = (_k - r.Origin().X()) / r.Direction().X();
            if (t < t0 || t > t1)
            {
                rec.T = 0.0;
                rec.U = 0.0;
                rec.V = 0.0;
                rec.P = null;
                rec.Normal = null;
                rec.Material = null;

                return false;    
            }

            double y = r.Origin().Y() + t * r.Direction().Y();
            double z = r.Origin().Z() + t * r.Direction().Z();
            if (y < _y0 || y > _y1 || z < _z0 || z > _z1)
            {
                rec.T = 0.0;
                rec.U = 0.0;
                rec.V = 0.0;
                rec.P = null;
                rec.Normal = null;
                rec.Material = null;
                
                return false;
            }

            rec.U = (y - _y0) / (_y1 - _y0);
            rec.V = (z - _z0) / (_z1 - _z0);
            rec.T = t;
            rec.Material = _mat;
            rec.P = r.PointAtParameter(t);
            rec.Normal = new Vec3(1.0, 0.0, 0.0);

            return true;
        }

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox box)
        {
            box = new AxisAlignedBoundingBox(new Vec3(_k - 0.0001, _y0, _z0), new Vec3(_k + 0.0001, _y1, _z1));
            return true;
        }
    }
}
