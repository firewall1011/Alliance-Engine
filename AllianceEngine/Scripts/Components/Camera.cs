﻿using System.Numerics;

namespace AllianceEngine
{
    public class Camera : Component
    {
        public static Camera MainCamera { get; private set; }
        
        public float FarPlaneDistance { get; set; } = 100.0f;
        public float NearPlaneDistance { get; set; } = 0.1f;
        public float CameraZoom { get; set; } = 45f;

        public Matrix4x4 View => Matrix4x4.CreateLookAt(parent.Transform.Position, parent.Transform.Position
                                                                                   + parent.Transform.Forward, parent.Transform.Up);
        public Matrix4x4 Projection => Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians * CameraZoom, Program.Width / (float) Program.Height, NearPlaneDistance, FarPlaneDistance);


        public static void SwitchMainCamera(Camera camera)
        {
            MainCamera = camera;
        }

    }
}
