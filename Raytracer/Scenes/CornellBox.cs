using System;
using Raytracer.BaseClasses;
using Raytracer.Geometry;
using Raytracer.Materials;
using Raytracer.Textures;
using System.Collections.Generic;

namespace Raytracer.Scenes
{
    public class CornellBox : Scene
    {
        public CornellBox()
        {
        }

        public override Hitable GetObjects()
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


            var box1 = new Box(new Vec3(0, 0, 0), new Vec3(165, 165, 165), white);
            var box2 = new Box(new Vec3(0, 0, 0), new Vec3(165, 330, 165), white);
            list.Add(new Translate(new RotateY(box1, -18), new Vec3(130, 0, 65)));
            list.Add(new Translate(new RotateY(box2, 15), new Vec3(265, 0, 295)));

            return (Hitable) new HitableList(list);
        }

        public override Camera GetCamera(double aspectRatio)
        {
            Vec3 lookfrom = new Vec3(278, 278, -800);
            Vec3 lookat = new Vec3(278, 278, 0);
            double distToFocus = 10.0;
            double aperture = 0.0;
            double vFov = 40.0;

            return new Camera(lookfrom, lookat, new Vec3(0.0, 1.0, 0.0), vFov, aspectRatio, aperture, distToFocus, 0.0, 1.0);
        }
    }
}
