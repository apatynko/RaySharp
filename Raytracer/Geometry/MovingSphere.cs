using Raytracer.BaseClasses;
using Raytracer.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.Geometry
{
    class MovingSphere : Hitable
    {
        private Vec3 _center0;
        private Vec3 _center1;

        private double _radius;

        private Material _material;

        private double _time0;
        private double _time1;

        #region Constructors
        public MovingSphere()
        {

        }

        public MovingSphere(Vec3 cen0, Vec3 cen1, double ti0, double ti1, double r, Material m)
        {
            _center0 = cen0;
            _center1 = cen1;
            _radius = r;
            _material = m;
            _time0 = ti0;
            _time1 = ti1;
        }
        #endregion

        #region Public Methods
        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            Vec3 oc = r.Origin() - Center(r.Time());
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
                    rec.Normal = (rec.P - Center(r.Time())) / _radius;
                    rec.Material = _material;
                    return true;
                }

                temp = (-b + Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = r.PointAtParameter(rec.T);
                    rec.Normal = (rec.P - Center(r.Time())) / _radius;
                    rec.Material = _material;
                    return true;
                }
            }

            rec.T = 0.0;
            rec.P = null;
            rec.Normal = null;
            rec.Material = null;
            return false;
        }
        #endregion

        #region Private Methods
        private Vec3 Center(double t)
        {
            return _center0 + ((t - _time0) / (_time1 - _time0)) * (_center1 - _center0);
        }
        #endregion
    }
}
