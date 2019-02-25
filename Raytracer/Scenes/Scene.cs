using System;
using Raytracer.BaseClasses;

namespace Raytracer.Scenes
{
    public abstract class Scene
    {
        public abstract Hitable GetObjects();

        public abstract Camera GetCamera(double aspectRatio);
    }
}
