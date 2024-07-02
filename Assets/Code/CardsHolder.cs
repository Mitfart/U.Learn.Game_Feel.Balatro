using System;
using System.Collections.Generic;
using UnityEngine;

public class CardsHolder : MonoBehaviour {
   private const string _SLOT_NAME = "slot";

   public event Action<Card> OnHoverCard;
   public event Action<Card> OnUnhoverCard;
   public event Action<Card> OnPickCard;
   public event Action<Card> OnDropCard;
   public event Action<Card> OnAddCard;
   public event Action<Card> OnRemoveCard;

   private readonly List<Card>      _cards = new();
   private readonly List<Transform> _slots = new();

   private Card _hoveredCard;
   private Card _pickedCard;

   public IReadOnlyList<Card> Cards => _cards;

   public Card HoveredCard {
      get => _hoveredCard;
      private set {
         if (_hoveredCard != null)
            OnUnhoverCard?.Invoke(_hoveredCard);

         _hoveredCard = value;

         if (_hoveredCard != null)
            OnHoverCard?.Invoke(_hoveredCard);
      }
   }

   public Card PickedCard {
      get => _pickedCard;
      private set {
         if (_pickedCard != null)
            OnDropCard?.Invoke(_pickedCard);

         _pickedCard = value;

         if (_pickedCard != null)
            OnPickCard?.Invoke(_pickedCard);
      }
   }



   public void Add(Card cardPrefab) {
      Transform slot = InsSlot();
      Card      card = Instantiate(cardPrefab, slot);

      _slots.Add(slot);
      _cards.Add(card);

      card.OnBeginHover += BeginHover;
      card.OnEndHover   += EndHover;
      card.OnPick       += Pick;
      card.OnDrop       += Drop;
      card.OnMove       += CheckPosition;

      OnAddCard?.Invoke(card);
   }

   public void Remove(Card card) {
      RemoveAt(_cards.IndexOf(card));
   }

   public void RemoveAt(int index) {
      Destroy(_slots[index].gameObject);

      Card card = _cards[index];
      card.OnBeginHover -= BeginHover;
      card.OnEndHover   -= EndHover;
      card.OnPick       -= Pick;
      card.OnDrop       -= Drop;
      card.OnMove       -= CheckPosition;

      _cards.RemoveAt(index);
      _slots.RemoveAt(index);

      OnRemoveCard?.Invoke(card);
   }



   private Transform InsSlot() {
      var slot = new GameObject(_SLOT_NAME, typeof(RectTransform));
      slot.transform.SetParent(transform);
      slot.transform.localScale = Vector3.one;
      return slot.transform;
   }



   private void BeginHover(Card card) => HoveredCard = card;

   private void EndHover(Card card) {
      if (HoveredCard == card)
         HoveredCard = null;
   }

   private void Pick(Card card) => PickedCard = card;

   private void Drop(Card card) {
      if (PickedCard == card)
         PickedCard = null;
   }



   private void CheckPosition(Card card) {
      int     cardIndex = _cards.IndexOf(card);
      Vector3 cardPos   = _cards[cardIndex].transform.position;

      int i = cardIndex;
      while (!Last() && cardPos.x > Next().position.x)
         Swap(cardIndex, ++i);

      while (!First() && cardPos.x < Prev().position.x)
         Swap(cardIndex, --i);

      return;

      bool      Last()  => i == _cards.Count - 1;
      bool      First() => i == 0;
      Transform Next()  => _cards[i + 1].transform;
      Transform Prev()  => _cards[i - 1].transform;
   }



   private void Swap(int indexA, int indexB) {
      Card      cardA = _cards[indexA];
      Transform slotA = _slots[indexA];

      Card      cardB = _cards[indexB];
      Transform slotB = _slots[indexB];

      cardA.transform.SetParent(slotB);
      cardB.transform.SetParent(slotA);

      _cards[indexA] = cardB;
      _cards[indexB] = cardA;

      ResetCardsPosition();
   }

   private void ResetCardsPosition() {
      foreach (Card c in _cards) {
         if (c == PickedCard)
            continue;

         c.transform.localPosition = Vector3.zero;
      }
   }
}