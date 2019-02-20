using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Textures;

namespace Raytracer.Materials
{
    public abstract class Material
    {
        public abstract bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered);
        
        public virtual Vec3 emitted(double u, double v, Vec3 p)
        {
            return new Vec3(0.0, 0.0, 0.0);
        }
    }
}
