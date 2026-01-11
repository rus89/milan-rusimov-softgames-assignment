using UnityEngine;

namespace Softgames.Levels.PhoenixFlame
{
	public class ColorStateBehaviour : StateMachineBehaviour
	{
		[Tooltip("0=Orange, 1=Green, 2=Blue")]
		public int TargetColorIndex;

		//-------------------------------------------------------------------------
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var controller = animator.GetComponent<PhoenixFireController>();
			if (controller != null)
			{
				controller.SetTargetColorState(TargetColorIndex);
			}
		}
	}
}
