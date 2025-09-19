using Godot;
using static Godot.GD;
using System;
using System.Transactions;
using Asteroids.Scripts.Components;
using Asteroids.Scripts.Interfaces;

namespace Asteroids.Scripts.Entities {
    public partial class Player : Ship, IControllable {
        
        private Node2D _pivot;
        private Timer _shieldTimer;
        private Timer _shootCooldownTimer;
        private AudioStreamPlayer _hitSound;
        private bool _boosting;
        private bool _canShoot = true;
        private bool _shielded;
        private Bullet _bulletFactory = new ();

        [Export] public Vector2 BaseVelocity;
        [Export] public float MaxVelocity = 300.0f; 
        [Export] public float MinVelocity = 2.0f;                
        [Export] public new float Speed = 300.0f;                
        [Export] public float RotationSpeed = (float)(Math.PI/180.0f * 180.0f);
        [Export] public int SlowRange = 50;
        [Export] public Color ShieldModulateColor = Color.Color8(0, 255, 255);

        public override void _Ready() {
            Wrappable = true;
            
            _pivot = GetNode<Node2D>("Pivot");
            
            _shieldTimer = GetNode<Timer>("ShieldTimer");
            _shieldTimer.Timeout += OnShieldTimerTimeout;
            
            _shootCooldownTimer = GetNode<Timer>("ShootCooldownTimer");
            _shootCooldownTimer.Timeout += OnShootTimerTimeout;
            
            _hitSound = GetNode<AudioStreamPlayer>("HitSound");
            
            Health = MaxHealth;
            Velocity = BaseVelocity;
            
            base._Ready();
        }

        public override void _Process(double delta) {
            ClampVelocity();
            Animate(delta);
            base._Process(delta);
        }

        public void Boost(double delta) {
            _boosting = true;
            Velocity += GetForward() * (Speed * (float)delta);
        }

        public void Slow(double delta) {
            _boosting = true;
            if (Velocity.Length() <= 0.0f) return;
            var decel = Speed * (float)delta;
            var v = Velocity;
            var newLen = v.Length() - decel;
            Velocity = (newLen > 0f) ? v.Normalized() * newLen : Vector2.Zero;
        }

        public void Left(double delta) {
            _pivot.Rotation -= RotationSpeed * (float)delta;
        }

        public void Right(double delta) {
            _pivot.Rotation += RotationSpeed * (float)delta;
        }

        public override void Shoot() {
            if (_shootCooldownTimer.IsStopped() == false) return;
            _canShoot = false;
            var bullet = (Bullet)_bulletFactory.Spawn();
            bullet.GlobalPosition = GlobalPosition;
            bullet.Rotation = _pivot.Rotation;
            bullet.Target = Bullet.BulletType.Enemy;
            GetTree().CurrentScene.AddChild(bullet);
            _shootCooldownTimer.Start();
            base.Shoot();
        }
        
        public void ActivateShield() {
            Print("ACTIVATE SHIELD");
            _shieldTimer.Stop();
            _shielded = true;
            _shieldTimer.Start();
            ShipSprite.SetModulate(ShieldModulateColor);
        }
        
        private void DisableShield() {
            _shielded = false;
            ShipSprite.SetModulate(DefaultModulateColor);
        }

        protected override void OnAreaEntered(Area2D area) {
            if (area.GetParent().IsInGroup("Asteroids") || 
                (area.GetParent() is Bullet && ((Bullet)area.GetParent()).Target == Bullet.BulletType.Player)) {
                if (!_shielded) {
                    _hitSound.Play();
                    Hit();
                }
            }
            if (area.GetParent().IsInGroup("Powerups")) {
                var powerup = (Powerup)area.GetParent();
                powerup.ApplyEffect(this);
                powerup.QueueFree();
            }
        }

        private Vector2 GetForward() => -_pivot.Transform.Y.Normalized();

        private void ClampVelocity() {
            var maxSpeed = MaxVelocity;
            var len = Velocity.Length();

            if (len > maxSpeed) {
                Velocity = Velocity.Normalized() * maxSpeed;
            }

            if (len < MinVelocity) {
                Velocity = Vector2.Zero;
            }
        }

        private void Animate(double delta) {
            if (BoostEffect == null) return;
            BoostEffect.Animation = _boosting ? "boost" : "idle";
            _boosting = false;
        }
        
        private void OnShieldTimerTimeout() {
            DisableShield();
        }

        private void OnShootTimerTimeout() {
            _canShoot = true;
        }
    }
}
