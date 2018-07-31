using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameServices;

namespace InputManager
{
    public class InputManager : GameService, IInputManager
    {
        private KeyboardState m_PrevKeyboardState;
        private KeyboardState m_KeyboardState;
        private MouseState m_PrevMouseState;
        private MouseState m_MouseState;

        public InputManager(Game i_Game)
            : base(i_Game, int.MinValue)
        {
        }

        public override void Initialize()
        {
            m_PrevKeyboardState = Keyboard.GetState();
            m_KeyboardState = m_PrevKeyboardState;
            m_PrevMouseState = Mouse.GetState();
            m_MouseState = m_PrevMouseState;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_PrevKeyboardState = m_KeyboardState;
            m_KeyboardState = Keyboard.GetState();
            m_PrevMouseState = m_MouseState;
            m_MouseState = Mouse.GetState();
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(IInputManager), this);
        }

        public KeyboardState PrevKeyboardState
        {
            get { return m_PrevKeyboardState; }
        }

        public KeyboardState KeyboardState
        {
            get { return m_KeyboardState; }
        }

        public MouseState PrevMouseState
        {
            get { return m_PrevMouseState; }
        }

        public MouseState MouseState
        {
            get { return m_MouseState; }
        }

        public bool MouseLeftBtnPressed()
        {
            return m_PrevMouseState.LeftButton == ButtonState.Released && m_MouseState.LeftButton == ButtonState.Pressed;
        }

        public bool MouseLeftBtnReleased()
        {
            return m_PrevMouseState.LeftButton == ButtonState.Pressed && m_MouseState.LeftButton == ButtonState.Released;
        }

        public bool MouseLeftBtnHeld()
        {
            return m_PrevMouseState.LeftButton == ButtonState.Pressed && m_MouseState.LeftButton == ButtonState.Pressed;
        }

        public bool MouseRightBtnPressed()
        {
            return m_PrevMouseState.RightButton == ButtonState.Released && m_MouseState.RightButton == ButtonState.Pressed;
        }

        public bool MouseRightBtnReleased()
        {
            return m_PrevMouseState.RightButton == ButtonState.Pressed && m_MouseState.RightButton == ButtonState.Released;
        }

        public bool MouseRightBtnHeld()
        {
            return m_PrevMouseState.RightButton == ButtonState.Pressed && m_MouseState.RightButton == ButtonState.Pressed;
        }

        public bool KeyPressed(Keys i_Key)
        {
            return m_PrevKeyboardState.IsKeyUp(i_Key) && m_KeyboardState.IsKeyDown(i_Key);
        }

        public bool KeyReleased(Keys i_Key)
        {
            return m_PrevKeyboardState.IsKeyDown(i_Key) && m_KeyboardState.IsKeyUp(i_Key);
        }

        public bool KeyHeld(Keys i_Key)
        {
            return m_PrevKeyboardState.IsKeyDown(i_Key) && m_KeyboardState.IsKeyDown(i_Key);
        }

        public Vector2 MousePositionDelta
        {
            get { return new Vector2(m_MouseState.X - m_PrevMouseState.X, m_MouseState.Y - m_PrevMouseState.Y); }
        }
    }
}
