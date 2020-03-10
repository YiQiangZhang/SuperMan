
using UnityEngine;
using System.Collections;

public class CameraLerpToTransform : MonoBehaviour
{

    public Transform _Player;
    private Vector3 _offset;
    void Start()
    {
        _offset = transform.position - _Player.position;
    }


    void Update()
    {
        transform.position = _Player.position + _offset;
    }
}