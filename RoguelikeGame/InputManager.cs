using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace RoguelikeGame
{
    public enum InputAction
    {
        NONE,
        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_DOWN,
        MOVE_NW,
        MOVE_NE,
        MOVE_SW,
        MOVE_SE,
        REST,
        ESCAPE
    }
    public class InputManager
    {
        private KeyboardState _lastKeyboardState;
        private KeyboardState _currentKeyboardState;
        private Dictionary<Keys, InputAction> _actionsMap;
        private int _currentActions = 0;
        public KeyboardState LastKeyboardState => _lastKeyboardState;
        public KeyboardState CurrentKeyboardState => _currentKeyboardState;

        public Action<InputAction> KeyPressed;
        public Action<InputAction> KeyDown;
        public Action<InputAction> KeyReleased;

        private const string kInputFile = "inputMap.txt";

        public Vector2 MousePosition => Mouse.GetState().Position.ToVector2();

        public InputManager()
        {
            _actionsMap = new Dictionary<Keys, InputAction>
            {
                 { Keys.Left, InputAction.MOVE_LEFT},
                 { Keys.A, InputAction.MOVE_LEFT},
                 { Keys.K, InputAction.MOVE_LEFT},
                 { Keys.NumPad4, InputAction.MOVE_LEFT},
                 { Keys.Right, InputAction.MOVE_RIGHT},
                 { Keys.D, InputAction.MOVE_RIGHT},
                 { Keys.OemSemicolon, InputAction.MOVE_RIGHT},
                 { Keys.NumPad6, InputAction.MOVE_RIGHT},
                 { Keys.Up, InputAction.MOVE_UP},
                 { Keys.W, InputAction.MOVE_UP},
                 { Keys.O, InputAction.MOVE_UP},
                 { Keys.NumPad8, InputAction.MOVE_UP},
                 { Keys.Down, InputAction.MOVE_DOWN},
                 { Keys.S, InputAction.MOVE_DOWN},
                 { Keys.OemPeriod, InputAction.MOVE_DOWN},
                 { Keys.NumPad2, InputAction.MOVE_DOWN},
                 { Keys.I, InputAction.MOVE_NW},
                 { Keys.NumPad7, InputAction.MOVE_NW},
                 { Keys.P, InputAction.MOVE_NE},
                 { Keys.NumPad9, InputAction.MOVE_NE},
                 { Keys.OemComma, InputAction.MOVE_SW},
                 { Keys.NumPad1, InputAction.MOVE_SW},
                 { Keys.OemQuestion, InputAction.MOVE_SE},
                 { Keys.NumPad3, InputAction.MOVE_SE},
                 { Keys.R, InputAction.REST},
                 { Keys.Escape, InputAction.ESCAPE},
            };
            
            _lastKeyboardState = Keyboard.GetState();
            _currentKeyboardState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            _lastKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            foreach (var kvp in _actionsMap)
            {
                if (IsKeyPressed(kvp.Key))
                {
                    KeyPressed?.Invoke(kvp.Value);
                }
                else if (IsKeyDown(kvp.Key))
                {
                    KeyDown?.Invoke(kvp.Value);
                    SetAction(kvp.Value);
                }
                else if (IsKeyReleased(kvp.Key))
                {
                    KeyReleased?.Invoke(kvp.Value);
                    UnsetAction(kvp.Value);
                }
            }
        }

        public bool IsKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public bool CheckAction(InputAction act)
        {
            return 1 == (1 & (_currentActions >> (int)act));
        }

        private void SetAction(InputAction inputAction)
        {
            _currentActions = _currentActions | (1 << (int)inputAction);
            //System.Console.WriteLine(_currentActions);
        }

        private void UnsetAction(InputAction inputAction)
        {
            _currentActions = _currentActions & ~(1 << (int)inputAction);
            //System.Console.WriteLine(_currentActions);
        }
    }
}
