using Godot;
using static Godot.GD;
using System;
using Asteroids.Scripts.Components;
using Asteroids.Scripts.Interfaces;

namespace Asteroids.Scripts.Entities {
    public partial class Player : Object, IControllable {
        private AnimatedSprite2D _boostEffect;
        private Area2D _hitBox;
        private Node2D _pivot;
        private bool _boosting;
        
        [Export] public Vector2 BaseVelocity;
        [Export] public Vector2 MaxVelocity = new(300.0f, 300.0f);
        [Export] public float MinVelocity = 5.0f;
        [Export] public float Speed = 5.0f;
        [Export] public float RotationSpeed = (float)(Math.PI/180.0f * 2);
        [Export] public int SlowRange = 50;

        public override void _Ready() {
            _boostEffect = GetNode<AnimatedSprite2D>("Pivot/BoostEffect");
            _hitBox = GetNode<Area2D>("Pivot/Hitbox");
            _hitBox.AreaEntered += OnAreaEntered;
            _pivot = GetNode<Node2D>("Pivot");
            _boostEffect.Play();
            Velocity = BaseVelocity;
        }
        
        public override void _Process(double delta) {
            ClampVelocity();
            Animate();
            //Print(Velocity.Length());
            base._Process(delta);
        }

        private Vector2 GetForward() {
            return -_pivot.Transform.Y.Normalized();
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

        private void ClampVelocity() {
            if (Velocity.Length() > MaxVelocity.Length())
                Velocity = (Velocity.Normalized() * MaxVelocity);
            if (Velocity.Length() < MinVelocity) {
                Velocity = Vector2.Zero;
            }
        }

        private void Animate() {
            _boostEffect.Animation = _boosting  ? "boost" : "idle";
            _boosting = false;
        }

        private void OnAreaEntered(Area2D area) {
            
        }
    }
}
