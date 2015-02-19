﻿#region
using System;
using System.Collections;
using System.Linq;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using System.Collections.Generic;
using System.Threading;
#endregion

namespace StonedSeriesAIO
{
    internal class Ryze
    {
        public Ryze()
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }
        private const string Champion = "Ryze";

        private static Orbwalking.Orbwalker Orbwalker;

        private static List<Spell> SpellList = new List<Spell>();

        private static Spell Q;

        private static Spell W;

        private static Spell E;

        private static Spell R;

        private static Menu Config;

        private static Items.Item RDO;

        private static Items.Item DFG;

        private static Items.Item YOY;

        private static Items.Item BOTK;

        private static Items.Item HYD;

        private static Items.Item CUT;

        private static Items.Item TYM;

        private static Obj_AI_Hero Player;
        

        static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (ObjectManager.Player.BaseSkinName != Champion) return;

            Q = new Spell(SpellSlot.Q, 625);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 600);
            R = new Spell(SpellSlot.R, 0);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            RDO = new Items.Item(3143, 490f);
            HYD = new Items.Item(3074, 175f);
            DFG = new Items.Item(3128, 750f);
            YOY = new Items.Item(3142, 185f);
            BOTK = new Items.Item(3153, 450f);
            CUT = new Items.Item(3144, 450f);
            TYM = new Items.Item(3077, 175f);
            

            
            Config = new Menu(Champion, "StonedRyze", true);

            
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            
            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("RHP", "Hp to use ult").SetValue(new Slider(0, 100, 0)));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseItems", "Use Items")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Harass", "Harras"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q")).SetValue(true);
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W")).SetValue(false);
            Config.SubMenu("Harass").AddItem(new MenuItem("UseEHarass", "Use E")).SetValue(false);
            Config.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));
            
            Config.AddSubMenu(new Menu("Jungle Clear", "Jungle"));
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseQClear", "Use Q")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseWClear", "Use W")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseEClear", "Use E")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("ActiveClear", "Jungle Key").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            
            Config.AddSubMenu(new Menu("Wave Clear", "Wave"));
            Config.SubMenu("Wave").AddItem(new MenuItem("UseQWave", "Use Q")).SetValue(true);
            Config.SubMenu("Wave").AddItem(new MenuItem("UseWWave", "Use W")).SetValue(true);
            Config.SubMenu("Wave").AddItem(new MenuItem("UseEWave", "Use E")).SetValue(true);
            Config.SubMenu("Wave").AddItem(new MenuItem("ActiveWave", "WaveClear Key").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));


            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            Config.AddToMainMenu();

            Game.OnGameUpdate += OnGameUpdate;
            Drawing.OnDraw += OnDraw;

        }

        private static void OnGameUpdate(EventArgs args)
        {
            Player = ObjectManager.Player;


            Orbwalker.SetAttack(true);
            if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if (Config.Item("ActiveHarass").GetValue<KeyBind>().Active)
            {
                Harras();
            }
            if (Config.Item("ActiveClear").GetValue<KeyBind>().Active)
            {
                JungleClear();
            }
            if (Config.Item("ActiveWave").GetValue<KeyBind>().Active)
            {
                WaveClear();
            }

        }

        private static void Harras()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (target == null) return;

            if (Q.IsReady() && target.Distance(Player) <= Q.Range && target.IsValidTarget() && Config.Item("UseQHarass").GetValue<bool>())
            {
                Q.Cast(target);
            }
            if (W.IsReady() && target.IsValidTarget() && target.Distance(Player) <= W.Range && Config.Item("UseWHarass").GetValue<bool>())
            {
                W.Cast(target);
            }
            if (E.IsReady() && target.IsValidTarget() && target.Distance(Player) <= E.Range && Config.Item("UseEHarass").GetValue<bool>())
            {
                E.Cast(target);
            }
        }

        private static void WaveClear()
        {
            var Minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range);

            var useQ = Config.Item("UseQClear").GetValue<bool>();
            var useW = Config.Item("UseWClear").GetValue<bool>();
            var useE = Config.Item("UseEClear").GetValue<bool>();

            var minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range);

            if (minions.Count > 0)
            {
                if (useQ && Q.IsReady() && minions[0].IsValidTarget() && Player.Distance(minions[0]) <= Q.Range)
                {
                    Q.Cast(minions[0]);
                }

                if (useW && W.IsReady() && minions[0].IsValidTarget() && Player.Distance(minions[0]) <= W.Range)
                {
                    W.Cast(minions[0]);
                }

                if (useE && E.IsReady() && minions[0].IsValidTarget() && Player.Distance(minions[0]) <= E.Range)
                {
                    E.Cast(minions[0]);
                }
            }
        }

        private static void JungleClear()
        {
            var useQ = Config.Item("UseQClear").GetValue<bool>();
            var useW = Config.Item("UseWClear").GetValue<bool>();
            var useE = Config.Item("UseEClear").GetValue<bool>();

            var allminions = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (allminions.Count > 0)
            {
                if (useQ && Q.IsReady() && allminions[0].IsValidTarget() && Player.Distance(allminions[0]) <= Q.Range)
                {
                    Q.Cast(allminions[0]);
                }

                if (useW && W.IsReady() && allminions[0].IsValidTarget() && Player.Distance(allminions[0]) <= W.Range)
                {
                    W.Cast(allminions[0]);
                }

                if (useE && E.IsReady() && allminions[0].IsValidTarget() && Player.Distance(allminions[0]) <= E.Range)
                {
                    E.Cast(allminions[0]);
                }
            }
        }



        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (target == null) return;

            if (Q.IsReady() && target.Distance(Player) <= Q.Range && target.IsValidTarget() && Config.Item("UseQCombo").GetValue<bool>())
            {
                Q.Cast(target);
            }
            if (W.IsReady() && target.IsValidTarget() && target.Distance(Player) <= W.Range && Config.Item("UseWCombo").GetValue<bool>())
            {
                W.Cast(target);
            }
            if (E.IsReady() && target.IsValidTarget() && target.Distance(Player) <= E.Range && Config.Item("UseECombo").GetValue<bool>())
            {
                E.Cast(target);
            }
            if (R.IsReady() && target.IsValidTarget() && Player.HealthPercentage() <= Config.Item("RHP").GetValue<Slider>().Value && Config.Item("UseRCombo").GetValue<bool>())
            {
                R.Cast();
            }

            if (Config.Item("UseItems").GetValue<bool>())
            {
                if (Player.Distance(target) <= RDO.Range)
                {
                    RDO.Cast(target);
                }
                if (Player.Distance(target) <= HYD.Range)
                {
                    HYD.Cast(target);
                }
                if (Player.Distance(target) <= DFG.Range)
                {
                    DFG.Cast(target);
                }
                if (Player.Distance(target) <= BOTK.Range)
                {
                    BOTK.Cast(target);
                }
                if (Player.Distance(target) <= CUT.Range)
                {
                    CUT.Cast(target);
                }
                if (Player.Distance(target) <= 125f)
                {
                    YOY.Cast();
                }
                if (Player.Distance(target) <= TYM.Range)
                {
                    TYM.Cast(target);
                }
            }
        }


        private static void OnDraw(EventArgs args)
        {
            if (Config.Item("CircleLag").GetValue<bool>())
            {
                if (Config.Item("DrawQ").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value);
                }
                if (Config.Item("DrawW").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value);
                }
                if (Config.Item("DrawE").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value);
                }
              }

            else
            {
                if (Config.Item("DrawQ").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.White);
                }
                if (Config.Item("DrawW").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.White);
                }
                if (Config.Item("DrawE").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.White);
                }
            }
        }
    }
}
