using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimator : MonoBehaviour {
   public Canvas             canvas;
   public Card               card;
   public RectTransform      body;
   public Image              view;
   public CardCastShadow     shadow;
   public CardAnimatorParams @params;

   private Vector3 _viewPosition;
   private Vector3 _viewAngles;

   private int     _savedSiblingCount;
   private int     _savedSiblingIndex;
   private Vector2 _mouseHoverOffset;



   private void OnEnable() {
      card.OnBeginHover += BeginHover;
      card.OnHover      += Hover;
      card.OnEndHover   += EndHover;

      card.OnSelect   += Select;
      card.OnUnselect += Unselect;

      card.OnPick += OnPick;
      card.OnDrop += OnDrop;
   }

   private void OnDisable() {
      card.OnBeginHover -= BeginHover;
      card.OnHover      -= Hover;
      card.OnEndHover   -= EndHover;

      card.OnSelect   -= Select;
      card.OnSelect   -= Select;
      card.OnUnselect -= Unselect;

      card.OnPick -= OnPick;
      card.OnDrop -= OnDrop;
   }



   private void Awake() {
      InitVariables();
      MoveShadow(@params.shadow.defaultOffset);
   }

   private void Update() {
      OffsetSelectedCard();

      UpdateVariables();

      FollowPosition();
      FollowRotation();
      CardTilt();

      SetViewTransform();
   }



   private void InitVariables() {
      Transform cardT = card.transform;
      _viewPosition = cardT.position;
      _viewAngles   = cardT.eulerAngles;
   }

   private void UpdateVariables() {
      _savedSiblingCount = card.Picked ? _savedSiblingCount : card.transform.parent.parent.childCount;
      _savedSiblingIndex = card.Picked ? _savedSiblingIndex : card.transform.parent.GetSiblingIndex();
   }


   private void OffsetSelectedCard() {
      if (card.Picked)
         return;

      card.transform.localPosition = card.Selected
         ? card.transform.up * @params.select.punchAmount
         : Vector3.zero;
   }


   private void FollowPosition() {
      _viewPosition = Vector3.Lerp(
         _viewPosition,
         card.transform.position,
         @params.follow.speed * Time.deltaTime
      );
   }

   private void FollowRotation() {
      float deltaTime      = Time.deltaTime;
      float targetRotation = card.transform.eulerAngles.z;

      if (card.Picked) {
         float   maxAngle         = @params.follow.maxAngle;
         Vector3 movementDelta    = (_viewPosition - card.transform.position) * deltaTime;
         float   movementRotation = movementDelta.x                           * maxAngle;

         if (@params.follow.inverseRotation)
            movementRotation *= -1f;

         targetRotation = Mathf.Clamp(movementRotation, -maxAngle, maxAngle);
      }

      _viewAngles.z = Mathf.LerpAngle(_viewAngles.z, targetRotation, @params.follow.rotationSpeed * deltaTime);
   }

   private void CardTilt() {
      float tiltX = Mathf.Sin(Time.time + _savedSiblingIndex);
      float tiltY = Mathf.Cos(Time.time + _savedSiblingIndex);

      Vector2 tilt = Vector2.zero;

      if (card.Hovered) {
         Rect    viewRect    = view.rectTransform.rect;
         float   scaleFactor = view.canvas.scaleFactor;
         Vector2 scale       = viewRect.size * scaleFactor;

         tilt.x = -_mouseHoverOffset.y / scale.y;
         tilt.y = +_mouseHoverOffset.x / scale.x;

         tilt *= @params.tilt.hoverTiltAmount * 2f; // Tilt goes from -0.5f to 0.5f 

         tiltX *= @params.tilt.hoverAutoTiltScale;
         tiltY *= @params.tilt.hoverAutoTiltScale;
      }

      float t     = @params.tilt.speed * Time.deltaTime;
      float lerpX = Mathf.LerpAngle(_viewAngles.x, tilt.x + tiltX * @params.tilt.autoTiltAmount, t);
      float lerpY = Mathf.LerpAngle(_viewAngles.y, tilt.y + tiltY * @params.tilt.autoTiltAmount, t);

      _viewAngles = new Vector3(lerpX, lerpY, _viewAngles.z);
   }

   private void SetViewTransform() {
      Transform bodyT = body.transform;
      bodyT.eulerAngles = _viewAngles;
      bodyT.position    = _viewPosition;
   }


   private void BeginHover(Card _) {
      Scale(@params.hover.scale);
      PunchRotate(@params.hover.punchAngle);
   }

   private void Hover(Card _, Vector2 mouse) {
      _mouseHoverOffset = (Vector2)body.position - mouse;
   }

   private void EndHover(Card _) {
      Scale(1f);
   }


   private void Select(Card c) {
      PunchRotate(@params.select.punchAngle);
      MoveShadow(@params.shadow.pickedOffset);
   }

   private void Unselect(Card c) {
      MoveShadow(@params.shadow.defaultOffset);
   }


   private void OnPick(Card _) {
      Scale(@params.pick.scale);
      RenderViewFront(true);
      MoveShadow(@params.shadow.pickedOffset);
   }

   private void OnDrop(Card c) {
      Scale(1f);
      RenderViewFront(false);

      if (!card.Selected)
         MoveShadow(@params.shadow.defaultOffset);
   }



   private void RenderViewFront(bool on) {
      canvas.overrideSorting = on;
      canvas.sortingOrder    = on ? 10 : 0;
   }

   private void Scale(float scale) {
      DOTween.Kill(body, true);
      body.transform
          .DOScale(scale, @params.animations.duration)
          .SetEase(@params.animations.ease)
          .SetId(body);
   }

   private void PunchRotate(float angle) {
      DOTween.Kill(view, true);
      view.transform
          .DOPunchRotation(Vector3.forward * angle, @params.animations.duration)
          .SetEase(@params.animations.ease)
          .SetId(view);
   }

   private void MoveShadow(float offset) {
      DOTween.Kill(shadow, true);
      DOTween.To(
                 () => shadow.offset,
                 value => shadow.offset = value,
                 Vector2.down * offset,
                 @params.animations.duration
              )
             .SetEase(@params.animations.ease)
             .SetId(shadow);
   }
}