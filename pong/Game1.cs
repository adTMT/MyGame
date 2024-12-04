using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using pong.Input;
using System;
using System.ComponentModel;
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
        }

        private void InitializeGameObject()
        {
            hero = new Hero(_texture, new KeyBoardReader());
            //blokje
            blokje = new Rectangle((int)positie.X, (int)positie.Y, 50, 50);
            blokje2 = new Rectangle((int)positie.X + 50, (int)positie.Y + 50, 50, 50);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            hero.Update(gameTime);
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
            hero.Draw(_spriteBatch, HitboxTexture);
            _spriteBatch.End();
            base.Draw(gameTime);
            //blokje
            _spriteBatch.Begin();
            _spriteBatch.Draw(blokTexture, blokje, Color.Red);
            _spriteBatch.Draw(blokTexture, blokje2, Color.Green);
            _spriteBatch.End();
        }
    }
}
