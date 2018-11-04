using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Controllers;
using MonoGameTest2.Helpers;
using MonoGameTest2.Managers;
using MonoGameTest2.Physics;

namespace MonoGameTest2.Entities
{
    public class Player : Character, IInputController, ICollidable
    {
        private bool _isWalking;
        private int _stand;
        private int _walkRight;
        private int _walkLeft;
        private InputEventHandler PlayerInputEvents;

        private List<Rectangle> _testCollisions;

        public Vector2 Velocity { get; private set; }
        public Rectangle Hitbox { get; set; }

        public Player(Vector2 spawnPosition) : base(GameManager.Instance.ContentManager.Load<Texture2D>("characters/new_man"), spawnPosition, 1, 5)
        {
            Speed = 4;
            TravelDirection = new Vector2(0, 0);
            Hitbox = new Rectangle((int)spawnPosition.X - 8, (int)spawnPosition.Y - 8, 16, 16);
            PlayerInputEvents = new InputEventHandler();

            PlayerInputEvents.AddKeyHoldHandlers(new Keys[4] { Keys.W, Keys.A, Keys.S, Keys.D },
                                                new Action[4] { GoUp, GoLeft, GoDown, GoRight });

            PlayerInputEvents.AddKeyHoldHandlers(new Keys[4] { Keys.Up, Keys.Left, Keys.Down, Keys.Right },
                                                new Action[4] { GoUp, GoLeft, GoDown, GoRight });

            _testCollisions = new List<Rectangle>();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _stand = AddAnimation(new Animation("stand", new byte[] { 0 }));
            _walkRight = AddAnimation(new Animation("walk-left", new byte[] { 1, 2, 3, 4 }, 2));
            _walkLeft = AddAnimation(new Animation("walk-right", new byte[] { 4, 3, 2, 1 }, 2));

            SetAnimation(_stand);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            if (Velocity == Vector2.Zero)
            {
                if (_isWalking)
                {
                    _isWalking = false;
                    StopAnimation(_walkLeft);
                    StopAnimation(_walkRight);
                }
            }
            else
            {
                if (!_isWalking)
                {
                    _isWalking = true;

                    if (Velocity.X > 0)
                    {
                        SetAnimation(_walkRight);
                    }
                    else
                    {
                        SetAnimation(_walkLeft);
                    }
                }
            }

            spriteBatch.DrawRectangle(Hitbox, Color.Blue);

            foreach (var collision in _testCollisions)
            {
                spriteBatch.DrawRectangle(collision, Color.Red);
            }

            base.Draw(spriteBatch);
        }

        public void HandleInput()
        {
            ClearTravelDir();
            PlayerInputEvents.HandleInput();
            NormalizeTravelDir();

            Velocity = TravelDirection * Speed;
            Hitbox = new Rectangle(Position.ToPoint() - new Point(Hitbox.Size.X/2, Hitbox.Size.Y/2), Hitbox.Size);

            if (Velocity.LengthSquared() > float.Epsilon)
            {
                CheckCollision();
            }
            
            Move(Velocity);
        }

        private void CheckCollision()
        {
            var tilePos = LevelManager.WorldPositionToTile(Hitbox.Center).ToPoint();

            _testCollisions.Clear();
            foreach (var collision in GameManager.Instance.LevelManager.Level.GetPossibleCollisions(tilePos.X, tilePos.Y))
            {
                if (!Hitbox.Intersects(collision))
                {
                    continue;
                }

                var offset = new Vector2();

                if (Velocity.X < 0) 
                {
                    offset.X += collision.Right - Hitbox.Left;
                }
                else if (Velocity.X > 0)
                {
                    offset.X -= collision.Left - Hitbox.Right;
                }

                if (Velocity.Y < 0)
                {
                    offset.Y += collision.Bottom - Hitbox.Top;
                }
                else if (Velocity.Y > 0)
                {
                    offset.Y -= collision.Top - Hitbox.Bottom;
                }

                _testCollisions.Add(collision);

                Velocity += offset;
                Hitbox = new Rectangle(Hitbox.Location + offset.ToPoint(), new Point(16, 16));
            }
        }
    }
}
