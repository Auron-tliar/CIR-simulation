using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RobotAgent : Agent
{
    public bool IsDiscrete = false;

    public RoomController Room;
    public Transform Target;
    public Camera NormalCamera;
    public string TargetName;
    public float SpawnDistance = 0.3f;

    public float BaseTargetDistance = 1f;

    [Header("Rewards")]
    public float TargetReward = 10000f;
    public float TargetInSightReward = 1f;
    public float TargetDistanceRewardCoef = 1f;
    public float TimestepPenalty = 2f;
    public float CollisionPenalty = 1000f;
    public float ContactPenalty = 250f;

    [HideInInspector]
    public string CurrentQR;
    [HideInInspector]
    public float NextReward = -1f;
    //[HideInInspector]
    //public string TargetPosition;


    private RobotController _controller;
    private RobotAcademy _academy;

    private Rigidbody _rigidbody;

    private float _targetDistance;

    private int _numActions;
    private float _velStep = 1f;

    private bool _collisionFlag = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _academy = GameObject.FindGameObjectWithTag("GameController").GetComponent<RobotAcademy>();
        _controller = GetComponent<RobotController>();

        if (brain != null)
        {
            _numActions = brain.brainParameters.vectorActionSize[0];
            _numActions = (_numActions - 1) / 2;
            _velStep = 1f / _numActions;
        }
    }

    public void SetTargetDistance(float size)
    {
        _targetDistance = BaseTargetDistance + size;
    }

    public override void AgentReset()
    {
        base.AgentReset();

        _academy.ResetRoomId(Room.RoomId);
    }

    public void ResetPosition()
    {
        bool isCollision;
        do
        {
            //Debug.Log("Got here");
            isCollision = false;
            transform.position = Room.transform.position +
                new Vector3(Random.Range(Room.Room.RoomBounds.XMin + SpawnDistance,
                Room.Room.RoomBounds.XMax), transform.position.y,
                Random.Range(Room.Room.RoomBounds.ZMin + SpawnDistance, Room.Room.RoomBounds.ZMax));
            transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            foreach (Transform t in Room.Boxes)
            {
                if ((transform.position - t.position).magnitude < (SpawnDistance + t.localScale.x))
                {
                    isCollision = true;
                    break;
                }
            }
        }
        while (isCollision);

        NextReward = -TimestepPenalty;
    }

    public override void CollectObservations()
    {
        AddVectorObs(_controller.LeftValue);
        AddVectorObs(_controller.RightValue);
        AddVectorObs(TargetName == CurrentQR);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (IsDiscrete)
        {
            //Debug.Log("Action 1: " + vectorAction[0] + ", \tAction 2: " + vectorAction[1]);
            int left = (int)vectorAction[0];
            int right = (int)vectorAction[1];

            //left
            if (left == 0)
            {
                _controller.LeftValue = 0f;
            }
            else if (left > _numActions)
            {
                left -= _numActions;
                left *= -1;
            }

            if (left == _numActions)
            {
                _controller.LeftValue = 1f;
            }
            else if (left == -_numActions)
            {
                _controller.LeftValue = -1f;
            }
            else
            {
                _controller.LeftValue = _velStep * left;
            }

            // right
            if (right == 0)
            {
                _controller.RightValue = 0f;
            }
            else if (right > _numActions)
            {
                right -= _numActions;
                right *= -1;
            }

            if (right == _numActions)
            {
                _controller.RightValue = 1f;
            }
            else if (right == -_numActions)
            {
                _controller.RightValue = -1f;
            }
            else
            {
                _controller.RightValue = _velStep * right;
            }
        }
        else
        {
            float left = Mathf.Clamp(vectorAction[0], -1f, 1f);
            if (Mathf.Abs(left) < 0.1)
            {
                left = 0f;
            }
            _controller.LeftValue = left;
            float right = Mathf.Clamp(vectorAction[1], -1f, 1f);
            if (Mathf.Abs(right) < 0.1)
            {
                right = 0f;
            }
            _controller.RightValue = Mathf.Clamp(right, -1f, 1f);
            Debug.Log("Left: " + left + ". \tRight: " + right);
            //Debug.Log("Right: " + right);
        }

        _collisionFlag = false;
        Vector3 screenPoint = NormalCamera.WorldToViewportPoint(Target.position);
        if (Vector3.Distance(transform.position, Target.position) <= _targetDistance)
        {
            NextReward += TargetReward;
            Debug.Log("Reward: " + NextReward);
            SetReward(NextReward);
            Done();
            return;
        }
        else if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            NextReward += TargetInSightReward;
            //if (TargetName == CurrentQR)
            //{
            //    NextReward += 0.5f;
            //}
        }

        NextReward += TargetDistanceRewardCoef / Vector3.Distance(transform.position, Target.position);
        Debug.Log("Reward: " + NextReward);
        SetReward(NextReward);

        NextReward = -TimestepPenalty;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Box" || collision.transform.tag == "Wall" && !_collisionFlag)
        {
            if (collision.impulse.magnitude >= 1f)
            {
                NextReward -= CollisionPenalty;
            }
            _collisionFlag = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Box" || collision.transform.tag == "Wall" && !_collisionFlag)
        {
            NextReward -= ContactPenalty;
            _collisionFlag = true;
        }
    }
}
