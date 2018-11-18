using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script for sample scene to spawn objects.
 * 
 * Author: github.com/S1r0hub
 */
public class SpawnObject : MonoBehaviour {

    public uint maxObjects = 20;
    public Vector3 location = new Vector3(0, 2, 4);
    public Transform parent;
    private Queue<GameObject> objects = new Queue<GameObject>();

    public void Spawn(GameObject prefab) {
        GameObject newObj = Instantiate(prefab, location, prefab.transform.rotation);
        if (parent) { newObj.transform.SetParent(parent); }

        // remove old objects if limit reached
        objects.Enqueue(newObj);
        if (objects.Count > maxObjects) {
            Destroy(objects.Dequeue());
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(location, 0.5f);
    }
}
