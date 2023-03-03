using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    [SerializeField] private Transform _transformCamera;
    private Vector3 _correct;
    [SerializeField] private float _step = 0.015625f;
    private int _result;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void LateUpdate()
    {
        _correct.Set
            (
            _step * math.trunc(_transformCamera.position.x / _step),
            _step * math.trunc(_transformCamera.position.y / _step),
            _transformCamera.position.z
            );
        transform.position = _correct;
    }
}
