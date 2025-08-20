using Godot;
using static Godot.GD;
using System;

namespace Asteroids.Scripts {
    public partial class Player : Object {
        private AnimatedSprite2D _boostEffect;
        private bool _boosting;
        
        [Export] public Vector2 BaseVelocity;
        [Export] public Vector2 MaxVelocity = new (300f, 300f);
        [Export] public float Speed = 100.0f;
        [Export] public float RotationSpeed = (float)(Math.PI/180.0f * 2);

        public override void _Ready() {
            _boostEffect = GetNode<AnimatedSprite2D>("BoostEffect");
            _boostEffect.Play();
            SetVelocity(BaseVelocity);
        }
        
        public override void _Process(double delta) {
            ClampVelocity();
            Animate();
            base._Process(delta);
        }

        public void Boost() {
            _boosting = true;
            var forward = -Transform.Y.Normalized();  
            var newVelocity = GetVelocity() + (forward * Speed);
            SetVelocity(newVelocity);
        }

        public void Stop() {
            SetVelocity(new Vector2());
        }

        public void Left() {
            Rotation -= RotationSpeed;
        }

        public void Right() {
            Rotation += RotationSpeed;
        }

        private void ClampVelocity() {
            if (GetVelocity().Length() > MaxVelocity.Length())
                SetVelocity(GetVelocity().Normalized() * MaxVelocity);
        }

        private void Animate() {
            _boostEffect.Animation = _boosting  ? "boost" : "idle";
            _boosting = false;
        }
    }
}
