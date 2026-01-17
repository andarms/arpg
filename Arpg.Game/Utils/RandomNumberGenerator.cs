namespace Arpg.Game.Utils;

public class RandomNumberGenerator
{
  readonly Random random = new();

  public int Next(int minValue, int maxValue)
  {
    return random.Next(minValue, maxValue);
  }

  public float NextFloat(float minValue, float maxValue)
  {
    return (float)(minValue + random.NextDouble() * (maxValue - minValue));
  }

  public bool NextBool()
  {
    return random.Next(0, 2) == 0;
  }
}