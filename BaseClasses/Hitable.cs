using Raytracer.Geometry;
using Raytracer.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.BaseClasses
{
    public struct HitRecord
    {
        public float T;
        public Vec3 P;
        public Vec3 Normal;
        public Material Material;
    }

    public abstract class Hitable
    {
        public abstract bool Hit(Ray r, float tMin, float tMax, out HitRecord rec);
    }
}
