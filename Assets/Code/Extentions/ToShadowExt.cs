using UnityEngine;

public static class ToShadowExt {
   public static Color ToShadow(this Color color) {
      float alpha = color.a;

      color   *= 1f - alpha;
      color.a =  alpha * .5f;

      return color;
   }
}