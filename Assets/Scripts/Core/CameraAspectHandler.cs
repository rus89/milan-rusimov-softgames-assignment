using UnityEngine;

namespace Softgames.Core
{
	public class CameraAspectHandler : MonoBehaviour
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private float _targetWidth;

		//-----------------------------------------------------------------------
		private void Start()
		{
			AdjustCamera();
		}

		//-----------------------------------------------------------------------
		private void AdjustCamera()
		{
			float unitsPerPixel = _targetWidth / Screen.width;
			float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

			_camera.orthographicSize = Mathf.Max(5f, desiredHalfHeight);
		}
	}	
}
