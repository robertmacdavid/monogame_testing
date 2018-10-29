using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Controllers;
using MonoGameTest2.Entities;
using MonoGameTest2.Managers;

namespace MonoGameTest2.Entities
{
    public class Player : Character, IInputController
    {
        private bool _isWalking;
        private int _stand;
        private int _walkRight;
        private int _walkLeft;
        private InputEventHandler PlayerInputEvents;

        public Vector2 Velocity { get; private set; }

        public Player(Vector2 spawnPosition) : base(GameManager.Instance.ContentManager.Load<Texture2D>("characters/new_man"), spawnPosition, 1, 5)
        {
            Speed = 50;

            TravelDirection = new Vector2(0, 0);

            PlayerInputEvents = new InputEventHandler();

            PlayerInputEvents.AddKeyHoldHandlers(new Keys[4] { Keys.W, Keys.A, Keys.S, Keys.D },
                                                new Action[4] { GoUp, GoLeft, GoDown, GoRight });

            PlayerInputEvents.AddKeyHoldHandlers(new Keys[4] { Keys.Up, Keys.Left, Keys.Down, Keys.Right },
                                                new Action[4] { GoUp, GoLeft, GoDown, GoRight });

        }

        public void LoadContent(ContentManager contentManager)
        {
            _stand = AddAnimation(new Animation("stand", new byte[] { 0 }));
            _walkRight = AddAnimation(new Animation("stand", new byte[] { 1, 2, 3, 4 }, 2));
            _walkLeft = AddAnimation(new Animation("stand", new byte[] { 4, 3, 2, 1 }, 2));

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

            base.Draw(spriteBatch);
        }

        public void HandleInput()
        {
            ClearTravelDir();
            PlayerInputEvents.HandleInput();
            NormalizeTravelDir();

            Velocity = TravelDirection * Speed;
            Move(Velocity);
        }

    }
}
