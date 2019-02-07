using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public bool ManualControl = true;

    public WheelCollider FrontLeftWheel, FrontRightWheel, BackLeftWheel, BackRightWheel;

    public float TorqueModifier = 1f;

    // In case of autonomous driving these values are used for control, each sets the power to the
    // corresponding motor. Probably it might be wise to either clamp it in ML code to [-1f, 1f] or
    // even to {-1f, 0f, 1f}.
    //[HideInInspector]
    //public float flValue = 0f, frValue = 0f, blValue = 0f, brValue = 0f;
    [HideInInspector]
    public float LeftValue = 0f, RightValue = 0f;

    private Rigidbody _rigidbody;
    private Transform FLWheelTransform, FRWheelTransform, BLWheelTransform, BRWheelTransform;

    private void Awake()
    {
        FLWheelTransform = FrontLeftWheel.transform;
        FRWheelTransform = FrontRightWheel.transform;
        BLWheelTransform = BackLeftWheel.transform;
        BRWheelTransform = BackRightWheel.transform;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _rigidbody.centerOfMass - new Vector3(0f, 0.02f, 0f);
    }

    private void FixedUpdate()
    {
        if (ManualControl)
        {/*
            bool lf = Input.GetButton("LeftFull");
            bool lh = Input.GetButton("LeftHalf");
            bool rf = Input.GetButton("RightFull");
            bool rh = Input.GetButton("RightHalf");

            if (lf)
            {
                FrontLeftWheel.motorTorque = TorqueModifier;
                BackLeftWheel.motorTorque = TorqueModifier;
            }
            else if (lh)
            {
                FrontLeftWheel.motorTorque = 0.5f * TorqueModifier;
                BackLeftWheel.motorTorque = 0.5f * TorqueModifier;
            }

            if (rf)
            {
                FrontRightWheel.motorTorque = TorqueModifier;
                BackRightWheel.motorTorque = TorqueModifier;
            }
            else if (rh)
            {
                FrontRightWheel.motorTorque = 0.5f * TorqueModifier;
                BackRightWheel.motorTorque = 0.5f * TorqueModifier;
            }*/

            //float z = Input.GetAxisRaw("Vertical");
            //float x = Input.GetAxisRaw("Horizontal");
            float l = Input.GetAxisRaw("LeftWheels");
            float r = Input.GetAxisRaw("RightWheels");

            if (l != 0)
            {
                FrontLeftWheel.motorTorque = l * TorqueModifier;
                //FrontRightWheel.motorTorque = z * TorqueModifier;
                BackLeftWheel.motorTorque = l * TorqueModifier;
                //BackRightWheel.motorTorque = z * TorqueModifier;
            }
            else
            {
                FrontLeftWheel.motorTorque = 0f;
                BackLeftWheel.motorTorque = 0f;
            }

            if (r != 0)
            {
                //FrontLeftWheel.motorTorque = x * TorqueModifier;
                FrontRightWheel.motorTorque = r * TorqueModifier;
                //BackLeftWheel.motorTorque = x * TorqueModifier;
                BackRightWheel.motorTorque = r * TorqueModifier;
            }
            else
            {
                FrontRightWheel.motorTorque = 0f;
                BackRightWheel.motorTorque = 0f;
            }
        }
        else
        {
            //Debug.Log("Left: " + LeftValue + ", \tRight: " + RightValue);
            FrontLeftWheel.motorTorque = LeftValue * TorqueModifier;
            FrontRightWheel.motorTorque = RightValue * TorqueModifier;
            BackLeftWheel.motorTorque = LeftValue * TorqueModifier;
            BackRightWheel.motorTorque = RightValue * TorqueModifier;
        }

        Vector3 pos;
        Quaternion rot;
        FrontLeftWheel.GetWorldPose(out pos, out rot);
        FLWheelTransform.rotation = rot;

        FrontRightWheel.GetWorldPose(out pos, out rot);
        FRWheelTransform.rotation = rot;

        BackLeftWheel.GetWorldPose(out pos, out rot);
        BLWheelTransform.rotation = rot;

        BackRightWheel.GetWorldPose(out pos, out rot);
        BRWheelTransform.rotation = rot;

        _rigidbody.AddForce(new Vector3(0f, -100f, 0f));
    }
}
