using System;
using Raytracer.BaseClasses;
using Raytracer.Materials;

namespace Raytracer.Geometry
{
    public class XYRect : Hitable
    {
       private double _x0;
       private double _x1;
       private double _y0;
       private double _y1;
       private double _k;
       private Material _mat;

        public XYRect()
        {
        }

        public XYRect(double x0, double x1, double y0, double y1, double k, Material mat)
        {
            _x0 = x0;
            _x1 = x1;
            _y0 = y0;
            _y1 = y1;
            _k = k;
            _mat = mat;
        }

        public override bool Hit(Ray r, double t0, double t1, out HitRecord rec)
        {
            double t = (_k - r.Origin().Z()) / r.Direction().Z();
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
            double y = r.Origin().Y() + t * r.Direction().Y();
            if (x < _x0 || x > _x1 || y < _y0 || y > _y1)
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
            rec.V = (y - _y0) / (_y1 - _y0);
            rec.T = t;
            rec.Material = _mat;
            rec.P = r.PointAtParameter(t);
            rec.Normal = new Vec3(0.0, 0.0, 1.0);

            return true;
        }

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox box)
        {
            box = new AxisAlignedBoundingBox(new Vec3(_x0, _y0, _k - 0.0001), new Vec3(_x1, _y1, _k + 0.0001));
            return true;
        }
    }
}
