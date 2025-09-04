using Godot;
using static Godot.GD;
using System;
using Asteroids.Scripts.Entities;

namespace Asteroids.Scripts.Components {
    public partial class AsteroidSpawner : Node2D {
        private Timer _spawnTimer;
        private Vector2[] _spawnPoints;
        private Asteroid _asteroidFactory;
        
        public override void _Ready() {
            _spawnPoints = [
                new Vector2(GetViewportRect().Size.X / 2.0f, 0.0f), // top middle
                new Vector2(0.0f, GetViewportRect().Size.Y / 2.0f), // left middle
                new Vector2(GetViewportRect().Size.X / 2.0f, GetViewportRect().Size.Y), // bottom middle
                new Vector2(GetViewportRect().Size.X, GetViewportRect().Size.Y / 2.0f), // right middle
            ];
            _spawnTimer = GetNode<Timer>("Timer");
            _spawnTimer.Timeout += OnSpawnTimerTimeout;
            _asteroidFactory = new Asteroid();
        }

        private void OnSpawnTimerTimeout() {
            SpawnAsteroid();
        }

        private void SpawnAsteroid() {
            var asteroid = (Asteroid)_asteroidFactory.Spawn();
            asteroid.GlobalPosition = _spawnPoints[Globals.Random.Next(0, _spawnPoints.Length)];
            GetTree().CurrentScene.AddChild(asteroid);
        }
    }
}
