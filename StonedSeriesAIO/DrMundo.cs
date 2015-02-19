#region
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
    internal class DrMundo
    {
        private const string Champion = "DrMundo";

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

        public DrMundo()
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (Player.BaseSkinName != Champion) return;

            Q = new Spell(SpellSlot.Q, 930);
            W = new Spell(SpellSlot.W, 320);
            E = new Spell(SpellSlot.E, 225);
            R = new Spell(SpellSlot.R, 0);

            Q.SetSkillshot(0.50f, 75f, 1500f, true, SkillshotType.SkillshotLine);

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

            Config = new Menu(Champion, "StonedMundo", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseItems", "Use Items")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("CountHarass", "Don't Harass if HP < %").SetValue(new Slider(30, 100, 0)));
            Config.SubMenu("Harass").AddItem(new MenuItem("Harass Tog", "Use Harass (toggle)").SetValue<KeyBind>(new KeyBind('T', KeyBindType.Toggle)));
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

            Config.AddSubMenu(new Menu("Misc", "Misc"));
            Config.SubMenu("Misc").AddItem(new MenuItem("KS", "Killsteal Q").SetValue(false));
            Config.SubMenu("Misc").AddItem(new MenuItem("Rsave", "Life saving Ultimate").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("Rhp", "R Saving Ult % health").SetValue(new Slider(30, 100, 0)));

            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            Config.AddToMainMenu();

            Game.OnGameUpdate += OnGameUpdate;
            Drawing.OnDraw += OnDraw;

            
        }



        private static void OnGameUpdate(EventArgs args)
        {
            {
                if (Player.IsDead) return;

                Player = ObjectManager.Player;

                Orbwalker.SetAttack(true);

                if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
                {
                    Combo();
                }
                if (Config.Item("ActiveClear").GetValue<KeyBind>().Active)
                {
                    JungleClear();
                }
                if (Config.Item("ActiveWave").GetValue<KeyBind>().Active)
                {
                    WaveClear();
                }
                if (Config.Item("ActiveHarass").GetValue<KeyBind>().Active)
                {
                    Harass();
                }
                if (Config.Item("Harass Tog").GetValue<KeyBind>().Active)
                {
                    HarassTog();
                }
                if (Config.Item("Rsave").GetValue<bool>())
                {
                    Rsave();
                }
                if (Config.Item("KS").GetValue<bool>())
                {
                    Killsteal();
                }
            }
        }

        private static void Killsteal()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            var QDamage = Player.GetSpellDamage(target, SpellSlot.Q) * 0.96;

            if (target.IsValidTarget() && Config.Item("KS").GetValue<bool>() && Q.IsReady() && Player.Distance(target) <= Q.Range && target.Health < QDamage)
            {
                Q.Cast(target, true);
            }
        }

        private static void HarassTog()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);

            var CountHarass = Config.Item("CountHarass").GetValue<Slider>().Value;
            var HealthPer = Player.Health * 100 / Player.MaxHealth;

            if (target.IsValidTarget() && Q.IsReady() && HealthPer >= CountHarass && Player.Distance(target) <= Q.Range)
            {
                PredictionOutput Qpredict = Q.GetPrediction(target);
                if (Qpredict.Hitchance >= HitChance.High)
                    Q.Cast(Qpredict.CastPosition);
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);

            var CountHarass = Config.Item("CountHarass").GetValue<Slider>().Value;
            var HealthPer = Player.Health * 100 / Player.MaxHealth;

            if (target.IsValidTarget() && Q.IsReady() && HealthPer >= CountHarass && Player.Distance(target) <= Q.Range)
            {
                Q.Cast(target, true);
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            bool ActiveW = false;
            if (Player.HasBuff("BurningAgony"))
            {
                ActiveW = true;
            }
            else
            {
                ActiveW = false;
            }

            if (target.IsValidTarget() && Config.Item("UseQCombo").GetValue<bool>() && Q.IsReady() && Player.Distance(target) <= Q.Range)
            {
                Q.Cast(target, true);
            }

            if (target.IsValidTarget() && Config.Item("UseWCombo").GetValue<bool>() && W.IsReady() && Player.Distance(target) <= W.Range && !ActiveW)
            {
                W.Cast();
            }
            if (target.IsValidTarget() && Config.Item("UseWCombo").GetValue<bool>() && W.IsReady() && Player.Distance(target) > 700f && ActiveW)
            {
                W.Cast();
            }

            if (Config.Item("UseECombo").GetValue<bool>() && E.IsReady() && Player.Distance(target) <= E.Range)
            {
                E.Cast();
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

        private static void Rsave()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);

            if (Player.Health < (Player.Health * Config.Item("Rhp").GetValue<Slider>().Value * 0.01) && R.IsReady() && CountR(target) >= 1 || CountR(target) == 1)
            {
                R.Cast();
            }

        }
        private static void WaveClear()
        {
            var Minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range);

            bool ActiveW = false;
            if (Player.HasBuff("BurningAgony"))
            {
                ActiveW = true;
            }
            else
            {
                ActiveW = false;
            }

            var useQ = Config.Item("UseQClear").GetValue<bool>();
            var useW = Config.Item("UseWClear").GetValue<bool>();
            var useE = Config.Item("UseEClear").GetValue<bool>();

            var minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range);

            if (minions.Count > 0)
            {
                if (useQ && Q.IsReady() && minions[0].IsValidTarget() && Player.Distance(minions[0]) <= Q.Range)
                {
                    Q.Cast(minions[0].Position);
                }

                if (useW && W.IsReady() && minions[0].IsValidTarget() && !ActiveW && Player.Distance(minions[0]) <= 700)
                {
                    W.Cast();
                }
                if (useW && W.IsReady() && minions[0].IsValidTarget() && ActiveW && Player.Distance(minions[0]) > 700)
                {
                    W.Cast();
                }

                if (useE && E.IsReady() && minions[0].IsValidTarget() && Player.Distance(minions[0]) <= E.Range)
                {
                    E.Cast();
                }
            }
        }

        private static void JungleClear()
        {

            bool ActiveW = false;
            if (Player.HasBuff("BurningAgony"))
            {
                ActiveW = true;
            }
            else
            {
                ActiveW = false;
            }

            var useQ = Config.Item("UseQClear").GetValue<bool>();
            var useW = Config.Item("UseWClear").GetValue<bool>();
            var useE = Config.Item("UseEClear").GetValue<bool>();

            var allminions = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (allminions.Count > 0)
            {
                if (useQ && Q.IsReady() && allminions[0].IsValidTarget() && Player.Distance(allminions[0]) <= Q.Range)
                {
                    Q.Cast(allminions[0].Position);
                }

                if (useW && W.IsReady() && allminions[0].IsValidTarget() && Player.Distance(allminions[0]) <= 700 && !ActiveW)
                {
                    W.Cast();
                }

                if (useW && W.IsReady() && allminions[0].IsValidTarget() && Player.Distance(allminions[0]) > 700 && ActiveW)
                {
                    W.Cast();
                }

                if (useE && E.IsReady() && allminions[0].IsValidTarget() && Player.Distance(allminions[0]) <= E.Range)
                {
                    E.Cast();
                }
            }
        }

        private static int CountR(Obj_AI_Base target)
        {
            int totalHit = 0;
            foreach (Obj_AI_Hero current in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (current.IsEnemy && Vector3.Distance(Player.Position, current.Position) <= R.Range)
                {
                    totalHit = totalHit + 1;
                }
            }
            return totalHit;
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
                if (Config.Item("DrawR").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, R.Range, System.Drawing.Color.White);
                }

            }
        }
    }
}
