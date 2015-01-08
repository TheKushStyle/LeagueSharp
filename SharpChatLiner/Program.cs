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

            Config.AddSubMenu(new Menu("Text 1", "Text 1"));
            Config.SubMenu("Text 1").AddItem(new MenuItem("1", "Say txt File 1").SetValue(new KeyBind("1".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Text 1").AddItem(new MenuItem("ALL1", "Say txt File 1 To ALL")).SetValue(false);

            Config.AddSubMenu(new Menu("Text 2", "Text 2"));
            Config.SubMenu("Text 2").AddItem(new MenuItem("2", "Say txt File 2").SetValue(new KeyBind("2".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Text 2").AddItem(new MenuItem("ALL2", "Say txt File 2 To ALL")).SetValue(false);

            Config.AddSubMenu(new Menu("Text 3", "Text 3"));
            Config.SubMenu("Text 3").AddItem(new MenuItem("3", "Say txt File 3").SetValue(new KeyBind("3".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Text 3").AddItem(new MenuItem("ALL3", "Say txt File 3 To ALL")).SetValue(false);

            Config.AddSubMenu(new Menu("Text 4", "Text 4"));
            Config.SubMenu("Text 4").AddItem(new MenuItem("4", "Say txt File 4").SetValue(new KeyBind("4".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Text 4").AddItem(new MenuItem("ALL4", "Say txt File 4 To ALL")).SetValue(false);

            Config.AddSubMenu(new Menu("Text 5", "Text 5"));
            Config.SubMenu("Text 5").AddItem(new MenuItem("5", "Say txt File 5").SetValue(new KeyBind("5".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Text 5").AddItem(new MenuItem("ALL5", "Say txt File 5 To ALL")).SetValue(false);

            Config.AddSubMenu(new Menu("Text 6", "Text 6"));
            Config.SubMenu("Text 6").AddItem(new MenuItem("6", "Say txt File 6").SetValue(new KeyBind("6".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Text 6").AddItem(new MenuItem("ALL6", "Say txt File 6 To ALL")).SetValue(false);

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
            var All6 = Config.Item("ALL1").GetValue<bool>();
            string say6 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\6.txt");

            if (All6 == true)
            {
                Game.Say("/all " + say6);
            }
            else
            {
                Game.Say(say6);
            }

        }

        private static void Write5()
        {
            var All5 = Config.Item("ALL5").GetValue<bool>();
            string say5 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\5.txt");

            if (All5 == true)
            {
                Game.Say("/all " + say5);
            }
            else
            {
                Game.Say(say5);
            }

        }

        private static void Write4()
        {
            var All4 = Config.Item("ALL1").GetValue<bool>();
            string say4 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\4.txt");

            if (All4 == true)
            {
                Game.Say("/all " + say4);
            }
            else
            {
                Game.Say(say4);
            }

        }

        private static void Write3()
        {
            var All3 = Config.Item("ALL3").GetValue<bool>();
            string say3 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\3.txt");

            if (All3 == true)
            {
                Game.Say("/all " + say3);
            }
            else
            {
                Game.Say(say3);
            }

        }

        private static void Write2()
        {
            var All2 = Config.Item("ALL2").GetValue<bool>();
            string say2 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\2.txt");

            if (All2 == true)
            {
                Game.Say("/all " + say2);
            }
            else
            {
                Game.Say(say2);
            }

        }

        private static void Write1()
        {
            var All1 = Config.Item("ALL1").GetValue<bool>();
            string say1 = System.IO.File.ReadAllText(@"C:\SharpChatLiner\1.txt");

            if (All1 == true)
            {
            Game.Say("/all " + say1);
            }
            else 
            {
            Game.Say(say1);
            }
            
        }
    }
}
