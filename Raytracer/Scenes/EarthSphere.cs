using System;
using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Materials;
using Raytracer.Textures;
using System.Collections.Generic;

namespace Raytracer.Scenes
{
    public class EarthSphere : Scene
    {
        private string _earthTexturePath;

        public EarthSphere(string earthTexturePath)
        {
            _earthTexturePath = earthTexturePath;
        }

        public override Hitable GetObjects()
        {
            Texture earth = new ImageTexture(_earthTexturePath);
            Texture color = new ConstantTexture(new Vec3(0.75, 0.75, 0.75));
            List<Hitable> list = new List<Hitable>();

            list.Add(new Sphere(new Vec3(0.0, -1000.0, 0), 1000, new Lambertian(color)));
            list.Add(new Sphere(new Vec3(0.0, 2.0, 0.0), 2, new Lambertian(earth)));

            return (Hitable) new HitableList(list);
        }

        public override Camera GetCamera(double aspectRatio)
        {
            Vec3 lookfrom = new Vec3(13.0, 2.0, 3.0);
            Vec3 lookat = new Vec3(0.0, 0.0, 0.0);
            double distToFocus = 10.0;
            double aperture = 0.1;
            Camera cam = new Camera(lookfrom, lookat, new Vec3(0.0, 1.0, 0.0), 35.0, aspectRatio, aperture, distToFocus, 0.0, 1.0);

            return cam;
        }

    }
}
