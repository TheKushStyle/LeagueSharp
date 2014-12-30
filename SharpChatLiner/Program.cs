using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace SharpChatLiner
{
   internal class Program
    {
        private static Menu Config;
        private static int LastTick = 0;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Config = new Menu("SharpChatLiner", "SharpChatLiner", true);
            Config.AddSubMenu(new Menu("SharpChatLiner", "SharpChatLiner"));
            Config.SubMenu("SharpChatLiner").AddItem(new MenuItem("1", "Say txt File 1").SetValue(new KeyBind("1".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("SharpChatLiner").AddItem(new MenuItem("2", "Say txt File 2").SetValue(new KeyBind("2".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("SharpChatLiner").AddItem(new MenuItem("3", "Say txt File 3").SetValue(new KeyBind("3".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("SharpChatLiner").AddItem(new MenuItem("4", "Say txt File 4").SetValue(new KeyBind("4".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("SharpChatLiner").AddItem(new MenuItem("5", "Say txt File 5").SetValue(new KeyBind("5".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("SharpChatLiner").AddItem(new MenuItem("6", "Say txt File 6").SetValue(new KeyBind("6".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("SharpChatLiner").AddItem(new MenuItem("7", "Say txt File 7").SetValue(new KeyBind("7".ToCharArray()[0], KeyBindType.Press)));

            Config.AddToMainMenu();

            Game.OnGameUpdate += OnGameUpdate;
            Game.PrintChat("<font color='#FF00BF'>Sharp Chat Liner Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>");

        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (Config.Item("1").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 300) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, Write1);
            }
            if (Config.Item("2").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 300) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, Write2);
            }
            if (Config.Item("3").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 300) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, Write3);
            }
            if (Config.Item("4").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 300) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, Write4);
            }
            if (Config.Item("5").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 300) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, Write5);
            }
            if (Config.Item("6").GetValue<KeyBind>().Active)
            {
                if (Environment.TickCount - LastTick < 3 * 300) return;
                LastTick = Environment.TickCount;
                Utility.DelayAction.Add(165, Write6);
            }
        }

        private static void Write6()
        {
            string say6 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\6.txt");

            Game.Say(say6);
        }

        private static void Write5()
        {
            string say5 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\5.txt");

            Game.Say(say5);
        }

        private static void Write4()
        {
            string say4 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\4.txt");

            Game.Say(say4);
        }

        private static void Write3()
        {
            string say3 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\3.txt");

            Game.Say(say3);
        }

        private static void Write2()
        {
            string say2 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\2.txt");

            Game.Say(say2);
        }

        private static void Write1()
        {
            string say1 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\1.txt");
            Game.Say(say1);
        }
    }
}
