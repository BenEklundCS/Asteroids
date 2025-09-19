using Godot;
using static Godot.GD;
using System;

namespace Asteroids.Scripts.Entities {
    public partial class Object : Node2D {
        [Export] public Vector2 Velocity { get; protected set; }
        [Export] public float Speed = 100.0f;
        [Export] public int Value = 1;
        protected bool Wrappable { get; set; }

        public override void _Process(double delta) {
            MoveAndWrap(delta);
        }

        private void MoveAndWrap(double delta) {
            Position += Velocity * (float)delta;
            
            if (!Wrappable) return;
            
            var size = GetViewportRect().Size;
            Position = new Vector2(
                Mathf.PosMod(Position.X, size.X),
                Mathf.PosMod(Position.Y, size.Y)
            );
        }
        
        protected void SeekCenter() {
            Vector2 GetCenter() {
                var viewport = GetViewportRect(); 
                return new Vector2(viewport.Size.X / 2.0f, viewport.Size.Y / 2.0f);
            }
            Velocity = Position.DirectionTo(GetCenter()) * Speed;
        }

        protected void SeekRandom() {
            Velocity = Position.DirectionTo(new Vector2(
                (float)Globals.Random.NextDouble() * GetViewportRect().Size.X,
                (float)Globals.Random.NextDouble() * GetViewportRect().Size.Y
            )) * Speed;
        }
    }
}
