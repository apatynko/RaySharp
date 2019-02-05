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

        private Vec3 _u;
        private Vec3 _v;
        private Vec3 _w;

        private float _lensRadius;

        #region Constructors
        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vup, float vFov, float aspect, float aperture, float focusDist) // vFov = top to bottom in deg
        {
            _lensRadius = aperture / 2.0F;
            _u = new Vec3();
            _v = new Vec3();
            _w = new Vec3();

            float theta = vFov * (float)Math.PI / 180;
            float halfHeight = (float)Math.Tan(theta / 2);
            float halfWidth = aspect * halfHeight;

            _origin = lookFrom;
            _w = Vec3.UnitVector(lookFrom - lookAt);
            _u = Vec3.UnitVector(Vec3.Cross(vup, _w));
            _v = Vec3.Cross(_w, _u);

            _lowerLeftCorner = _origin - halfWidth * focusDist * _u - halfHeight * focusDist * _v - focusDist * _w;
            _horizontal = 2 * halfWidth * focusDist * _u;
            _vertical = 2 * halfHeight * focusDist * _v;
        }
        #endregion

        public Ray GetRay(float s, float t)
        {
            Vec3 rd = _lensRadius * Vec3.RandomInUnitDisk();
            Vec3 offset = _u * rd.X() + _v * rd.Y();
            return new Ray(_origin + offset, _lowerLeftCorner + s * _horizontal + t * _vertical - _origin - offset);
        }
    }
}
