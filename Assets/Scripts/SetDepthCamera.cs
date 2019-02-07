using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDepthCamera : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}
