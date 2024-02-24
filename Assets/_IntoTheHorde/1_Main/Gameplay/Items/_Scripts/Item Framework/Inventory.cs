using System;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Attach this script to the player
 * 
 */
namespace IntoTheHorde
{
    [DisallowMultipleComponent]
	[RequireComponent(typeof( GameActor ), typeof( InputReader ))]
    public class Inventory : MonoBehaviour
    {
		public enum SLOT_TYPE { PRIMARY, SECONDARY, THROWABLE, MEDKIT, CONSUMABLE }

		[SerializeField] Transform _itemHolder;

		GameActor						m_owner;
		Dictionary<SLOT_TYPE, ItemSlot> m_itemSlotMap;
		SLOT_TYPE						m_currSlotType;
		int								m_numSlotTypes;
		InputReader						m_inputReader;

		// Public properties for the UI to use
		public GameActor Owner			 => m_owner;
		public SLOT_TYPE CurrentSlotType => m_currSlotType;
		public ItemSlot  PrimarySlot	 => m_itemSlotMap[SLOT_TYPE.PRIMARY];
		public ItemSlot  SecondarySlot   => m_itemSlotMap[SLOT_TYPE.SECONDARY];
		public ItemSlot  ThrowableSlot   => m_itemSlotMap[SLOT_TYPE.THROWABLE];
		public ItemSlot  MedkitSlot	     => m_itemSlotMap[SLOT_TYPE.MEDKIT];
		public ItemSlot  ConsumableSlot  => m_itemSlotMap[SLOT_TYPE.CONSUMABLE];
		public ItemSlot GetItemSlot(SLOT_TYPE slotType) => m_itemSlotMap[slotType];
		public ItemSlot CurrentItemSlot() => m_itemSlotMap[m_currSlotType];

		void Awake()
		{
			m_owner		  = GetComponent<GameActor>();
			m_inputReader = GetComponent<InputReader>();

			m_numSlotTypes = Enum.GetNames(typeof( SLOT_TYPE )).Length;

			m_itemSlotMap = new Dictionary<SLOT_TYPE, ItemSlot>();
			for (int i = 0; i < m_numSlotTypes; ++i)
			{
				m_itemSlotMap[(SLOT_TYPE)i] = new ItemSlot();
			}
		}

		void OnEnable()
		{
			EventManager.AddListener( GameEvent.OnHealingSuccess, HealingSuccessHandler );
			EventManager.AddListener( GameEvent.OnUseThrowable, OnThrowableUsedHandler );
			EventManager.AddListener( GameEvent.OnActorDeath, ActorDeathHandler );
		}

		void OnDisable()
		{
			EventManager.RemoveListener( GameEvent.OnHealingSuccess, HealingSuccessHandler );
			EventManager.RemoveListener( GameEvent.OnUseThrowable, OnThrowableUsedHandler );
			EventManager.RemoveListener( GameEvent.OnActorDeath, ActorDeathHandler );
		}

		void Update()
		{
			Item currItem = m_itemSlotMap[m_currSlotType].GetItem();

			if (currItem != null)
            {
				HandleUseInput( currItem );
				HandleReloadInput( currItem );
				HandleScopeInput( currItem );
            }

			HandleNumKeyInput();
			HandleScrollInput();
		}

