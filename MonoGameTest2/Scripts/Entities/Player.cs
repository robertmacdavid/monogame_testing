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
        private int _standAnimID;
        private int _walkAnimID;
        private bool _isWalking;
        private InputEventHandler PlayerInputEvents;

        private Vector2 InputDirection;



        public Vector2 Velocity { get; private set; }

        public Player(Texture2D texture, Vector2 spawnPosition) : base(texture, spawnPosition, 4, 4)
        {
            Speed = 500;

            InputDirection = new Vector2(0, 0);

            PlayerInputEvents = new InputEventHandler();

            PlayerInputEvents.AddKeyHoldHandlers(new Keys[4] { Keys.W, Keys.A, Keys.S, Keys.D },
                                                new Action[4] { GoUp, GoLeft, GoDown, GoRight });

            PlayerInputEvents.AddKeyHoldHandlers(new Keys[4] { Keys.Up, Keys.Left, Keys.Down, Keys.Right },
                                                new Action[4] { GoUp, GoLeft, GoDown, GoRight });

        }

        public void LoadContent(ContentManager contentManager)
        {
            _standAnimID = AddAnimation("stand", 0, 4, 4, 0);
            _walkAnimID = AddAnimation("walk", 0, 16, 32, 1);

            SetAnimation(_standAnimID);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            if (Velocity == Vector2.Zero)
            {
                if (_isWalking)
                {
                    _isWalking = false;
                    StopAnimation(_walkAnimID);
                }
            }
            else
            {
                if (!_isWalking)
                {
                    _isWalking = true;
                    SetAnimation(_walkAnimID);
                }
            }

            base.Draw(spriteBatch);
        }

        public void HandleInput()
        {
            var keyState = Keyboard.GetState();
            HashSet<Action> ActionsToCall = new HashSet<Action>();


            InputDirection.X = 0;
            InputDirection.Y = 0;


            PlayerInputEvents.HandleInput();

            if (InputDirection.LengthSquared() - 1.0 > float.Epsilon)
                InputDirection.Normalize();
            Velocity = InputDirection * Speed;
            Move(Velocity);
        }

        public void GoLeft()
        {
            InputDirection.X -= 1;
        }
        public void GoRight()
        {
            InputDirection.X += 1;
        }
        public void GoDown()
        {
            InputDirection.Y += 1;
        }
        public void GoUp()
        {
            InputDirection.Y -= 1;
        }
    }
}
