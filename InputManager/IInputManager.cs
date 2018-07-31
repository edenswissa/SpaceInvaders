using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InputManager
{
    public interface IInputManager
    {
        KeyboardState KeyboardState { get; }

        MouseState MouseState { get; }

        bool MouseLeftBtnPressed();

        bool MouseLeftBtnReleased();

        bool MouseLeftBtnHeld();

        bool MouseRightBtnPressed();

        bool MouseRightBtnReleased();

        bool MouseRightBtnHeld();

        bool KeyPressed(Keys i_Key);

        bool KeyReleased(Keys i_Key);

        bool KeyHeld(Keys i_Key);

        Vector2 MousePositionDelta { get; }
    }
}