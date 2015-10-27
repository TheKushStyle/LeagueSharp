using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace KappaSeries
{
    class Ahri
    {
        public Ahri()
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

        private void Load(EventArgs args)
        {
            _player = ObjectManager.Player;

            _q = new Spell(SpellSlot.Q, 840);
            _w = new Spell(SpellSlot.W, 800);
            _e = new Spell(SpellSlot.E, 975);
            _r = new Spell(SpellSlot.R, 550);

            _q.SetSkillshot(_q.Instance.SData.SpellCastTime, 90f, _q.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotLine);
            _e.SetSkillshot(_e.Instance.SData.SpellCastTime, 100f, _e.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);

            SpellList.Add(_q);
            SpellList.Add(_w);
            SpellList.Add(_e);
            SpellList.Add(_r);

            IgniteSlot = _player.GetSpellSlot("SummonerDot");

            _cfg = new Menu("Ahri", "Ahri", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            _cfg.AddSubMenu(targetSelectorMenu);

            _cfg.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_cfg.SubMenu("Orbwalking"));

            _cfg.AddSubMenu(new Menu("Combo", "Combo"));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("RCombo", "Use R Combo").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseR", "Usage Of R").SetValue(new StringList(new[] { "To Mouse", "To Enemy", "Don't Use" }, 1)));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("RKill", "Only Use R When Killable").SetValue(true));

            _cfg.AddSubMenu(new Menu("Harass", "Harass"));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarQ", "Use Q In Harass").SetValue(true));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarW", "Use W In Harass").SetValue(true));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarE", "Use E In Harass").SetValue(true));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarMana", "Min Mana %").SetValue(new Slider(50,0,100)));

            _cfg.AddSubMenu(new Menu("LaneClear", "LaneClear"));
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("ActiveLane", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseQLane", "Use Q")).SetValue(true);
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseWLane", "Use W")).SetValue(true);

            _cfg.AddSubMenu(new Menu("JungleClear", "JungleClear"));
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("ActiveJungle", "JungleClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseQJungle", "Use Q")).SetValue(true);
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseWJungle", "Use W")).SetValue(true);

            _cfg.AddSubMenu(new Menu("KillSteal", "KillSteal"));
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("SmartKS", "Smart KillSteal")).SetValue(true);
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("AutoIgnite", "Auto Ignite")).SetValue(true);

            _cfg.AddSubMenu(new Menu("Drawings", "Drawings"));
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("Qdraw", "Draw Q Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("Wdraw", "Draw W Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("Edraw", "Draw E Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("Rdraw", "Draw R Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("LagFree", "Lag Free Cirlces")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            _cfg.AddSubMenu(new Menu("Misc", "Misc"));
            _cfg.SubMenu("Misc").AddItem(new MenuItem("TowerE", "Auto E Under Turret")).SetValue(false);
            _cfg.SubMenu("Misc").AddItem(new MenuItem("IntE", "Auto Interrupt with E")).SetValue(false);
            _cfg.SubMenu("Misc").AddItem(new MenuItem("IntMed", "Interrupt Medium Danger Spells")).SetValue(false);

            _cfg.AddSubMenu(new Menu("Flee", "Flee"));
            _cfg.SubMenu("Flee").AddItem(new MenuItem("ActiveFlee", "Flee!").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Press)));

            _cfg.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += OnDraw;
            Interrupter2.OnInterruptableTarget += OnPossibleToInterrupt;
        }

        private void Game_OnUpdate(EventArgs args)
        {
            if (_player.IsDead)
            {
                return;
            }

            if (_cfg.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if (_cfg.Item("ActiveHarass").GetValue<KeyBind>().Active)
            {
                if (GetManaPercent(_player) >= _cfg.Item("HarMana").GetValue<Slider>().Value)
                {
                   Harass(); 
                }
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
            if (_cfg.Item("ActiveFlee").GetValue<KeyBind>().Active)
            {
                Flee();
            }
            if (_cfg.Item("TowerE").IsActive())
            {
                TowerE();
            }
            
        }

        private void TowerE()
        {
            var allyturret = ObjectManager.Get<Obj_AI_Turret>().First(obj => obj.IsAlly && obj.Distance(_player) <= 775f);
            var minUnderTur = MinionManager.GetMinions(allyturret.ServerPosition, 775, MinionTypes.All, MinionTeam.Enemy);

            foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(target => target.IsValidTarget(_e.Range)))
            {
                if (allyturret != null && minUnderTur == null && target.IsValidTarget())
                {
                    _e.Cast(target);
                }
            }
        }

        private static void Flee()
        {
            _player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (_r.IsReady())
            {
                _r.Cast(Game.CursorPos);
            }
            var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);
            if (_e.IsReady() && t.IsValidTarget(_e.Range))
            {
                _e.Cast(t);
            }
        }

        private static void Smartks()
        {
            foreach (
                var t in ObjectManager.Get<Obj_AI_Hero>().Where(t => t.IsEnemy).Where(t => t.IsValidTarget(_q.Range)))
            {
                if (t.Health <= _q.GetDamage(t) && _q.IsReady())
                {
                    _q.Cast(t);
                }
                else if (t.Health <= (_q.GetDamage(t) + _e.GetDamage(t)) && _q.IsReady() && _e.IsReady())
                {
                    _e.Cast(t);
                    _q.Cast(t);
                }
                else if (t.Health <= (_e.GetDamage(t)) && _e.IsReady())
                {
                    _e.Cast(t);
                }
                if (IgniteSlot.IsReady() && IgniteSlot != SpellSlot.Unknown && t.Distance(_player) <= 600 && t.Health <= _player.GetSummonerSpellDamage(t, Damage.SummonerSpell.Ignite))
                {
                    _player.Spellbook.CastSpell(IgniteSlot, t);
                }
            }
            
        }

        private static void Laneclear()
        {
            var minion = MinionManager.GetMinions(_player.ServerPosition, _q.Range);

            if (minion.Count < 3)
                return;
            if (_cfg.Item("UseQLane").IsActive() && _q.IsReady())
            {
                _q.Cast(minion[0].ServerPosition);
            }
            if (_cfg.Item("UseWLane").IsActive() && _w.IsReady())
            {
                _w.Cast();
            }
        }

        private static void Jungleclear()
        {
            var junglemonster = MinionManager.GetMinions(_player.ServerPosition, _q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (junglemonster.Count == 0) return;

            if (_cfg.Item("UseQJungle").IsActive() && _q.IsReady())
            {
                _q.Cast(junglemonster[0].ServerPosition);
            }
            if (_cfg.Item("UseWJungle").IsActive() && _w.IsReady())
            {
                _w.Cast();
            }
        }

        private static float GetManaPercent(Obj_AI_Hero player)
        {
            return player.Mana * 100 / player.MaxMana;
        }

        private static void Harass()
        {
            var t = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);

            if (_player.Distance(t) <= _e.Range && _e.IsReady() && _e.MinHitChance >= HitChance.Medium)
            {
                _e.Cast(t);
            }

            if (_player.Distance(t) <= _q.Range && _q.IsReady() && _q.MinHitChance >= HitChance.Medium)
            {
                _q.Cast(t);
            }

            if (_player.Distance(t) <= _w.Range && _w.IsReady())
            {
                _w.Cast();
            }
        }

        private static void Combo()
        {
            var t = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);

            if (t.IsValidTarget() && t != null)
            {
                if (_cfg.Item("RCombo").IsActive())//REQW
                {
                    if (!_cfg.Item("RKill").IsActive())
                    {
                        if (_cfg.Item("UseR").GetValue<StringList>().SelectedIndex == 0 )
                        {
                            _r.Cast(Game.CursorPos);
                        }

                    if (_cfg.Item("UseR").GetValue<StringList>().SelectedIndex == 1)

                        {
                            _r.Cast(t.ServerPosition);
                        }
                    }

                    if (_cfg.Item("RKill").IsActive() && (((_r.GetDamage(t) * 3) + _q.GetDamage(t) + _w.GetDamage(t) + _e.GetDamage(t)) >= t.Health) && _q.IsReady() && _w.IsReady() && _e.IsReady())
                    {
                        if (_cfg.Item("UseR").GetValue<StringList>().SelectedIndex == 0)
                        {
                            _r.Cast(Game.CursorPos);
                        }

                        if (_cfg.Item("UseR").GetValue<StringList>().SelectedIndex == 1)
                        {
                            _r.Cast(t.ServerPosition);
                        }
                    }

                    if (_player.Distance(t) <= _e.Range && _e.IsReady() && _e.MinHitChance >= HitChance.Medium)
                    {
                        _e.Cast(t);
                    }

                    if (_player.Distance(t) <= _q.Range && _q.IsReady() && _q.MinHitChance >= HitChance.Medium)
                    {
                        _q.Cast(t);
                    }

                    if (_player.Distance(t) <= _w.Range && _w.IsReady())
                    {
                        _w.Cast();
                    }
                    
                }
                else 
                {
                    if (_player.Distance(t) <= _e.Range && _e.IsReady() && _e.MinHitChance >= HitChance.Medium)
                    {
                        _e.Cast(t);
                    }

                    if (_player.Distance(t) <= _q.Range && _q.IsReady() && _q.MinHitChance >= HitChance.Medium)
                    {
                        _q.Cast(t);
                    }

                    if (_player.Distance(t) <= _w.Range && _w.IsReady())
                    {
                        _w.Cast();
                    }
                }
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
                        ObjectManager.Player.Position, _w.Range, System.Drawing.Color.Chartreuse,
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
                        ObjectManager.Player.Position, _r.Range, System.Drawing.Color.Crimson,
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
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _w.Range, System.Drawing.Color.Chartreuse);
                }

                if (_cfg.Item("Edraw").IsActive())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.BlueViolet);
                }

                if (_cfg.Item("Rdraw").IsActive())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _r.Range, System.Drawing.Color.Crimson);
                }
            }
        }

        private static void OnPossibleToInterrupt(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!_cfg.Item("IntE").IsActive() || !_e.IsReady() || !sender.IsValidTarget(_q.Range))
            {
                return;
            }
            if (args.DangerLevel == Interrupter2.DangerLevel.High || args.DangerLevel == Interrupter2.DangerLevel.Medium && _cfg.Item("IntMed").IsActive())
            {
                _e.Cast(sender);
            }
        }
    }
}
