﻿using Silk.NET.Input;
using System;
using System.Numerics;
using Tutorial;

namespace World_3D
{
    class CameraMovement: Component
    {
        private Vector2 LastMousePosition;
        public float CameraYaw = -90f;
        public float CameraPitch = 0f;

        public override void Update(double deltaTime)
        {
            var moveSpeed = 10f * (float)deltaTime;

            MoveCamera(moveSpeed);
            OnMouseMove();
        }
        private unsafe void OnMouseMove()
        {
            var lookSensitivity = 0.1f;

            Vector2 mousePos = Input.Mouse.Position;

            if (LastMousePosition == default)
            {
                LastMousePosition = mousePos;
            }
            else
            {
                var xOffset = (mousePos.X - LastMousePosition.X) * lookSensitivity;
                var yOffset = (mousePos.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = mousePos;

                CameraYaw = xOffset;
                CameraPitch = yOffset;

                Console.WriteLine();

                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                CameraPitch = Math.Clamp(CameraPitch, -89.0f, 89.0f);

                parent.transform.Rotate(MathHelper.DegreesToRadians(-CameraYaw), MathHelper.DegreesToRadians(-CameraPitch));
            }
        }

        private void MoveCamera(float moveSpeed)
        {
            if (Input.Keyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                parent.transform.Position += moveSpeed * parent.transform.Forward;
            }
            if (Input.Keyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                parent.transform.Position -= moveSpeed * parent.transform.Forward;
            }
            if (Input.Keyboard.IsKeyPressed(Key.A))
            {
                //Move left
                parent.transform.Position -= Vector3.Normalize(Vector3.Cross(parent.transform.Forward, parent.transform.Up)) * moveSpeed;
            }
            if (Input.Keyboard.IsKeyPressed(Key.D))
            {
                //Move right
                parent.transform.Position += Vector3.Normalize(Vector3.Cross(parent.transform.Forward, parent.transform.Up)) * moveSpeed;
            }
        }

    }
}
