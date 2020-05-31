using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefabs : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] float lifeTime;

    [SerializeField] Transform pointSpawn;

    static public InstantiatePrefabs _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public void Instance()
    {
        GameObject instatiateGO = Instantiate(prefab, pointSpawn.transform.position, Quaternion.identity);

        if(lifeTime > 0f)
        {
            DestroyObject(instatiateGO, lifeTime);
        }
    }

   



}
