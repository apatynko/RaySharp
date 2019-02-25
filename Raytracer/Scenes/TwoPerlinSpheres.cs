using System;
using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Materials;
using Raytracer.Textures;
using System.Collections.Generic;

namespace Raytracer.Scenes
{
    public class TwoPerlinSpheres : Scene
    {
        public TwoPerlinSpheres()
        {
        }

        public override Hitable GetObjects()
        {
            Texture perlin = new NoiseTexture(5.0);
            List<Hitable> list = new List<Hitable>();

            list.Add(new Sphere(new Vec3(0.0, -1000.0, 0), 1000, new Lambertian(perlin)));
            list.Add(new Sphere(new Vec3(0.0, 2.0, 0.0), 2, new Lambertian(perlin)));

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
