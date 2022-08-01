using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private List<PoolItem> poolItems;
        private Dictionary<EnvironmentalItemTypes, List<GameObject>> pools;

        public static ObjectPool Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            pools = new Dictionary<EnvironmentalItemTypes, List<GameObject>>();
            for (var i = 0; i < poolItems.Count; i++)
            {
                List<GameObject> pooledObjects = new List<GameObject>();
                for (int j = 0; j < poolItems[i].Amount; j++)
                {
                    var tempItem = Instantiate(poolItems[i].Prefab, transform, true);
                    tempItem.SetActive(false);
                    pooledObjects.Add(tempItem);
                }

                pools.Add(poolItems[i].Type, pooledObjects);
            }
        }

        public GameObject GetPooledObject(EnvironmentalItemTypes type, Vector3 position, Vector3 rotation)
        {
            for (int i = 0; i < pools[type].Count; i++)
            {
                if (!pools[type][i].activeInHierarchy)
                {
                    pools[type][i].transform.position = position;
                    pools[type][i].transform.eulerAngles = rotation;
                    pools[type][i].SetActive(true);
                    return pools[type][i];
                }
            }

            return null;
        }

        public void PutInPool(GameObject obj)
        {
            obj.SetActive(false);
        }

    }


    [System.Serializable]
    public struct PoolItem
    {
        [SerializeField] EnvironmentalItemTypes type;
        [SerializeField] GameObject prefab;
        [SerializeField] int amount;

        public EnvironmentalItemTypes Type
        {
            get => type;
            set => type = value;
        }

        public GameObject Prefab
        {
            get => prefab;
            set => prefab = value;
        }

        public int Amount
        {
            get => amount;
            set => amount = value;
        }
    }

    [System.Serializable]
    public enum EnvironmentalItemTypes
    {
        Null,
        Road,
        Cube,
        Shoe,
        Diamond,
        Finish
    }
}