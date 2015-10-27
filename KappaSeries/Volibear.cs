using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ItemData = LeagueSharp.Common.Data.ItemData;


namespace KappaSeries
{
    class Volibear
    {
        private static Orbwalking.Orbwalker _orbwalker;
        public static readonly List<Spell> SpellList = new List<Spell>();
        private static Spell _q;
        private static Spell _w;
        private static Spell _e;
        private static Spell _r;
        private static Menu _cfg;
        private static Items.Item _rdo;
        private static Items.Item _yoy;
        private static Items.Item _botk;
        private static Items.Item _hyd;
        private static Items.Item _rg;
        private static Items.Item _cut;
        private static Obj_AI_Hero _player;

        public Volibear()
        {
            CustomEvents.Game.OnGameLoad += Load;
        }

        private static void Load(EventArgs args)
        {
            _player = ObjectManager.Player;

            _q = new Spell(SpellSlot.Q, 600);
            _w = new Spell(SpellSlot.W, 400);
            _e = new Spell(SpellSlot.E, 400);
            _r = new Spell(SpellSlot.R, 125);

            SpellList.Add(_q);
            SpellList.Add(_w);
            SpellList.Add(_e);
            SpellList.Add(_r);

            _rdo = ItemData.Randuins_Omen.GetItem();
            _yoy = ItemData.Youmuus_Ghostblade.GetItem();
            _botk = ItemData.Blade_of_the_Ruined_King.GetItem();
            _hyd = ItemData.Ravenous_Hydra_Melee_Only.GetItem();
            _rg = ItemData.Righteous_Glory.GetItem();
            _cut = ItemData.Bilgewater_Cutlass.GetItem();

            _cfg = new Menu("Volibear", "Volibear", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            _cfg.AddSubMenu(targetSelectorMenu);

            _cfg.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_cfg.SubMenu("Orbwalking"));

            _cfg.AddSubMenu(new Menu("Combo", "Combo"));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("QCombo", "Use Q")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("WCombo", "Use W")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("ECombo", "Use E")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("RCombo", "Use R")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseItems", "Use Items")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("HPW", "Min Enemy HP% To use W")).SetValue(new Slider(30, 0, 100));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("AutoR", "Auto Use R")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("CountR", "Num of Enemy in Range to Ult").SetValue(new Slider(3, 5, 0)));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            _cfg.AddSubMenu(new Menu("Harass", "Harass"));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarassW", "Use W in Harass")).SetValue(true);
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarassE", "Use E in Harass")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("HPWHarras", "Min Enemy HP% To use W")).SetValue(new Slider(100, 0, 100));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind(97, KeyBindType.Press)));

            _cfg.AddSubMenu(new Menu("LaneClear", "LaneClear"));
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("ActiveLane", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseQLane", "Use Q")).SetValue(false);
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseWLane", "Use W")).SetValue(false);
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseELane", "Use E")).SetValue(true);

            _cfg.AddSubMenu(new Menu("JungleClear", "JungleClear"));
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("ActiveJungle", "JungleClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseQJungle", "Use Q")).SetValue(true);
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseWJungle", "Use W")).SetValue(true);
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseEJungle", "Use E")).SetValue(true);

