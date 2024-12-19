using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using pong.Input;
using pong.Levels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using static System.Formats.Asn1.AsnWriter;

namespace pong
{
    public enum LevelSelect
    {
        Level1,
        Level2
    }
    public enum GameState
    {
        Start,
        Playing,
        GameOver,
        Won
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        Hero hero;
        //blokje
        Texture2D blokTexture;
        Rectangle blokje;
        Rectangle blokje2;
        Vector2 positie = new Vector2(50, 50);
        Color backgroundColor = Color.Black;
        private GameState currentGameState = GameState.Start;
        //hitboxtexture
        Texture2D HitboxTexture;
        //level
        Level level; // Level-klasse als variabele
        Texture2D tilesetTexture; // Texture voor de tileset
        List<Enemy> enemies;
        private Texture2D enemyTexture;
        bool firsttime = true;
        bool firsttime2 = true;
        private EnemyManager enemyManager;
        Level1 level1 = new Level1();
        //coins
        private List<Coin> coins;
        private Texture2D coinTexture;
        private int coinsCollected = 0;
        private int coinsNeeded = 5;
        private LevelSelect currentLevel = LevelSelect.Level1;
        private Song backgroundMusic;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            //muziek
            MediaPlayer.IsRepeating = true; // Laat de muziek herhalen
            MediaPlayer.Volume = 0.5f;      // Stel het volume in (0.0 tot 1.0)
            MediaPlayer.Play(backgroundMusic);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _texture = Content.Load<Texture2D>("Ratfolk Mage Sprite Sheet");
            InitializeGameObject();
            backgroundMusic = Content.Load<Song>("DarkFantasy");
        }

        private void InitializeGameObject()
        {
            hero = new Hero(_texture, new KeyBoardReader());
            //blokje
            blokje = new Rectangle((int)positie.X + 200, (int)positie.Y+150, 50, 100);
            blokje2 = new Rectangle((int)positie.X + 400, (int)positie.Y + 150, 50, 100);
            //
            //if (currentLevel == LevelSelect.Level1)
            //{
            //    LoadLevel1();
            //}
            //else if(currentLevel == LevelSelect.Level2)
            //{
            //    LoadLevel2();
            //}

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (currentGameState == GameState.Start)
            {
                KeyboardState state = Keyboard.GetState();

                // Navigeren tussen levels
                if (state.IsKeyDown(Keys.Left))
                    currentLevel = LevelSelect.Level1;
                else if (state.IsKeyDown(Keys.Right))
                    currentLevel = LevelSelect.Level2;

                // Bevestig de keuze met Enter
                if (state.IsKeyDown(Keys.Enter))
                {
                    currentGameState = GameState.Playing;

                    if (currentLevel == LevelSelect.Level1)
                        LoadLevel1();
                    else if (currentLevel == LevelSelect.Level2)
                        LoadLevel2();
                }
            }
            // TODO: Add your update logic here
            if (currentGameState == GameState.Playing)
            {
                hero.Update(gameTime, level, enemyManager.enemies);
                enemyManager.UpdateEnemies(gameTime, level, hero);
                
                if (hero.CheckCollision(blokje) && firsttime)
                {
                    firsttime = false;
                    enemyManager.AddEnemy(new Vector2(400, 300), Color.Green, 1f, 1f,100,2);
                    enemyManager.AddEnemy(new Vector2(400, 100), Color.Green, 1f, 1f,100,2);
                }
                if (hero.CheckCollision(blokje2) && firsttime2)
                {
                    firsttime2 = false;
                    enemyManager.AddEnemy(new Vector2(600, 200), Color.Red, 4f, 0.4f, 250, 1);
                }
                if (hero.Health <= 0)
                {
                    currentGameState = GameState.GameOver;
                }
                if (enemyManager.enemies.Count == 0 && coinsCollected == coinsNeeded)
                {
                    currentGameState = GameState.Won;
                }
                for (int i = coins.Count - 1; i >= 0; i--)
                {
                    if (hero.CheckCollision(coins[i].Hitbox))
                    {
                        coins.RemoveAt(i); // Verwijder de munt uit de lijst
                        coinsCollected++;  // Verhoog de teller
                    }
                }

            }
            else if (currentGameState == GameState.GameOver)
            {
                // Herstart het spel als op R wordt gedrukt
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    RestartGame();
                }
            }

