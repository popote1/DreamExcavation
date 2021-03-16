using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class testMultiple : MonoBehaviour
{
    public GameObject tested;

    public int whide;
    public int height;
    public float Cellsize;
    public int nomberTested;

    private GameObject[,] objects= new GameObject[0, 0];


    [ContextMenu("Génératobject")]
    public void GeneratNewObjects()
    {
        if (objects.Length == 0)
        {
            objects = new GameObject[whide,height];
            for (int x = 0; x < whide; x++) {
                for (int y = 0; y < height; y++)
                {
                    objects[x,y]=Instantiate(tested, new Vector3(x,0, y) * Cellsize, quaternion.identity);
                    objects[x, y].SetActive(false);
                }
            }
            nomberTested = height * whide;
        }
        else
        {
            Debug.Log("netoyez le terrain avant de recréer");
        }
    }

    [ContextMenu(" NetoyerLeBord")]
    public void NetoyerLeBord()
    {
        foreach (var obj in objects)
        {
            Destroy(obj, 0.1f);
        }

        objects = new GameObject[0, 0];

    }
}
