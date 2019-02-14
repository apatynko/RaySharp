using Raytracer.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.Textures
{
    public abstract class Texture
    {
        public abstract Vec3 Value(double u, double v, Vec3 p);
    }
}
