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

        static void Main(string[] args)
        {
            // Seed RNG
            FastRandom.Initialize((uint)new Random().Next());

            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            var outputFileName = Path.Combine(new string[] { homePath, "output.jpg" });

            var nx = 600;  // Horizontal resolution
            var ny = 400;   // Vertical resolution
            var ns = 150;    // Antialising samples per pixel

            Console.WriteLine($"Width:\t{nx}");
            Console.WriteLine($"Height:\t{ny}");
            Console.WriteLine($"Antialiasing:\t{ns}");
            Console.WriteLine();

            var scene = new Scenes.CornellBox();
            var world = scene.GetObjects();
            var cam = scene.GetCamera((double) nx / (double) ny);

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
