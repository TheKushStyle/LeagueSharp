#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


#endregion
namespace CustomOverlay
{
    class Program
    {
        private static Menu config;
        private static Render.Sprite sprite;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                config = new Menu("Custom Overlay", "Custom Overlay", true);
                config.AddItem(new MenuItem("slider", "Choose Overlay").SetValue(new Slider(1, 1,42)));

                sprite = new Render.Sprite(Properties.Resources.hud_1, new Vector2(1, 1));
                sprite.Add(0);
                Game.PrintChat("<font color='#FF00BF'>Custom Overlay Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>");
            };

            Game.OnGameUpdate += eventArgs =>
            {
                UpdateImage((Bitmap)Properties.Resources.ResourceManager.GetObject(string.Format("hud_{0}", config.Item("slider").GetValue<Slider>().Value))); // try to update image
            };
        }

        static void UpdateImage(Bitmap bitmap)
        {
            if (sprite.Bitmap == bitmap)
                return;

            sprite.UpdateTextureBitmap(bitmap); 
        }
    }
 
}

        
