using Godot;
using static Godot.GD;
using System;

namespace Asteroids.Scripts {
    public partial class Object : Node2D {
        private Vector2 _velocity;
        
        public override void _Process(double delta) {
            MoveAndWrap(delta);
        }

        private void MoveAndWrap(double delta) {
            Position = new Vector2(
                Mathf.PosMod(Position.X, GetViewportRect().Size.X), 
                Mathf.PosMod(Position.Y, GetViewportRect().Size.Y)
            ) + _velocity * (float)delta;
        }

        protected void SetVelocity(Vector2 velocity) {
            _velocity = velocity;
        }

        protected Vector2 GetVelocity() {
            return _velocity;
        }
    }
}
