using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Materials;
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

        private static Vec3 Color(Ray r, HitableList world, int depth)
        {
            HitRecord rec = new HitRecord();

            if (world.Hit(r, 0.001F, float.MaxValue, out rec))
            {
                Ray scattered = new Ray();
                Vec3 attenuation = new Vec3();

                if (depth < 50 && rec.Material.scatter(r, rec, out attenuation, out scattered))
                {
                    return attenuation * Color(scattered, world, depth + 1);
                }
                else
                {
                    return new Vec3(0.0F, 0.0F, 0.0F);
                }
                
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
            float radius = (float)Math.Cos(Math.PI / 4);

            list.Add(new Sphere(new Vec3(0.0F, 0.0F, -1.0F), 0.5F, new Lambertian(new Vec3(0.1F, 0.2F, 0.5F))));
            list.Add(new Sphere(new Vec3(0.0F, -100.5F, -1.0F), 100.0F, new Lambertian(new Vec3(0.8F, 0.8F, 0.0F))));
            list.Add(new Sphere(new Vec3(1.0F, 0.0F, -1.0F), 0.5F, new Metal(new Vec3(0.8F, 0.6F, 0.2F), 0.3F)));
            list.Add(new Sphere(new Vec3(-1.0F, 0.0F, -1.0F), 0.5F, new Dielectric(1.5F)));
            list.Add(new Sphere(new Vec3(-1.0F, 0.0F, -1.0F), -0.45F, new Dielectric(1.5F)));

            HitableList world = new HitableList(list);

            Camera cam = new Camera(new Vec3(-2.0F, 2.0F, 1.0F), new Vec3(0.0F, 0.0F, -1.0F), new Vec3(0.0F, 1.0F, 0.0F), 90.0F, (float)nx / (float)ny);

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
                        col += Color(r, world, 0);
                    }
                    col /= (float)ns;
                    col = new Vec3((float)Math.Sqrt(col[0]), (float)Math.Sqrt(col[1]), (float)Math.Sqrt(col[2]));
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
