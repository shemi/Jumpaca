using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;

    private bool _followPlayer;

    public float minYThreshold = -2.6f;
    
    // Start is called before the first frame update
    void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    void Follow()
    {
        var tempTransform = transform;

        if (_target.position.y < (tempTransform.position.y - minYThreshold))
        {
            _followPlayer = false;
        }

        if (_target.position.y > tempTransform.position.y)
        {
            _followPlayer = true;
        }

        if (_followPlayer)
        {
            Vector3 temp = tempTransform.position;
            temp.y = _target.position.y;
            tempTransform.position = temp;
        }
    }
    
}
