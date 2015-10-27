using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace KappaSeries
{
    internal class DrMundo
    {
        public DrMundo()
        {
            CustomEvents.Game.OnGameLoad += Load;
        }

        private static Orbwalking.Orbwalker _orbwalker;
        public static readonly List<Spell> SpellList = new List<Spell>();
        private static Spell _q;
        private static Spell _w;
        private static Spell _e;
        private static Spell _r;
        public static SpellSlot IgniteSlot;
        private static Menu _cfg;
        private static Obj_AI_Hero _player;

        private static void Load(EventArgs args)
        {
            _player = ObjectManager.Player;

            _q = new Spell(SpellSlot.Q, 1000);
            _w = new Spell(SpellSlot.W, 162);
            _e = new Spell(SpellSlot.E, 125);
            _r = new Spell(SpellSlot.R, 0);

            _q.SetSkillshot(_q.Instance.SData.SpellCastTime, 60f, 2000f, true, SkillshotType.SkillshotLine);

            SpellList.Add(_q);
            SpellList.Add(_w);
            SpellList.Add(_e);
            SpellList.Add(_r);

            IgniteSlot = _player.GetSpellSlot("SummonerDot");

            _cfg = new Menu("DrMundo", "DrMundo", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            _cfg.AddSubMenu(targetSelectorMenu);

            _cfg.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_cfg.SubMenu("Orbwalking"));

            _cfg.AddSubMenu(new Menu("Combo", "Combo"));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseQ", "Use Q")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseW", "Use W")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseE", "Use E")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseItems", "Use Items")).SetValue(true);

            _cfg.AddSubMenu(new Menu("R Settings", "R Settings"));
            _cfg.SubMenu("R Settings").AddItem(new MenuItem("UseR", "Use R")).SetValue(true);
            _cfg.SubMenu("R Settings").AddItem(new MenuItem("RRange", "R Range to auto R").SetValue(new Slider(600, 0, 2000)));
            _cfg.SubMenu("R Settings").AddItem(new MenuItem("RHP", "Auto R % HP").SetValue(new Slider(15, 0, 100)));

            _cfg.AddSubMenu(new Menu("Harass", "Harass"));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "Auto Q").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarassHP", "Don't Harass if HP <= %").SetValue(new Slider(30, 0, 100)));
           

            _cfg.AddSubMenu(new Menu("LaneClear", "LaneClear"));
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("ActiveLane", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseQLane", "Use Q")).SetValue(true);
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseWLane", "Use W")).SetValue(true);
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseELane", "Use E")).SetValue(true);

            _cfg.AddSubMenu(new Menu("JungleClear", "JungleClear"));
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("ActiveJungle", "JungleClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseQJungle", "Use Q")).SetValue(true);
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseWJungle", "Use W")).SetValue(true);
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseEJungle", "Use E")).SetValue(true);

            _cfg.AddSubMenu(new Menu("LastHit", "LastHit"));
            _cfg.SubMenu("LastHit").AddItem(new MenuItem("UseQLasthit", "Use Q")).SetValue(true);
            _cfg.SubMenu("LastHit").AddItem(new MenuItem("AutoUseQLasthit", "Auto Q Lasthit")).SetValue(false);

            _cfg.AddSubMenu(new Menu("KillSteal", "KillSteal"));
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("SmartKS", "Smart KillSteal")).SetValue(true);
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("AutoIgnite", "Auto Ignite")).SetValue(true);

            _cfg.AddSubMenu(new Menu("Drawings", "Drawings"));
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("Qdraw", "Draw Q Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("Wdraw", "Draw W Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("Edraw", "Draw E Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("Rdraw", "Draw R Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("LagFree", "Lag Free Cirlces")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 1, 10)));

            _cfg.AddSubMenu(new Menu("Misc", "Misc"));
            _cfg.SubMenu("Misc").AddItem(new MenuItem("GapQ", "Auto Q on Gapcloser")).SetValue(true);
            _cfg.SubMenu("Misc").AddItem(new MenuItem("WRange", "W Range")).SetValue(new Slider(500, 100, 1000));


            _cfg.AddSubMenu(new Menu("Flee", "Flee"));
            _cfg.SubMenu("Flee").AddItem(new MenuItem("ActiveFlee", "Flee!").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("Flee").AddItem(new MenuItem("QFlee", "Use Q Flee")).SetValue(true);
            _cfg.SubMenu("Flee").AddItem(new MenuItem("WFlee", "Use W Flee")).SetValue(false);
            _cfg.SubMenu("Flee").AddItem(new MenuItem("RFlee", "Use R Flee")).SetValue(false);

            _cfg.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += OnDraw;
            AntiGapcloser.OnEnemyGapcloser += OnGapCloser;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (_player.IsDead)
            {
                return;
            }

            if (_cfg.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }

            if (_cfg.Item("AutoQ").IsActive())
            {
                AutoQ();   
            }

            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit && _cfg.Item("UseQLasthit").IsActive())
            {
                LastHit();
            }

           if (_cfg.Item("UseQLasthit").GetValue<bool>() && _cfg.Item("AutoUseQLasthit").GetValue<bool>())
            {
                AutoLastHit();
            }        

          if (_cfg.Item("UseR").IsActive())
            {
                SaveR();
            }

           if (_cfg.Item("ActiveFlee").GetValue<KeyBind>().Active)
            {
                Flee();
            }

          if (_cfg.Item("ActiveHarass").GetValue<KeyBind>().Active)
            {
                Harass();
            }

          if (_cfg.Item("ActiveJungle").GetValue<KeyBind>().Active)
            {
                Jungleclear();
            }

         if (_cfg.Item("ActiveLane").GetValue<KeyBind>().Active)
            {
                Laneclear();
            }

         if (_cfg.Item("SmartKS").IsActive())
            {
                Smartks();
            }

        }

        private static void AutoQ()
        {
            var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);
            if (t == null) return;
            if (_cfg.Item("HarassHP").GetValue<Slider>().Value >= ((_player.Health/_player.MaxHealth)*100)
                && _q.IsReady())
            {
                _q.Cast(t);
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (_cfg.Item("LagFree").IsActive())
            {
                if (_cfg.Item("Qdraw").IsActive())
                {
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position, _q.Range, System.Drawing.Color.Cyan,
                        _cfg.Item("CircleThickness").GetValue<Slider>().Value);
                }

                if (_cfg.Item("Wdraw").IsActive())
                {
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position, _cfg.Item("WRange").GetValue<Slider>().Value, System.Drawing.Color.Chartreuse,
                        _cfg.Item("CircleThickness").GetValue<Slider>().Value);
                }

                if (_cfg.Item("Edraw").IsActive())
                {
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position, _e.Range, System.Drawing.Color.BlueViolet,
                        _cfg.Item("CircleThickness").GetValue<Slider>().Value);
                }

                if (_cfg.Item("Rdraw").IsActive())
                {
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position, _cfg.Item("RRange").GetValue<Slider>().Value, System.Drawing.Color.Crimson,
                        _cfg.Item("CircleThickness").GetValue<Slider>().Value);
                }

            }
            else
            {
                if (_cfg.Item("Qdraw").IsActive())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _q.Range, System.Drawing.Color.Cyan);
                }

                if (_cfg.Item("Wdraw").IsActive())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _cfg.Item("WRange").GetValue<Slider>().Value, System.Drawing.Color.Chartreuse);
                }

                if (_cfg.Item("Edraw").IsActive())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.BlueViolet);
                }

                if (_cfg.Item("Rdraw").IsActive())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _cfg.Item("RRange").GetValue<Slider>().Value, System.Drawing.Color.Crimson);
                }
            }
        }

        private static void OnGapCloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender == null) return;
            if (_cfg.Item("GapQ").IsActive() && _q.IsReady() && gapcloser.Sender.IsValidTarget())
            {
                _q.Cast(gapcloser.Sender);
            }
        }

        private static void Laneclear()
        {
            var activeW = _player.HasBuff("BurningAgony");
            var minion = MinionManager.GetMinions(_player.ServerPosition, _q.Range);
            if (minion.Count < 2 || minion[0] == null) return;

            if (_cfg.Item("UseQLane").IsActive() && _q.IsReady() && _player.Distance(minion[0]) <= _q.Range)
            {
                _q.Cast(minion[0]);
            }
            if (_w.IsReady() && minion[0].Distance(_player) <= _cfg.Item("WRange").GetValue<Slider>().Value && !activeW)
            {
                _w.Cast();
            }

            if (_w.IsReady() && minion[0].Distance(_player) >= _cfg.Item("WRange").GetValue<Slider>().Value && activeW)
            {
                _w.Cast();
            }
            if (_cfg.Item("UseELane").IsActive() && _e.IsReady() && _player.Distance(minion[0]) <= _e.Range)
            {
                _e.Cast();
            }
        }

        private static void Jungleclear()
        {
            var junglemonster = MinionManager.GetMinions(_player.ServerPosition, _q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            var activeW = _player.HasBuff("BurningAgony");
            if (junglemonster.Count < 2 || junglemonster[0] == null) return;

            if (_cfg.Item("UseQLane").IsActive() && _q.IsReady() && _player.Distance(junglemonster[0]) <= _q.Range)
            {
                _q.Cast(junglemonster[0]);
            }

            if (_w.IsReady() && junglemonster[0].Distance(_player) <= _cfg.Item("WRange").GetValue<Slider>().Value && !activeW)
            {
                _w.Cast();
            }

            if (_w.IsReady() && junglemonster[0].Distance(_player) >= _cfg.Item("WRange").GetValue<Slider>().Value && activeW)
            {
                _w.Cast();
            }
            if (_cfg.Item("UseELane").IsActive() && _e.IsReady() && _player.Distance(junglemonster[0]) <= _e.Range)
            {
                _e.Cast();
            }
        }

        private static void AutoLastHit()
        {
            var minionQ = MinionManager.GetMinions(_player.ServerPosition, _q.Range);
            var qdmg = _q.GetDamage(minionQ[0]);

            if (minionQ[0] == null) return;

            if (_q.IsReady() && _player.Distance(minionQ[0]) <= _q.Range && qdmg >= minionQ[0].Health)
            {
                _q.Cast(minionQ[0]);
            }
        }

        private static void LastHit()
        {
            var minion = MinionManager.GetMinions(_player.ServerPosition, _q.Range);
            var qdmg = _q.GetDamage(minion[0]);
            if (minion[0] == null ) return;

            if (_q.IsReady() && _player.Distance(minion[0]) <= _q.Range && qdmg >= minion[0].Health)
            {
                _q.Cast(minion[0]);
            }
        }

        private static void Flee()
        {
            _player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            var activeW = _player.HasBuff("BurningAgony");
            var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);
            if (_cfg.Item("QFlee").IsActive() && _q.IsReady() && t.Distance(_player) <= _q.Range)
            {
                _q.Cast(t);
            }
            if (_cfg.Item("WFlee").IsActive() && _w.IsReady() && !activeW)
            {
                _w.Cast();
            }
            if (_cfg.Item("RFlee").IsActive() && _r.IsReady())
            {
                _r.Cast();
            }
        }

        private static void Smartks()
        {
            var activeW = _player.HasBuff("BurningAgony");
            foreach (
                var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(t => t.IsEnemy).Where(t => t.IsValidTarget(_q.Range)))
            {
                #region Q
                if (_q.IsReady() && _q.GetDamage(enemy) >= enemy.Health && enemy.IsValidTarget() && enemy.Distance(_player) <= _q.Range)
                {
                    _q.Cast(enemy);
                }
                    #endregion
                    #region W
                else if (_w.IsReady() && _w.GetDamage(enemy) >= enemy.Health && enemy.IsValidTarget() && enemy.Distance(_player) <= _w.Range && !activeW)
                {
                    _w.Cast();
                }
                    #endregion
                    #region QW

                else if (_q.IsReady() && _w.IsReady() && enemy.IsValidTarget() &&
                         enemy.Health <= (_q.GetDamage(enemy) + _w.GetDamage(enemy)) && !activeW && _player.Distance(enemy) <= _w.Range)
                {
                    _q.Cast(enemy);
                    _w.Cast();
                }
                    #endregion

                AutoIgnite(enemy);
            }
        }

        private static void AutoIgnite(Obj_AI_Hero enemy)
        {
            if (enemy.IsValidTarget(600f) && enemy.Health <= 50 + (20 * _player.Level) && IgniteSlot.IsReady())
            {
                _player.Spellbook.CastSpell(IgniteSlot, enemy);
            }
        }

        private static void SaveR()
        {     
            var EnInRang = _player.CountEnemiesInRange(_cfg.Item("RRange").GetValue<Slider>().Value);

            if (((_player.Health / _player.MaxHealth * 100) <= _cfg.Item("RHP").GetValue<Slider>().Value) && _r.IsReady() && EnInRang >= 1)
            {
                _r.Cast();
            }
        }

        private static void Harass()
        {
            var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);

            if (t == null) return;

            if ( _cfg.Item("HarassHP").GetValue<Slider>().Value <= ((_player.Health / _player.MaxHealth) *100))
            {
                _q.Cast(t);
            }
        }

        private static void Combo()
        {
            var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);
            //var hitslider = _cfg.Item("Hitchance").GetValue<StringList>().SelectedValue;
            var activeW = _player.HasBuff("BurningAgony");

            if (!t.IsValidTarget() || t == null) return;

            if (_cfg.Item("UseItems").IsActive())
            {
                UseItems(t);
            }
            #region Q
            if (_q.IsReady() && t.Distance(_player) <= _q.Range && //_q.MinHitChance.ToString() == hitslider &&
                _cfg.Item("UseQ").IsActive())
            {
                _q.Cast(t);
            }

            if (_cfg.Item("UseE").IsActive() && t.Distance(_player) <= _e.Range && _e.IsReady() && t.IsValidTarget())
            {
                _e.Cast();
            }
            #endregion
            #region W
            if (_w.IsReady() && t.Distance(_player) <= _cfg.Item("WRange").GetValue<Slider>().Value && !activeW && _cfg.Item("UseW").IsActive())
            {
                _w.Cast();
            }

            if (_w.IsReady() && t.Distance(_player) >= _cfg.Item("WRange").GetValue<Slider>().Value && activeW && _cfg.Item("UseW").IsActive())
            {
                _w.Cast();
            }
            #endregion
            #region E
            if (_cfg.Item("UseE").IsActive() && t.Distance(_player) <= _e.Range && _e.IsReady() && t.IsValidTarget())
            {
                _e.Cast();
            }
