using System;
using Raytracer.Geometry;
using Raytracer.BaseClasses;
using Raytracer.Materials;
using System.Collections.Generic;

namespace Raytracer.Geometry
{
    public class Box : Hitable
    {
        private HitableList _list;
        private Vec3 _pMin;
        private Vec3 _pMax;

        public Box()
        {
        }

        public Box(Vec3 p0, Vec3 p1, Material mat)
        {
            _pMin = p0;
            _pMax = p1;

            List<Hitable> tmpList = new List<Hitable>();
            tmpList.Add(new XYRect(p0.X(), p1.X(), p0.Y(), p1.Y(), p1.Z(), mat));
            tmpList.Add(new FlipNormals(new XYRect(p0.X(), p1.X(), p0.Y(), p1.Y(), p0.Z(), mat)));
            tmpList.Add(new XZRect(p0.X(), p1.X(), p0.Z(), p1.Z(), p1.Y(), mat));
            tmpList.Add(new FlipNormals(new XZRect(p0.X(), p1.X(), p0.Z(), p1.Z(), p0.Y(), mat)));
            tmpList.Add(new YZRect(p0.Y(), p1.Y(), p0.Z(), p1.Z(), p1.X(), mat));
            tmpList.Add(new FlipNormals(new YZRect(p0.Y(), p1.Y(), p0.Z(), p1.Z(), p0.X(), mat)));
            
            _list = new HitableList(tmpList);
        }

        public override bool Hit(Ray r, double t0, double t1, out HitRecord rec)
        {
            return _list.Hit(r, t0, t1, out rec);
        }

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox box)
        {
            box = new AxisAlignedBoundingBox(_pMin, _pMax);
            return true;
        }
    }
}
