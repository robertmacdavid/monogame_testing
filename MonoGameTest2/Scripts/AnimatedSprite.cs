﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using MonoGameTest2.Managers;

namespace MonoGameTest2
{
    /// <summary>
    /// A sprite with a sprite sheet texture used for animation.
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        private int _currentFrame;
        private readonly int _totalFrames;

        private List<Animation> _animations;
        private SortedSet<Animation> _activeAnimations;

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
        /// Adds a sequence description to this sprite.
        /// </summary>
        /// <param name="startFrame"></param>
        /// <param name="numFrames"></param>
        /// <param name="framerate"></param>
        /// <param name="priority"></param>
        /// <returns>An ID for this animation.</returns>
        public int AddAnimation(int startFrame, int numFrames, int framerate, int priority)
        {
            var animation = new Animation(startFrame, numFrames, framerate, priority);
            _animations.Add(animation);
            return _animations.Count - 1;
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
        }

        // Computes the frame index of the highest priority active sequence
        public void Update()
        {
            Animation topSequence = _activeAnimations.Max();

            _currentFrame = topSequence.Update();
            if (_currentFrame == -1)
            {
                _activeAnimations.Remove(topSequence);
            }
        }


        public new void Draw(SpriteBatch spriteBatch)
        {
            int row = (int)((float)_currentFrame / (float)Columns);
            int column = _currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(Width * column, Height * row, Width, Height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }

    /// <summary>
    /// Defines a sequence of frames for a sprite.
    /// </summary>
    public class Animation : IComparable<Animation>
    {
        public bool IsActive;

        private bool _hasBeenUpdated;
        private double _animStartTime;
        private int _startFrame;
        private int _priority;

        private readonly int _numFrames;
        private readonly bool _loop;
        private readonly double _millisecondsPerFrame;

        public Animation(int startFrame, int numFrames, int framerate, int priority, bool loop = true)
        {
            _startFrame = startFrame;
            _numFrames = numFrames;
            _millisecondsPerFrame = 1000.0d / framerate;
            _priority = priority;
            _loop = loop;
        }

        public int CompareTo(Animation that)
        {
            if (_priority == that._priority)
            {
                return _startFrame.CompareTo(that._startFrame);
            }

            return _priority.CompareTo(that._priority);
        }

        public void Start()
        {
            IsActive = true;
            _hasBeenUpdated = false;
        }

        // Returns the index of the frame to be drawn
        public int Update()
        {
            double callTime = GameManager.Instance.DeltaTime * 1000;
            if (!_hasBeenUpdated)
            {
                _hasBeenUpdated = true;
                _animStartTime = callTime;
            }

            int currentFrame = (int)Math.Floor((callTime - _animStartTime) / _millisecondsPerFrame);
            if (!_loop && currentFrame >= _numFrames)
            {
                return -1;
            }

            return _startFrame + (currentFrame % _numFrames);
        }

        public void Stop()
        {
            IsActive = false;
        }
    }
}