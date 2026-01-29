using Arpg.Engine.Assets;
using Arpg.Engine.Gom;

namespace Arpg.Game.GameObjects;

public class Player : GameObject
{
  public Player() : base()
  {
    var animatedSprite = new AnimatedSprite()
    {
      Texture = AssetsManager.Textures["Sprites/base"],
      Anchor = new Vector2(8, 16)
    };
    animatedSprite.AddAnimation("WalkDown", new Animation
    {
      Frames = [new Rectangle(0, 16, 16, 16), new Rectangle(16, 16, 16, 16)],
      FrameDuration = 0.2f
    });
    animatedSprite.AddAnimation("WalkUp", new Animation
    {
      Frames = [new Rectangle(96, 32, 16, 16), new Rectangle(112, 32, 16, 16)],
      FrameDuration = 0.2f
    });
    animatedSprite.AddAnimation("WalkLeft", new Animation
    {
      Frames = [new Rectangle(0, 80, 16, 16), new Rectangle(16, 80, 16, 16)],
      FrameDuration = 0.2f
    });
    animatedSprite.AddAnimation("WalkRight", new Animation
    {
      Frames = [new Rectangle(192, 48, 16, 16), new Rectangle(208, 48, 16, 16)],
      FrameDuration = 0.2f
    });
    components.Add(animatedSprite);
    animatedSprite.Play("WalkDown");
    components.Add(new CameraFollowComponent());
    components.Add(new FacingDirection());

    // Add collision component
    var collider = new Collider
    {
      Offset = new Vector2(-4, -8), // Offset to align with sprite anchor (8, 16)
      Size = new Vector2(8, 8), // Same as sprite size
      Solid = false, // Player is not a solid obstacle
      CollisionMask = GameObjectGroup.Obstacle // Player collides with obstacles
    };
    Add(collider);

    States.Register(new PlayerMoving());
    States.SetInitial<PlayerMoving>();
  }
}
