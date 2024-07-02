using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardCastShadow : MonoBehaviour {
   private const string _SHADOW_NAME = "Shadow";

   public Image   body;
   public Vector2 offset;
   public Vector2 scale = Vector2.one;
   public bool    freezeRotation;

   [SerializeField] private Image shadow;



   private void Awake()  => SetupShadow();
   private void Update() => UpdateShadow();

   private void OnDrawGizmos() {
      if (Application.isPlaying)
         return;

      SetupShadow();
   }


   private void SetupShadow() {
      if (shadow.IsUnityNull())
         CreateShadow();

      UpdateShadow();
   }

   private void CreateShadow() {
      var shadowIns = new GameObject(_SHADOW_NAME);
      shadowIns.transform.SetParent(body.transform);
      shadowIns.transform.SetSiblingIndex(0);

      shadow               = shadowIns.AddComponent<Image>();
      shadow.raycastTarget = false;

      shadowIns.GetComponent<RectTransform>().sizeDelta = body.GetComponent<RectTransform>().rect.size;
   }


   private void UpdateShadow() {
      UpdateShadowRender();
      UpdateShadowTransform();
   }

   private void UpdateShadowRender() {
      shadow.sprite = body.sprite;
      shadow.color  = body.color.ToShadow();
   }

   private void UpdateShadowTransform() {
      var inverseRot = Quaternion.Inverse(body.transform.rotation);

      Transform shadowT = shadow.transform;
      shadowT.localPosition = inverseRot * offset;
      shadowT.localScale    = scale;

      if (!freezeRotation)
         shadowT.rotation = body.transform.rotation;
   }
}