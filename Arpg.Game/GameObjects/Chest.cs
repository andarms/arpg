using Arpg.Engine.Assets;
using Arpg.Game.Gom;

namespace Arpg.Game.GameObjects;

public class Chest : GameObject
{
  public Chest() : base()
  {
    components.Add(new Sprite()
    {
      Texture = AssetsManager.Textures["TinyDungeon"],
      Source = new Rectangle(96, 112, 16, 16),
      Anchor = new Vector2(8, 16)
    });
  }
}