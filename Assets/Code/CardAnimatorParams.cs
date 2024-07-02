using System;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu]
public class CardAnimatorParams : ScriptableObject {
   public Follow     follow;
   public Animations animations;
   public Select     select;
   public Hover      hover;
   public Pick       pick;
   public Shadow     shadow;
   public Tilt       tilt;

   // ============================================================================================================== //
   
   [Serializable]
   public struct Follow {
      [Min(0f)]        public float speed;
      [Range(0f, 90f)] public float maxAngle;
      [Min(0f)]        public float rotationSpeed;
      public                  bool  inverseRotation;

      public Follow(
         float speed,
         float maxAngle,
         float rotationSpeed,
         bool  inverseRotation
      ) {
         this.speed           = speed;
         this.maxAngle        = maxAngle;
         this.rotationSpeed   = rotationSpeed;
         this.inverseRotation = inverseRotation;
      }
   }

   [Serializable]
   public struct Animations {
      [Min(0f)] public float duration;
      public           Ease  ease;

      public Animations(float duration = .15f, Ease ease = Ease.OutBack) {
         this.duration = duration;
         this.ease     = ease;
      }
   }

   [Serializable]
   public struct Select {
      [Min(0f)] public float punchAmount;
      [Min(0f)] public float punchAngle;

      public Select(float punchAmount = 0f, float punchAngle = 5f) {
         this.punchAmount = punchAmount;
         this.punchAngle  = punchAngle;
      }
   }

   [Serializable]
   public struct Hover {
      [Min(0f)] public float punchAngle;
      [Min(0f)] public float scale;

      public Hover(float punchAngle = 15, float scale = 1.125f) {
         this.punchAngle = punchAngle;
         this.scale      = scale;
      }
   }

   [Serializable]
   public struct Pick {
      [Min(0f)] public float scale;

      public Pick(float scale = 1.25f) {
         this.scale = scale;
      }
   }

   [Serializable]
   public struct Shadow {
      [Min(0f)] public float defaultOffset;
      [Min(0f)] public float pickedOffset;

      public Shadow(
         float defaultOffset = 2f,
         float pickedOffset  = 10f
      ) {
         this.defaultOffset = defaultOffset;
         this.pickedOffset  = pickedOffset;
      }
   }

   [Serializable]
   public struct Tilt {
      [Min(0f)]       public float autoTiltAmount;
      [Min(0f)]       public float speed;
      [Min(0f)]       public float hoverTiltAmount;
      [Range(0f, 1f)] public float hoverAutoTiltScale;

      public Tilt(
         float autoTiltAmount     = 20f,
         float speed              = 20f,
         float hoverTiltAmount    = 30f,
         float hoverAutoTiltScale = .5f
      ) {
         this.autoTiltAmount     = autoTiltAmount;
         this.speed              = speed;
         this.hoverTiltAmount    = hoverTiltAmount;
         this.hoverAutoTiltScale = hoverAutoTiltScale;
      }
   }
}