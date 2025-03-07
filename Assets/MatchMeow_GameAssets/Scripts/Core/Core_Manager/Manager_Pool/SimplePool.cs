﻿///
/// Simple pooling for Unity.
///   Author: Martin "quill18" Glaude (quill18@quill18.com)
///   Latest Version: https://gist.github.com/quill18/5a7cfffae68892621267
///   License: CC0 (http://creativecommons.org/publicdomain/zero/1.0/)
///   UPDATES:
/// 	2015-04-16:
/// Support Minh tito CTO ABI games studio
/// Advantage Linh soi game developer
///   UPDATES:
///     2017-09-10
///     - simple pool with gameobject
///     - release game object
///     2019-10-09
///     - Pool Clamp to keep the quantity within a certain range
///     - Pool collect all to despawn all object comeback the pool
///     - Spawn with generic T
///     - Optimize pool
///     2022-10-09
///     - Pool with pool type from resources
///     - pool with pool container
///     2022-11-27
///     - Remove clamp pool
///     - Spawn in parent transform same instantiate(gameobject, transform)
///     - Get list object is actived

using UnityEngine;
using System.Collections.Generic;
using System;
using _Pool.Pool;

public static class SimplePool
{
    class Pool
    {
        //parent contain all pool member
        Transform m_sRoot = null;
        //object is can collect back to pool
        bool m_collect;
        //list object in pool
        Queue<PoolUnit> m_inactive;
        //collect obj active ingame
        HashSet<PoolUnit> m_active;
        // The prefab that we are pooling
        PoolUnit m_prefab;

        public bool IsCollect { get => m_collect; }
        public HashSet<PoolUnit> Active => m_active;
        public int Count => m_inactive.Count + m_active.Count;
        public Transform Root => m_sRoot;

        // Constructor
        public Pool(PoolUnit prefab, int initialQty, Transform parent, bool collect)
        {
            m_inactive = new Queue<PoolUnit>(initialQty);
            m_sRoot = parent;
            this.m_prefab = prefab;
            m_collect = collect;
            if (m_collect) m_active = new HashSet<PoolUnit>();
        }

        // Spawn an object from our pool with position and rotation
        public PoolUnit Spawn(Vector3 pos, Quaternion rot)
        {
            PoolUnit obj = Spawn();

            obj.TF.SetPositionAndRotation(pos, rot);

            return obj;
        }

        //spawn PoolUnit
        public PoolUnit Spawn()
        {
            PoolUnit obj;
            if (m_inactive.Count == 0)
            {
                obj = (PoolUnit)GameObject.Instantiate(m_prefab, m_sRoot);
            }
            else
            {
                // Grab the last object in the inactive array
                obj = m_inactive.Dequeue();

                if (obj == null)
                {
                    return Spawn();
                }
            }

            if (m_collect) m_active.Add(obj);

            obj.gameObject.SetActive(true);

            return obj;
        }

        // Return an object to the inactive pool.
        public void Despawn(PoolUnit obj)
        {
            if (obj != null /*&& !inactive.Contains(obj)*/)
            {
                obj.gameObject.SetActive(false);
                m_inactive.Enqueue(obj);

                if (memberInParent.Contains(obj.GetInstanceID()))
                {
                    obj.TF.SetParent(GetPool(obj).Root);
                    memberInParent.Remove(obj.GetInstanceID());
                }
            }

            if (m_collect) m_active.Remove(obj);
        }

        //destroy all unit in pool
        public void Release()
        {
            while (m_inactive.Count > 0)
            {
                PoolUnit go = m_inactive.Dequeue();
                GameObject.DestroyImmediate(go);
            }
            m_inactive.Clear();
        }

        //collect all unit comeback to pool
        public void Collect()
        {
            //while (m_active.Count > 0)
            //{
            //    Despawn(m_active[0]);
            //}


            HashSet<PoolUnit> units = new HashSet<PoolUnit>(m_active);
            foreach (var item in units)
            {
                Despawn(item);
            }
        }
    }

    public const int DEFAULT_POOL_SIZE = 3;

    //dict for faster search from pool type to prefab
    static Dictionary<PoolType, PoolUnit> poolTypes = new Dictionary<PoolType, PoolUnit>();

    //save member that is child transform other object
    static HashSet<int> memberInParent = new HashSet<int>();

    private static Transform root;

    public static Transform Root
    {
        get
        {
            if (root == null)
            {
                PoolControler controler = GameObject.FindObjectOfType<PoolControler>();
                root = controler != null ? controler.transform : new GameObject("Pool").transform;
            }

            return root;
        }
    }

    // All of our pools
    static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    // preload object and pool
    static public void Preload(PoolUnit prefab, int qty = 1, Transform parent = null, bool collect = false)
    {
        if (!poolTypes.ContainsKey(prefab.poolType))
        {
            poolTypes.Add(prefab.poolType, prefab);
        }

        if (prefab == null)
        {
            Debug.LogError(parent.name + " : IS EMPTY!!!");
            return;
        }

        InitPool(prefab, qty, parent, collect);

        // Make an array to grab the objects we're about to pre-spawn.
        PoolUnit[] obs = new PoolUnit[qty];
        for (int i = 0; i < qty; i++)
        {
            obs[i] = Spawn(prefab);
        }

        // Now despawn them all.
        for (int i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }
    }

