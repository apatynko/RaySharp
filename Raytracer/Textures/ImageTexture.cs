using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Raytracer.Geometry;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace Raytracer.Textures
{
    public class ImageTexture : Texture
    {
        private Rgba32[] _pixels;
        private int _nx;
        private int _ny;

        public ImageTexture()
        {
        }

        public ImageTexture(string filePath)
        {
            var image = Image.Load<Rgba32>(filePath);
            _nx = image.Width;
            _ny = image.Height;
          
            _pixels = new Rgba32[_nx * _ny];
            image.GetPixelSpan().CopyTo(_pixels);
        }

        public override Vec3 Value(double u, double v, Vec3 p)
        {
            int i = (int)(u * _nx);
            int j = (int)((1 - v) * _ny - 0.001);

            i = (i < 0) ? 0 : i;
            j = (j < 0) ? 0 : j;

            i = (i > (_nx - 1)) ? _nx - 1 : i;
            j = (j > (_ny - 1)) ? _ny - 1 : j;

            var pixel = _pixels[i + _nx * j];
            double r = (double) pixel.R / 255;
            double g = (double) pixel.G / 255;
            double b = (double) pixel.B / 255;
            
            return new Vec3(r, g, b);
        }
    }
}
