using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Asteroids.Scripts.Interfaces;

namespace Asteroids.Scripts.Entities {
    public partial class Powerup : Object, ISpawnable {
        public enum PowerupType {
            ExtraLife,
            Shield,
        }
        
        private readonly Dictionary<PowerupType, string> _powerupSprites = new() {
            { PowerupType.ExtraLife, "res://Assets/Powerups/extralife.png" },
            { PowerupType.Shield, "res://Assets/Powerups/shield.png" },
        };
    
        private PowerupType _typeBacking = PowerupType.ExtraLife;
        private PowerupType Type {
            get => _typeBacking;
            set {
                _typeBacking = value;
                _sprite.Texture = Load<Texture2D>(_powerupSprites[Type]);
            }
        }
        
        private Area2D _area;
        private Sprite2D _sprite;
 
        public override void _Ready() {
            AddToGroup("Powerups");
            Wrappable = true;
            _area = GetNode<Area2D>("Hitbox");
            _sprite = GetNode<Sprite2D>("Sprite2D");
            Type = GetRandomPowerupType();
            SeekRandom();
        }
        
        public Object Spawn() {
            return (Powerup)Load<PackedScene>("res://Scenes/Entities/powerup.tscn").Instantiate();
        }
        
        public void ApplyEffect(Player player) {
            switch (_typeBacking) {
                case PowerupType.ExtraLife:
                    player.Health += 1;
                    break;
                case PowerupType.Shield:
                    player.ActivateShield();
                    break;
            }
            QueueFree(); // Remove the powerup after applying its effect
        }

        private static PowerupType GetRandomPowerupType() {
            var values = Enum.GetValues<PowerupType>();
            return values[Globals.Random.Next(0, values.Length)];
        }
    }
}
