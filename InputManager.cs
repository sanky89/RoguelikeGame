using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace RogueTest
{
    public enum InputAction
    {
        NONE,
        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_DOWN,
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
            if (!ReadInputFile())
            {
                _actionsMap = new Dictionary<Keys, InputAction>
                {
                    { Keys.Left, InputAction.MOVE_LEFT},
                    { Keys.A, InputAction.MOVE_LEFT},
                    { Keys.Right, InputAction.MOVE_RIGHT},
                    { Keys.D, InputAction.MOVE_RIGHT},
                    { Keys.Up, InputAction.MOVE_UP},
                    { Keys.W, InputAction.MOVE_UP},
                    { Keys.Down, InputAction.MOVE_DOWN},
                    { Keys.S, InputAction.MOVE_DOWN},
                    { Keys.Escape, InputAction.ESCAPE},
                };
                WriteInputFile();
            }
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

        private bool IsKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key);
        }

        private bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
        }

        private bool IsKeyDown(Keys key)
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

        private bool ReadInputFile()
        {
            if(!File.Exists(kInputFile))
            {
                return false;
            }
            string mapString = File.ReadAllText(kInputFile);
            if(string.IsNullOrEmpty(mapString))
            {
                return false;
            }
            _actionsMap = new Dictionary<Keys, InputAction>();
            string[] lines = mapString.Split("\n");
            for (int i = 0; i < lines.Length-1; i++) //Avoid the final empty line
            {
                string[] action = lines[i].Split('|');
                Enum.TryParse<Keys>(action[0], false, out var key);
                Enum.TryParse<InputAction>(action[1], false, out var inputAction);
                _actionsMap[key] = inputAction;
            }
            return true;
        }

        private void WriteInputFile()
        {
            
            string content = "";
            //using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Append)))
            //{
                foreach (var kvp in _actionsMap)
                {
                   content += kvp.Key + "|" + kvp.Value + "\n";
                }
            //}
            if(!File.Exists(kInputFile))
                File.WriteAllText(kInputFile, content);
        }
    }
}
