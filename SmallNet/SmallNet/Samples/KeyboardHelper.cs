using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SmallNet.Samples
{
    class KeyboardHelper
    {
        KeyboardState _current;
        KeyboardState _previous;

        /// <summary>
        /// Updates the keyboard states
        /// </summary>
        public void Update()
        {
            _previous = _current;
            _current = Keyboard.GetState();
        }

        /// <summary>
        /// List of all pressed keys
        /// </summary>
        public Keys[] PressedKeys()
        {
            return _current.GetPressedKeys();
        }

        /// <summary>
        /// List of keys that have been just been pressed
        /// </summary>
        public Keys[] NewPressedKeys()
        {
            List<Keys> newPressed = _current.GetPressedKeys().ToList();
            List<Keys> old = _previous.GetPressedKeys().ToList();
            foreach (Keys i in old)
            {
                newPressed.Remove(i);
            }

            return newPressed.ToArray();
        }

        /// <summary>
        /// Check to see if a given key is pressed
        /// </summary>
        /// <param name="key">Key to check</param>
        public bool KeyDown(Keys key)
        {
            return _current.IsKeyDown(key);
        }

        /// <summary>
        /// Check to see if a given key is released
        /// </summary>
        /// <param name="key">Key to check</param>
        public bool KeyUp(Keys key)
        {
            return _current.IsKeyUp(key);
        }

        /// <summary>
        /// Check to see if a given key has just been pressed
        /// </summary>
        /// <param name="key">Key to check</param>
        public bool NewKeyDown(Keys key)
        {
            return _current.IsKeyDown(key) && _previous.IsKeyUp(key);
        }

        /// <summary>
        /// Check to see if a given key has just been released
        /// </summary>
        /// <param name="key">Key to check</param>
        public bool NewKeyUp(Keys key)
        {
            return _current.IsKeyUp(key) && _previous.IsKeyDown(key);
        }


    }
}
