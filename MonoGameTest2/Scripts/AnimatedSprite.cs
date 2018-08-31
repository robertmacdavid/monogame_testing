using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGameTest2
{
    public static class AnimationPriority
    {
        public static int STANDING = 0;
        public static int IDLE = 1;
        public static int WALKING = 2;
        public static int ACTION = 3;
    }


    public class AnimatedSprite : Sprite
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        private int _width;
        private int _height;
        private int _currentFrame;
        private readonly int _totalFrames;

        private List<SpriteSequence> Sequences;
        private SortedSet<SpriteSequence> activeSequences;

        public AnimatedSprite(Texture2D texture, Vector2 initialPosition, int rows, int columns, int frameCount) : base(texture, initialPosition)
        {
            Rows = rows;
            Columns = columns;

            _width = texture.Width / columns;
            _height = texture.Height / rows;
            _currentFrame = 0;
            _totalFrames = frameCount;


            Sequences = new List<SpriteSequence>();
            activeSequences = new SortedSet<SpriteSequence>();
        }

        public AnimatedSprite(Texture2D texture, Vector2 initialPosition, int rows, int columns) : this(texture, initialPosition, rows, columns, rows*columns) { }

        // Adds a sequence description to this sprite. 
        // Returns an index for that sequence
        public int AddSequence(int startFrame, int numFrames, int framerate, int priority)
        {
            var sequence = new SpriteSequence(startFrame, numFrames, framerate, priority);
            Sequences.Add(sequence);
            return Sequences.Count - 1;
        }

        // Activates the sequence corresponding to the given index
        public void StartSequence(int sequenceIndex)
        {
            SpriteSequence sequence = Sequences[sequenceIndex];
            if (!activeSequences.Contains(sequence))
                activeSequences.Add(sequence);
            sequence.Begin();
        }

        // Deactivates the sequence corresponding to the given index
        public void EndSequence(int sequenceIndex)
        {
            SpriteSequence sequence = Sequences[sequenceIndex];
            if (activeSequences.Contains(sequence))
                activeSequences.Remove(sequence);
            sequence.End();
        }

        // Computes the frame index of the highest priority active sequence
        public void Update(GameTime gameTime)
        {
            SpriteSequence topSequence = activeSequences.Max();

            _currentFrame = topSequence.Update(gameTime);
        }


        public new void Draw(SpriteBatch spriteBatch)
        {
            int row = (int)((float)_currentFrame / (float)Columns);
            int column = _currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(_width * column, _height * row, _width, _height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, _width, _height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }


    public class SpriteSequence : IComparable<SpriteSequence>
    {
        public bool isActive;
        private bool hasBeenUpdated;
        private double animStartTime;
        private int startFrame;
        private int numFrames;
        private int priority;

        private double millisecondsPerFrame;

        public SpriteSequence(int startFrame, int numFrames, int framerate, int priority)
        {
            this.startFrame = startFrame;
            this.numFrames = numFrames;
            this.millisecondsPerFrame = 1000.0d / (double)framerate;
            this.priority = priority;
        }

        public int CompareTo(SpriteSequence that)
        {
            if (this.priority == that.priority)
                return this.startFrame.CompareTo(that.startFrame);
            return this.priority.CompareTo(that.priority);
        }

        public void Begin()
        {
            isActive = true;
            hasBeenUpdated = false;
        }

        // Returns the index of the frame to be drawn
        public int Update(GameTime gameTime)
        {
            double callTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (!hasBeenUpdated)
            {
                hasBeenUpdated = true;
                animStartTime = callTime;
            }

            return startFrame +
                ((int)Math.Floor((callTime - animStartTime) / millisecondsPerFrame) % numFrames);
        }

        public void End()
        {
            isActive = false;
        }


    }
}