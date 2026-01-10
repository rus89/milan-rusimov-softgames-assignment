using UnityEngine;

namespace Levels.AceOfShadows
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _cardSprite;
		[SerializeField] private SpriteRenderer _shadowSprite;
		
		//-----------------------------------------------------------------------
		public void SetCardSortingOrder(int orderIndex)
		{
			_cardSprite.sortingOrder = orderIndex;
			_shadowSprite.sortingOrder = orderIndex - 1;
		}
	}
}
		