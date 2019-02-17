using System;
using System.Collections.Generic;
using System.Text;
using Raytracer.Geometry;

namespace Raytracer.Textures
{
    public class NoiseTexture : Texture
    {
        private Perlin _noise;
        private double _scale;

        public NoiseTexture(double scale)
        {
            _scale = scale;
            _noise = new Perlin(); 
        }

        public override Vec3 Value(double u, double v, Vec3 p)
        {
            return new Vec3(1.0, 1.0, 1.0) * 0.5 * (1 + Math.Sin(_scale * p.Z() + 10 * _noise.Turbulence(p)));
        }
    }
}
