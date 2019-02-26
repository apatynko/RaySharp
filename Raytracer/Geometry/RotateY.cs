using System;
using Raytracer.Geometry;
using Raytracer.BaseClasses;

namespace Raytracer.Geometry
{
    public class RotateY : Hitable
    {
        private Hitable _hitable;
        private bool _hasbox;
        private AxisAlignedBoundingBox _bbox;
        private double _sinTheta;
        private double _cosTheta;

        public RotateY(Hitable hitable, double angle)
        {
            _hitable = hitable;

            double radians = (Math.PI / 180.0) * angle;
            _sinTheta = Math.Sin(radians);
            _cosTheta = Math.Cos(radians);
            _hasbox = _hitable.BoundingBox(0, 1, out _bbox);

            Vec3 min = new Vec3(double.MaxValue, double.MaxValue, double.MaxValue);
            Vec3 max = new Vec3(-double.MaxValue, -double.MaxValue, -double.MaxValue);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        double x = i * _bbox.Max().X() + (1 - i) * _bbox.Min().X();
                        double y = j * _bbox.Max().Y() + (1 - j) * _bbox.Min().Y();
                        double z = k * _bbox.Max().Z() + (1 - k) * _bbox.Min().Z();

                        double newX = _cosTheta * x + _sinTheta * z;
                        double newZ = -_sinTheta * x + _cosTheta * z;
                        Vec3 tester = new Vec3(newX, y, newZ);
                        for (int c = 0; c < 3; c++)
                        {
                            if (tester[c] > max[c])
                            {
                                max[c] = tester[c];
                            }

                            if (tester[c] < min[c])
                            {
                                min[c] = tester[c];
                            }
                        }
                    }
                }
            }

            _bbox = new AxisAlignedBoundingBox(min, max);
        }

        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            Vec3 origin = new Vec3(r.Origin()[0], r.Origin()[1], r.Origin()[2]);
            Vec3 direction = new Vec3(r.Direction()[0], r.Direction()[1], r.Direction()[2]);

            origin[0] = _cosTheta * r.Origin()[0] - _sinTheta * r.Origin()[2];
            origin[2] = _sinTheta * r.Origin()[0] + _cosTheta * r.Origin()[2]; // TODO Check if this needs to be prefixed with a minus
            direction[0] = _cosTheta * r.Direction()[0] - _sinTheta * r.Direction()[2];
            direction[2] = _sinTheta * r.Direction()[0] + _cosTheta * r.Direction()[2]; // TODO see above
            Ray rotatedR = new Ray(origin, direction, r.Time());

            if (_hitable.Hit(rotatedR, tMin, tMax, out rec))
            {
                Vec3 p = new Vec3(rec.P[0], rec.P[1], rec.P[2]);
                Vec3 normal = new Vec3(rec.Normal[0], rec.Normal[1], rec.Normal[2]);

                p[0] = _cosTheta * rec.P[0] + _sinTheta * rec.P[2];
                p[2] = -_sinTheta * rec.P[0] + _cosTheta * rec.P[2];
                normal[0] = _cosTheta * rec.Normal[0] + _sinTheta * rec.Normal[2];
                normal[2] = -_sinTheta * rec.Normal[2] + _cosTheta * rec.Normal[2];

                rec.P = p;
                rec.Normal = normal;

                return true;
            }
            else
            {
                rec.P = null;
                rec.Normal = null;
                rec.T = 0;
                rec.U = 0;
                rec.V = 0;
                return false;
            }
        }

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox box)
        {
            box = _bbox;
            return _hasbox;
        }
    }
}
