using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Softgames.Utilities
{
	public class UIButtonJuice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private float _pressedScale = 0.92f;
		[SerializeField] private float _duration = 0.1f;
        
		private Tween _activeTween;

		//-------------------------------------------------------------------------
		public void OnPointerDown(PointerEventData eventData)
		{
			_activeTween.Stop();
			_activeTween = Tween.Scale(transform, _pressedScale, _duration, Ease.OutQuad);
		}

		//-------------------------------------------------------------------------
		public void OnPointerUp(PointerEventData eventData)
		{
			_activeTween.Stop();
			_activeTween = Tween.Scale(transform, 1f, _duration, Ease.OutBack);
		}
     
		//-------------------------------------------------------------------------
		private void OnDisable()
		{
			_activeTween.Stop();
			transform.localScale = Vector3.one;
		}
	}
}
