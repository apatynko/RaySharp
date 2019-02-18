using Raytracer.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.Geometry
{
    public class BvhNode : Hitable
    {
        public Hitable left;
        public Hitable right;
        public AxisAlignedBoundingBox box;

        #region Constructors
        public BvhNode()
        {

        }

        public BvhNode(List<Hitable> list, double time0, double time1)
        {
            int axis = (int)(FastRandom.RandomDouble() * 3);
            list.Sort(compareHitable(axis));

            if (list.Count == 1)
            {
                left = right = list[0];
            }
            else if (list.Count == 2)
            {
                left = list[0];
                right = list[1];
            }
            else
            {
                left = new BvhNode(list.GetRange(0, list.Count/2), time0, time1);
                right = new BvhNode(list.GetRange(list.Count/2, list.Count - list.Count/2), time0, time1);
            }

            AxisAlignedBoundingBox boxLeft, boxRight;
            if (!left.BoundingBox(time0, time1, out boxLeft) || !right.BoundingBox(time0, time1, out boxRight))
            {
                throw new Exception("No Bounding Box in BvhNode Constructor.");
            }

            box = AxisAlignedBoundingBox.SurroundingBox(boxLeft, boxRight);
        }
        #endregion

        #region Public Methods
        public override bool BoundingBox(double t0, double t1, out AxisAlignedBoundingBox boundingBox)
        {
            boundingBox = box;
            return true;
        }

        public override bool Hit(Ray r, double tMin, double tMax, out HitRecord rec)
        {
            if (box.Hit(r, tMin, tMax))
            {
                HitRecord leftRecord, rightRecord;
                bool hitLeft = left.Hit(r, tMin, tMax, out leftRecord);
                bool hitRight = right.Hit(r, tMin, tMax, out rightRecord);

                if (hitLeft && hitRight)
                {
                    if (leftRecord.T < rightRecord.T)
                    {
                        rec = leftRecord;
                    }
                    else
                    {
                        rec = rightRecord;
                    }
                    return true;
                }
                else if (hitLeft)
                {
                    rec = leftRecord;
                    return true;
                }
                else if (hitRight)
                {
                    rec = rightRecord;
                    return true;
                }
                else
                {
                    rec.Material = null;
                    rec.Normal = null;
                    rec.P = null;
                    rec.T = 0.0;
                    rec.U = 0.0;
                    rec.V = 0.0;
                    return false;
                }
            }
            else
            {
                rec.Material = null;
                rec.Normal = null;
                rec.P = null;
                rec.T = 0.0;
                rec.U = 0.0;
                rec.V = 0.0;
                return false;
            }
        }
        #endregion

        #region Private Methods
        private Comparison<Hitable> compareHitable(int axis)
        {
            if (axis == 0)
            {
                return new Comparison<Hitable>(compareHitableForSortingX);
            }
            else if (axis == 1)
            {
                return new Comparison<Hitable>(compareHitableForSortingY);
            }
            else if (axis == 2)
            {
                return new Comparison<Hitable>(compareHitableForSortingZ);
            }

            throw new ArgumentException("Axis out of range - Expected: {0,1,2}, Actual: " + axis.ToString());
        }

        private int compareHitableForSortingX(Hitable a, Hitable b)
        {
            AxisAlignedBoundingBox boxLeft, boxRight;
            if (!a.BoundingBox(0,0, out boxLeft) || !b.BoundingBox(0, 0, out boxRight))
            {
                throw new Exception("No Bounding Box in BvhNode Constructor.");
            }

            if (boxLeft.Min().X() - boxRight.Min().X() < 0.0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private int compareHitableForSortingY(Hitable a, Hitable b)
        {
            AxisAlignedBoundingBox boxLeft, boxRight;
            if (!a.BoundingBox(0, 0, out boxLeft) || !b.BoundingBox(0, 0, out boxRight))
            {
                throw new Exception("No Bounding Box in BvhNode Constructor.");
            }

            if (boxLeft.Min().Y() - boxRight.Min().Y() < 0.0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private int compareHitableForSortingZ(Hitable a, Hitable b)
        {
            AxisAlignedBoundingBox boxLeft, boxRight;
            if (!a.BoundingBox(0, 0, out boxLeft) || !b.BoundingBox(0, 0, out boxRight))
            {
                throw new Exception("No Bounding Box in BvhNode Constructor.");
            }

            if (boxLeft.Min().Z() - boxRight.Min().Z() < 0.0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        #endregion
    }
}
