using System;

namespace connect4
{
	public class ComputerPlayer : Player
	{

		const int MAX_DEPTH = 5;
		const float WIN_VALUE = 1f;
		const float LOSE_VALUE = -1f;
		const float UNCERTAIN_VALUE = 0f;

		public ComputerPlayer (GameGrid.Token color, GameGrid grid) : base (color, grid)
		{
		}

		public override int GetMove ()
		{
			double maxValue = double.NegativeInfinity;
			int move = 0;
			for (int col = 0; col < grid.Grid.GetLength (1); ++col) {
				if (grid.IsMoveValid (col)) {
					double value = moveValue (col);
					if (move == 0 || value > maxValue) {
						maxValue = value;
						move = col;
						if (value == WIN_VALUE) {
							break;
						}
					}
				}
			}
			return move;
		}

		double moveValue (int col)
		{
			grid.PutToken (col, color);
			double value = alphaBetaPruning (MAX_DEPTH,
				               double.NegativeInfinity,
				               double.PositiveInfinity,
				               false);
			grid.Undo ();
			return value;
		}

		double alphaBetaPruning (int depth, double alpha, double beta, bool maxPlayer)
		{
			bool hasWinner = GameGrid.IsTokenPlayer (grid.IsGameDone ());

			// contrôle de la récursion
			if (depth == 0 || hasWinner) {
				double score = 0;
				if (hasWinner) {
					score = grid.IsGameDone () == color ? WIN_VALUE : LOSE_VALUE;
				} else {
					score = UNCERTAIN_VALUE;
				}
				// on préferrera une victoire imminente à une victoire tardive
				return score / (MAX_DEPTH - depth + 1);
			}

			if (maxPlayer) {
				for (int col = 0; col < grid.Grid.GetLength (1); ++col) {
					if (grid.IsMoveValid (col)) {
						grid.PutToken (col, color);
						alpha = Math.Max (alpha,
							alphaBetaPruning (depth - 1, alpha, beta, false));
						grid.Undo ();
						if (beta <= alpha) {
							break;
						}
					}	
				}
				return alpha;
			} else {
				for (int col = 0; col < grid.Grid.GetLength (1); ++col) {
					if (grid.IsMoveValid (col)) {
						grid.PutToken (col, GameGrid.GetOppositeToken (color));
						beta = Math.Min (beta,
							alphaBetaPruning (depth - 1, alpha, beta, true));
						grid.Undo ();
						if (beta <= alpha) {
							break;
						}
					}	
				}
				return beta;
			}

		}
	}
}

