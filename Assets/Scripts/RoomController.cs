using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomController : MonoBehaviour
{
    public RobotAgent RoboticCar;

    [Header("Boxes")]
    public Transform TargetContainer;
    public Transform ObstacleContainer;

    public Transform LightContainer;

    [HideInInspector]
    public int RoomId;
    [HideInInspector]
    public List<Transform> Boxes;
    [HideInInspector]
    public RoomContainer Room;

    private RobotAcademy _robotAcademy;

    private void Start()
    {
        _robotAcademy = GameObject.FindGameObjectWithTag("GameController").GetComponent<RobotAcademy>();
    }

    public void ResetRoom(Material wallMaterial, Material floorMaterial, int targetCount, int obstacleCount)
    {
        try
        {
            Destroy(Room.gameObject);
        }
        catch (System.NullReferenceException ex) { }

        Room = Instantiate(_robotAcademy.RoomPrefabs[Random.Range(0, _robotAcademy.RoomPrefabs.Count)],
            transform).GetComponent<RoomContainer>();

        if (targetCount > Room.OverrideMaxBoxCount)
        {
            targetCount = Room.OverrideMaxBoxCount;
        }
        if (obstacleCount > Room.OverrideMaxBoxCount)
        {
            obstacleCount = Room.OverrideMaxBoxCount;
        }

        foreach (GameObject w in Room.Walls)
        {
            w.GetComponent<Renderer>().material = wallMaterial;
        }

        foreach (GameObject f in Room.Floors)
        {
            f.GetComponent<Renderer>().material = floorMaterial;
        }

        Boxes = new List<Transform>();

        GameObject tempObject;
        Vector3 scale;
        foreach(Transform child in TargetContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < targetCount; i++)
        {
            scale = new Vector3(Random.Range(_robotAcademy.MinBoxSize, _robotAcademy.MaxBoxSize), 
                Random.Range(_robotAcademy.MinBoxSize, _robotAcademy.MaxBoxSize),
                Random.Range(_robotAcademy.MinBoxSize, _robotAcademy.MaxBoxSize));
            tempObject = Instantiate(_robotAcademy.TargetBoxPrefab, transform.position + RandomPosition(scale.y / 2f),
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), TargetContainer);
            tempObject.GetComponent<Target>().SetSize(scale);
            if (i == 0)
            {
                float[] sizes = new float[] { scale.x, scale.y, scale.z };
                RoboticCar.SetTargetDistance(Mathf.Max(sizes));
                RoboticCar.Target = tempObject.transform;
            }
            tempObject.GetComponent<Target>().Set(_robotAcademy.QRCodes[i]);
            Boxes.Add(tempObject.transform);
        }
        
        foreach (Transform child in ObstacleContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < obstacleCount; i++)
        {
            scale = new Vector3(Random.Range(_robotAcademy.MinBoxSize, _robotAcademy.MaxBoxSize),
                Random.Range(_robotAcademy.MinBoxSize, _robotAcademy.MaxBoxSize),
                Random.Range(_robotAcademy.MinBoxSize, _robotAcademy.MaxBoxSize));
            tempObject = Instantiate(_robotAcademy.ObstacleBoxPrefab, transform.position + RandomPosition(scale.y / 2f),
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), ObstacleContainer);
            tempObject.transform.localScale = scale;
            Boxes.Add(tempObject.transform);
        }

        int lightCount = Random.Range(Room.MinLightCount, Room.MaxLightCount);
        foreach (Transform child in LightContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < lightCount; i++)
        {
            tempObject = Instantiate(_robotAcademy.LightPrefab,
                transform.position + RandomPosition(_robotAcademy.LightPrefab.transform.position.y),
                Quaternion.Euler(_robotAcademy.LightPrefab.transform.rotation.eulerAngles.x, Random.Range(0f, 360f),
                _robotAcademy.LightPrefab.transform.rotation.eulerAngles.z), LightContainer);
        }

        RoboticCar.ResetPosition();
        /*foreach (NavMeshSurface s in Room.NavMeshSurfaces)
        {
            s.BuildNavMesh();
        }*/
    }

    private Vector3 RandomPosition(float ySize)
    {
        return new Vector3(Random.Range(Room.RoomBounds.XMin, Room.RoomBounds.XMax), ySize,
            Random.Range(Room.RoomBounds.ZMin, Room.RoomBounds.ZMax));
    }
}
