using System;
using System.Collections.Generic;
using System.IO;

namespace Raytracer
{
    class Program
    {
        private static Vec3 Color(Ray r)
        {
            Vec3 unitDirection = Vec3.UnitVector(r.Direction());
            float t = 0.5F * (unitDirection.Y() + 1.0F);

            return (1.0F - t) * new Vec3(1.0F, 1.0F, 1.0F) + t * new Vec3(0.5F, 0.7F, 1.0F);
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

            outputLines.Add("P3");          // Filetype identifier
            outputLines.Add($"{nx} {ny}");  // Image dimensions
            outputLines.Add("255");         // Max value for colors

            Vec3 lowerLeftCorner = new Vec3(-2.0F, -1.0F, -1.0F);   // Starting point for "scanning"
            Vec3 horizontal = new Vec3(4.0F, 0.0F, 0.0F);           // Horizontal field of view
            Vec3 vertical = new Vec3(0.0F, 2.0F, 0.0F);             // Vertical field of view
            Vec3 origin = new Vec3(0.0F, 0.0F, 0.0F);               // Camera center / "eye position"

            for (int j = ny - 1; j >= 0; j--)
            {
                for (int i = 0; i < nx; i++)
                {
                    float u = (float)i / (float)nx;
                    float v = (float)j / (float)ny;

                    Ray r = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical);
                    Vec3 col = Color(r);

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
