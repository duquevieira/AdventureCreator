using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;

namespace MoreMountains.InventoryEngine
{	
	[CreateAssetMenu(fileName = "EquipableItem", menuName = "MoreMountains/InventoryEngine/EquipableItem", order = 2)]
	[Serializable]
	/// <summary>
	/// Demo class for a equipable item
	/// </summary>
	public class EquipableItem : InventoryItem 
	{
		[Header("Equipable")]
		/// the sprite to use to show the item when equipped
		public Sprite EquipableSprite;

		/// <summary>
		/// What happens when the object is used 
		/// </summary>
		public override bool Equip(string playerID)
		{
			base.Equip(playerID);
			return true;
		}

		/// <summary>
		/// What happens when the object is used 
		/// </summary>
		public override bool UnEquip(string playerID)
		{
			base.UnEquip(playerID);
			return true;
		}
		
	}
}