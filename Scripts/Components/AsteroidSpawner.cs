using Godot;
using static Godot.GD;
using System;
using Asteroids.Scripts.Entities;
using Asteroids.Scripts.Interfaces;

namespace Asteroids.Scripts.Components {
    public partial class AsteroidSpawner : Node2D {
        private Timer _spawnTimer;
        private Timer _powerupTimer;
        private Timer _invaderTimer;
        private Vector2[] _spawnPoints;

        private Asteroid _asteroidFactory = new ();
        private Invader _invaderFactory = new ();
        private Powerup _powerupFactory = new ();

        public override void _Ready() {
            _spawnPoints = [
                new Vector2(GetViewportRect().Size.X / 2.0f, 0.0f), // top middle
                new Vector2(0.0f, GetViewportRect().Size.Y / 2.0f), // left middle
                new Vector2(GetViewportRect().Size.X / 2.0f, GetViewportRect().Size.Y), // bottom middle
                new Vector2(GetViewportRect().Size.X, GetViewportRect().Size.Y / 2.0f), // right middle
            ];

            _spawnTimer = GetNode<Timer>("AsteroidTimer");
            _powerupTimer = GetNode<Timer>("PowerupTimer");
            _invaderTimer = GetNode<Timer>("InvaderTimer");

            _spawnTimer.Timeout += OnSpawnTimerTimeout;
            _powerupTimer.Timeout += OnPowerupTimerTimeout;
            _invaderTimer.Timeout += OnInvaderTimerTimeout;
        }

        private void OnSpawnTimerTimeout() {
            SpawnObject(_asteroidFactory);
        }

        private void OnPowerupTimerTimeout() {
            SpawnObject(_powerupFactory);
        }
        
        private void OnInvaderTimerTimeout() {
            SpawnObject(_invaderFactory);
        }

        private void SpawnObject(ISpawnable factory) {
            var obj = factory.Spawn();
            obj.GlobalPosition = GetRandomSpawnPoint();
            GetTree().CurrentScene.AddChild(obj);
        }

        private Vector2 GetRandomSpawnPoint() {
            return _spawnPoints[Globals.Random.Next(0, _spawnPoints.Length)];
        }
    }
}
