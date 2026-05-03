using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; } // Singleton

    private Dictionary<string, List<GameObject>> _pools = new Dictionary<string, List<GameObject>>(); // Associo nome Object-Prefab a Lista

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Check dizionario e se esiste gia' creato, riciclo. Creazione solo se non ci sono oggetti liberi
    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string key = prefab.name;

        if (!_pools.ContainsKey(key))
        {
            _pools.Add(key, new List<GameObject>()); // Gestione automatica
        }

        foreach (GameObject obj in _pools[key]) // Riciclo
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(prefab, position, rotation); // Creazione che poi tendera' a non servire grazie al LOOP di ricili

        _pools[key].Add(newObj);

        return newObj;
    }
}
