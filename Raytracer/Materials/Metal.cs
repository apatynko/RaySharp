using Raytracer.BaseClasses;
using Raytracer.Geometry;

namespace Raytracer.Materials
{
    public class Metal : Material
    {
        private Vec3 _albedo;
        private double _fuzz;

        public Metal(Vec3 a, double f)
        {
            _albedo = a;
            _fuzz = f < 1 ? f : 1;
        }

        public override bool Scatter(Ray rIn, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 reflected = Vec3.Reflect(Vec3.UnitVector(rIn.Direction()), rec.Normal);
            scattered = new Ray(rec.P, reflected + _fuzz * Vec3.RandomInUnitSphere());
            attenuation = _albedo;
            return (Vec3.Dot(scattered.Direction(), rec.Normal) > 0);
        }
    }
}
