﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Raytracer
{
    class Program
    {
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

            for (int j = ny - 1; j >= 0; j--)
            {
                for (int i = 0; i < nx; i++)
                {
                    Vec3 col = new Vec3((float)i / (float)nx, (float)j / (float)ny, 0.2F);
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
