using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.Geometry
{
    public class Vec3
    {
        private float[] _e = new float[3];

        #region Constructors
        public Vec3()
        {

        }

        public Vec3(float e0, float e1, float e2)
        {
            _e[0] = e0;
            _e[1] = e1;
            _e[2] = e2;
        }
        #endregion

        #region Component Getters
        public float X()
        {
            return _e[0];
        }

        public float Y()
        {
            return _e[1];
        }

        public float Z()
        {
            return _e[2];
        }

        public float R()
        {
            return _e[0];
        }

        public float G()
        {
            return _e[1];
        }

        public float B()
        {
            return _e[2];
        }
        #endregion

        #region Operators
        public static Vec3 operator -(Vec3 a)
        {
            return new Vec3(-a.X(), -a.Y(), -a.Z());
        }

        public float this[int key]
        {
            get
            {
                return _e[key];
            }
            set
            {
                _e[key] = value;
            }
        }

        public static Vec3 operator+(Vec3 a, Vec3 b)
        {
            return new Vec3(a[0] + b[0], a[1] + b[1], a[2] + b[2]);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a[0] - b[0], a[1] - b[1], a[2] - b[2]);
        }

        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(a[0] * b[0], a[1] * b[1], a[2] * b[2]);
        }

        public static Vec3 operator /(Vec3 a, Vec3 b)
        {
            return new Vec3(a[0] / b[0], a[1] / b[1], a[2] / b[2]);
        }

        public static Vec3 operator *(float t, Vec3 v)
        {
            return new Vec3(t * v[0], t * v[1], t * v[2]);
        }

        public static Vec3 operator /(Vec3 v, float t)
        {
            return new Vec3(v[0] / t, v[1] / t, v[2] / t);
        }

        public static Vec3 operator *(Vec3 v, float t)
        {
            return new Vec3(t * v[0], t * v[1], t * v[2]);
        }
        #endregion

        #region Public Methods
        public float Length()
        {
            return (float)Math.Sqrt(_e[0]*_e[0] + _e[1] * _e[1] + _e[2] * _e[2]);
        }

        public float SquaredLength()
        {
            return _e[0] * _e[0] + _e[1] * _e[1] + _e[2] * _e[2];
        }

        public void MakeUnitVector()
        {
            float k = 1.0F / this.Length();
            _e[0] *= k;
            _e[1] *= k;
            _e[2] *= k;
        }

        public static float Dot(Vec3 a, Vec3 b)
        {
            return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        }

        public Vec3 Cross(Vec3 a, Vec3 b)
        {
            float e0 = a[1] * b[2] - a[2] * b[1];
            float e1 = -(a[0] * b[2] - a[2] * b[0]);
            float e2 = a[0] * b[1] - a[1] * b[0];

            return new Vec3(e0, e1, e2);
        }

        public static Vec3 UnitVector(Vec3 v)
        {
            return v / v.Length();
        }

        public static Vec3 RandomInUnitSphere()
        {
            Vec3 p = new Vec3();
            Random rnd = new Random();
            do
            {
                p = 2.0F * new Vec3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()) - new Vec3(1.0F, 1.0F, 1.0F);
            } while (p.SquaredLength() >= 1.0F);
            return p;
        }

        public static Vec3 Reflect(Vec3 v, Vec3 n)
        {
            return v - 2 * Dot(v, n) * n;
        }

        public static bool Refract(Vec3 v, Vec3 n, float niOverNt, out Vec3 refracted)
        {
            Vec3 uv = UnitVector(v);
            float dt = Dot(uv, n);
            float discriminant = 1.0F - niOverNt * niOverNt * (1 - dt * dt);

            if (discriminant > 0)
            {
                refracted = niOverNt * (uv - n * dt) - n * (float)Math.Sqrt(discriminant);
                return true;
            }
            else
            {
                refracted = null;
                return false;
            }
        }
        #endregion
    }
}
