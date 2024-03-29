﻿using Silk.NET.Input;
using System;
using System.Numerics;

namespace AllianceEngine
{
    class CameraControls: Component
    {
        public float CameraYaw = -90f;
        public float CameraPitch = 0f;
        private const float _maxZoom = 60f;
        private const float _minZoom = 10.0f;
        
        private Vector2 _lastMousePosition;
        private Camera _camera;

        public CameraControls(Camera camera)
        {
            _camera = camera;
        }

        public override void Update(double deltaTime)
        {
            var moveSpeed = 5f * (float)deltaTime;
            
            if (!UI.IsUIOpen)
            {
                ZoomCamera(Input.Mouse.ScrollWheels[0]);
                RotateCamera();
                MoveCamera(moveSpeed);
            }
            else
            {
                _lastMousePosition = Input.Mouse.Position;
            }

        }

        private unsafe void RotateCamera()
        {
            var lookSensitivity = 0.1f;

            Vector2 mousePos = Input.Mouse.Position;

            if (_lastMousePosition == default)
            {
                _lastMousePosition = mousePos;
            }
            else
            {
                Vector2 offset = (mousePos - _lastMousePosition) * lookSensitivity;
                _lastMousePosition = mousePos;

                parent.Transform.Rotate(MathHelper.DegreesToRadians * -offset.X, Vector3.UnitX);
                parent.Transform.Rotate(MathHelper.DegreesToRadians * (offset.Y), Vector3.UnitY);
                
                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                const float YAngleCap = (MathF.PI / 2.1f);
                
                Vector3 v = parent.Transform.Rotation;
                v.Y = Math.Clamp(parent.Transform.Rotation.Y, -YAngleCap, YAngleCap);
                parent.Transform.Rotation = v;
            }
        }

        private void MoveCamera(float moveSpeed)
        {
            if (Input.Keyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                parent.Transform.Position += moveSpeed * parent.Transform.Forward;
            }
            if (Input.Keyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                parent.Transform.Position -= moveSpeed * parent.Transform.Forward;
            }
            if (Input.Keyboard.IsKeyPressed(Key.A))
            {
                //Move left
                parent.Transform.Position -= Vector3.Normalize(Vector3.Cross(parent.Transform.Forward, parent.Transform.Up)) * moveSpeed;
            }
            if (Input.Keyboard.IsKeyPressed(Key.D))
            {
                //Move right
                parent.Transform.Position += Vector3.Normalize(Vector3.Cross(parent.Transform.Forward, parent.Transform.Up)) * moveSpeed;
            }
            if (Input.Keyboard.IsKeyPressed(Key.ShiftLeft))
            {
                //Move up
                parent.Transform.Position += parent.Transform.Up * moveSpeed;
            }
            if (Input.Keyboard.IsKeyPressed(Key.ControlLeft))
            {
                //Move down
                parent.Transform.Position -= parent.Transform.Up * moveSpeed;
            }    
        }

        public void ZoomCamera(ScrollWheel scrollWheel)
        {
            //Clamp zoom
            _camera.CameraZoom = Math.Clamp(_camera.CameraZoom - scrollWheel.Y, _minZoom, _maxZoom);
        }
    }
}
