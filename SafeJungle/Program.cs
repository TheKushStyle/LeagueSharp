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
using Color = System.Drawing.Color;
#endregion 

namespace SafeJungle
{
   internal class Program
    {
        private static Menu Config;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
                 
        Config = new Menu("SafeJungle", "SafeJungle", true);

        Config.AddSubMenu(new Menu("Walk to Safe Spot", "WalkSpot"));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("EnableWalk", "Enable Walk to Spot?")).SetValue(true);
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("TeamSwitch", "off = Purple").SetValue(true));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot1", "Red").SetValue(new KeyBind(35,KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot2", "Wraiths").SetValue(new KeyBind(40, KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot3", "Wolves").SetValue(new KeyBind(34, KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot4", "Blue").SetValue(new KeyBind(37, KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot5", "Golems").SetValue(new KeyBind(12, KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot6", "Dragon").SetValue(new KeyBind(39, KeyBindType.Press)));

        Config.AddSubMenu(new Menu("Drawings", "Drawings"));
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot1Draw", "Draw Red")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot2Draw", "Draw Wraiths")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot3Draw", "Draw Wolves")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot4Draw", "Draw Blue")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot5Draw", "Draw Golems")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot6Draw", "Draw Dragon")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
        Config.SubMenu("Drawings").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
        Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(30, 100, 1)));
        Config.SubMenu("Drawings").AddItem(new MenuItem("Width", "Circle Width").SetValue(new Slider(30, 100)));
        
        Config.AddToMainMenu();
        
        Game.OnGameUpdate += OnGameUpdate;
        Drawing.OnDraw += OnDraw;
        Game.PrintChat("<font color='#FF00BF'>Safe Jungle Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>");
        }

        private static void OnDraw(EventArgs args)
        {
            if (Config.Item("CircleLag").GetValue<bool>() == true)
            {
                var Width = Config.Item("Width").GetValue<Slider>().Value;
                if (Config.Item("TeamSwitch").GetValue<bool>() == true)
                
                {

                if (Config.Item("Spot1Draw").GetValue<bool>())
                {
                    Utility.DrawCircle(new Vector3(7444.8623046875f, 2980.26171875f, 56.261837005615f), Width, Color.Aqua ,Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot2Draw").GetValue<bool>())
                {
                    Utility.DrawCircle(new Vector3(7232.5737304688f, 4671.7133789063f, 51.952449798584f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot3Draw").GetValue<bool>())
                {
                    Utility.DrawCircle(new Vector3(4142.5556640625f, 5695.958984375f, 55.266135169434f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot4Draw").GetValue<bool>())
                {
                    Utility.DrawCircle(new Vector3(3402.3193359375f, 8429.149410625f, 53.792419433594f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot5Draw").GetValue<bool>())
                {
                    Utility.DrawCircle(new Vector3(7213.7822265625f, 2103.2778320313f, 54.743419647217f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot6Draw").GetValue<bool>())
                {
                    Utility.DrawCircle(new Vector3(10270.611328125f, 54f, 4974.5263671875f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
             
                }
                else
                {
                    if (Config.Item("Spot1Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(6859.1840820313f, 11497.25f, 52.699733734131f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot2Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(7010.9077148438f, 10021.69140625f, 57.372627258301f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot3Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(9850.3623046875f, 8781.2353515625f, 52.639091491699f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot4Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(11128.295898438f, 6225.5424804688f, 54.852607727051f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot5Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(6905.4633789063f, 12402.211914063f, 53.68051904004f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot6Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(10270.611328125f, 54f, 4974.5263671875f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                }
            }
        }

        

        private static void OnGameUpdate(EventArgs args)
        {
            
            if (Config.Item("EnableWalk").GetValue<bool>() == true)
            {
               //Blue Side
               if (Config.Item("TeamSwitch").GetValue<bool>() == true)
                {
                   if      (Config.Item("WalkSpot1").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7444.8623046875f, 2980.26171875f)).Send();
                   }
                   else if (Config.Item("WalkSpot2").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7232.5737304688f, 4671.7133789063f)).Send();
                   }
                   else if (Config.Item("WalkSpot3").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(4142.5556640625f, 5695.958984375f)).Send();
                   }
                   else if (Config.Item("WalkSpot4").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(3402.3193359375f, 8429.149410625f)).Send();
                   }
                   else if (Config.Item("WalkSpot5").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7213.7822265625f, 2103.2778320313f)).Send();
                   }
                   else if (Config.Item("WalkSpot6").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(10270.611328125f, 54f)).Send();
                   }
                   }
                //Purple Side
               else
                {
                    if      (Config.Item("WalkSpot1").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(6859.1840820313f, 11497.25f)).Send();
                    }
                    else if (Config.Item("WalkSpot2").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7010.9077148438f, 10021.69140625f)).Send();
                    }
                    else if (Config.Item("WalkSpot3").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(9850.3623046875f, 8781.2353515625f)).Send();
                    }
                    else if (Config.Item("WalkSpot4").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(11128.295898438f, 6225.5424804688f)).Send();
                    }
                    else if (Config.Item("WalkSpot5").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(6905.4633789063f, 12402.211914063f)).Send();
                    }
                    else if (Config.Item("WalkSpot6").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(10270.611328125f, 54)).Send();
                    }
                }
            }
        }
    }
}

