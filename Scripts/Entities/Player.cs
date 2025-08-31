using Godot;
using static Godot.GD;
using System;
using Asteroids.Scripts.Components;
using Asteroids.Scripts.Interfaces;

namespace Asteroids.Scripts.Entities {
    public partial class Player : Object, IControllable {
        [Signal] public delegate void OnDeathEventHandler();

        [Signal] public delegate void OnHitEventHandler(int health);
        
        private AnimatedSprite2D _boostEffect;
        private Sprite2D _ship;
        private Area2D _hitBox;
        private Node2D _pivot;
        private AudioStreamPlayer _shootSound;
        private Timer _flashTimer;
        private bool _hit;
        private int _flashedTimes = 0;
        private bool _boosting;
        private int _health;
        
        [Export] public Vector2 BaseVelocity;
        [Export] public Vector2 MaxVelocity = new(300.0f, 300.0f);
        [Export] public float MinVelocity = 5.0f;
        [Export] public float Speed = 5.0f;
        [Export] public float RotationSpeed = (float)(Math.PI/180.0f * 2);
        [Export] public int SlowRange = 50;
        [Export] public int MaxHealth = 3;
        [Export] public int FlashTimes = 6;
        [Export] public Color OnHitModulateColor = Color.Color8(255, 0, 0);
        [Export] public Color DefaultModulateColor = Color.Color8(255, 255, 255);

        public override void _Ready() {
            _boostEffect = GetNode<AnimatedSprite2D>("Pivot/BoostEffect");
            _ship = GetNode<Sprite2D>("Pivot/Ship");
            _hitBox = GetNode<Area2D>("Pivot/Hitbox");
            _hitBox.AreaEntered += OnAreaEntered;
            _pivot = GetNode<Node2D>("Pivot");
            _shootSound = GetNode<AudioStreamPlayer>("ShootSound");
            _boostEffect.Play();
            _flashTimer = GetNode<Timer>("FlashTimer");
            _flashTimer.Timeout += OnFlashTimerTimeout;
            _health = MaxHealth;
            Velocity = BaseVelocity;
        }
        
        public override void _Process(double delta) {
            ClampVelocity();
            Animate(delta);
            base._Process(delta);
        }

        public void Boost() {
            _boosting = true;
            Velocity += GetForward() * Speed;
        }

        public void Slow() {
            _boosting = true;
            if (Velocity.Length() > 0) {
                Velocity += -(Velocity.Normalized() * Speed);
            }
        }

        public void Left() {
            _pivot.Rotation -= RotationSpeed;
        }

        public void Right() {
            _pivot.Rotation += RotationSpeed;
        }
        
        public void Shoot() {
            var bullet = (Bullet)Load<PackedScene>("res://Scenes/Entities/Bullet.tscn").Instantiate();
            bullet.GlobalPosition = GlobalPosition;
            bullet.Rotation = _pivot.Rotation;
            GetTree().Root.AddChild(bullet);
            _shootSound.Play();
        }
        
        private Vector2 GetForward() {
            return -_pivot.Transform.Y.Normalized();
        }

        private void ClampVelocity() {
            if (Velocity.Length() > MaxVelocity.Length())
                Velocity = (Velocity.Normalized() * MaxVelocity);
            if (Velocity.Length() < MinVelocity) {
                Velocity = Vector2.Zero;
            }
        }

        private void Animate(double delta) {
            _boostEffect.Animation = _boosting  ? "boost" : "idle";
            _boosting = false;
        }
        
        private void OnFlashTimerTimeout() {
            if (!_hit) {
                return;
            }
            
            _flashedTimes++;
            if (_flashedTimes > FlashTimes) {
                ResetHit();
            }
            
            var color = (_flashedTimes % 2 != 0) 
                ? OnHitModulateColor 
                : DefaultModulateColor;
            _ship.SetModulate(color);
        }

        private void ResetHit() {
            _ship.SetModulate(DefaultModulateColor);
            _hit = false;
            _flashedTimes = 0;
        }

        private void OnAreaEntered(Area2D area) {
            if (area.GetParent() is Asteroid) {
                Hit();
            }
        }

        private void Hit() {
            _hit = true;
            _health--;
            if (_health <= 0) {
                Die();
            }
            EmitSignalOnHit(_health);
        }

        private void Die() {
            EmitSignalOnDeath();
        }
    }
}
