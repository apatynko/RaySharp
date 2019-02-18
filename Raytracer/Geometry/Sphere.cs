using Raytracer.BaseClasses;
using Raytracer.Materials;
using System;

namespace Raytracer.Geometry
{
    public class Sphere : Hitable
    {
        private Vec3 _center;
        private double _radius;
        private Material _material;

        #region Constructors
        public Sphere()
        {

        }

        public Sphere(Vec3 cen, double r, Material mat)
        {
            _center = cen;
            _radius = r;
            _material = mat;
        }
        #endregion

        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            Vec3 oc = r.Origin() - _center;
            double a = Vec3.Dot(r.Direction(), r.Direction());
            double b = Vec3.Dot(oc, r.Direction());
            double c = Vec3.Dot(oc, oc) - _radius * _radius;

            double discriminant = b * b - a * c;

            if (discriminant > 0)
            {
                double temp = (-b - Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = r.PointAtParameter(rec.T);
                    rec.Normal = (rec.P - _center) / _radius;
                    rec.Material = _material;
                    GetSphereUV((rec.P - _center) / _radius, out rec.U, out rec.V);
                    return true;
                }

                temp = (-b + Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = r.PointAtParameter(rec.T);
                    rec.Normal = (rec.P - _center) / _radius;
                    rec.Material = _material;
                    GetSphereUV((rec.P - _center) / _radius, out rec.U, out rec.V);
                    return true;
                }
            }

            rec.T = 0.0;
            rec.P = null;
            rec.U = 0.0;
            rec.V = 0.0;
            rec.Normal = null;
            rec.Material = null;
            return false;
        }

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox boundingBox)
        {
            boundingBox = new AxisAlignedBoundingBox(_center - new Vec3(_radius, _radius, _radius),
                _center + new Vec3(_radius, _radius, _radius));
            return true;
        }

        public static void GetSphereUV(Vec3 p, out double u, out double v)
        {
            double phi = Math.Atan2(p.Z(), p.X());
            double theta = Math.Asin(p.Y());

            u = 1 - (phi + Math.PI) / (2 * Math.PI);
            v = (theta + Math.PI / 2) / Math.PI;
            return;
        }
    }
}
