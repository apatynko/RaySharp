using Raytracer.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.BaseClasses
{
    public class Camera
    {
        private Vec3 _origin;           // Camera center / "eye position"
        private Vec3 _lowerLeftCorner;  // Starting point for "scanning"
        private Vec3 _horizontal;       // Horizontal field of view
        private Vec3 _vertical;         // Vertical field of view

        #region Constructors
        public Camera()
        {
            _lowerLeftCorner = new Vec3(-2.0F, -1.0F, -1.0F);
            _horizontal = new Vec3(4.0F, 0.0F, 0.0F);
            _vertical = new Vec3(0.0F, 2.0F, 0.0F);
            _origin = new Vec3(0.0F, 0.0F, 0.0F);
        }
        #endregion

        public Ray GetRay(float u, float v)
        {
            return new Ray(_origin, _lowerLeftCorner + u * _horizontal + v * _vertical - _origin);
        }
    }
}
