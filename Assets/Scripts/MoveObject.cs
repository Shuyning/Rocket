using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MoveObject : MonoBehaviour
{
    [SerializeField] Vector3 movePosition;
    [SerializeField] float moveSpeed;
    [SerializeField] [Range(0, 1)] float movePtogress;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movePtogress = Mathf.PingPong(Time.time * moveSpeed, 1);

        Vector3 offSet = movePosition * movePtogress;
        transform.position = startPosition + offSet;
    }
}
