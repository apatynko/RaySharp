using System;
using System.Collections.Generic;
using System.Text;

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

        public override bool Hit(Ray r, float tMin, float tMax, out HitRecord rec)
        {
            HitRecord tempRecord = new HitRecord();
            bool hitAnything = false;
            float closestSoFar = tMax;

            rec.T = 0.0F;
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
    }
}
