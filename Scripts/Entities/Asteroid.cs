using Godot;
using static Godot.GD;
using System;
using System.Transactions;

namespace Asteroids.Scripts.Entities {
    public partial class Asteroid : Object {
        [Signal] public delegate void OnDestroyEventHandler();
        
        [Export] public int Min = -5;
        [Export] public int Max = 5;
        [Export] public int DefaultRotationSpeed = 5;
        [Export] public int SplitCount = 2;
        [Export] public float Speed = 100.0f;
        [Export] public bool Parent = true;
        [Export] public int SpawnCount = 2;
        
        private float _rotationSpeed;
        private AnimatedSprite2D _asteroidSprite;
        private Area2D _hitBox;
        private Timer _graceTimer;
        private bool _explodable = false;
        
        public override void _Ready() {
            _asteroidSprite = GetNode<AnimatedSprite2D>("AsteroidSprite");
            _asteroidSprite.AnimationFinished += OnAnimationFinished;
            _hitBox = GetNode<Area2D>("Hitbox");
            _hitBox.AreaEntered += OnAreaEntered;
            _graceTimer = GetNode<Timer>("GraceTimer");
            _graceTimer.Timeout += OnGraceTimerTimeout;
            
            var rvalue = Globals.Random.Next(Min, Max);
            _rotationSpeed = rvalue != 0 ? rvalue : DefaultRotationSpeed;

            if (Parent) {
                SeekCenter();
            }
            else {
                SeekGod();
            }
        }

        public override void _Process(double delta) {
            Rotation += _rotationSpeed * (float)delta;
            base._Process(delta);
        }

        private void Explode() {
            if (_explodable) {
                _asteroidSprite.Play();
            }
        }

        private void SeekCenter() {
            Func<Vector2> getCenter = () => { var viewport = GetViewportRect(); return new Vector2(viewport.Size.X / 2.0f, viewport.Size.Y / 2.0f); };
            Velocity = Position.DirectionTo(getCenter()) * Speed;
        }

        private void SeekGod() {
            var rand = Globals.Random.Next(0, 2);
            var randomPosition = new Vector2(
                Position.X + (float)(Globals.Random.NextDouble() * (rand == 0 ? -1.0 : 1.0)),
                Position.Y + (float)(Globals.Random.NextDouble() * (rand == 0 ? -1.0 : 1.0))
            );
            Velocity = Position.DirectionTo(randomPosition) * Speed;
        }

        private void SplitAndDestroy() {
            if (SplitCount > 0) {
                for (int i = 0; i < SpawnCount; i++) {
                    var asteroid = (Asteroid)Load<PackedScene>("res://Scenes/Entities/Asteroid.tscn").Instantiate();
            
                    asteroid.GlobalPosition = GlobalPosition;
                    asteroid.Scale = Scale / 2.0f;
                    asteroid.SplitCount = SplitCount - 1;
                    asteroid.Parent = false;
            
                    GetTree().Root.AddChild(asteroid);
                }
            }
            QueueFree();
        }
        
        private void OnAnimationFinished() {
            SplitAndDestroy();
        }

        private void OnAreaEntered(Area2D area) {
            Explode();
        }

        private void OnGraceTimerTimeout() {
            _explodable = true;
        }
    }
}
