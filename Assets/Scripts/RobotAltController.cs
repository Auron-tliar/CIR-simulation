using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAltController : MonoBehaviour
{
    public Rigidbody FrontLeftWheel, FrontRightWheel, BackLeftWheel, BackRightWheel;

    public float TorqueModifier = 1f;

    private Rigidbody _rigidbody;
    private Transform FLWheelTransform, FRWheelTransform, BLWheelTransform, BRWheelTransform;

    private void Awake()
    {
        FLWheelTransform = FrontLeftWheel.transform;
        FRWheelTransform = FrontRightWheel.transform;
        BLWheelTransform = BackLeftWheel.transform;
        BRWheelTransform = BackRightWheel.transform;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _rigidbody.centerOfMass - new Vector3(0f, 0.01f, 0f);
    }

    private void FixedUpdate()
    {
        float z = Input.GetAxisRaw("CarForward");
        float x = Input.GetAxisRaw("CarSideways");
        float fl = Input.GetAxisRaw("FLWheel");
        float fr = Input.GetAxisRaw("FRWheel");
        float bl = Input.GetAxisRaw("BLWheel");
        float br = Input.GetAxisRaw("BRWheel");

        if (z != 0)
        {
            StopBraking();

            FrontLeftWheel.AddTorque(new Vector3(z * TorqueModifier, 0f, 0f));
            FrontRightWheel.AddTorque(new Vector3(z * TorqueModifier, 0f, 0f));
            BackLeftWheel.AddTorque(new Vector3(z * TorqueModifier, 0f, 0f));
            BackRightWheel.AddTorque(new Vector3(z * TorqueModifier, 0f, 0f));
        }
        else if (x != 0)
        {
            StopBraking();

            FrontLeftWheel.AddTorque(new Vector3(x * TorqueModifier, 0f, 0f));
            FrontRightWheel.AddTorque(new Vector3(- x * TorqueModifier, 0f, 0f));
            BackLeftWheel.AddTorque(new Vector3(x * TorqueModifier, 0f, 0f));
            BackRightWheel.AddTorque(new Vector3(x * TorqueModifier, 0f, 0f));
        }
        else
        {
            Brake();
            //FrontLeftWheel.motorTorque = fl * TorqueModifier;
            //FrontRightWheel.motorTorque = fr * TorqueModifier;
            //BackLeftWheel.motorTorque = bl * TorqueModifier;
            //BackRightWheel.motorTorque = br * TorqueModifier;
        }

        _rigidbody.AddForce(new Vector3(0f, -1f, 0f));
    }

    private void Brake()
    {
    //    FrontLeftWheel.brakeTorque = FrontLeftWheel.motorTorque;
    //    FrontRightWheel.brakeTorque = FrontRightWheel.motorTorque;
    //    BackLeftWheel.brakeTorque = BackLeftWheel.motorTorque;
    //    BackRightWheel.brakeTorque = BackRightWheel.motorTorque;
    }

    private void StopBraking()
    {
    //    FrontLeftWheel.brakeTorque = 0f;
    //    FrontRightWheel.brakeTorque = 0f;
    //    BackLeftWheel.brakeTorque = 0f;
    //    BackRightWheel.brakeTorque = 0f;
    }
}
