using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CardShaderController : MonoBehaviour {
   private const string _EDITION = "_EDITION_";

   private static readonly int _Rotation = Shader.PropertyToID("_Rotation");

   public CardViewType cardViewType;
   public CardAnimator cardAnimator;

   private Material _material;



   private void Awake() {
      InitMaterial();
      SetView(cardViewType);
   }

   private void Update() {
      SetRotationToMaterial();
   }

   private void OnValidate() {
      if (Application.isPlaying)
         SetView(cardViewType);
   }



   public void SetView(CardViewType newCardViewType) {
      cardViewType = newCardViewType;

      foreach (LocalKeyword key in _material.enabledKeywords)
         _material.DisableKeyword(key);

      _material.EnableKeyword(_EDITION + cardViewType);
   }



   private void InitMaterial() {
      Image image = cardAnimator.view;
      _material      = new Material(image.material);
      image.material = _material;
   }
   

   private void SetRotationToMaterial() {
      Vector3 eulerAngles = cardAnimator.view.transform.eulerAngles;

      float xAngle = eulerAngles.x;
      float yAngle = eulerAngles.y;

      xAngle = ClampAngle(xAngle, min: -90f, max: 90f);
      yAngle = ClampAngle(yAngle, min: -90f, max: 90);

      _material.SetVector(
         _Rotation,
         new Vector4(
            xAngle,
            yAngle
         )
      );
   }

   private static float ClampAngle(float angle, float min, float max) {
      if (angle < -180f)
         angle = 180f;

      if (angle > 180f)
         angle = -180f;

      return Mathf.Clamp(angle, min, max);
   }
}