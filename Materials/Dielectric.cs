using System;
using System.Collections.Generic;
using System.Text;
using Raytracer.BaseClasses;
using Raytracer.Geometry;

namespace Raytracer.Materials
{
    public class Dielectric : Material
    {
        private float _refIndex;

        #region Constructors
        public Dielectric(float ri)
        {
            _refIndex = ri;
        }
        #endregion

        public override bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 outwardNormal = new Vec3();
            Vec3 reflected = Vec3.Reflect(rIn.Direction(), rec.Normal);
            float niOverNt;
            attenuation = new Vec3(1.0F, 1.0F, 1.0F);
            Vec3 refracted = new Vec3();
            float reflectProb;
            float cosine;

            if (Vec3.Dot(rIn.Direction(), rec.Normal) > 0)
            {
                outwardNormal = -rec.Normal;
                niOverNt = _refIndex;
                cosine = _refIndex * Vec3.Dot(rIn.Direction(), rec.Normal) / rIn.Direction().Length();
            }
            else
            {
                outwardNormal = rec.Normal;
                niOverNt = 1.0F / _refIndex;
                cosine = -Vec3.Dot(rIn.Direction(), rec.Normal) / rIn.Direction().Length();
            }

            if (Vec3.Refract(rIn.Direction(), outwardNormal, niOverNt, out refracted))
            {
                reflectProb = Schlick(cosine, _refIndex);
            }
            else
            {
                reflectProb = 1.0F;
            }

            var rnd = new Random();
            if (rnd.NextDouble() < reflectProb)
            {
                scattered = new Ray(rec.P, reflected);
            }
            else
            {
                scattered = new Ray(rec.P, refracted);
            }

            return true;
        }

        #region Private Methods
        private float Schlick(float cosine, float refIndex)
        {
            float r0 = (1 - refIndex) / (1 + refIndex);
            r0 *= r0;
            return r0 + (1 - r0) * (float)Math.Pow((double)1-cosine, 5);
        }
        #endregion
    }
}
