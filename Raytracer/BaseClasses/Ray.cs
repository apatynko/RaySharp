using Raytracer.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.BaseClasses
{
    public class Ray
    {
        private Vec3 _a;
        private Vec3 _b;

        #region Constructors
        public Ray()
        {

        }

        public Ray(Vec3 a, Vec3 b)
        {
            _a = a;
            _b = b;
        }
        #endregion

        #region Public Methods
        public Vec3 Origin()
        {
            return _a;
        }

        public Vec3 Direction()
        {
            return _b;
        }

        public Vec3 PointAtParameter(float t)
        {
            return _a + t * _b;
        }
        #endregion
    }
}
