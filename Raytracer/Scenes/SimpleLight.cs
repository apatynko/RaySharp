using System;
using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Materials;
using Raytracer.Textures;
using System.Collections.Generic;

namespace Raytracer.Scenes
{
    public class SimpleLight : Scene
    {
        public SimpleLight()
        {
        }

        public override Hitable GetObjects()
        {
            Texture per = new NoiseTexture(4.0);
            var list = new List<Hitable>();

            list.Add(new Sphere(new Vec3(0.0, -1000.0, 0.0), 1000, new Lambertian(per)));
            list.Add(new Sphere(new Vec3(0.0, 2.0, 0.0), 2.0, new Lambertian(per)));
            list.Add(new Sphere(new Vec3(0.0, 7.0, 0.0), 2.0, new DiffuseLight(new ConstantTexture(new Vec3(4.0, 4.0, 4.0)))));
            list.Add(new XYRect(3.0, 5.0, 1.0, 3.0, -2.0, new DiffuseLight(new ConstantTexture(new Vec3(4.0, 4.0, 4.0)))));

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
