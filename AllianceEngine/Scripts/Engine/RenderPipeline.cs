﻿using System.Collections.Generic;

namespace AllianceEngine
{
    public class RenderPipeline
    {
        List<Renderer> renderers = new();

        public RenderPipeline()
        {
            Renderer.OnCreation += AddRenderer;
        }

        public void AddRenderer(Renderer renderer)
        {
            renderers.Add(renderer);
        }

        public void DrawRenderers()
        {
            foreach(Renderer r in renderers)
            {
                r.Draw();
            }
        }
    }
}