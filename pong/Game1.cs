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
        Color backgroundColor = Color.CornflowerBlue;
        //hitboxtexture
        Texture2D HitboxTexture;
        //level
        Level1 level; // Level-klasse als variabele
        Texture2D tilesetTexture; // Texture voor de tileset
        List<Enemy> enemies;
        private Texture2D enemyTexture;

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
            enemyTexture = new Texture2D(GraphicsDevice, 1, 1);
            enemyTexture.SetData(new[] { Color.Red });
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
            blokje = new Rectangle((int)positie.X, (int)positie.Y, 50, 50);
            blokje2 = new Rectangle((int)positie.X + 50, (int)positie.Y + 50, 50, 50);
            //
            enemies = new List<Enemy>
            {
            new Enemy(new Vector2(100, 100)),
            new Enemy(new Vector2(200, 150)),
            new Enemy(new Vector2(300, 200))
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            hero.Update(gameTime, level, enemies);
            if (hero.CheckCollision(blokje)|| hero.CheckCollision(blokje2))
            {
                backgroundColor = Color.Black;   
            }
            else
            {
                backgroundColor = Color.CornflowerBlue;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            level.Draw(_spriteBatch);
            hero.Draw(_spriteBatch, HitboxTexture);
            // Teken vijanden
            foreach (var enemy in enemies)
            {
                _spriteBatch.Draw(enemyTexture, new Rectangle((int)enemy.Positie.X, (int)enemy.Positie.Y, 50, 50), Color.Red);
                _spriteBatch.Draw(HitboxTexture, enemy.Bounds, Color.Green);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
