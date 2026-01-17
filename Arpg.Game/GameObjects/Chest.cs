using Arpg.Game.Assets;
using Arpg.Game.Gom;

namespace Arpg.Game.GameObjects;

public class Chest : GameObject
{
  public Chest() : base()
  {
    Sprite = new() { Texture = AssetsManager.Textures["TinyDungeon"], Source = new Rectangle(96, 112, 16, 16) };
  }
}