            _cfg.AddSubMenu(new Menu("KillSteal", "KillSteal"));
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("SmartKS", "Smart Killsteal")).SetValue(true);

            _cfg.AddSubMenu(new Menu("Flee", "Flee"));
            _cfg.SubMenu("Flee").AddItem(new MenuItem("ActiveFlee", "Flee!").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Press)));

            _cfg.AddSubMenu(new Menu("Drawings", "Drawings"));
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Draw R")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            _cfg.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_cfg.Item("CircleLag").GetValue<bool>())
            {
                if (_cfg.Item("DrawQ").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _q.Range, System.Drawing.Color.BlueViolet,
                        _cfg.Item("CircleThickness").GetValue<Slider>().Value);
                }
                if (_cfg.Item("DrawW").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _w.Range, System.Drawing.Color.Blue,
                        _cfg.Item("CircleThickness").GetValue<Slider>().Value);
                }
                if (_cfg.Item("DrawE").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.DarkGoldenrod,
                        _cfg.Item("CircleThickness").GetValue<Slider>().Value);
                }
                if (_cfg.Item("DrawR").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _r.Range, System.Drawing.Color.DarkMagenta,
                        _cfg.Item("CircleThickness").GetValue<Slider>().Value);
                }
            }
            else
            {
                if (_cfg.Item("DrawQ").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _q.Range, System.Drawing.Color.BlueViolet);
                }
                if (_cfg.Item("DrawW").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _w.Range, System.Drawing.Color.Blue);
                }
                if (_cfg.Item("DrawE").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.DarkGoldenrod);
                }
                if (_cfg.Item("DrawR").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _r.Range, System.Drawing.Color.DarkMagenta);
                }

            }  
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (_player.IsDead)
            {
                return;
            }
            if (_cfg.Item("SmartKS").IsActive())
            {
                SmartKs();
            }

            if (_cfg.Item("ActiveCombo").IsActive())
            {
                Combo();
            }

            if (_cfg.Item("ActiveHarass").IsActive())
            {
                Harass();
            }

            if (_cfg.Item("ActiveJungle").IsActive())
            {
                Jungleclear();
            }

            if (_cfg.Item("ActiveLane").IsActive())
            {
                Laneclear();
            }

            if (_cfg.Item("ActiveFlee").IsActive())
            {
                Flee();
            }
        }

        private static void Flee()
        {
            _player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            if (_q.IsReady())
            {
                _q.Cast();
            }
        }

        private static void Laneclear()
        {
            var minion = MinionManager.GetMinions(_player.ServerPosition, _w.Range);

            if (minion.Count < 2)
                return;

            if (_cfg.Item("UseQLane").IsActive() && _q.IsReady())
            {
                _q.Cast();
            }

            if (_cfg.Item("UseWLane").IsActive() && _w.IsReady())
            {
                _w.Cast(minion[0]);
            }

            if (_cfg.Item("UseELane").IsActive() && _e.IsReady())
            {
                _e.Cast();
            }
        }

        private static void Jungleclear()
        {
            var junglemonster = MinionManager.GetMinions(_player.ServerPosition, _w.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (junglemonster.Count == 0) return;

            if (_cfg.Item("UseQJungle").IsActive() && _q.IsReady())
            {
                _q.Cast();
            }

            if (_cfg.Item("UseWJungle").IsActive() && _w.IsReady())
            {
                _w.Cast(junglemonster[0]);
            }

            if (_cfg.Item("UseEJungle").IsActive() && _e.IsReady())
            {
                _e.Cast();
            }
        }

        private static void Harass()
        {

            var t = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (t == null || !t.IsValidTarget()) return;

            var health = t.Health;
            var maxhealth = t.MaxHealth;
            float wcount = _cfg.Item("CountWHarass").GetValue<Slider>().Value;

            if (_cfg.Item("HarassE").IsActive() && _player.Distance(t) <= _e.Range && _e.IsReady())
            {
                _e.Cast();
            }

            if (!(health < ((maxhealth*wcount)/100))) return;
            if (_cfg.Item("HarassW").IsActive() && _player.Distance(t) <= _w.Range && _w.IsReady())
            {
                _w.Cast(t);
            }
        }

        private static void SmartKs()
        {
            foreach (
                var t in ObjectManager.Get<Obj_AI_Hero>().Where(t => t.IsEnemy).Where(t => t.IsValidTarget(_w.Range)))
            {
                if (t.Health <= _w.GetDamage(t) && _w.IsReady() && _player.Distance(t) <= _w.Range && t.IsValidTarget())
                {
                    _w.Cast(t);
                }

                if (t.Health <= _e.GetDamage(t) && _e.IsReady() && _player.Distance(t) <= _e.Range && t.IsValidTarget())
                {
                    _e.Cast();
                }

                else if (t.Health <= (_e.GetDamage(t) + _w.GetDamage(t)) && _e.IsReady() && _w.IsReady() && t.IsValidTarget())
                {
                    _w.Cast(t);
                    _e.Cast();
                }
            }
        }
        
        private static void Combo()
        {
            var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Physical);
            if (t == null) return;

            if (_player.Distance(t) <= _q.Range && _q.IsReady() && _cfg.Item("QCombo").IsActive() && t.IsValidTarget())
            {
                _q.Cast();
            }

            if (_player.Distance(t) <= _e.Range && _e.IsReady() && _cfg.Item("ECombo").IsActive() && t.IsValidTarget())
            {
                _e.Cast();
            }

            var health = t.Health;
            var maxhealth = t.MaxHealth;

            float wcount = _cfg.Item("HPW").GetValue<Slider>().Value;

            if (health < ((maxhealth * wcount) / 100))
            {
                if (_cfg.Item("WCombo").IsActive() && _w.IsReady() && _player.Distance(t) <= _w.Range && t.IsValidTarget())
                {
                    _w.Cast(t);
                }
            }

            if (_cfg.Item("AutoR").IsActive() && _player.CountEnemiesInRange(_r.Range) >= _cfg.Item("CountR").GetValue<Slider>().Value && _r.IsReady() && t.IsValidTarget())
            {
                _r.Cast();
            }

            if (!_cfg.Item("UseItems").IsActive() || !t.IsValidTarget())
            {
                return;
            }
            if (_player.Distance(t) <= _rdo.Range && _rdo.IsReady() && _rdo.IsOwned())
            {
                _rdo.Cast();
            }

            if (_player.Distance(t) <= _hyd.Range && _hyd.IsReady() && _hyd.IsOwned())
            {
                _hyd.Cast();
            }

            if (_player.Distance(t) <= _botk.Range && _botk.IsReady() && _botk.IsOwned())
            {
                _botk.Cast(t);
                
            }

            if (_player.Distance(t) <= _cut.Range && _cut.IsReady() && _cut.IsOwned())
            {
                _cut.Cast(t);
            }

            if (_player.Distance(t) <= 125f && _yoy.IsReady() && _yoy.IsOwned())
            {
                _yoy.Cast();
            }

            if (_player.Distance(t) <= 600f && _rg.IsReady() && _rg.IsOwned())
            {
                _rg.Cast();
            }
        }
    }
}