        public void AddItem(Item item)
        {
			if (item == null)
				Debug.LogError("Inventory.cs : AddItem(), Item param is NULL");

			ItemSlot currItemSlot = m_itemSlotMap[item.SlotType];

			// Set item's GameActor owner
			item.SetOwner( m_owner );

			// Init gameObject model
			GameObject currOBJ = currItemSlot.GetItemModel();
			if (currOBJ != null)
				Destroy( currOBJ );

			currOBJ = Instantiate( item.ItemModelPrefab );
			currOBJ.transform.SetParent(_itemHolder);
			currOBJ.transform.localPosition = item.ItemLocalTransform.Position;
			currOBJ.transform.localRotation = Quaternion.Euler( item.ItemLocalTransform.Rotation );
			currOBJ.transform.localScale	= item.ItemLocalTransform.Scale;

			// Set data
			currItemSlot.SetItemData( currOBJ, item );
			item.Init();

			// Add animator (if it's a gun)
			BaseGunItem gunItem = item as BaseGunItem;
			if (gunItem != null)
            {
				Animator animator = currOBJ.AddComponent<Animator>();
				animator.runtimeAnimatorController = gunItem.GunAnimController;
				gunItem.SetAnimator( animator );
			}

			m_itemSlotMap[m_currSlotType].GetItem()?.OnUnequip();
			m_currSlotType = item.SlotType;
			SetCurrentItem( m_currSlotType );
			m_itemSlotMap[m_currSlotType].GetItem()?.OnEquip();
		}

		public void RemoveItem(SLOT_TYPE slotType)
        {
			if (m_itemSlotMap[slotType].GetItem() != null)
            {
				GameObject obj = m_itemSlotMap[slotType].GetItemModel();

				if (obj != null)
					Destroy( obj );

				m_itemSlotMap[slotType].SetItemData( null, null );
				EventManager.RaiseEvent(GameEvent.OnInventoryChanged, new InventoryChangedEventArgs());
			}
        }

		void SetCurrentItem(SLOT_TYPE slotType)
        {
            for (int i = 0; i < m_numSlotTypes; ++i)
            {
				SLOT_TYPE currSlotTypeItr = (SLOT_TYPE)i;
				ItemSlot  currItemSlot	  = m_itemSlotMap[currSlotTypeItr];

				bool active = (currSlotTypeItr == slotType);
				currItemSlot.GetItemModel()?.SetActive( active );
            }

			EventManager.RaiseEvent(GameEvent.OnInventoryChanged, new InventoryChangedEventArgs());
		}

		void HandleUseInput(Item item)
		{
			if (m_inputReader.Fire1Pressed)
            {
				item.OnUseBegin();
				EventManager.RaiseEvent(GameEvent.OnInventoryChanged, new InventoryChangedEventArgs());
			}
			else if (m_inputReader.Fire1Held)
            {
				item.OnUseHeld();
				EventManager.RaiseEvent(GameEvent.OnInventoryChanged, new InventoryChangedEventArgs());
			}
			else if (m_inputReader.Fire1Released)
            {
				item.OnUseReleased();
				EventManager.RaiseEvent(GameEvent.OnInventoryChanged, new InventoryChangedEventArgs());
			}
		}

		void HandleReloadInput(Item item)
		{
			if (m_inputReader.ReloadPressed)
			{
				IReloadable reloadable = item as IReloadable;
				reloadable?.Reload();
			}
		}

		void HandleScopeInput(Item item)
		{
			IScopeable scopeable = item as IScopeable;

			if (scopeable != null)
            {
				if		(m_inputReader.Fire2Pressed)  scopeable.OnScope();
				else if (m_inputReader.Fire2Released) scopeable.OnUnscope();
			}
		}

		void HandleNumKeyInput()
        {
			SLOT_TYPE nextSlotType = m_currSlotType;

			if		(m_inputReader.ItemSlot1Pressed) nextSlotType = SLOT_TYPE.PRIMARY;
			else if (m_inputReader.ItemSlot2Pressed) nextSlotType = SLOT_TYPE.SECONDARY;
			else if (m_inputReader.ItemSlot3Pressed) nextSlotType = SLOT_TYPE.THROWABLE;
			else if (m_inputReader.ItemSlot4Pressed) nextSlotType = SLOT_TYPE.MEDKIT;
			else if (m_inputReader.ItemSlot5Pressed) nextSlotType = SLOT_TYPE.CONSUMABLE;

			if (m_currSlotType != nextSlotType && m_itemSlotMap[nextSlotType].GetItem() != null)
            {
				m_itemSlotMap[m_currSlotType].GetItem()?.OnUnequip();
				m_currSlotType = nextSlotType;
				SetCurrentItem( m_currSlotType );
				m_itemSlotMap[m_currSlotType].GetItem()?.OnEquip();
			}
		}

