using System;
using Raytracer.BaseClasses;
using Raytracer.Geometry;

namespace Raytracer.Materials
{
    public class Dielectric : Material
    {
        private double _refIndex;

        #region Constructors
        public Dielectric(double ri)
        {
            _refIndex = ri;
        }
        #endregion

        public override bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 outwardNormal = new Vec3();
            Vec3 reflected = Vec3.Reflect(rIn.Direction(), rec.Normal);
            double niOverNt;
            attenuation = new Vec3(1.0, 1.0, 1.0);
            Vec3 refracted = new Vec3();
            double reflectProb;
            double cosine;

            if (Vec3.Dot(rIn.Direction(), rec.Normal) > 0)
            {
                outwardNormal = -rec.Normal;
                niOverNt = _refIndex;
                cosine = _refIndex * Vec3.Dot(rIn.Direction(), rec.Normal) / rIn.Direction().Length();
            }
            else
            {
                outwardNormal = rec.Normal;
                niOverNt = 1.0 / _refIndex;
                cosine = -Vec3.Dot(rIn.Direction(), rec.Normal) / rIn.Direction().Length();
            }

            if (Vec3.Refract(rIn.Direction(), outwardNormal, niOverNt, out refracted))
            {
                reflectProb = Schlick(cosine, _refIndex);
            }
            else
            {
                reflectProb = 1.0;
            }

            var rnd = new Random();
            if (rnd.NextDouble() < reflectProb)
            {
                scattered = new Ray(rec.P, reflected, rIn.Time());
            }
            else
            {
                scattered = new Ray(rec.P, refracted, rIn.Time());
            }

            return true;
        }

        #region Private Methods
        private double Schlick(double cosine, double refIndex)
        {
            double r0 = (1 - refIndex) / (1 + refIndex);
            r0 *= r0;
            return r0 + (1 - r0) * Math.Pow(1-cosine, 5);
        }
        #endregion
    }
}