            base.Update(gameTime);
        }

        private void RestartGame()
        {
            if (currentLevel == LevelSelect.Level1)
                RestartLevel1();
            else if (currentLevel == LevelSelect.Level2)
                RestartLevel2();
        }
        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            if (currentGameState == GameState.Playing)
            {
                level.Draw(_spriteBatch);
                hero.Draw(_spriteBatch);
                // Teken vijanden
                enemyManager.DrawEnemies(_spriteBatch);
                foreach (var coin in coins)
                {
                    coin.Draw(_spriteBatch);
                }
                DrawCoinStatus();
                DrawHealthStatus();

            }
            else if(currentGameState == GameState.GameOver)
            {
                DrawGameOverScreen();
            }
            else if (currentGameState == GameState.Won)
            {
                DrawWonScreen();
            }
            else if (currentGameState == GameState.Start)
            {
                DrawStart();
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawGameOverScreen()
        {
            string gameOverText = "Game Over";
            string restartText = "Press R to Restart";

            var font = Content.Load<SpriteFont>("Gamefont");

            Vector2 gameOverPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2 - font.MeasureString(gameOverText).X / 2,
                _graphics.PreferredBackBufferHeight / 2 - font.MeasureString(gameOverText).Y
            );

            Vector2 restartPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2 - font.MeasureString(restartText).X / 2,
                _graphics.PreferredBackBufferHeight / 2 + font.MeasureString(restartText).Y
            );

            _spriteBatch.DrawString(font, gameOverText, gameOverPosition, Color.Red);
            _spriteBatch.DrawString(font, restartText, restartPosition, Color.White);
        }
        private void DrawWonScreen()
        {
            string WonText = "You WIN";
            string restartText = "Congratulations";

            var font = Content.Load<SpriteFont>("Gamefont");

            Vector2 gameOverPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2 - font.MeasureString(WonText).X / 2,
                _graphics.PreferredBackBufferHeight / 2 - font.MeasureString(WonText).Y
            );

            Vector2 restartPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2 - font.MeasureString(restartText).X / 2,
                _graphics.PreferredBackBufferHeight / 2 + font.MeasureString(restartText).Y
            );
            
            _spriteBatch.DrawString(font, WonText, gameOverPosition, Color.Blue);
            _spriteBatch.DrawString(font, restartText, restartPosition, Color.White);
        }
        private void DrawCoinStatus()
        {
            var font = Content.Load<SpriteFont>("Gamefont"); // Laad een font voor de tekst
            string coinText = $"Coins: {coinsCollected}/{coinsNeeded}";
            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth - 150, 20); // Rechtsboven

            _spriteBatch.DrawString(font, coinText, position, Color.White);
        }
        private void DrawHealthStatus()
        {
            var font = Content.Load<SpriteFont>("Gamefont"); // Laad een font voor de tekst
            string coinText = $"Health: {hero.Health}";
            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth/2 - font.MeasureString(coinText).X, 20); // Rechtsboven

            _spriteBatch.DrawString(font, coinText, position, Color.White);
        }
        private void DrawStart()
        {
            var font = Content.Load<SpriteFont>("Gamefont");
            string startText = "Press Enter To start";
            string titleText = "Choose Your Level";
            string level1Text = "Level 1";
            string level2Text = "Level 2";
            Vector2 startpositie = new Vector2(_graphics.PreferredBackBufferWidth / 2 - font.MeasureString(startText).X / 2, 400);
            _spriteBatch.DrawString(font, startText, startpositie, Color.Red);
            //
            Vector2 titlePosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - font.MeasureString(titleText).X / 2,100);
            _spriteBatch.DrawString(font, titleText, titlePosition, Color.White);
            //
            Vector2 level1Position = new Vector2(_graphics.PreferredBackBufferWidth / 4 - font.MeasureString(level1Text).X / 2,200);
            _spriteBatch.DrawString(font, level1Text, level1Position, currentLevel == LevelSelect.Level1 ? Color.Yellow : Color.White);
            //
            Vector2 level2Position = new Vector2(3 * _graphics.PreferredBackBufferWidth / 4 - font.MeasureString(level2Text).X / 2,200);
            _spriteBatch.DrawString(font, level2Text, level2Position, currentLevel == LevelSelect.Level2 ? Color.Yellow : Color.White);

        }
        private void LoadLevel1()
        {
            blokTexture = new Texture2D(GraphicsDevice, 1, 1);
            blokTexture.SetData(new[] { Color.White });
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //hitbox
            HitboxTexture = new Texture2D(GraphicsDevice, 1, 1);
            HitboxTexture.SetData(new[] { Color.White });
            //enemies
            enemyManager = new EnemyManager(Content.Load<Texture2D>("Ghost-Sheet"));
            enemyManager.AddEnemy(new Vector2(200, 200), Color.White);
            enemyManager.AddEnemy(new Vector2(50, 200), Color.White);
            enemyManager.AddEnemy(new Vector2(50, 350), Color.White);
            //enemyTexture.SetData(new[] { Color.Red });
            //coin
            coinTexture = Content.Load<Texture2D>("Coin-Sheetpng");
            coins = new List<Coin>
                    {
                        new Coin(new Vector2(100, 100), coinTexture),
                        new Coin(new Vector2(750, 200), coinTexture),
                        new Coin(new Vector2(30, 200), coinTexture),
                        new Coin(new Vector2(50, 400), coinTexture),
                        new Coin(new Vector2(750, 400), coinTexture)
                    };
            //level
            tilesetTexture = Content.Load<Texture2D>("0x72_DungeonTilesetII_v1.7");
            Dictionary<int, Rectangle> tileMapping = new Dictionary<int, Rectangle>{
                                                         { 0, new Rectangle(16, 64, 16, 16) },  // Vloer
                                                         { 1, new Rectangle(16, 16, 16, 16) }}; //muur
            level = new Level(tilesetTexture, 32, tileMapping); // Stel de grootte van de tiles in (bijvoorbeeld 32x32)
            int[,] levelLayout = new int[,]{
                                           { 0, 0, 1, 1,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 0, 0, 0, 0,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 0, 0,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 0, 0,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 1, 1, 1,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 0, 0, 1, 1,0,0, 0,0,0,0 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 0, 0, 0, 0,0,0, 0,0,0,0 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 0, 0,0,0, 0,0,0,0 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 0, 0,0,0, 0,0,0,0 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 1, 1, 1,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 0, 0, 1, 1,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 0, 0, 0, 0,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 0, 0,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 0, 0,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 1, 1, 1,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1}};
            level.LoadLevel(levelLayout);
        }
        private void LoadLevel2()
        {
            blokTexture = new Texture2D(GraphicsDevice, 1, 1);
            blokTexture.SetData(new[] { Color.White });
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //hitbox
            HitboxTexture = new Texture2D(GraphicsDevice, 1, 1);
            HitboxTexture.SetData(new[] { Color.White });
            //enemies
            enemyManager = new EnemyManager(Content.Load<Texture2D>("Ghost-Sheet"));
            enemyManager.AddEnemy(new Vector2(200, 200), Color.White);
            enemyManager.AddEnemy(new Vector2(50, 200), Color.White);
            enemyManager.AddEnemy(new Vector2(50, 350), Color.White);
            //enemyTexture.SetData(new[] { Color.Red });
            //coin
            coinTexture = Content.Load<Texture2D>("Coin-Sheetpng");
            coins = new List<Coin>
                    {
                        new Coin(new Vector2(100, 100), coinTexture),
                        new Coin(new Vector2(750, 95), coinTexture),
                        new Coin(new Vector2(30, 200), coinTexture),
                        new Coin(new Vector2(50, 400), coinTexture),
                        new Coin(new Vector2(750, 400), coinTexture)
                    };
            //level
            tilesetTexture = Content.Load<Texture2D>("0x72_DungeonTilesetII_v1.7");
            Dictionary<int, Rectangle> tileMapping = new Dictionary<int, Rectangle>{
                                                         { 0, new Rectangle(16, 64, 16, 16) },  // Vloer
                                                         { 1, new Rectangle(16, 16, 16, 16) }}; //muur
            level = new Level(tilesetTexture, 32, tileMapping); // Stel de grootte van de tiles in (bijvoorbeeld 32x32)
            int[,] levelLayout = new int[,]{
                                           { 0, 0, 1, 1,0,0, 1,1,1,1 ,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},
                                           { 0, 0, 0, 0,0,0, 1,0,0,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 0, 0,0,0, 1,0,0,1 ,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},
                                           { 1, 0, 0, 0,0,0, 1,0,0,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 1, 1, 1,1,0, 1,0,0,1 ,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},
                                           { 1, 0, 0, 0,0,0, 0,0,0,0 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 1, 0,0,0, 0,0,0,0 ,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1},
                                           { 1, 0, 1, 0,0,0, 0,0,0,0 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 1, 0,0,0, 0,0,0,0 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 1, 1,0,0, 1,0,0,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 1, 1,0,0, 1,0,0,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 1, 0,0,0, 1,0,0,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 1, 0,0,0, 1,0,0,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 1, 0,0,0, 1,0,0,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                           { 1, 0, 1, 1,0,0, 1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1}};
            level.LoadLevel(levelLayout);
        }
        private void RestartLevel1()
        {
            currentGameState = GameState.Playing;
            firsttime = true;
            firsttime2 = true;
            hero = new Hero(_texture, new KeyBoardReader()); // Reset hero
            enemyManager = new EnemyManager(Content.Load<Texture2D>("Ghost-Sheet")); // Reset enemy manager
            enemyManager.AddEnemy(new Vector2(200, 200), Color.White);
            enemyManager.AddEnemy(new Vector2(50, 200), Color.White);
            enemyManager.AddEnemy(new Vector2(50, 350), Color.White);
            coinsCollected = 0;//resset coins
            coinTexture = Content.Load<Texture2D>("Coin-Sheetpng");
            coins = new List<Coin>
                    {
                        new Coin(new Vector2(100, 100), coinTexture),
                        new Coin(new Vector2(750, 200), coinTexture),
                        new Coin(new Vector2(30, 200), coinTexture),
                        new Coin(new Vector2(50, 400), coinTexture),
                        new Coin(new Vector2(750, 400), coinTexture)
                    };
        }
        private void RestartLevel2()
        {
            currentGameState = GameState.Playing;
            firsttime = true;
            firsttime2 = true;
            hero = new Hero(_texture, new KeyBoardReader()); // Reset hero
            enemyManager = new EnemyManager(Content.Load<Texture2D>("Ghost-Sheet")); // Reset enemy manager
            enemyManager.AddEnemy(new Vector2(200, 200), Color.White);
            enemyManager.AddEnemy(new Vector2(50, 200), Color.White);
            enemyManager.AddEnemy(new Vector2(50, 350), Color.White);
            coinsCollected = 0;//resset coins
            coinTexture = Content.Load<Texture2D>("Coin-Sheetpng");
            coins = new List<Coin>
                    {
                        new Coin(new Vector2(100, 100), coinTexture),
                        new Coin(new Vector2(750, 95), coinTexture),
                        new Coin(new Vector2(30, 200), coinTexture),
                        new Coin(new Vector2(50, 400), coinTexture),
                        new Coin(new Vector2(750, 400), coinTexture)
                    };
        }

    }
}
