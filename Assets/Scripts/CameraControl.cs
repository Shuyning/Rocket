using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Player;

    private Vector3 offSet;

    // Start is called before the first frame update
    void Start()
    {
        offSet = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Player.transform.position + offSet;
    }
}
