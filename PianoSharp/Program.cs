using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using System.Media;

namespace PianoSharp
{
    internal class Program
    {
       private static int LastTick = 0;
       private static SoundPlayer a1 = new SoundPlayer(PianoSharp.Properties.Resources.a1);
       private static SoundPlayer b1 = new SoundPlayer(PianoSharp.Properties.Resources.b1);
       private static SoundPlayer c1 = new SoundPlayer(PianoSharp.Properties.Resources.c1);
       private static SoundPlayer c2 = new SoundPlayer(PianoSharp.Properties.Resources.c2);
       private static SoundPlayer d1 = new SoundPlayer(PianoSharp.Properties.Resources.d1);
       private static SoundPlayer f1 = new SoundPlayer(PianoSharp.Properties.Resources.f1);
       private static SoundPlayer g1 = new SoundPlayer(PianoSharp.Properties.Resources.g1);

        private static Menu Config;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Config = new Menu("Piano Sharp", "Piano Sharp", true);
            Config.AddSubMenu(new Menu("PianoSharp", "PianoSharp"));
            Config.SubMenu("PianoSharp").AddItem(new MenuItem("a1", "Play a1").SetValue(new KeyBind("q".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("PianoSharp").AddItem(new MenuItem("b1", "Play a1").SetValue(new KeyBind("w".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("PianoSharp").AddItem(new MenuItem("c1", "Play a1").SetValue(new KeyBind("e".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("PianoSharp").AddItem(new MenuItem("c2", "Play a1").SetValue(new KeyBind("r".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("PianoSharp").AddItem(new MenuItem("d1", "Play a1").SetValue(new KeyBind("x".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("PianoSharp").AddItem(new MenuItem("f1", "Play a1").SetValue(new KeyBind("v".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("PianoSharp").AddItem(new MenuItem("g1", "Play a1").SetValue(new KeyBind(32, KeyBindType.Press)));
            
            Config.AddToMainMenu();

            Game.OnGameUpdate += OnGameUpdate;
            Game.PrintChat("<font color='#FF00BF'>Piano Sharp Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>");

        }

       

        private static void OnGameUpdate(EventArgs args)
        {
             

            if (Config.Item("a1").GetValue<KeyBind>().Active )
            {
                if (Environment.TickCount - LastTick < 3 * 65) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, PlayA1);
            }
            if (Config.Item("b1").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 65) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, PlayB1);
            }
            if (Config.Item("c1").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 65) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, PlayC1);
            }
            if (Config.Item("c2").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 65) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, PlayC2);
            }
            if (Config.Item("d1").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 65) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, PlayD1);
            }
            if (Config.Item("f1").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 65) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, PlayF1);
            }
            if (Config.Item("g1").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 65) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, PlayG1);
            }
        }

        private static void PlayA1()
        {
            a1.Play();
        }
        private static void PlayB1()
        {
            b1.Play();
        }
        private static void PlayC1()
        {
            c1.Play();
        }
        private static void PlayC2()
        {
            c2.Play();
        }
        private static void PlayD1()
        {
            d1.Play();
        }
        private static void PlayF1()
        {
            f1.Play();
        }
        private static void PlayG1()
        {
            g1.Play();
        }

    }
}
