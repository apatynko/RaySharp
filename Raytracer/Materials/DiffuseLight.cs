using System;
using Raytracer.BaseClasses;
using Raytracer.Materials;
using Raytracer.Geometry;
using Raytracer.Textures;

namespace Raytracer.Materials
{
    public class DiffuseLight : Material
    {
        private Texture _emit;

        public DiffuseLight(Texture emit)
        {
            _emit = emit;
        }

        public override bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            rec.Material = null;
            rec.Normal = null;
            attenuation = null;
            scattered = null;

            return false; 
        }

        public override Vec3 emitted(double u, double v, Vec3 p)
        {
            return _emit.Value(u, v, p);
        }
    }
}
