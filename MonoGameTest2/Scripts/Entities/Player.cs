using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Controllers;
using MonoGameTest2.Entities;

namespace MonoGameTest2.Entites
{
    public class Player : Character, IInputController
    {
        private int _standAnimID;
        private int _walkAnimID;
        private bool _isWalking;

        public Vector2 Velocity { get; private set; }

        public Player(Texture2D texture, Vector2 spawnPosition) : base(texture, spawnPosition, 4, 4)
        {
            Speed = 500;
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
            var direction = new Vector2(0, 0);

            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                direction.X += 1;
            }
            if (keyState.IsKeyDown(Keys.Left) | keyState.IsKeyDown(Keys.A))
            {
                direction.X -= 1;
            }
            if (keyState.IsKeyDown(Keys.Up) | keyState.IsKeyDown(Keys.W))
            {
                direction.Y -= 1;
            }
            if (keyState.IsKeyDown(Keys.Down) | keyState.IsKeyDown(Keys.S))
            {
                direction.Y += 1;
            }

            // TODO: Normalize direction
            Velocity = direction * Speed;
            Move(Velocity);
        }
    }
}
