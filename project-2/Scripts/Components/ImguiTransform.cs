﻿using System;
using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;

namespace World_3D
{
    public class ImguiTransform: Component
    {
        public override void Update(double deltaTime)
        {
            ImGui.Begin("Transform of " + parent.Name );

            Vector3 position = parent.Transform.Position;
            Vector3 rotation = parent.Transform.Rotation;
            
            ImGui.DragFloat3("Position", ref position);
            ImGui.DragFloat3("Rotation", ref rotation);

            parent.Transform.Position = position;
            parent.Transform.Rotation = rotation;
            
            ImGui.End();
        }
    }
}