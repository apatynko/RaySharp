using System;
using System.Collections.Generic;
using System.IO;

namespace Raytracer
{
    class Program
    {
        public static float HitSphere(Vec3 center, float radius, Ray r)
        {
            Vec3 oc = r.Origin() - center;
            float a = Vec3.Dot(r.Direction(), r.Direction());
            float b = 2.0F * Vec3.Dot(oc, r.Direction());
            float c = Vec3.Dot(oc, oc) - radius * radius;
            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                return -1.0F;
            }
            else
            {
                return (-b - (float)Math.Sqrt(discriminant)) / 2.0F * a;
            }
        }

        private static Vec3 Color(Ray r, HitableList world)
        {
            HitRecord rec = new HitRecord();

            if (world.Hit(r, 0.0F, float.MaxValue, out rec))
            {
                return 0.5F * new Vec3(rec.Normal.X() + 1, rec.Normal.Y() + 1, rec.Normal.Z() + 1);
            }
            else
            {
                Vec3 unitDirection = Vec3.UnitVector(r.Direction());
                float t = 0.5F * (unitDirection.Y() + 1.0F);
                return (1.0F - t) * new Vec3(1.0F, 1.0F, 1.0F) + t * new Vec3(0.5F, 0.7F, 1.0F);
            }
        }

        static void Main(string[] args)
        {
            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            var outputFileName = Path.Combine(homePath, "output.ppm");

            var outputLines = new List<string>();

            var nx = 200;
            var ny = 100;
            var ns = 100;

            outputLines.Add("P3");          // Filetype identifier
            outputLines.Add($"{nx} {ny}");  // Image dimensions
            outputLines.Add("255");         // Max value for colors

            List<Hitable> list = new List<Hitable>();
            list.Add(new Sphere(new Vec3(0.0F, 0.0F, -1.0F), 0.5F));
            list.Add(new Sphere(new Vec3(0.0F, -100.5F, -1.0F), 100.0F));
            HitableList world = new HitableList(list);

            Camera cam = new Camera();

            for (int j = ny - 1; j >= 0; j--)
            {
                for (int i = 0; i < nx; i++)
                {
                    Vec3 col = new Vec3(0.0F, 0.0F, 0.0F);
                    var rnd = new Random();
                    for (int s = 0; s < ns; s++)
                    {
                        float u = (float)(i + rnd.NextDouble()) / (float)nx;
                        float v = (float)(j + rnd.NextDouble()) / (float)ny;

                        Ray r = cam.GetRay(u, v);
                        col += Color(r, world);
                    }
                    col /= (float)ns;

                    int ir = (int)(255.9 * col[0]);
                    int ig = (int)(255.9 * col[1]);
                    int ib = (int)(255.9 * col[2]);

                    outputLines.Add($"{ir} {ig} {ib}");
                }
            }

            using(var writer = File.CreateText(outputFileName))
            {
                foreach (var line in outputLines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}
