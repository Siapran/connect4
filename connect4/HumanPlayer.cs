using System;

namespace connect4
{
	public class HumanPlayer : Player
	{
		public HumanPlayer (GameGrid.Token color, GameGrid grid) : base (color, grid)
		{
		}

		public override int GetMove ()
		{
			if (InputManager.IsMouseClicked ()) {
				return InputManager.GetMousePotition ().X / 150;
			} else {
				return -1;
			}
		}
	}
}

