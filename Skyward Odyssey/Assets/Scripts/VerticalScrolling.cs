using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScrolling : MonoBehaviour
{
    [SerializeField]
    Transform followTarget;
    [SerializeField]
    float followViewportHeight = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, 
            followTarget.position.y+followViewportHeight, transform.position.z);
    }
}
