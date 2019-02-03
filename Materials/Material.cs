using Raytracer.BaseClasses;
using Raytracer.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.Materials
{
    public abstract class Material
    {
        public abstract bool scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered);
    }
}