#endregion
        }

        private static void UseItems(Obj_AI_Hero t)
        {
           var rdo = LeagueSharp.Common.Data.ItemData.Randuins_Omen;
           var yoy = LeagueSharp.Common.Data.ItemData.Youmuus_Ghostblade;
           var botk = LeagueSharp.Common.Data.ItemData.Blade_of_the_Ruined_King;
           var hyd = LeagueSharp.Common.Data.ItemData.Ravenous_Hydra_Melee_Only;
           var rg = LeagueSharp.Common.Data.ItemData.Righteous_Glory;
           var cut = LeagueSharp.Common.Data.ItemData.Bilgewater_Cutlass;

            if (t == null || t.IsDead || !t.IsValidTarget()) return;

            if (_player.Distance(t) <= rdo.Range && Items.HasItem(rdo.Id) && Items.CanUseItem(rdo.Id))
            {
                Items.UseItem(rdo.Id);
            }

            if (_player.Distance(t) <= hyd.Range && Items.HasItem(hyd.Id) && Items.CanUseItem(hyd.Id))
            {
                Items.UseItem(hyd.Id);
            }

            if (_player.Distance(t) <= botk.Range && Items.HasItem(botk.Id) && Items.CanUseItem(botk.Id))
            {
                Items.UseItem(botk.Id,t);
            }

            if (_player.Distance(t) <= cut.Range && Items.HasItem(cut.Id) && Items.CanUseItem(cut.Id))
            {
                Items.UseItem(cut.Id,t);
            }

            if (_player.Distance(t) <= 125f && Items.HasItem(yoy.Id) && Items.CanUseItem(yoy.Id))
            {
                Items.UseItem(yoy.Id);
            }

            if (_player.Distance(t) <= 600f && Items.HasItem(rg.Id) && Items.CanUseItem(rg.Id))
            {
                Items.UseItem(rg.Id);
            }
        }
    }
}