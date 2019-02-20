using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Materials;
using Raytracer.Textures;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Raytracer
{
    class Program
    {
        private static Vec3 Color(Ray r, Hitable world, int depth)
        {
            HitRecord rec = new HitRecord();

            if (world.Hit(r, 0.001, double.MaxValue, out rec))
            {
                Ray scattered = new Ray();
                Vec3 attenuation = new Vec3();
                Vec3 emitted = rec.Material.emitted(rec.U, rec.V, rec.P);

                if (depth < 50 && rec.Material.Scatter(r, rec, out attenuation, out scattered))
                {
                    return emitted + attenuation * Color(scattered, world, depth + 1);
                }
                else
                {
                    return emitted;
                }
            }
            else
            {
                return new Vec3(0.0, 0.0, 0.0);
            }
        }


        public static HitableList CornellBox()
        {
            var list = new List<Hitable>();
            
            Material red = new Lambertian(new ConstantTexture(new Vec3(0.65, 0.05, 0.05)));
            Material white = new Lambertian(new ConstantTexture(new Vec3(0.73, 0.73, 0.73)));
            Material green = new Lambertian(new ConstantTexture(new Vec3(0.12, 0.45, 0.15)));
            Material light = new DiffuseLight(new ConstantTexture(new Vec3(15.0, 15.0, 15.0)));

            list.Add(new FlipNormals(new YZRect(0, 555, 0, 555, 555, green)));
            list.Add(new YZRect(0, 555, 0, 555, 0, red));
            list.Add(new XZRect(213, 343, 227, 332, 554, light));
            list.Add(new FlipNormals(new XZRect(0, 555, 0, 555, 555, white)));
            list.Add(new XZRect(0, 555, 0, 555, 0, white));
            list.Add(new FlipNormals(new XYRect(0, 555, 0, 555, 555, white)));

            return new HitableList(list);
        }

        public static HitableList SimpleLight()
        {
            Texture per = new NoiseTexture(4.0);
            var list = new List<Hitable>();

            list.Add(new Sphere(new Vec3(0.0, -1000.0, 0.0), 1000, new Lambertian(per)));
            list.Add(new Sphere(new Vec3(0.0, 2.0, 0.0), 2.0, new Lambertian(per)));
            list.Add(new Sphere(new Vec3(0.0, 7.0, 0.0), 2.0, new DiffuseLight(new ConstantTexture(new Vec3(4.0, 4.0, 4.0)))));
            list.Add(new XYRect(3.0, 5.0, 1.0, 3.0, -2.0, new DiffuseLight(new ConstantTexture(new Vec3(4.0, 4.0, 4.0)))));

            return new HitableList(list);
        }
        
        public static HitableList TwoPerlinSpheres()
        {
            Texture perlin = new NoiseTexture(5.0);
            List<Hitable> list = new List<Hitable>();

            list.Add(new Sphere(new Vec3(0.0, -1000.0, 0), 1000, new Lambertian(perlin)));
            list.Add(new Sphere(new Vec3(0.0, 2.0, 0.0), 2, new Lambertian(perlin)));

            return new HitableList(list);
        }

        public static HitableList EarthSphere()
        {
            Texture earth = new ImageTexture(Path.Combine("Images", "earth.jpeg"));
            Texture color = new ConstantTexture(new Vec3(0.75, 0.75, 0.75));
            List<Hitable> list = new List<Hitable>();

            list.Add(new Sphere(new Vec3(0.0, -1000.0, 0), 1000, new Lambertian(color)));
            list.Add(new Sphere(new Vec3(0.0, 2.0, 0.0), 2, new Lambertian(earth)));

            return new HitableList(list);
        }

        public static BvhNode CreateRandomScene()
        {
            List<Hitable> list = new List<Hitable>();

            var checker = new CheckerTexture(new ConstantTexture(new Vec3(0.2, 0.3, 0.1)), new ConstantTexture(new Vec3(0.9, 0.9, 0.9)));
            list.Add(new Sphere(new Vec3(0.0, -1000.0, 0.0), 1000, new Lambertian(checker)));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    double chooseMaterial = FastRandom.RandomDouble();
                    Vec3 center = new Vec3(a + 0.9 * FastRandom.RandomDouble(), 0.2, b + 0.9 * FastRandom.RandomDouble());

                    if ((center-new Vec3(4.0,0.2,0.0)).Length() > 0.9)
                    {
                        if (chooseMaterial < 0.8) // diffuse
                        {
                            list.Add(new MovingSphere(center, center + new Vec3(0.0, 0.5 * FastRandom.RandomDouble(), 0.0), 0.0, 1.0, 0.2, new Lambertian(new ConstantTexture(new Vec3(FastRandom.RandomDouble() * FastRandom.RandomDouble(), FastRandom.RandomDouble() * FastRandom.RandomDouble(), (FastRandom.RandomDouble() * FastRandom.RandomDouble()))))));
                        }
                        else if (chooseMaterial < 0.95) // metal
                        {
                            list.Add(new Sphere(center, 0.2, new Metal(new Vec3(0.5*(1.0+ FastRandom.RandomDouble()), 0.5 * (1.0 + FastRandom.RandomDouble()), 0.5 * (1.0 + FastRandom.RandomDouble())), 0.5* FastRandom.RandomDouble())));
                        }
                        else // dielectric
                        {
                            list.Add(new Sphere(center, 0.2, new Dielectric(1.5)));
                        }
                    }
                }
            }

            list.Add(new Sphere(new Vec3(0.0, 1.0, 0.0), 1.0, new Dielectric(1.5)));
            list.Add(new Sphere(new Vec3(-4.0, 1.0, 0.0), 1.0, new Lambertian(new ConstantTexture(new Vec3(0.4, 0.2, 0.1)))));
            list.Add(new Sphere(new Vec3(4.0, 1.0, 0.0), 1.0, new Metal(new Vec3(0.7, 0.6, 0.5), 0.0)));

            return new BvhNode(list, 0.0, 1.0);
        }

        static void Main(string[] args)
        {
            // Seed RNG
            FastRandom.Initialize((uint)new Random().Next());

            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            var outputFileName = Path.Combine(new string[] { homePath, "output.jpg" });

            var nx = 1200;  // Horizontal resolution
            var ny = 800;   // Vertical resolution
            var ns = 100;    // Antialising samples per pixel

            //var nx = 600;  // Horizontal resolution
            //var ny = 400;   // Vertical resolution
            //var ns = 10;    // Antialising samples per pixel

            Console.WriteLine($"Width:\t{nx}");
            Console.WriteLine($"Height:\t{ny}");
            Console.WriteLine($"Antialiasing:\t{ns}");
            Console.WriteLine();

            HitableList world = CornellBox();

            // Vec3 lookfrom = new Vec3(13.0, 2.0, 3.0);
            // Vec3 lookat = new Vec3(0.0, 0.0, 0.0);
            // double distToFocus = 10.0;
            // double aperture = 0.1;
            // Camera cam = new Camera(lookfrom, lookat, new Vec3(0.0, 1.0, 0.0), 35.0, (double)nx / (double)ny, aperture, distToFocus, 0.0, 1.0);

            Vec3 lookfrom = new Vec3(278, 278, -800);
            Vec3 lookat = new Vec3(278, 278, 0);
            double distToFocus = 10.0;
            double aperture = 0.0;
            double vFov = 40.0;

            Camera cam = new Camera(lookfrom, lookat, new Vec3(0.0, 1.0, 0.0), vFov, (double) nx / (double) ny, aperture, distToFocus, 0.0, 1.0);

            byte[] outputBytes = new byte[4 * nx * ny];

            Console.WriteLine("Rendering...");
            UpdateProgress(ny, 0);

            var linesRendered = 0;
            Parallel.For(0, ny, j =>
            {
                for (int i = 0; i < nx; i++)
                {
                    Vec3 col = new Vec3(0.0, 0.0, 0.0);

                    for (int s = 0; s < ns; s++)
                    {
                        double u = (i + FastRandom.RandomDouble()) / nx;
                        double v = (j + FastRandom.RandomDouble()) / ny;

                        Ray r = cam.GetRay(u, v);
                        col += Color(r, world, 0);
                    }

                    col /= ns;
                    col = new Vec3(Math.Sqrt(col[0]), Math.Sqrt(col[1]), Math.Sqrt(col[2]));
                    
                    outputBytes[4 * ((ny - 1 - j) * nx) + (4 * i)] = (byte)(255.9 * col[0]);
                    outputBytes[4 * ((ny - 1 - j) * nx) + (4 * i) + 1] = (byte)(255.9 * col[1]);
                    outputBytes[4 * ((ny - 1 - j) * nx) + (4 * i) + 2] = (byte)(255.9 * col[2]);
                    outputBytes[4 * ((ny - 1 - j) * nx) + (4 * i) + 3] = 255;
                }

                linesRendered++;
                UpdateProgress(ny, linesRendered);
            });

            Console.WriteLine("\nRendering completed, writing to file...");

            var outputImg = Image.LoadPixelData<Byte4>(outputBytes, nx, ny);
            outputImg.Save(outputFileName);

            Console.WriteLine("Saving completed, press any key to exit.");
            Console.ReadKey();
        }

        private static void UpdateProgress(int totalLines, int renderedLines)
        {
            double progress = (double)renderedLines / (double)totalLines * 100.0;
            int lineLen = totalLines.ToString().Length;
            Console.WriteLine($"Lines completed: {renderedLines.ToString().PadLeft(lineLen,'0')}/{totalLines} ({Math.Round(progress, 2).ToString("000.00")}%)");
        }
    }
}
