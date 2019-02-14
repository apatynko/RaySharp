using Raytracer.Geometry;
using Raytracer.Materials;

namespace Raytracer.BaseClasses
{
    public struct HitRecord
    {
        public double T;
        public Vec3 P;
        public Vec3 Normal;
        public Material Material;
    }

    public abstract class Hitable
    {
        public abstract bool Hit(Ray r, double tMin, double tMax, out HitRecord rec);
        public abstract bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox boundingBox);
    }
}
