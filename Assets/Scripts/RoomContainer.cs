using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomContainer : MonoBehaviour
{
    [System.Serializable]
    public class Bounds
    {
        public float XMin, XMax, ZMin, ZMax;
    }

    public Bounds RoomBounds; 
    //public List<NavMeshSurface> NavMeshSurfaces;
    public List<GameObject> Walls;
    public List<GameObject> Floors;
    public int OverrideMaxBoxCount = 100;
    public int MinLightCount = 2;
    public int MaxLightCount = 4;
}
