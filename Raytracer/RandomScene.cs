using System;
using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Materials;
using Raytracer.Textures;
using System.Collections.Generic;

namespace Raytracer.Scenes
{
    public class RandomScene : Scene
    {
        public RandomScene()
        {
        }

        public override Hitable GetObjects()
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

            return (Hitable) new BvhNode(list, 0.0, 1.0);
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
