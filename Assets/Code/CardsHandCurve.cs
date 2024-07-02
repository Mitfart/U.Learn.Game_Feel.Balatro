using System.Collections.Generic;
using UnityEngine;



public class CardsHandCurve : MonoBehaviour {
   public           CardsHolder holder;
   [Min(0f)] public float       handHeight        = 5f;
   [Min(0f)] public float       handRotationAngle = 15f;

   private IReadOnlyList<Card> Cards => holder.Cards;



   private void LateUpdate() {
      float positionMult = 1f / (Cards.Count - 1);

      for (var i = 0; i < Cards.Count; i++) {
         Card      card  = Cards[i];
         Transform cardT = card.transform;

         if (card.Picked) {
            cardT.eulerAngles = Vector3.zero;
            continue;
         }

         float position = i * positionMult;
         float offset   = Mathf.Sin(position * Mathf.PI);

         float handOffset   = handHeight        * offset;
         float handRotation = handRotationAngle * (position - .5f) * -2f;

         cardT.localPosition += Vector3.up * handOffset;
         cardT.eulerAngles   = Vector3.forward * handRotation;
      }
   }
}