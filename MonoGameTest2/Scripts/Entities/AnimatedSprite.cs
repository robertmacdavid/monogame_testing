using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using MonoGameTest2.Managers;

namespace MonoGameTest2.Entities
{
    /// <summary>
    /// A sprite with a sprite sheet texture used for animation.
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Animation CurrentAnimation { get; private set; }

        private int _currentFrame;
        private readonly int _totalFrames;

        private List<Animation> _animations;
        public SortedSet<Animation> _activeAnimations;

        /// <summary>
        /// Creates a sprite with animations.
        /// </summary>
        /// <param name="texture">The texture that contains the sprite sheet.</param>
        /// <param name="initialPosition">The position the sprite will spawn at.</param>
        /// <param name="rows">The number of rows in the sprite sheet.</param>
        /// <param name="columns">The number of columns in the sprite sheet.</param>
        /// <param name="frameCount">The total number of sprites in the sprite sheet.</param>
        public AnimatedSprite(Texture2D texture, Vector2 initialPosition, int rows, int columns, int frameCount) : base(texture, initialPosition)
        {
            Rows = rows;
            Columns = columns;
            Width = texture.Width / columns;
            Height = texture.Height / rows;

            _currentFrame = 0;
            _totalFrames = frameCount;
            _animations = new List<Animation>();
            _activeAnimations = new SortedSet<Animation>();
        }

        public AnimatedSprite(Texture2D texture, Vector2 initialPosition, int rows, int columns) : this(texture, initialPosition, rows, columns, rows*columns) { }

        /// <summary>
        /// Adds an animation to this sprite.
        /// </summary>
        /// <param name="animation"></param>
        /// <returns>The ID for this animation.</returns>
        public int AddAnimation(Animation animation)
        {
            _animations.Add(animation);
            return animation.ID;
        }

        /// <summary>
        /// Plays an animation for the sprite.
        /// </summary>
        /// <param name="index">The index of the sequence.</param>
        public void SetAnimation(int index)
        {
            Animation animation = _animations[index];

            if (!_activeAnimations.Contains(animation))
            {
                _activeAnimations.Add(animation);
            }

            animation.Start();

            CurrentAnimation = _activeAnimations.Max();
        }

        /// <summary>
        /// Stops the animation for a sprite.
        /// </summary>
        /// <param name="index">The index of the sequence.</param>
        public void StopAnimation(int index)
        {
            Animation animation = _animations[index];

            if (_activeAnimations.Contains(animation))
            {
                _activeAnimations.Remove(animation);
            }

            animation.Stop();

            CurrentAnimation = _activeAnimations.Max();
        }

        /// <summary>
        /// Computes the frame index of the highest priority active sequence.
        /// </summary>
        public void Update()
        {
            _currentFrame = CurrentAnimation.GetFrame();
            if (_currentFrame == -1)
            {
                _activeAnimations.Remove(CurrentAnimation);
                CurrentAnimation = _activeAnimations.Max();
            }
        }

        /// <summary>
        /// Draws the animated sprite on it's current frame of animation.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render this sprite.</param>
        public new void Draw(SpriteBatch spriteBatch)
        {
            var row = _currentFrame / Columns;
            var column = _currentFrame % Columns;

            var sourceRectangle = new Rectangle(Width * column, Height * row, Width, Height);
            var destinationRectangle = new Rectangle((int)Position.X - Width/2, (int)Position.Y - Height/2, Width, Height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }

    /// <summary>
    /// Defines a sequence of frames for a sprite.
    /// </summary>
    public class Animation : IComparable<Animation>
    {
        private static int _animationIDs = 0;
        private static int GetAnimationID()
        {
            return _animationIDs++;
        }

        public string Name { get; set; }
        public bool Playing { get; private set; }
        public int ID { get; private set; }
        public int Priority { get; set; }
        public byte[] Sequence { get; private set; }

        private int _frameRate;
        public int FrameRate {
            get
            {
                return _frameRate;
            }
            set
            {
                _frameRate = value;
                _millisecondsPerFrame = 1000d / _frameRate;
            }
        }
        public bool Loop { get; set; }

        private bool _hasBeenUpdated;
        private double _animStartTime;
        private double _millisecondsPerFrame;

        public Animation(string name, byte[] sequence, int frameRate = 4, int priority = 0, bool loop = true)
        {
            ID = GetAnimationID();
            Name = name;
            FrameRate = frameRate;
            Priority = priority;
            Loop = loop;
            Sequence = sequence;
        }

        public override string ToString()
        {
            return  "Animation(" +
                $"Name: {Name}, " +
                $"ID: {ID}, " +
                $"Started at: {_animStartTime}, " +
                $"{Sequence.Length} Frames, " +
                $"{FrameRate} FPS, " +
                (Loop ? "Looped, " : "Unlooped, ") +
                (Playing ? "Active" : "Inactive") + 
                ")";
        }

        public int CompareTo(Animation that)
        {
            if (Priority == that.Priority)
            {
                return Sequence[0].CompareTo(that.Sequence[0]);
            }

            return Priority.CompareTo(that.Priority);
        }

        public void Start()
        {
            Playing = true;
            _hasBeenUpdated = false;
        }

        /// <summary>
        /// Updates the animation to go to the next frame.
        /// </summary>
        /// <returns>The current frame.</returns>
        public int GetFrame()
        {
            var callTime = GameManager.Instance.CurrentTimeMS;
            if (!_hasBeenUpdated)
            {
                _hasBeenUpdated = true;
                _animStartTime = callTime;
            }

            var currentFrame = (int)Math.Floor((callTime - _animStartTime) / _millisecondsPerFrame);
            if (!Loop && currentFrame > Sequence.Length)
            {
                return -1;
            }

            return Sequence[currentFrame % Sequence.Length];
        }

        public void Stop()
        {
            Playing = false;
        }
    }
}