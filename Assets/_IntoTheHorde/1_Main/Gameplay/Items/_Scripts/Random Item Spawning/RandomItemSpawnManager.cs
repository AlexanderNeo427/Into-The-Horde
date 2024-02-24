using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    public class RandomItemSpawnManager : Singleton<RandomItemSpawnManager>, IRegisterable<ItemSpawnLocation>
    {
        [SerializeField] PickupInteraction[] _primaryItems;
        [SerializeField] PickupInteraction[] _secondaryItems;
        [SerializeField] PickupInteraction[] _throwableItems;
        [SerializeField] PickupInteraction[] _medkitItems;
        [SerializeField] PickupInteraction[] _consumableItems;

        List<ItemSpawnLocation> m_itemSpawnLocations = new List<ItemSpawnLocation>();

        void Start() => PopulateItemSpawnLocations();

        public void PopulateItemSpawnLocations()
        {
            foreach (ItemSpawnLocation spawnLocation in m_itemSpawnLocations)
            {
                var spawnableItemArrays = new List<PickupInteraction[]>();

                if (spawnLocation.CanSpawnPrimary)    spawnableItemArrays.Add(_primaryItems);
                if (spawnLocation.CanSpawnSecondary)  spawnableItemArrays.Add(_secondaryItems);
                if (spawnLocation.CanSpawnThrowable)  spawnableItemArrays.Add(_throwableItems);
                if (spawnLocation.CanSpawnMedkit)     spawnableItemArrays.Add(_medkitItems);
                if (spawnLocation.CanSpawnConsumable) spawnableItemArrays.Add(_consumableItems);

                int numItemTypes = spawnableItemArrays.Count;
                PickupInteraction[] spawnableItems = spawnableItemArrays[Random.Range(0, numItemTypes - 1)];

                int itemIndex = Random.Range( 0, spawnableItems.Length );
                PickupInteraction objToSpawn = spawnableItems[itemIndex];
                Instantiate( objToSpawn, spawnLocation.Position, Quaternion.identity );
            }
        }

        public void Register(ItemSpawnLocation location) => m_itemSpawnLocations.Add( location );

        public void Unregister(ItemSpawnLocation location)
        {
            if (m_itemSpawnLocations.Contains( location ))
                m_itemSpawnLocations.Remove( location );
        }
    }
}
