﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TexturePackerLoader;

namespace monogame_test.Core.AnimationSystem
{
    public class Animation
    {
        private readonly AnimationFrame[] _frames;
        private TimeSpan _timeElapsed;
        public bool IsLooping { get; set; } = true;

        private TimeSpan Duration => TimeSpan.FromTicks(_frames.Sum(x => x.Duration.Ticks));        
            
        public Animation(AnimationFrame[] frames)
        {
            _frames = frames;
        }
        public Animation(AnimationFrame[] frames, bool isLooping)
        {
            _frames = frames;
            IsLooping = isLooping;
        }

        public Animation(TimeSpan timeBetweenFrames, SpriteSheet spriteSheet, SpriteEffects effect, 
            IEnumerable<string> spriteNames)
        {
            _frames = spriteNames.Select(x =>            
                new AnimationFrame
                {
                    Duration = timeBetweenFrames,
                    SpriteFrame = spriteSheet.Sprite(x),
                    SpriteEffect = effect
                })
                .ToArray();
        }

        public Animation(TimeSpan timeBetweenFrames, SpriteSheet spriteSheet, SpriteEffects effect,
            bool isLooping, IEnumerable<string> spriteNames)
        {
            _frames = spriteNames.Select(x =>
                new AnimationFrame
                {
                    Duration = timeBetweenFrames,
                    SpriteFrame = spriteSheet.Sprite(x),
                    SpriteEffect = effect
                })
                .ToArray();
            IsLooping = isLooping;
        }

        // TODO: Updating this mid-animation will usually skip us forward a few frames. Address this somehow, maybe by explicitly setting _timeElapsed.
        public void SetTimeBetweenFrames(TimeSpan newTime)
        {
            foreach(var frame in _frames)
            {
                frame.Duration = newTime;
            }
        }

        public void Reset()
        {
            _timeElapsed = TimeSpan.Zero;
        }

        public void Update(GameTime deltaTime)
        {
            double millisecondsElapsed = _timeElapsed.TotalMilliseconds + deltaTime.ElapsedGameTime.TotalMilliseconds;
            double remainder = millisecondsElapsed % Duration.TotalMilliseconds;
            _timeElapsed = TimeSpan.FromMilliseconds(remainder);
        }

        //TODO: Finish implementing IsLooping
        public AnimationFrame GetCurrentFrame()
        {
            AnimationFrame currentFrame = null;
            TimeSpan accumulatedTime = TimeSpan.Zero;
            foreach(var frame in _frames)
            {
                if (accumulatedTime + frame.Duration >= _timeElapsed)
                {
                    currentFrame = frame;
                    break;
                }
                else
                {
                    accumulatedTime += frame.Duration;
                }
            }

            // If we've somehow failed to find the frame
            if (currentFrame == null)
            {
                return _frames.LastOrDefault();
            }
            else
            {
                return currentFrame; 
            }
        }
    }
}
