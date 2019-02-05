using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Materials;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Raytracer
{
    class Program
    {
        private static Vec3 Color(Ray r, HitableList world, int depth)
        {
            HitRecord rec = new HitRecord();

            if (world.Hit(r, 0.001F, float.MaxValue, out rec))
            {
                Ray scattered = new Ray();
                Vec3 attenuation = new Vec3();

                if (depth < 50 && rec.Material.Scatter(r, rec, out attenuation, out scattered))
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

        public static HitableList CreateRandomScene()
        {
            List<Hitable> list = new List<Hitable>();

            list.Add(new Sphere(new Vec3(0.0F, -1000.0F, 0.0F), 1000F, new Lambertian(new Vec3(0.5F, 0.5F, 0.5F))));

            var rnd = new Random();
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    float chooseMaterial = (float)rnd.NextDouble();
                    Vec3 center = new Vec3(a + 0.9F * (float)rnd.NextDouble(), 0.2F, b + 0.9F * (float)rnd.NextDouble());

                    if ((center-new Vec3(4.0F,0.2F,0.0F)).Length() > 0.9)
                    {
                        if (chooseMaterial < 0.8) // diffuse
                        {
                            list.Add(new Sphere(center, 0.2F, new Lambertian(new Vec3((float)(rnd.NextDouble() * rnd.NextDouble()), (float)(rnd.NextDouble() * rnd.NextDouble()), (float)(rnd.NextDouble() * rnd.NextDouble())))));
                        }
                        else if (chooseMaterial < 0.95) // metal
                        {
                            list.Add(new Sphere(center, 0.2F, new Metal(new Vec3(0.5F*(1.0F+(float)rnd.NextDouble()), 0.5F * (1.0F + (float)rnd.NextDouble()), 0.5F * (1.0F + (float)rnd.NextDouble())), 0.5F*(float)rnd.NextDouble())));
                        }
                        else // dielectric
                        {
                            list.Add(new Sphere(center, 0.2F, new Dielectric(1.5F)));
                        }
                    }
                }
            }

            list.Add(new Sphere(new Vec3(0.0F, 1.0F, 0.0F), 1.0F, new Dielectric(1.5F)));
            list.Add(new Sphere(new Vec3(-4.0F, 1.0F, 0.0F), 1.0F, new Lambertian(new Vec3(0.4F, 0.2F, 0.1F))));
            list.Add(new Sphere(new Vec3(4.0F, 1.0F, 0.0F), 1.0F, new Metal(new Vec3(0.7F, 0.6F, 0.5F), 0.0F)));

            return new HitableList(list);
        }

        static void Main(string[] args)
        {
            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            var outputFileName = Path.Combine(homePath, "output.ppm");

            var nx = 1200;
            var ny = 800;
            var ns = 10; // Antialising samples per pixel
            
            HitableList world = CreateRandomScene();

            Vec3 lookfrom = new Vec3(13.0F, 2.0F, 3.0F);
            Vec3 lookat = new Vec3(0.0F, 0.0F, 0.0F);
            float distToFocus = 10.0F;
            float aperture = 0.1F;
            Camera cam = new Camera(lookfrom, lookat, new Vec3(0.0F, 1.0F, 0.0F), 20.0F, (float)nx / (float)ny, aperture, distToFocus);

            var rnd = new Random();

            var lineDict = new Dictionary<int, List<string>>();
            Parallel.For(0, ny, j =>
            {
                var lineList = new List<string>();

                for (int i = 0; i < nx; i++)
                {
                    Vec3 col = new Vec3(0.0F, 0.0F, 0.0F);

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

                    lineList.Add($"{ir} {ig} {ib}");
                }

                lineDict.Add(j, lineList);
            });

            using (var writer = File.CreateText(outputFileName))
            {
                writer.WriteLine("P3");          // Filetype identifier
                writer.WriteLine($"{nx} {ny}");  // Image dimensions
                writer.WriteLine("255");         // Max value for colors

                for (int j = ny - 1; j >= 0; j--)
                {
                    foreach (var line in lineDict[j])
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}
