﻿using Microsoft.Xna.Framework;

using MonoGameTest2.Entities;
using MonoGameTest2.Managers;

namespace MonoGameTest2.Controllers
{
    public class CameraController
    {
        public Sprite Target { get; set; }
        public bool FollowExactly { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Rectangle? _deadzone;

        public void SetDeadzoneDimensions(int width, int height)
        {
            _deadzone = new Rectangle(GameManager.NATIVE_SCREEN_WIDTH/2 - width/2, GameManager.NATIVE_SCREEN_HEIGHT/2 - height/2, width, height);
        }

        public void Update()
        {
            var camera = GameManager.Instance.MainCamera;

            if (Target != null)
            {
                if (!FollowExactly && _deadzone.HasValue)
                {
                    var deadzone = _deadzone.Value;
                    var targetLocalPos = camera.WorldToScreen(Target.Position);

                    if (!deadzone.Contains(targetLocalPos))
                    {
                        if (targetLocalPos.X < deadzone.Left)
                        {
                            camera.Position += new Vector2(targetLocalPos.X - deadzone.Left, 0);
                        }
                        else if (targetLocalPos.X > deadzone.Right)
                        {
                            camera.Position += new Vector2(targetLocalPos.X - deadzone.Right, 0);
                        }

                        if (targetLocalPos.Y < deadzone.Top)
                        {
                            camera.Position += new Vector2(0, targetLocalPos.Y - deadzone.Top);
                        }
                        else if (targetLocalPos.Y > deadzone.Bottom)
                        {
                            camera.Position += new Vector2(0, targetLocalPos.Y - deadzone.Bottom);
                        }
                    }
                }
                else
                {
                    camera.Position = Target.Position;
                }
            }
        }
    }
}
