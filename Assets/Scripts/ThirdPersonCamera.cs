using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{

    [SerializeField] private float _distanceAway;
    [SerializeField] private float _distanceUp;
    [SerializeField] private float _smooth;
    [SerializeField] private Transform _follow;
    private Vector3 _targetPosition;

    void Start()
    {
        _follow = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {

    }

    void LateUpdate()
    {
        _targetPosition = _follow.position + _follow.up * _distanceUp - _follow.forward * _distanceAway;
        Debug.DrawRay(_follow.position, Vector3.up * _distanceUp, Color.red);
        Debug.DrawRay(_follow.position, -1f * _follow.forward * _distanceAway, Color.blue);
        Debug.DrawLine(_follow.position, _targetPosition, Color.magenta);
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _smooth);

        transform.LookAt(_follow);
    }
}
