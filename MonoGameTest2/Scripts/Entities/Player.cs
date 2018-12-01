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
        private Rectangle _realHitbox;

        public Vector2 Velocity { get; private set; }
        public Rectangle Hitbox { get; set; }

        public Player(Vector2 spawnPosition) : base(GameManager.Instance.ContentManager.Load<Texture2D>("characters/new_man"), spawnPosition, 1, 5)
        {
            Speed = 4;
            TravelDirection = new Vector2(0, 0);
            Hitbox = new Rectangle(-8, -4, 8, 8);
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

            spriteBatch.DrawRectangle(_realHitbox, Color.Blue);

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
            _realHitbox = new Rectangle(Position.ToPoint() + Hitbox.Center, Hitbox.Size);
            if (Velocity.LengthSquared() > float.Epsilon)
            {
                CheckCollision();
            }
            
            Move(Velocity);
        }

        private void CheckCollision()
        {
            var tilePos = LevelManager.WorldPositionToTile(_realHitbox.Center).ToPoint();
            var level = GameManager.Instance.LevelManager.Level;
            var newVelocity = Velocity;

            _testCollisions.Clear();
            foreach (var wall in level.GetPossibleCollisions(tilePos.X, tilePos.Y))
            {
                // Add X position first
                var nextStep = _realHitbox;
                nextStep.X += (int)Velocity.X;
                if (nextStep.Intersects(wall))
                {
                    // Move this back.
                    if (_realHitbox.X < wall.X)
                    {
                        // Move this left.
                        newVelocity.X = wall.Left - _realHitbox.Right;
                    }
                    else
                    {
                        // Move this right.
                        newVelocity.X = wall.Right - _realHitbox.Left;
                    }
                }

                // Then Y position.
                nextStep.X -= (int)Velocity.X;
                nextStep.Y += (int)Velocity.Y;
                if (nextStep.Intersects(wall))
                {
                    // Move this back.
                    if (_realHitbox.Y < wall.Y)
                    {
                        // Move this up.
                        newVelocity.Y = wall.Top - _realHitbox.Bottom;
                    }
                    else
                    {
                        // Move this down.
                        newVelocity.Y = wall.Bottom - _realHitbox.Top;
                    }
                }

                _testCollisions.Add(wall);
                Velocity = newVelocity;
            }
        }
    }
}
