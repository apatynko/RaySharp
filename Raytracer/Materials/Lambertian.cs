using Raytracer.Geometry;
using Raytracer.BaseClasses;

namespace Raytracer.Materials
{
    public class Lambertian : Material
    {
        private Vec3 _albedo;

        #region Constructors
        public Lambertian(Vec3 a)
        {
            _albedo = a;
        }
        #endregion

        public override bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 target = rec.P + rec.Normal + FastRandom.RandomInUnitSphere();
            scattered = new Ray(rec.P, target - rec.P, rIn.Time());
            attenuation = _albedo;
            return true;
        }
    }
}
