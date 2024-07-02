using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card
   : MonoBehaviour,
     IDragHandler,
     IBeginDragHandler,
     IEndDragHandler,
     IPointerUpHandler,
     IPointerEnterHandler,
     IPointerMoveHandler,
     IPointerExitHandler {
   public event Action<Card> OnMove;
   public event Action<Card> OnPick;
   public event Action<Card> OnDrop;

   public event Action<Card>          OnBeginHover;
   public event Action<Card, Vector2> OnHover;
   public event Action<Card>          OnEndHover;

   public event Action<Card> OnSelect;
   public event Action<Card> OnUnselect;

   public bool Hovered  { get; private set; }
   public bool Selected { get; private set; }
   public bool Picked   { get; private set; }

   private int _savedSiblingCount;
   private int _savedSiblingIndex;



   public void OnDrag(PointerEventData eventData) {
      transform.position = eventData.position;

      OnMove?.Invoke(this);
   }

   public void OnBeginDrag(PointerEventData eventData) {
      Picked  = true;
      Hovered = false;

      OnPick?.Invoke(this);
   }

   public void OnEndDrag(PointerEventData eventData) {
      Picked  = false;
      Hovered = false;

      transform.localPosition = Vector3.zero;

      OnDrop?.Invoke(this);
   }


   public void OnPointerEnter(PointerEventData eventData) {
      if (Picked)
         return;

      OnBeginHover?.Invoke(this);
      Hovered = true;
   }

   public void OnPointerMove(PointerEventData eventData) {
      if (Picked)
         return;

      OnHover?.Invoke(this, eventData.position);
   }

   public void OnPointerExit(PointerEventData eventData) {
      if (Picked)
         return;

      OnEndHover?.Invoke(this);
      Hovered = false;
   }

   public void OnPointerUp(PointerEventData eventData) {
      if (Picked)
         return;

      Selected = !Selected;

      if (Selected)
         OnSelect?.Invoke(this);
      else
         OnUnselect?.Invoke(this);
   }
}