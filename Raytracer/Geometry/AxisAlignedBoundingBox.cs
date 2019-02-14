using Raytracer.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.Geometry
{
    public class AxisAlignedBoundingBox
    {
        private Vec3 _min;
        private Vec3 _max;

        #region Constructors
        public AxisAlignedBoundingBox()
        {

        }

        public AxisAlignedBoundingBox(Vec3 a, Vec3 b)
        {
            _min = a;
            _max = b;
        }
        #endregion

        #region Public Methods
        public Vec3 Min()
        {
            return _min;
        }

        public Vec3 Max()
        {
            return _max;
        }

        public bool Hit(Ray r, double tMin, double tMax)
        {
            // TODO: Optimize intersection check
            double tmin = (_min[0] - r.Origin()[0]) / r.Direction()[0];
            double tmax = (_max[0] - r.Origin()[0]) / r.Direction()[0];

            if (tmin > tmax)
            {
                var tmp = tmax;
                tmax = tmin;
                tmin = tmp;
            }

            double tymin = (_min[1] - r.Origin()[1]) / r.Direction()[1];
            double tymax = (_max[1] - r.Origin()[1]) / r.Direction()[1];

            if (tymin > tymax)
            {
                var tmp = tymax;
                tymax = tymin;
                tymin = tmp;
            }

            if ((tmin > tymax) || (tymin > tmax))
            {
                return false;
            }

            if (tymin > tmin)
            {
                tmin = tymin;
            }

            if (tymax < tmax)
            {
                tmax = tymax;
            }

            double tzmin = (_min[2] - r.Origin()[2]) / r.Direction()[2];
            double tzmax = (_max[2] - r.Origin()[2]) / r.Direction()[2];

            if (tzmin > tzmax)
            {
                var tmp = tzmax;
                tzmax = tzmin;
                tzmin = tmp;
            }

            if ((tmin > tzmax) || (tzmin > tmax))
            {
                return false;
            }

            if (tzmin > tmin)
            {
                tmin = tzmin;
            }

            if (tzmax > tmax)
            {
                tmax = tzmax;
            }

            return true;
        }

        public static AxisAlignedBoundingBox SurroundingBox(AxisAlignedBoundingBox box0, AxisAlignedBoundingBox box1)
        {
            var small = new Vec3(Math.Min(box0.Min().X(), box1.Min().X()),
                Math.Min(box0.Min().Y(), box1.Min().Y()),
                Math.Min(box0.Min().Z(), box1.Min().Z()));
            var big = new Vec3(Math.Max(box0.Max().X(), box1.Max().X()),
                Math.Max(box0.Max().Y(), box1.Max().Y()),
                Math.Max(box0.Max().Z(), box1.Max().Z()));

            return new AxisAlignedBoundingBox(small, big);
        }
        #endregion
    }
}
