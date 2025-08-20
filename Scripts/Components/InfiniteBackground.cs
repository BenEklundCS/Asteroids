using Godot;
using System;

namespace Asteroids.Scripts.Components {
    public partial class InfiniteBackground : Node2D {
        [Export] public Texture2D Background;
        [Export] public Texture2D Midground;
        [Export] public Texture2D Foreground;
        
        [Export] public Vector2 SpriteScale = new (2.5f, 2.5f);
        [Export] public int SpriteZIndex = -100;

        [Export] public Player TrackedPlayer;

        private Sprite2D _backgroundSprite;
        private Sprite2D _midgroundSprite;
        private Sprite2D _foregroundSprite;

        private Vector2 _backgroundOffset;
        private Vector2 _midgroundOffset;
        private Vector2 _foregroundOffset;

        [Export] public float BackgroundScrollSpeed = 10.0f;
        [Export] public float MidgroundScrollSpeed = 30.0f;
        [Export] public float ForegroundScrollSpeed = 75.0f;

        public override void _Ready() {
            _backgroundSprite = GetNode<Sprite2D>("BackgroundSprite");
            _midgroundSprite = GetNode<Sprite2D>("MidgroundSprite");
            _foregroundSprite = GetNode<Sprite2D>("ForegroundSprite");
            
            BindTextures();
            
            Apply(sprite => {
                sprite.RegionEnabled = true;
                sprite.TextureRepeat = CanvasItem.TextureRepeatEnum.Enabled;
            });
        }

        public override void _Process(double delta) {
            var fdelta = (float)delta;
            ProcessSprite(_backgroundSprite, Background, ref _backgroundOffset, BackgroundScrollSpeed, fdelta);
            ProcessSprite(_midgroundSprite, Midground, ref _midgroundOffset, MidgroundScrollSpeed, fdelta);
            ProcessSprite(_foregroundSprite, Foreground, ref _foregroundOffset, ForegroundScrollSpeed, fdelta);
        }

        private void ProcessSprite(Sprite2D sprite, Texture2D texture, ref Vector2 offset, float speed, float fdelta) {
            var forward = -TrackedPlayer.Transform.Y.Normalized();
            
            offset.X += forward.X * speed * fdelta;
            offset.Y += forward.Y * speed * fdelta;
            
            if (offset.X > texture.GetWidth()) {
                offset.X = texture.GetWidth();
            }

            // reset x if it exceeds texture width
            offset.X = Mathf.PosMod(offset.X, texture.GetWidth());
            offset.Y = Mathf.PosMod(offset.Y, texture.GetHeight());

            if (offset.Y > texture.GetHeight() || offset.Y < 0) {
                offset.Y = texture.GetHeight();
            }

            var rect = new Rect2(offset, GetViewportRect().Size);
            sprite.RegionRect = rect;
        }
        
        private static float Wrap(float value, float max) => (value % max + max) % max;

        private void BindTextures() {
            _backgroundSprite.Texture = Background;
            _midgroundSprite.Texture = Midground;
            _foregroundSprite.Texture = Foreground;
        }
        
        private void Apply(Action<Sprite2D> action) {
            action(_backgroundSprite);
            action(_midgroundSprite);
            action(_foregroundSprite);
        }
    }
}
