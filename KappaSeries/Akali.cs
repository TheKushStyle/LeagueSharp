using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace KappaSeries
{
    internal class Akali
    {
        public Akali()
        {
            CustomEvents.Game.OnGameLoad += Load;
        }

        private static Orbwalking.Orbwalker _orbwalker;
        public static readonly List<Spell> SpellList = new List<Spell>();
        private static Spell _q;
        private static Spell _w;
        private static Spell _e;
        private static Spell _r;
        private static SpellSlot IgniteSlot;
        private static Menu _cfg;
        private static Obj_AI_Hero _player;
        private static bool QRKill = false;
        private static bool ERKill = false;
        private static bool IRKill = false;

        private static void Load(EventArgs args)
        {
            _player = ObjectManager.Player;

            _q = new Spell(SpellSlot.Q, 600);
            _w = new Spell(SpellSlot.W, 700);
            _e = new Spell(SpellSlot.E, 325);
            _r = new Spell(SpellSlot.R, 700);

            SpellList.Add(_q);
            SpellList.Add(_w);
            SpellList.Add(_e);
            SpellList.Add(_r);

            IgniteSlot = _player.GetSpellSlot("SummonerDot");

            _cfg = new Menu("Akali", "Ahri", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            _cfg.AddSubMenu(targetSelectorMenu);

            _cfg.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_cfg.SubMenu("Orbwalking"));

            _cfg.AddSubMenu(new Menu("Combo", "Combo"));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("ComboMode", "Combo Mode").SetValue(new StringList(new[] { "RQWE", "QRWE" }, 1)));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseItems", "Use Items")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseQ", "Use Q")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseW", "Use W After R")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseE", "Use E")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("UseR", "Use R")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("Qaa", "Force AA On Q buff").SetValue(true));
            _cfg.SubMenu("Combo").AddItem(new MenuItem("RChase", "Use R to Chase Target")).SetValue(true);
            _cfg.SubMenu("Combo").AddItem(new MenuItem("RChaseRange", "Range To Chase target with R").SetValue(new Slider(600, 0, 700)));

            _cfg.AddSubMenu(new Menu("Harass", "Harass"));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind(97, KeyBindType.Press)));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarQ", "Use Q In Harass").SetValue(true));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("AutoHarQ", "Auto Use Q In Harass").SetValue(false));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("HarE", "Use E In Harass").SetValue(true));
            _cfg.SubMenu("Harass").AddItem(new MenuItem("AutoHarE", "Auto Use Q In Harass").SetValue(false));

            _cfg.AddSubMenu(new Menu("LastHit", "LastHit"));
            _cfg.SubMenu("LastHit").AddItem(new MenuItem("ActiveFarm", "LastHit!").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("LastHit").AddItem(new MenuItem("UseQFarm", "Use Q to lasthit")).SetValue(true);
            _cfg.SubMenu("LastHit").AddItem(new MenuItem("UseEFarm", "Use E to lasthit")).SetValue(true);
            _cfg.SubMenu("LastHit").AddItem(new MenuItem("AutoUseQLane", "Auto Use Q to lasthit")).SetValue(false);
            _cfg.SubMenu("LastHit").AddItem(new MenuItem("AutoUseELane", "Auto Use E to lasthit")).SetValue(false);

            _cfg.AddSubMenu(new Menu("LaneClear", "LaneClear"));
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("ActiveLane", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseQLane", "Use Q")).SetValue(true);
            _cfg.SubMenu("LaneClear").AddItem(new MenuItem("UseELane", "Use E")).SetValue(true);

            _cfg.AddSubMenu(new Menu("JungleClear", "JungleClear"));
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("ActiveJungle", "JungleClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseQJungle", "Use Q")).SetValue(true);
            _cfg.SubMenu("JungleClear").AddItem(new MenuItem("UseEJungle", "Use E")).SetValue(true);

            _cfg.AddSubMenu(new Menu("KillSteal", "KillSteal"));
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("SmartKS", "Smart KillSteal")).SetValue(true);
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("RKS", "R KillSteal")).SetValue(true);
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("RKSMin", "R to minions to KillSteal")).SetValue(true);
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("RKSminHP", "Min HP % to KS with R ").SetValue(new Slider(30, 0, 100)));
            _cfg.SubMenu("KillSteal").AddItem(new MenuItem("AutoIgnite", "Auto Ignite")).SetValue(true);

            _cfg.AddSubMenu(new Menu("Drawings", "Drawings"));
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Draw R Range")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("LagFree", "Lag Free Cirlces")).SetValue(true);
            _cfg.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 1, 10)));

            _cfg.AddSubMenu(new Menu("Auto W Settings", "Wset"));
            _cfg.SubMenu("Misc").AddItem(new MenuItem("AutoWGap", "Auto W on Gapcloser")).SetValue(true);
            _cfg.SubMenu("Wset").AddItem(new MenuItem("AutoW", "Auto Use W")).SetValue(false);
            _cfg.SubMenu("Wset").AddItem(new MenuItem("WCount", "Auto W if Enemies in Range").SetValue(new Slider(2, 0, 5)));
            _cfg.SubMenu("Wset").AddItem(new MenuItem("WRange", "Auto W Range").SetValue(new Slider(250, 0, 1000)));
            _cfg.SubMenu("Wset").AddItem(new MenuItem("WHP", "Auto W % HP").SetValue(new Slider(20, 0, 100)));

            _cfg.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;
            AntiGapcloser.OnEnemyGapcloser += OnGapCloser;
            Drawing.OnDraw += Drawing_OnDraw;

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_cfg.Item("LagFree").GetValue<bool>())
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
        
        private static void OnGapCloser(ActiveGapcloser gapcloser)
        {
            if (_cfg.Item("AutoWGap").IsActive())
            {
                _w.Cast(_player.ServerPosition);
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
                Smartks();
            }
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit)
            {
                Lasthit();
            }

            if (_cfg.Item("AutoW").IsActive())
            {
                AutoW();
            }

            if (_cfg.Item("AutoHarQ").IsActive())
            {
                AutoHarassQ();
            }

            if (_cfg.Item("AutoHarE").IsActive())
            {
                AutoHarassE();
            }

            if (_cfg.Item("AutoUseQLane").IsActive())
            {
                AutoLaneQ();
            }

            if (_cfg.Item("AutoUseELane").IsActive())
            {
                AutoLaneE();
            }

            if (_cfg.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
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
        }

        private static void Lasthit()
        {
            var minion = MinionManager.GetMinions(_player.ServerPosition, _q.Range);
            var qdmg = _q.GetDamage(minion[0]);
            var edmg = _e.GetDamage(minion[0]);
            if (minion[0] == null) return;

            if (_cfg.Item("UseQFarm").IsActive() && minion[0].Distance(_player) <= _q.Range && minion[0].Health <= qdmg && _q.IsReady())
            {
                _q.Cast(minion[0]);
            }
            if (_cfg.Item("UseEFarm").IsActive() && minion[0].Distance(_player) <= _e.Range && minion[0].Health <= edmg && _e.IsReady())
            {
                _e.Cast();
            }
        }

        private static void Laneclear()
        {
            var minion = MinionManager.GetMinions(_player.ServerPosition, _q.Range);
            if (minion[0] == null) return;

            if (minion.Count < 2)
                return;

            if (_cfg.Item("UseQLane").IsActive() && _q.IsReady() && _player.Distance(minion[0]) <= _q.Range)
            {
                _q.CastOnUnit(minion[0]);
            }

            if (_cfg.Item("UseELane").IsActive() && _e.IsReady() && _player.Distance(minion[0]) <= _e.Range)
            {
                _e.Cast();
            }
        }

        private static void Jungleclear()
        {
            var junglemonster = MinionManager.GetMinions(_player.ServerPosition, _q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (junglemonster.Count == 0 || junglemonster[0] == null) return;

            if (_cfg.Item("UseQJungle").IsActive() && _q.IsReady() && _player.Distance(junglemonster[0]) <= _q.Range)
            {
                _q.CastOnUnit(junglemonster[0]);
            }
            if (_cfg.Item("UseEJungle").IsActive() && _e.IsReady() && _player.Distance(junglemonster[0]) <= _e.Range)
            {
                _e.Cast();
            }

        }

        private static void Harass()
        {
            var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);
            if (!t.IsValidTarget() || t == null || t.IsDead) return;
            if (_q.IsReady() && _player.Distance(t) <= _q.Range && _cfg.Item("HarQ").IsActive())
            {
                _q.CastOnUnit(t);
            }
            if (_e.IsReady() && _player.Distance(t) <= _e.Range && _cfg.Item("HarE").IsActive())
            {
                _e.Cast();
            }
        }

        private static void Combo()
        {
            var t = TargetSelector.GetTarget(_r.Range, TargetSelector.DamageType.Magical);
            if (!t.IsValidTarget() || t == null) return;
            #region items

            if (_cfg.Item("UseItems").IsActive())
            {
            UseItems(t);
            }

            #endregion

            if (_cfg.Item("ComboMode").GetValue<StringList>().SelectedValue == "QRWE")
            {
                UseQ(t);
               if (_cfg.Item("Qaa").IsActive() && _player.AttackRange <= t.Distance(_player) && t.HasBuff("AkaliMota"))
                {
                    return;
                }
                UseRChase(t);
                UseR(t);
                UseE(t);
            }
            if (_cfg.Item("ComboMode").GetValue<StringList>().SelectedValue == "RQWE")
            {
                UseRChase(t);
                UseR(t);
                UseQ(t);
                if (_cfg.Item("Qaa").IsActive() && _player.AttackRange <= t.Distance(_player) && t.HasBuff("AkaliMota"))
                {
                    return;
                }
                UseE(t);
            }
            
        }

        private static void UseR(Obj_AI_Hero t)
        {
            if (_cfg.Item("UseR").IsActive() && _r.IsReady() && t.Distance(_player) <= _r.Range)
            {
                Console.WriteLine("Casting R");
                if (_cfg.Item("UseW").IsActive() && _w.IsReady())
                {
                    _r.Cast(t);
                    Utility.DelayAction.Add(500, () => _w.Cast());
                }
                else
                {
                    _r.Cast(t);
                }
            }
        }

        private static void UseRChase(Obj_AI_Hero t)
        {
            if (_cfg.Item("UseR").IsActive() && _r.IsReady() && _cfg.Item("RChase").IsActive())
            {
                Console.WriteLine("RChase");
                if (_cfg.Item("UseW").IsActive() && _w.IsReady())
                {
                    ChaseR(t);
                    Utility.DelayAction.Add(500, () => _w.Cast());
                }
                else
                {
                    ChaseR(t);
                    return;
                }
            }
        }

        private static void UseE(Obj_AI_Hero tar)
        {
            var t = TargetSelector.GetTarget(_r.Range, TargetSelector.DamageType.Magical);
            if (_cfg.Item("UseE").IsActive() && !_q.IsReady() && _player.Distance(t) <= _e.Range)
            {
                Console.WriteLine("Casting E");
                _e.Cast();
            }
        }

        private static void UseQ(Obj_AI_Hero t)
        {
            if (_cfg.Item("UseQ").IsActive() && _q.IsReady() && (_player.Distance(t) <= _q.Range))
            {
                Console.WriteLine("Casting Q");
                _q.Cast(t);
                if (t.Distance(_player) <= _player.AttackRange && t.HasBuff("AkaliMota"))
                {
                    _player.IssueOrder(GameObjectOrder.AttackUnit, (t));
                }
            }

        }

        private static void ChaseR(Obj_AI_Hero target)
        {
            if (target != null && _player.Distance(target) >= _cfg.Item("RChaseRange").GetValue<Slider>().Value &&
                _r.IsReady())
            {
                _r.Cast(target);
            }
        }

        private static void UseItems(Obj_AI_Hero target)
        {
            var hextech = LeagueSharp.Common.Data.ItemData.Hextech_Gunblade;
            var bilge = LeagueSharp.Common.Data.ItemData.Bilgewater_Cutlass;
            
            if (target != null && !target.IsDead && target.IsValidTarget())
            {
                if (Items.HasItem(hextech.Id) && Items.CanUseItem(hextech.Id) && _player.Distance(target) <= hextech.Range)
                {
                    Items.UseItem(hextech.Id,target);
                }
                
                if (Items.HasItem(bilge.Id) && Items.CanUseItem(bilge.Id) && _player.Distance(target) <= bilge.Range)
                {
                    Items.UseItem(bilge.Id, target);
                }

            }
            
        }

        private static void AutoLaneE()
        {
            foreach(var minion in ObjectManager.Get<Obj_AI_Minion>().Where(minion => minion.IsValidTarget(_e.Range) && _e.IsReady() && _e.GetDamage(minion) >= minion.Health))
            {
                _e.Cast();
            }
        }

        private static void AutoLaneQ()
        {
            foreach (var minion in ObjectManager.Get<Obj_AI_Minion>().Where(minion => minion.IsValidTarget(_q.Range) && _q.IsReady() && _q.GetDamage(minion) >= minion.Health))
            {
                _q.CastOnUnit(minion);
            }
        }

        private static void AutoHarassE()
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(t => t.IsEnemy).Where(t => t.IsValidTarget(_e.Range)).Where(enemy => enemy.IsValidTarget(_e.Range)).Where(enemy => _player.Distance(enemy) <= _e.Range && _e.IsReady()))
            {
                _e.Cast();
            }
        }

        private static void AutoHarassQ()
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(t => t.IsEnemy).Where(t => t.IsValidTarget(_q.Range)).Where(enemy => enemy.IsValidTarget(_q.Range)).Where(enemy => _player.Distance(enemy) <= _q.Range && _q.IsReady()))
            {
                _q.CastOnUnit(enemy);
            }
        }

        private static void AutoW()
        {
            var wRange = _cfg.Item("WRange").GetValue<Slider>();
            var wEnemy = _cfg.Item("WCount").GetValue<Slider>();
            var wHP = _cfg.Item("WHP").GetValue<Slider>();
            var myHP = _player.Health;
            var myMaxHP = _player.MaxHealth;

            if (_player.CountEnemiesInRange(wRange.Value) >= wEnemy.Value)
            {
                _w.Cast(_player.ServerPosition);
            }
            else if (((myHP / myMaxHP) * 100) <= wHP.Value)
            {
                _w.Cast(_player.ServerPosition);
            }
        }

        private static void Smartks()
        {
            foreach (
                var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(t => t.IsEnemy).Where(t => t.IsValidTarget(_r.Range)))
            {
                var idmg = _player.GetSummonerSpellDamage(enemy, Damage.SummonerSpell.Ignite);
                var qdmg = _q.GetDamage(enemy);
                var edmg = _e.GetDamage(enemy);
                var rdmg = _r.GetDamage(enemy);

                if (enemy.IsValidTarget(_r.Range) && !enemy.IsDead)
                {
                    #region Q
                    if (enemy.Health <= qdmg && enemy.IsValidTarget(_q.Range) && _q.IsReady())
                    {
                        _q.CastOnUnit(enemy);
                    }
                    #endregion Q
                    #region QE
                    else if (enemy.Health <= qdmg + edmg && enemy.IsValidTarget(_e.Range) && _q.IsReady() && _e.IsReady())
                    {
                        _q.CastOnUnit(enemy);
                        _e.Cast();
                    }
                    #endregion
                    #region RQ
                    else if (enemy.Health <= (qdmg + rdmg) && enemy.IsValidTarget(_r.Range) && _q.IsReady() && _r.IsReady() && _cfg.Item("RKS").IsActive())
                    {
                        _r.CastOnUnit(enemy);
                        _q.CastOnUnit(enemy);
                    }
                    #endregion
                    #region RQE
                    else if (enemy.Health <= (qdmg + rdmg + edmg) && enemy.IsValidTarget(_r.Range) && _q.IsReady() && _e.IsReady() && _r.IsReady() &&_cfg.Item("RKS").IsActive())
                    {
                        _r.CastOnUnit(enemy);
                        _q.CastOnUnit(enemy);
                        _e.Cast();
                    }
                    #endregion
                    #region QRMIN 
                    
                    else if (enemy.Health <= (qdmg + rdmg) && enemy.IsValidTarget(_q.Range + _r.Range) &&
                             enemy.Distance(_player) > _r.Range && _q.IsReady() && _r.IsReady() &&
                             _cfg.Item("RKSMin").IsActive() && _cfg.Item("RKS").IsActive())
                    {
                        KillStealR(enemy);
                        QRKill = true;
                        ERKill = false;
                        IRKill = false;
                    }
                   
                    #endregion
                    #region ERMIN
                    
                    else if (enemy.Health <= (edmg + rdmg) && enemy.IsValidTarget(_r.Range + _e.Range) && enemy.Distance(_player) > _r.Range && _e.IsReady() && _r.IsReady() &&
                             _cfg.Item("RKSMin").IsActive() && _cfg.Item("RKS").IsActive())
                    {
                        KillStealR(enemy);
                        QRKill = false;
                        ERKill = true;
                        IRKill = false;
                    }
                    #endregion
                    
                    #region IginteRMIN
                   
                   else if (enemy.Health <= (idmg + rdmg) && enemy.IsValidTarget(_r.Range + 600f) && enemy.Distance(_player) > _r.Range && IgniteSlot.IsReady() && _r.IsReady() &&
                             _cfg.Item("RKSMin").IsActive() && _cfg.Item("RKS").IsActive())
                   {
                       KillStealR(enemy);
                       QRKill = false;
                       ERKill = false;
                       IRKill = true;
                   }
                   
                    #endregion
                
                    #region QIgnite

                   else if (enemy.Health <= (qdmg + idmg) && enemy.IsValidTarget(_q.Range) && _q.IsReady() && IgniteSlot.IsReady())
                   {
                       _q.CastOnUnit(enemy);
                       _player.Spellbook.CastSpell(IgniteSlot, enemy);
                   }
                    #endregion
                    #region E
                    else if (enemy.Health <= edmg && enemy.IsValidTarget(_e.Range) && _e.IsReady())
                    {
                        _e.Cast();
                    }
                    #endregion
                    #region RE
                    else if (enemy.Health <= (edmg + rdmg) && enemy.IsValidTarget(_r.Range) && _r.IsReady() && _e.IsReady() && _cfg.Item("RKS").IsActive())
                    {
                        _r.CastOnUnit(enemy);
                        _e.Cast(enemy);
                    }
                    #endregion
                    #region EIgnite
                    else if (enemy.Health <= (edmg + idmg) && enemy.IsValidTarget(_e.Range) && _e.IsReady() && IgniteSlot.IsReady())
                    {
                        _e.Cast();
                        _player.Spellbook.CastSpell(IgniteSlot, enemy);
                    }
                    #endregion
                    #region R
                    else if (enemy.Health <= rdmg && enemy.IsValidTarget(_r.Range) && _r.IsReady() && _cfg.Item("RKS").IsActive())
                    {
                        _r.CastOnUnit(enemy);
                    }
                    #endregion
                    #region R Ignite
                    else if (enemy.Health <= (rdmg + idmg) && enemy.IsValidTarget(_r.Range) && _r.IsReady() && IgniteSlot.IsReady() && _cfg.Item("RKS").IsActive())
                    {
                        _r.CastOnUnit(enemy);
                        _player.Spellbook.CastSpell(IgniteSlot, enemy);
                    }
                    #endregion
                    #region QERIgnite
                    else if (enemy.Health <= (qdmg + edmg +rdmg + idmg) && enemy.IsValidTarget(_r.Range) && _q.IsReady() && _e.IsReady() && _r.IsReady() && _cfg.Item("RKS").IsActive())
                    {
                        _r.CastOnUnit(enemy);
                        _q.CastOnUnit(enemy);
                        _e.Cast(enemy);
                        _player.Spellbook.CastSpell(IgniteSlot, enemy);
                    }
                    #endregion
                    #region REQ

                    else if(enemy.Health <= (qdmg + edmg + rdmg) && enemy.IsValidTarget(_r.Range) && _q.IsReady() && _e.IsReady() && _r.IsReady() && _cfg.Item("RKS").IsActive())
                    {
                        _r.CastOnUnit(enemy);
                        _e.Cast(enemy);
                        _q.CastOnUnit(enemy);
                    }
                    #endregion
                    #region AutoIgnite

                    if (_cfg.Item("AutoIgnite").IsActive())
                    {
                        AutoIgnite(enemy);
                    }
                    #endregion
                }
            }
        }

        private static void KillStealR(Obj_AI_Hero enemy)
        {
            var minion = MinionManager.GetMinions(_player.ServerPosition, _w.Range);

            if (((_player.Health / _player.MaxHealth) * 100) >= _cfg.Item("RKSminHP").GetValue<Slider>().Value)
            {
                if (QRKill)
                {
                    if (minion[0].Distance(enemy) <= _q.Range && minion[0].Distance(_player) <= _r.Range)
                    {
                        _r.CastOnUnit(minion[0]);
                        Utility.DelayAction.Add(500,() => _q.Cast(enemy));
                    }
                }
                if (ERKill)
                {
                    if (minion[0].Distance(enemy) <= _e.Range && minion[0].Distance(_player) <= _r.Range)
                    {
                        _r.CastOnUnit(minion[0]);
                        Utility.DelayAction.Add(500, () => _e.Cast(enemy));
                    }
                }
                if (IRKill)
                {
                    if (minion[0].Distance(enemy) <= 600f && minion[0].Distance(_player) <= _r.Range)
                    {
                        _r.CastOnUnit(minion[0]);
                        Utility.DelayAction.Add(500, () => _player.Spellbook.CastSpell(IgniteSlot, enemy));
                    }
                }
            }
        }

        private static void AutoIgnite(Obj_AI_Hero enemy)
        {
            if (enemy.IsValidTarget(600f) && enemy.Health <= 50 + (20*_player.Level) && IgniteSlot.IsReady())
            {
                _player.Spellbook.CastSpell(IgniteSlot, enemy);
            }
        }
        
    }
}
