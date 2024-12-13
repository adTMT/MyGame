using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using pong.Input;
using pong.Levels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using static System.Formats.Asn1.AsnWriter;

namespace pong
{
    public enum GameState
    {
        Start,
        Playing,
        GameOver
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
        private GameState currentGameState = GameState.Playing;
        //hitboxtexture
        Texture2D HitboxTexture;
        //level
        Level1 level; // Level-klasse als variabele
        Texture2D tilesetTexture; // Texture voor de tileset
        List<Enemy> enemies;
        private Texture2D enemyTexture;
        bool firsttime = true;
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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _texture = Content.Load<Texture2D>("Ratfolk Mage Sprite Sheet");
            InitializeGameObject();
            //blokje
            blokTexture = new Texture2D(GraphicsDevice, 1, 1);
            blokTexture.SetData(new[] { Color.White });
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //hitbox
            HitboxTexture = new Texture2D(GraphicsDevice, 1, 1);
            HitboxTexture.SetData(new[] { Color.White });
            //level
            Dictionary<int, Rectangle> tileMapping = new Dictionary<int, Rectangle>{
                                                         { 0, new Rectangle(16, 64, 16, 16) },  // Vloer
                                                         { 1, new Rectangle(16, 16, 16, 16) }}; //muur
            tilesetTexture = Content.Load<Texture2D>("0x72_DungeonTilesetII_v1.7");
            enemyTexture = Content.Load<Texture2D>("Ghost-Sheet");
            //enemyTexture.SetData(new[] { Color.Red });
            level = new Level1(tilesetTexture, 32, tileMapping); // Stel de grootte van de tiles in (bijvoorbeeld 32x32)
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

        private void InitializeGameObject()
        {
            hero = new Hero(_texture, new KeyBoardReader());
            //blokje
            blokje = new Rectangle((int)positie.X + 200, (int)positie.Y+150, 50, 100);
            blokje2 = new Rectangle((int)positie.X + 50, (int)positie.Y + 50, 50, 50);
            //
            enemies = new List<Enemy>
            {
            new Enemy(enemyTexture,new Vector2(200, 200),Color.White),
            new Enemy(enemyTexture,new Vector2(50, 200),Color.White),
            new Enemy(enemyTexture,new Vector2(50, 350),Color.White)
            };

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (currentGameState == GameState.Playing)
            {
                hero.Update(gameTime, level, enemies);
                foreach (var enemy in enemies)
                {
                    enemy.OnDeath += HandleEnemyDeath;
                    enemy.Follow(hero.positie);
                    enemy.Update(gameTime, level, hero);
                }
                if (hero.CheckCollision(blokje) && firsttime)
                {
                    firsttime = false;
                    enemies.Add(new Enemy(enemyTexture,new Vector2(400, 300),Color.Green,1f,1f));
                    enemies.Add(new Enemy(enemyTexture,new Vector2(400, 100),Color.Green,1f,1f));
                }
                if (hero.Health <= 0)
                {
                    currentGameState = GameState.GameOver;
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
            currentGameState = GameState.Playing;
            hero = new Hero(_texture, new KeyBoardReader()); // Reset hero
            enemies.Clear();
            enemies.Add(new Enemy(enemyTexture, new Vector2(200, 200), Color.White)); // Voeg vijanden opnieuw toe
            enemies.Add(new Enemy(enemyTexture, new Vector2(50, 200), Color.White));
            enemies.Add(new Enemy(enemyTexture, new Vector2(50, 350), Color.White));
        }
        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            if (currentGameState == GameState.Playing)
            {
                level.Draw(_spriteBatch);
                hero.Draw(_spriteBatch, HitboxTexture);
                // Teken vijanden
                foreach (var enemy in enemies)
                {
                    enemy.Draw(_spriteBatch,enemyTexture);
                    //_spriteBatch.Draw(enemyTexture, new Rectangle((int)enemy.Positie.X, (int)enemy.Positie.Y, 32, 32), Color.White);
                    //_spriteBatch.Draw(HitboxTexture, enemy.Bounds, Color.Green);
                }
            }
            else if(currentGameState == GameState.GameOver)
            {
                DrawGameOverScreen();
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

        private void HandleEnemyDeath(Enemy deadEnemy)
        {
            Console.WriteLine("Removing enemy at: " + deadEnemy.Positie);
            enemies.Remove(deadEnemy); // Verwijder de vijand uit de lijst
        }
    }
}
