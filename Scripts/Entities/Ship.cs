using Godot;
using static Godot.GD;

namespace Asteroids.Scripts.Entities {
    public partial class Ship : Object {
        
        [Signal] public delegate void OnDeathEventHandler(Ship ship);
        [Signal] public delegate void OnHitEventHandler(int health);
        
        private bool _hit;
        private int _flashedTimes = 0;
        private int _health = 3;

        private Node2D _pivot;
        private Area2D _hitBox;
        private Timer _flashTimer;
        private AudioStreamPlayer _shootSound;
        
        protected AnimatedSprite2D BoostEffect;
        protected Sprite2D ShipSprite;
        
        [Export] public int FlashTimes = 6;
        [Export] public Color OnHitModulateColor = Color.Color8(255, 0, 0);
        [Export] public Color DefaultModulateColor = Color.Color8(255, 255, 255);
        [Export] public int MaxHealth = 3;
        [Export] public int Health {
            get => _health;
            set {
                _health = value;
                EmitSignalOnHit(_health);
            }
        }
        
        public override void _Ready() {
            _flashTimer = GetNode<Timer>("FlashTimer");
            _flashTimer.Timeout += OnFlashTimerTimeout;
            _shootSound = GetNode<AudioStreamPlayer>("ShootSound");
            _pivot = GetNode<Node2D>("Pivot");
            ShipSprite = GetNode<Sprite2D>("Pivot/Ship");
            BoostEffect = GetNode<AnimatedSprite2D>("Pivot/BoostEffect");
            BoostEffect.Play();
            _hitBox = GetNode<Area2D>("Pivot/Hitbox");
            _hitBox.AreaEntered += OnAreaEntered;
        }

        protected void Hit(int damage = 1) {
            _hit = true;
            Health = _health - damage;
            if (_health <= 0) {
                Die();
            }
        }
        
        protected virtual void OnAreaEntered(Area2D area) {

        }

        public virtual void Shoot() {
            _shootSound.Play();
        }
        
        private void OnFlashTimerTimeout() {
            if (!_hit) return;

            _flashedTimes++;
            if (_flashedTimes > FlashTimes) {
                ResetHit();
                return;
            }

            var color = (_flashedTimes % 2 != 0)
                ? OnHitModulateColor
                : DefaultModulateColor;
            ShipSprite.SetModulate(color);
        }
        
        private void ResetHit() {
            ShipSprite.SetModulate(DefaultModulateColor);
            _hit = false;
            _flashedTimes = 0;
        }
        
        private void Die() {
            EmitSignalOnDeath(this);
            QueueFree();
        }
    }
}
