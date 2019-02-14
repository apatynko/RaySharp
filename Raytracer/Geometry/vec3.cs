using System;

namespace Raytracer.Geometry
{
    public class Vec3
    {
        private double[] _e = new double[3];

        #region Constructors
        public Vec3()
        {

        }

        public Vec3(double e0, double e1, double e2)
        {
            _e[0] = e0;
            _e[1] = e1;
            _e[2] = e2;
        }
        #endregion

        #region Component Getters
        public double X()
        {
            return _e[0];
        }

        public double Y()
        {
            return _e[1];
        }

        public double Z()
        {
            return _e[2];
        }

        public double R()
        {
            return _e[0];
        }

        public double G()
        {
            return _e[1];
        }

        public double B()
        {
            return _e[2];
        }
        #endregion

        #region Operators
        public static Vec3 operator -(Vec3 a)
        {
            return new Vec3(-a.X(), -a.Y(), -a.Z());
        }

        public double this[int key]
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

        public static Vec3 operator *(double t, Vec3 v)
        {
            return ScalarMultiplication(v, t);
        }

        public static Vec3 operator /(Vec3 v, double t)
        {
            return new Vec3(v[0] / t, v[1] / t, v[2] / t);
        }

        public static Vec3 operator *(Vec3 v, double t)
        {
            return ScalarMultiplication(v, t);
        }
        #endregion

        #region Public Methods
        public double Length()
        {
            return Math.Sqrt(_e[0]*_e[0] + _e[1] * _e[1] + _e[2] * _e[2]);
        }

        public double SquaredLength()
        {
            return _e[0] * _e[0] + _e[1] * _e[1] + _e[2] * _e[2];
        }

        public void MakeUnitVector()
        {
            double k = 1.0 / this.Length();
            _e[0] *= k;
            _e[1] *= k;
            _e[2] *= k;
        }

        public static double Dot(Vec3 a, Vec3 b)
        {
            return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            double e0 = a[1] * b[2] - a[2] * b[1];
            double e1 = -(a[0] * b[2] - a[2] * b[0]);
            double e2 = a[0] * b[1] - a[1] * b[0];

            return new Vec3(e0, e1, e2);
        }

        public static Vec3 UnitVector(Vec3 v)
        {
            return v / v.Length();
        }

        public static Vec3 Reflect(Vec3 v, Vec3 n)
        {
            return v - 2 * Dot(v, n) * n;
        }

        public static bool Refract(Vec3 v, Vec3 n, double niOverNt, out Vec3 refracted)
        {
            Vec3 uv = UnitVector(v);
            double dt = Dot(uv, n);
            double discriminant = 1.0 - niOverNt * niOverNt * (1 - dt * dt);

            if (discriminant > 0)
            {
                refracted = niOverNt * (uv - n * dt) - n * Math.Sqrt(discriminant);
                return true;
            }
            else
            {
                refracted = null;
                return false;
            }
        }
        #endregion

        #region Private Methods
        private static Vec3 ScalarMultiplication(Vec3 v, double t)
        {
            return new Vec3(t * v[0], t * v[1], t * v[2]);
        }
        #endregion
    }
}