		void HandleScrollInput()
        {
			if ( m_inputReader.SelectNextWeapon )
            {
				/*int nextIdx = (int)m_currSlotType - 1;
				nextIdx = (nextIdx < 0) ? m_numSlotTypes - 1 : nextIdx;*/
				int nextIdx = GetNextIdx(m_currSlotType, false);

				if (nextIdx >= 0)
                {
					m_itemSlotMap[m_currSlotType].GetItem()?.OnUnequip();
					m_currSlotType = (SLOT_TYPE)nextIdx;
					SetCurrentItem( m_currSlotType );
					m_itemSlotMap[m_currSlotType].GetItem()?.OnEquip();
                }
			}
			else if ( m_inputReader.SelectPrevWeapon )
			{
				/*int nextIdx = (int)m_currSlotType + 1;
				nextIdx = (nextIdx >= m_numSlotTypes) ? 0 : nextIdx;*/
				int nextIdx = GetNextIdx(m_currSlotType, true);

				if (nextIdx >= 0)
                {
					m_itemSlotMap[m_currSlotType].GetItem()?.OnUnequip();
					m_currSlotType = (SLOT_TYPE)nextIdx;
					SetCurrentItem( m_currSlotType );
					m_itemSlotMap[m_currSlotType].GetItem()?.OnEquip();
                }
			}
		}

		int GetNextIdx(SLOT_TYPE currentSlotType, bool inc)
        {
			int maxItr = m_numSlotTypes; 
			int currIdx = (int)currentSlotType;

			if (inc)
            {
				for (int i = 0; i < maxItr; ++i)
				{
					++currIdx;
					currIdx = (currIdx >= (m_numSlotTypes - 1)) ? 0 : currIdx;

					if (m_itemSlotMap[(SLOT_TYPE)currIdx].GetItem() != null)
					{
						return currIdx;
					}
				}
			}
			else
            {
				for (int i = 0; i < maxItr; ++i)
				{
					--currIdx;
					currIdx = (currIdx < 0) ? m_numSlotTypes - 1 : currIdx;

					if (m_itemSlotMap[(SLOT_TYPE)currIdx].GetItem() != null)
					{
						return currIdx;
					}
				}
			}

			return -1;
		}

		void HealingSuccessHandler(EventArgs eventArgs)
        {
			HealingSuccessEventArgs args = (HealingSuccessEventArgs)eventArgs;

			if (args.Actor == this.m_owner)
            {
				ItemSlot healthSlot = m_itemSlotMap[SLOT_TYPE.MEDKIT];

				if (healthSlot != null)
				{
					GameObject medkitOBJ = healthSlot.GetItemModel();
					if (medkitOBJ != null)
						Destroy( medkitOBJ );

					healthSlot.SetItemData( null, null );
				}
            }
        }

		void OnThrowableUsedHandler(EventArgs eventArgs)
        {
			ThrowableUsedEventArgs args = eventArgs as ThrowableUsedEventArgs;

			if (args.Thrower == this.m_owner)
				RemoveItem( SLOT_TYPE.THROWABLE );
        }

		public void OnPickupAmmo()
        {
			BaseGunItem gunItem = m_itemSlotMap[m_currSlotType].GetItem() as BaseGunItem;
			gunItem?.RefillAmmo();
        }

		void ActorDeathHandler(EventArgs eventArgs)
        {
			ActorDeathEventArgs args = eventArgs as ActorDeathEventArgs;

			if (args != null && args.Actor == this.m_owner)
				ClearInventory();
        }

		void ClearInventory()
        {
			for (int i = 0; i < m_numSlotTypes; ++i)
            {
				SLOT_TYPE slotType = (SLOT_TYPE)i;
				RemoveItem( slotType );
            }
        }
	}
}
