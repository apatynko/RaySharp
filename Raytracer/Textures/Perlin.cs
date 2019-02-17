using System;
using System.Collections.Generic;
using System.Text;
using Raytracer.Geometry;
using Raytracer.BaseClasses;

namespace Raytracer.Textures 
{
    public class Perlin
    {
        private Vec3[] _randomVec;
        private int[] _permX;
        private int[] _permY;
        private int[] _permZ;

        public Perlin()
        {
            _randomVec = PerlinGenerate();
            _permX = PerlinGeneratePerm();
            _permY = PerlinGeneratePerm();
            _permZ = PerlinGeneratePerm();
        }

        public double Noise(Vec3 p)
        {
            double u = p.X() - Math.Floor(p.X());
            double v = p.Y() - Math.Floor(p.Y());
            double w = p.Z() - Math.Floor(p.Z());

            int i = (int) Math.Floor(p.X());
            int j = (int) Math.Floor(p.Y());
            int k = (int) Math.Floor(p.Z());

            Vec3[,,] c = new Vec3[2, 2, 2];
            for (int di = 0; di < 2; di++)
            {
                for (int dj = 0; dj < 2; dj++)
                {
                    for (int dk = 0; dk < 2; dk++)
                    {
                        c[di, dj, dk] = _randomVec[_permX[(i + di) & 255] ^ _permY[(j + dj) & 255] ^ _permZ[(k + dk) & 255]];
                    }
                }
            }

            return TrilinearInterpolation(c, u, v, w);
        }

        public double Turbulence(Vec3 p, int depth=7)
        {
            double accum = 0;
            Vec3 tempP = p;
            double weight = 1.0;

            for (int i = 0; i < depth; i++)
            {
                accum += weight * Noise(tempP);
                weight *= 0.5;
                tempP *= 2;
            }

            return Math.Abs(accum);
        }

        private void Permute(int[] p, int n)
        {
            for (int i = n-1; i > 0; i--)
            {
                int target = (int)(FastRandom.RandomDouble() * (i + 1));
                int tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }

            return;
        }

        private Vec3[] PerlinGenerate()
        {
            Vec3[] p = new Vec3[256];

            for (int i = 0; i < 256; i++)
            {
                p[i] = Vec3.UnitVector(new Vec3(-1 + 2 * FastRandom.RandomDouble(),
                                                -1 + 2 * FastRandom.RandomDouble(),
                                                -1 + 2 * FastRandom.RandomDouble()));
            }

            return p;
        }

        private int[] PerlinGeneratePerm()
        {
            int[] p = new int[256];
            
            for (int i = 0; i < 256; i++)
            {
                p[i] = i;
            }

            Permute(p, 256);
            return p;
        }

        private double TrilinearInterpolation(Vec3[,,] c, double u, double v, double w)
        {
            double uu = u * u * (3 - 2 * u);
            double vv = v * v * (3 - 2 * v);
            double ww = w * w * (3 - 2 * w);
                
            double accum = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Vec3 weightV = new Vec3(u-i, v-j, w-k);
                        accum += (i * uu + (1 - i) * (1 - uu)) *
                                 (j * vv + (1 - j) * (1 - vv)) *
                                 (k * ww + (1 - k) * (1 - ww)) *
                                 Vec3.Dot(c[i, j, k], weightV);
                    }
                }
            }

            return accum;
        }
    }
}
