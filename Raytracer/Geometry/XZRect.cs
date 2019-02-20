using System;
using Raytracer.BaseClasses;
using Raytracer.Materials;

namespace Raytracer.Geometry
{
    public class XZRect : Hitable
    {
       private double _x0;
       private double _x1;
       private double _z0;
       private double _z1;
       private double _k;
       private Material _mat;

        public XZRect()
        {
        }

        public XZRect(double x0, double x1, double z0, double z1, double k, Material mat)
        {
            _x0 = x0;
            _x1 = x1;
            _z0 = z0;
            _z1 = z1;
            _k = k;
            _mat = mat;
        }

        public override bool Hit(Ray r, double t0, double t1, out HitRecord rec)
        {
            double t = (_k - r.Origin().Y()) / r.Direction().Y();
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

            double x = r.Origin().X() + t * r.Direction().X();
            double z = r.Origin().Z() + t * r.Direction().Z();
            if (x < _x0 || x > _x1 || z < _z0 || z > _z1)
            {
                rec.T = 0.0;
                rec.U = 0.0;
                rec.V = 0.0;
                rec.P = null;
                rec.Normal = null;
                rec.Material = null;
                
                return false;
            }

            rec.U = (x - _x0) / (_x1 - _x0);
            rec.V = (z - _z0) / (_z1 - _z0);
            rec.T = t;
            rec.Material = _mat;
            rec.P = r.PointAtParameter(t);
            rec.Normal = new Vec3(0.0, 1.0, 0.0);

            return true;
        }

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox box)
        {
            box = new AxisAlignedBoundingBox(new Vec3(_x0, _k - 0.0001, _z0), new Vec3(_x1, _k + 0.0001, _z0));
            return true;
        }
    }
}
