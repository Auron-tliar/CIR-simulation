using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RobotAcademy : Academy
{
    public List<GameObject> RoomPrefabs;

    public List<RoomController> Rooms;

    public List<Sprite> QRCodes;

    public List<Material> WallMaterials;
    public List<Material> FloorMaterials;

    [Header("Boxes")]
    public GameObject TargetBoxPrefab;
    public GameObject ObstacleBoxPrefab;
    public float MinBoxSize = 0.3f;
    public float MaxBoxSize = 1f;
    public int MinTargetCount = 1;
    public int MaxTargetCount = 1;
    public int MinObstacleCount = 1;
    public int MaxObstacleCount = 5;

    [Header("Lights")]
    public GameObject LightPrefab;

    public void Start()
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            Rooms[i].RoomId = i;
        }
    }

    public override void AcademyReset()
    {
        base.AcademyReset();

        if (MaxTargetCount > QRCodes.Count)
        {
            MaxTargetCount = QRCodes.Count;
        }

        int wallInd;
        int floorInd;
        foreach (RoomController room in Rooms)
        {
            wallInd = Random.Range(0, WallMaterials.Count);
            floorInd = Random.Range(0, FloorMaterials.Count);
            room.ResetRoom(WallMaterials[wallInd], FloorMaterials[floorInd], Random.Range(MinTargetCount, MaxTargetCount),
                Random.Range(MinObstacleCount, MaxObstacleCount));
        }
    }

    public void ResetRoomId(int id)
    {
        int wallInd = Random.Range(0, WallMaterials.Count);
        int floorInd = Random.Range(0, FloorMaterials.Count);

        Rooms[id].ResetRoom(WallMaterials[wallInd], FloorMaterials[floorInd], Random.Range(MinTargetCount, MaxTargetCount),
                Random.Range(MinObstacleCount, MaxObstacleCount));
    }
}
