using Raytracer.BaseClasses;
using Raytracer.Geometry;

namespace Raytracer.Materials
{
    public abstract class Material
    {
        public abstract bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered);
    }
}
