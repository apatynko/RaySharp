using System;
using System.Collections.Generic;
using System.Text;
using Raytracer.Geometry;

namespace Raytracer.Textures
{
    class ConstantTexture : Texture
    {
        private Vec3 _color;

        #region Constructors
        public ConstantTexture()
        {

        }

        public ConstantTexture(Vec3 color)
        {
            _color = color;
        }
        #endregion

        public override Vec3 Value(double u, double v, Vec3 p)
        {
            return _color;
        }
    }
}
