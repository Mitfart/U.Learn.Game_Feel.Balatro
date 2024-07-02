using UnityEngine;

public class DEBUG_HolderCleaner : MonoBehaviour {
   public          CardsHolder cardsHolder;
   [Min(0)] public int         indexFrom;
   [Min(0)] public int         indexTo;

   public bool clear;



   private void Awake()  => clear = true;
   private void Update() => Clear();



   private void Clear() {
      if (clear)
         return;

      clear = true;

      indexTo = Mathf.Min(indexTo, cardsHolder.Cards.Count - 1);

      int curIndexTo = indexTo;

      for (int i = indexFrom; i < curIndexTo; curIndexTo--) {
         Destroy(cardsHolder.Cards[i].gameObject);
         cardsHolder.RemoveAt(i);
      }
   }
}