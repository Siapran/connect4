using System;

namespace connect4
{
	public abstract class Player
	{
		protected GameGrid.Token color;
		protected GameGrid grid;

		public GameGrid.Token Color {
			get {
				return this.color;
			}
		}

		public Player (GameGrid.Token color, GameGrid grid)
		{
			this.color = color;
			this.grid = grid;
		}

		public abstract int GetMove ();
	}
}

