using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace connect4
{
	public class GameGrid
	{
		public static Texture2D GRID;
		public static Texture2D TOKEN_RED;
		public static Texture2D TOKEN_BLUE;

		public enum Token : int
		{
			NONE = 0,
			RED,
			BLUE,
			UNDEFINED = -1
		}

		private Token[,] _grid = new Token[6, 7];
		private int[] tops = new int[7];
		private Stack<int> moves;

		public Token[,] Grid {
			get {
				return this._grid;
			}
			set {
				_grid = value;
			}
		}

		public GameGrid ()
		{
			Reset ();
		}

		// copy constructor
		public GameGrid (GameGrid original)
		{
			_grid = (Token[,])original._grid.Clone ();
			tops = (int[])original.tops.Clone ();
			moves = new Stack<int> (original.moves);
		}


		public void Reset ()
		{
			for (int i = 0; i < _grid.GetLength (0); i++) {
				for (int j = 0; j < _grid.GetLength (1); j++) {
					_grid [i, j] = 0;
				}
			}
			for (int i = 0; i < tops.Length; i++) {
				tops [i] = _grid.GetLength (0);
			}
			moves = new Stack<int> ();
		}

		public bool PutToken (int column, Token token)
		{
			if (tops [column] > 0) {
				_grid [--tops [column], column] = token;
				moves.Push (column);
				return true;
			} else {
				return false;
			}
		}

		public bool Undo ()
		{
			if (moves.Count > 0) {
				int column = moves.Pop ();
				_grid [tops [column]++, column] = Token.NONE;
				return true;
			} else {
				return false;
			}
		}

		public bool IsColumnFull (int column)
		{
			
			return tops [column] <= 0;
			
		}

		public bool IsMoveValid (int col)
		{
			if (col >= 0 && col < tops.Length) {
				return !IsColumnFull (col);
			} else {
				return false;
			}
		}

		public Token IsGameDone ()
		{
			for (int i = 0; i < _grid.GetLength (0); i++) {
				for (int j = 0; j < _grid.GetLength (1); j++) {
					if (IsTokenPlayer (GetToken (i, j))) {
						if (CheckWin (i, j)) {
							return GetToken (i, j);
						}
					}
				}
			}
			bool cantFinish = true;
			for (int i = 0; i < tops.Length; i++) {
				cantFinish &= IsColumnFull (i);
			}
			if (cantFinish) {
				return Token.UNDEFINED;
			}
			return Token.NONE;
		}

		public static bool IsTokenPlayer (Token token)
		{
			return token == Token.BLUE || token == Token.RED;
		}

		public static Token GetOppositeToken (Token token)
		{
			switch (token) {
			case Token.BLUE:
				return Token.RED;
			case Token.RED:
				return Token.BLUE;
			default:
				return Token.NONE;
			}
		}

		private bool CheckWin (int i, int j)
		{
			Token token = GetToken (i, j);
			bool win = true;
			for (int k = 0; k < 4; ++k) {
				win &= GetToken (i + k, j) == token; // |
			}
			if (win) {
				return win;
			}
			win = true;
			for (int k = 0; k < 4; ++k) {
				win &= GetToken (i, j + k) == token; // -
			}
			if (win) {
				return win;
			}
			win = true;
			for (int k = 0; k < 4; ++k) {
				win &= GetToken (i + k, j + k) == token; // \
			}
			if (win) {
				return win;
			}
			win = true;
			for (int k = 0; k < 4; ++k) {
				win &= GetToken (i + k, j - k) == token; // /
			}
			if (win) {
				return win;
			}
			return false;
		}

		public Token GetToken (int i, int j)
		{
			return (
			    i >= 0 && i < _grid.GetLength (0) &&
			    j >= 0 && j < _grid.GetLength (1)
			) ? _grid [i, j] : Token.UNDEFINED;
		}

		public void ConsolePrint ()
		{
			for (int i = 0; i < _grid.GetLength (0); i++) {
				for (int j = 0; j < _grid.GetLength (1); j++) {
					Token token = _grid [i, j];
					switch (token) {
					case Token.BLUE:
						Console.Out.Write ('B');
						break;
					case Token.RED:
						Console.Out.Write ('R');
						break;
					default:
						break;
					}
				}
				Console.Out.WriteLine ();
			}
		}

		public void DrawCursor (Player currentPlayer, SpriteBatch spriteBatch)
		{
			int col = InputManager.GetMousePotition ().X / 150;
			if (currentPlayer is HumanPlayer && IsMoveValid (col)) {
				spriteBatch.Begin ();
				Vector2 pos = new Vector2 (col * 150, (tops [col] - 1) * 150);
				switch (currentPlayer.Color) {
				case Token.RED:
					spriteBatch.Draw (TOKEN_RED, pos, Color.White);
					break;
				case Token.BLUE:
					spriteBatch.Draw (TOKEN_BLUE, pos, Color.White);
					break;
				default:
					break;
				}
				spriteBatch.End ();
			}
		}

		public void Draw (SpriteBatch spriteBatch)
		{
			spriteBatch.Begin ();
			for (int i = 0; i < _grid.GetLength (0); i++) {
				for (int j = 0; j < _grid.GetLength (1); j++) {
					int xpos, ypos;
					xpos = j * 150;
					ypos = i * 150;
					Vector2 pos = new Vector2 (xpos, ypos);
					switch (GetToken (i, j)) {
					case Token.RED:
						spriteBatch.Draw (TOKEN_RED, pos, Color.White);
						break;
					case Token.BLUE:
						spriteBatch.Draw (TOKEN_BLUE, pos, Color.White);
						break;
					default:
						break;
					}
					spriteBatch.Draw (GRID, pos, Color.White);
				}
			}
			spriteBatch.End ();
		}
	}

}