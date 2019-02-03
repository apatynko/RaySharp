using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer
{
    public class Sphere : Hitable
    {
        private Vec3 _center;
        private float _radius;

        #region Constructors
        public Sphere()
        {

        }

        public Sphere(Vec3 cen, float r)
        {
            _center = cen;
            _radius = r;
        }
        #endregion

        public override bool Hit(Ray r, float tMin, float tMax, out HitRecord rec)
        {
            Vec3 oc = r.Origin() - _center;
            float a = Vec3.Dot(r.Direction(), r.Direction());
            float b = Vec3.Dot(oc, r.Direction());
            float c = Vec3.Dot(oc, oc) - _radius * _radius;

            float discriminant = b * b - a * c;

            if (discriminant > 0)
            {
                float temp = (-b - (float)Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = r.PointAtParameter(rec.T);
                    rec.Normal = (rec.P - _center) / _radius;
                    return true;
                }

                temp = (-b + (float)Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = r.PointAtParameter(rec.T);
                    rec.Normal = (rec.P - _center) / _radius;
                    return true;
                }
            }

            rec.T = 0.0F;
            rec.P = null;
            rec.Normal = null;
            return false;
        }
    }
}
