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
        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vup, float vFov, float aspect) // vFov = top to bottom in deg
        {
            Vec3 u = new Vec3();
            Vec3 v = new Vec3();
            Vec3 w = new Vec3();

            float theta = vFov * (float)Math.PI / 180;
            float halfHeight = (float)Math.Tan(theta / 2);
            float halfWidth = aspect * halfHeight;

            _origin = lookFrom;
            w = Vec3.UnitVector(lookFrom - lookAt);
            u = Vec3.UnitVector(Vec3.Cross(vup, w));
            v = Vec3.Cross(w, u);

            _lowerLeftCorner = _origin - halfWidth * u - halfHeight * v - w;
            _horizontal = 2 * halfWidth * u;
            _vertical = 2 * halfHeight * v;
        }
        #endregion

        public Ray GetRay(float s, float t)
        {
            return new Ray(_origin, _lowerLeftCorner + s * _horizontal + t * _vertical - _origin);
        }
    }
}
