using System.Collections.Generic;
using Raytracer.Geometry;

namespace Raytracer.BaseClasses
{
    public class HitableList : Hitable
    {
        private List<Hitable> _list;

        #region Constructors
        public HitableList()
        {

        }

        public HitableList(List<Hitable> l)
        {
            _list = l;
        }
        #endregion

        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            HitRecord tempRecord = new HitRecord();
            bool hitAnything = false;
            double closestSoFar = tMax;

            rec.T = 0.0;
            rec.P = null;
            rec.Normal = null;
            rec.Material = null;

            foreach (var hitable in _list)
            {
                if(hitable.Hit(r, tMin, closestSoFar, out tempRecord))
                {
                    hitAnything = true;
                    closestSoFar = tempRecord.T;
                    rec = tempRecord;
                }
            }
            
            return hitAnything;
        }

        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox boundingBox)
        {
            if (_list.Count < 1)
            {
                boundingBox = null;
                return false;
            }

            AxisAlignedBoundingBox tempBox;

            bool firstTrue = _list[0].BoundingBox(t0, t1, out tempBox);
            if (!firstTrue)
            {
                boundingBox = null;
                return false;
            }
            else
            {
                boundingBox = tempBox;
            }

            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].BoundingBox(t0, t1, out tempBox)) 
                {
                    boundingBox = AxisAlignedBoundingBox.SurroundingBox(boundingBox, tempBox);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
