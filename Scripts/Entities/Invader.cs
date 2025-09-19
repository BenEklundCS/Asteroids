using Godot;
using static Godot.GD;
using System;
using Asteroids.Scripts.Interfaces;

namespace Asteroids.Scripts.Entities {
    public partial class Invader : Ship, ISpawnable {

        private Timer _shootTimer;
        private Timer _moveDurationTimer;
        private Bullet _bulletFactory = new ();
        
        public override void _Ready() {
            AddToGroup("Invaders");
            Wrappable = true;
            Value = 10;
            
            _shootTimer = GetNode<Timer>("ShootTimer");
            _moveDurationTimer = GetNode<Timer>("MoveDurationTimer");
            
            _shootTimer.Timeout += OnShootTimerTimeout;
            _moveDurationTimer.Timeout += OnMoveDurationTimerTimeout;
            
            SeekRandom();
            
            base._Ready();
        }
        
        public Object Spawn() {
            return (Invader)Load<PackedScene>("res://Scenes/Entities/invader.tscn").Instantiate();
        }
        
        public override void Shoot() {
            var bullet = (Bullet)_bulletFactory.Spawn();
            bullet.GlobalPosition = GlobalPosition;
            bullet.Rotation = GlobalPosition
                .DirectionTo(
                    GetTree()
                        .CurrentScene
                        .GetNode<Player>("Controller/Player")
                        .GlobalPosition)
                .Angle() + Mathf.Pi / 2;
            bullet.Target = Bullet.BulletType.Player;
            GetTree().CurrentScene.AddChild(bullet);
            base.Shoot();
        }
        
        protected override void OnAreaEntered(Area2D area) {
            if (area.GetParent() is Bullet && ((Bullet)area.GetParent()).Target == Bullet.BulletType.Enemy) {
                Hit();
            }
        }
        
        private void OnShootTimerTimeout() {
            Shoot();
        }
        
        private void OnMoveDurationTimerTimeout() {
            Velocity = Vector2.Zero;
        }
    }
}
