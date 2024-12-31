using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Coinfolder
{
    class CoinManager
    {
        public List<Coin> coins;
        Texture2D coinTexture;
        public CoinManager(Texture2D coinTexture)
        {
            coins = new List<Coin>();
            this.coinTexture = coinTexture;
        }
        public void setCoins(LevelSelect lvl)
        {
            if (lvl == LevelSelect.Level1)
            {
                coins = new List<Coin>
                    {
                        new Coin(new Vector2(100, 100), coinTexture),
                        new Coin(new Vector2(750, 200), coinTexture),
                        new Coin(new Vector2(30, 200), coinTexture),
                        new Coin(new Vector2(50, 400), coinTexture),
                        new Coin(new Vector2(750, 400), coinTexture)
                    };
            }
            else if (lvl == LevelSelect.Level2)
            {
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
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var coin in coins)
            {
                coin.Draw(spriteBatch);
            }
        }
    }
}
