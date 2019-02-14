using Raytracer.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.BaseClasses
{
    public static class FastRandom
    {
        private static uint _state;

        public static void Initialize(uint seed)
        {
            _state = seed;
        }

        private static void XorShift()
        {
            _state ^= _state << 13;
            _state ^= _state >> 17;
            _state ^= _state << 15;
        }

        public static double RandomDouble()
        {
            XorShift();
            return _state * (1.0 / 4294967296.0);
        }

        public static Vec3 RandomInUnitDisk()
        {
            Vec3 p;
            do
            {
                p = 2.0 * new Vec3(RandomDouble(), RandomDouble(), 0.0) - new Vec3(1.0, 1.0, 0.0);
            } while (Vec3.Dot(p, p) >= 1.0);
            return p;
        }

        public static Vec3 RandomInUnitSphere()
        {
            Vec3 p;
            do
            {
                p = 2.0 * new Vec3(RandomDouble(), RandomDouble(), RandomDouble()) - new Vec3(1.0, 1.0, 1.0);
            } while (p.SquaredLength() >= 1.0);
            return p;
        }
    }
}
