using UnityEngine;

#if UNITY_EDITOR

namespace Levels.AceOfShadows
{
	public class WireSphereGizmo : MonoBehaviour
	{
		[SerializeField] private float _radius = 1f;
		[SerializeField] private Color _gizmoColor = Color.yellow;

		//-----------------------------------------------------------------------
		private void OnDrawGizmos()
		{
			Gizmos.color = _gizmoColor;
			Gizmos.DrawWireSphere(transform.position, _radius);
		}
	}
}

#endif