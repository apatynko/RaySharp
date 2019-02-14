using Raytracer.Geometry;
using Raytracer.BaseClasses;
using Raytracer.Textures;

namespace Raytracer.Materials
{
    public class Lambertian : Material
    {
        private Texture _albedo;

        #region Constructors
        public Lambertian(Texture a)
        {
            _albedo = a;
        }
        #endregion

        public override bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 target = rec.P + rec.Normal + FastRandom.RandomInUnitSphere();
            scattered = new Ray(rec.P, target - rec.P, rIn.Time());
            attenuation = _albedo.Value(0, 0, rec.P);
            return true;
        }
    }
}