    // init pool
    static void InitPool(PoolUnit prefab = null, int qty = DEFAULT_POOL_SIZE, Transform parent = null, bool collect = false)
    {
        if (prefab != null && !IsHasPool(prefab))
        {
            poolInstance.Add(prefab.poolType, new Pool(prefab, qty, parent, collect));
        }
    }
    static private bool IsHasPool(PoolUnit obj)
    {
        return poolInstance.ContainsKey(obj.poolType);
    }
    static private Pool GetPool(PoolUnit obj)
    {
        return poolInstance[obj.poolType];
    }
    public static PoolUnit GetPrefabByType(PoolType poolType)
    {
        if (!poolTypes.ContainsKey(poolType) || poolTypes[poolType] == null)
        {
            PoolUnit[] resources = Resources.LoadAll<PoolUnit>("Pool");

            for (int i = 0; i < resources.Length; i++)
            {
                poolTypes[resources[i].poolType] = resources[i];
            }
        }

        return poolTypes[poolType];
    }

    #region Get List object ACTIVE
    // get all member is active in game
    public static HashSet<PoolUnit> GetAllUnitIsActive(PoolUnit obj)
    {
        return IsHasPool(obj) ? GetPool(obj).Active : new HashSet<PoolUnit>();
    }
    public static HashSet<PoolUnit> GetAllUnitIsActive(PoolType poolType)
    {
        return GetAllUnitIsActive(GetPrefabByType(poolType));
    }  

    #endregion

    #region Spawn
    // Spawn Unit to use
    static public T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : PoolUnit
    {
        return Spawn(GetPrefabByType(poolType), pos, rot) as T;
    }
    static public T Spawn<T>(PoolType poolType) where T : PoolUnit
    {
        return Spawn<T>(GetPrefabByType(poolType));
    }
    static public T Spawn<T>(PoolUnit obj, Vector3 pos, Quaternion rot) where T : PoolUnit
    {
        return Spawn(obj, pos, rot) as T;
    }
    static public T Spawn<T>(PoolUnit obj) where T : PoolUnit
    {
        return Spawn(obj) as T;
    }

    // spawn PoolUnit with transform parent
    static public T Spawn<T>(PoolUnit obj, Transform parent) where T : PoolUnit
    {
        return Spawn<T>(obj, obj.TF.localPosition, obj.TF.localRotation, parent);
    }

    static public T Spawn<T>(PoolUnit obj, Vector3 localPoint, Quaternion localRot, Transform parent) where T : PoolUnit
    {
        T unit = Spawn<T>(obj);
        unit.TF.SetParent(parent);
        unit.TF.localPosition = localPoint;
        unit.TF.localRotation = localRot;
        unit.TF.localScale = Vector3.one;
        memberInParent.Add(unit.GetInstanceID());
        return unit;
    }

    static public T Spawn<T> (PoolType poolType, Vector3 localPoint, Quaternion localRot, Transform parent) where T : PoolUnit
    {
        return Spawn<T>(GetPrefabByType(poolType), localPoint, localRot, parent);
    }
     static public T Spawn<T> (PoolType poolType, Transform parent) where T : PoolUnit
    {
        return Spawn<T>(GetPrefabByType(poolType), parent);
    }

    static public PoolUnit Spawn(PoolUnit obj, Vector3 pos, Quaternion rot)
    {
        if (!IsHasPool(obj))
        {
            Transform newRoot = new GameObject(obj.name).transform;
            newRoot.SetParent(Root);
            Preload(obj, 1, newRoot, true);
        }

        return GetPool(obj).Spawn(pos, rot);
    }
    static public PoolUnit Spawn(PoolUnit obj)
    {
        if (!IsHasPool(obj))
        {
            Transform newRoot = new GameObject(obj.name).transform;
            newRoot.SetParent(Root);
            Preload(obj, 1, newRoot, true);
        }

        return GetPool(obj).Spawn();
    }
    #endregion

    #region Despawn
    //take PoolUnit to pool
    static public void Despawn(PoolUnit obj)
    {
        if (obj.gameObject.activeSelf)
        {
            if (IsHasPool(obj))
            {
                GetPool(obj).Despawn(obj);
            }
            else
            {
                GameObject.Destroy(obj.gameObject);
            }
        }
    }
    #endregion

    #region Release
    //destroy pool
    static public void Release(PoolUnit obj)
    {
        if (IsHasPool(obj))
        {
            GetPool(obj).Release();
        }
    }
    static public void Release(PoolType poolType)
    {
        Release(GetPrefabByType(poolType));
    }

    //DESTROY ALL POOL
    static public void ReleaseAll()
    {
        foreach (var item in poolInstance)
        {
            item.Value.Release();
        }
    }
    #endregion

    #region Collect
    //collect all pool member comeback to pool
    static public void Collect(PoolUnit obj)
    {
        if (IsHasPool(obj)) GetPool(obj).Collect();
    }
    static public void Collect(PoolType poolType)
    {
        Collect(GetPrefabByType(poolType));
    }

    //COLLECT ALL POOL
    static public void CollectAll()
    {
        foreach (var item in poolInstance)
        {
            if (item.Value.IsCollect)
            {
                item.Value.Collect();
            }
        }
    }
    #endregion
}
