using Godot;
using static Godot.GD;
using System;
using System.Transactions;
using Asteroids.Scripts.Interfaces;

namespace Asteroids.Scripts.Entities {
    public partial class Asteroid : Object, ISpawnable {
        [Export] public int Min = -5;
        [Export] public int Max = 5;
        [Export] public int DefaultRotationSpeed = 5;
        [Export] public int SplitCount = 2;
        [Export] public bool Parent = true;
        [Export] public int SpawnCount = 2;
        
        private float _rotationSpeed;
        private AnimatedSprite2D _asteroidSprite;
        private Area2D _hitBox;
        private Timer _graceTimer;
        private bool _explodable = false;
        
        public override void _Ready() {
            AddToGroup("Asteroids");
            Wrappable = true;
            Value = 1;
            
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

        public Object Spawn() {
            return (Asteroid)Load<PackedScene>("res://Scenes/Entities/asteroid.tscn").Instantiate();
        }

        public override void _Process(double delta) {
            Rotation += _rotationSpeed * (float)delta;
            base._Process(delta);
        }

        private void Explode() {
            if (!_explodable) return;
            _hitBox.QueueFree(); // delete the hitbox so it's not hittable
            _asteroidSprite.Play();
        }

        private void SeekGod() {
            var rand = Globals.Random.Next(0, 2);
            var randomPosition = new Vector2(
                Position.X + (float)(Globals.Random.NextDouble() * (rand == 0 ? -1.0f : 1.0f)),
                Position.Y + (float)(Globals.Random.NextDouble() * (rand == 0 ? -1.0f : 1.0f))
            );
            Velocity = Position.DirectionTo(randomPosition) * Speed;
        }

        private void SplitAndDestroy() {
            if (SplitCount > 0) {
                SpawnChildren();
            }
            QueueFree();
        }

        private void SpawnChildren() {
            for (var i = 0; i < SpawnCount; i++) {
                var asteroid = (Asteroid)Spawn();
            
                asteroid.GlobalPosition = GlobalPosition;
                asteroid.Scale = Scale / 2.0f;
                asteroid.SplitCount = SplitCount - 1;
                asteroid.Parent = false;
            
                GetTree().CurrentScene.AddChild(asteroid);
            }
        }
        
        private void OnAnimationFinished() {
            SplitAndDestroy();
        }

        private void OnAreaEntered(Area2D area) {
            if (area.GetParent() is Bullet) {
                _explodable = true;
            }
            Explode();
        }

        private void OnGraceTimerTimeout() {
            _explodable = true;
        }
    }
}
