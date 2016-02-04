using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace connect4
{
	public class InputManager
	{
		private static MouseState lastState = Mouse.GetState ();

		public static bool IsMouseClicked ()
		{
			bool clicked = false;
			MouseState currentState = Mouse.GetState ();
			if (lastState.LeftButton == ButtonState.Pressed && currentState.LeftButton == ButtonState.Released) {
				clicked = true;
			}
			lastState = currentState;
			return clicked;
		}

		public static Point GetMousePotition ()
		{
			return Mouse.GetState ().Position;
		}
	}
}

