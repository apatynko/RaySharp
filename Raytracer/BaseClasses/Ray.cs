using Raytracer.Geometry;

namespace Raytracer.BaseClasses
{
    public class Ray
    {
        private Vec3 _a;
        private Vec3 _b;

        private double _time;

        #region Constructors
        public Ray()
        {

        }

        public Ray(Vec3 a, Vec3 b, double ti = 0.0)
        {
            _a = a;
            _b = b;
            _time = ti;
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

        public Vec3 PointAtParameter(double t)
        {
            return _a + t * _b;
        }

        public double Time()
        {
            return _time;
        }
        #endregion
    }
}
