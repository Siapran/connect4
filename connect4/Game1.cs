#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace connect4
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		GameGrid gamegrid;

		private Player redPlayer;
		private Player bluePlayer;
		private Player currentPlayer;
		private Player winner;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = false;
			IsMouseVisible = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			gamegrid = new GameGrid ();
			winner = null;

			redPlayer = new HumanPlayer (GameGrid.Token.RED, gamegrid);
			bluePlayer = new ComputerPlayer (GameGrid.Token.BLUE, gamegrid);

			SwitchPlayer (); // defaults to red

			base.Initialize ();
				
		}

		private void SwitchPlayer ()
		{
			if (currentPlayer == redPlayer) {
				currentPlayer = bluePlayer;
			} else {
				currentPlayer = redPlayer;
			}
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//  changing the back buffer size changes the window size (when in windowed mode)
			graphics.PreferredBackBufferWidth = 1050;
			graphics.PreferredBackBufferHeight = 900;
			graphics.ApplyChanges ();
		
			GameGrid.GRID = Content.Load<Texture2D> ("grid");
			GameGrid.TOKEN_RED = Content.Load<Texture2D> ("token_red");
			GameGrid.TOKEN_BLUE = Content.Load<Texture2D> ("token_blue");
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif

			if (winner == null && gamegrid.IsGameDone () != GameGrid.Token.UNDEFINED) {

				int move = currentPlayer.GetMove ();
				if (move != -1 && gamegrid.IsMoveValid (move)) {
					gamegrid.PutToken (move, currentPlayer.Color);

					GameGrid.Token result = gamegrid.IsGameDone ();
					if (GameGrid.IsTokenPlayer (result) && currentPlayer.Color == result) {
						winner = currentPlayer;
					} else {
						SwitchPlayer ();
					}
				}

			} else {
				if (InputManager.IsMouseClicked ()) {
					winner = null;
					currentPlayer = null;
					gamegrid.Reset ();
					SwitchPlayer ();
				}
			}

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.White);
		
			//TODO: Add your drawing code here
			gamegrid.Draw (spriteBatch);

			if (winner == null) {
				gamegrid.DrawCursor (currentPlayer, spriteBatch);
			}

			base.Draw (gameTime);
		}
	}
}

