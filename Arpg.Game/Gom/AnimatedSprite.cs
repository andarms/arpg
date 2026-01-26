namespace Arpg.Game.Gom;

public class Animation
{
  public List<Rectangle> Frames { get; set; } = new();
  public float FrameDuration { get; set; } = 0.1f;

  float elapsedTime = 0f;
  int currentFrameIndex = 0;
  public Rectangle CurrentFrame => Frames[currentFrameIndex];

  public void Update(float dt)
  {
    elapsedTime += dt;
    if (elapsedTime >= FrameDuration)
    {
      elapsedTime = 0f;
      currentFrameIndex = (currentFrameIndex + 1) % Frames.Count;
    }
  }
}


public class AnimatedSprite : Sprite
{
  readonly Dictionary<string, Animation> animations = [];
  string animationName = string.Empty;

  public void Play(string name)
  {
    if (animations.ContainsKey(name))
    {
      animationName = name;
    }
  }

  public void AddAnimation(string name, Animation animation)
  {
    animations[name] = animation;
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (animationName != string.Empty && animations.ContainsKey(animationName))
    {
      animations[animationName].Update(dt);
    }
  }

  public override void Draw(Vector2 position)
  {
    if (animationName == string.Empty) return;

    DrawTexturePro(
      Texture,
      animations[animationName].CurrentFrame,
      new Rectangle(
        position.X,
        position.Y,
        animations[animationName].CurrentFrame.Width * Scale,
        animations[animationName].CurrentFrame.Height * Scale
      ),
      Anchor,
      Rotation,
      Tint
    );
  }
}