using UnityEngine;

public class DEBUG_HolderFiller : MonoBehaviour {
   public          CardsHolder  cardsHolder;
   public          Card         cardPrefab;
   [Min(1)] public int          amount;
   public          CardViewType spawnCardType;

   public bool filled;



   private void Update() => Fill();



   private void Fill() {
      if (filled)
         return;

      filled = true;

      if (cardPrefab.TryGetComponent(out CardShaderController shaderController))
         shaderController.cardViewType = spawnCardType;

      for (var i = 0; i < amount; i++)
         cardsHolder.Add(cardPrefab);
   }
}