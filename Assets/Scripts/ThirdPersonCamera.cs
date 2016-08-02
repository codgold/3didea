using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{

    [SerializeField] private float _distanceAway;
    [SerializeField] private float _distanceUp;
    [SerializeField] private float _smooth;
    [SerializeField] private Transform _targetFollow;
    [SerializeField] private float _camSmoothDampTime;
    private Vector3 _targetPosition;
    private Vector3 _lookDir;
    private Vector3 _offset = new Vector3(0f, 1.5f, 0f);
    private Vector3 _velocityCamSmooth = Vector3.zero;

    void Start()
    {
        _targetFollow = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {

    }

    void LateUpdate()
    {
        Vector3 characterOffset = _targetFollow.position + _offset;

        _lookDir = characterOffset - this.transform.position;
        _lookDir.y = 0;
        _lookDir.Normalize();

        _targetPosition = characterOffset + _targetFollow.up * _distanceUp - _lookDir * _distanceAway;
        // Debug.DrawRay(_targetFollow.position, Vector3.up * _distanceUp, Color.red);
        // Debug.DrawRay(_targetFollow.position, -1f * _targetFollow.forward * _distanceAway, Color.blue);
        // Debug.DrawLine(_targetFollow.position, _targetPosition, Color.magenta);

        SmoothPosition(this.transform.position, _targetPosition);

        transform.LookAt(_targetFollow);
    }

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref _velocityCamSmooth, _camSmoothDampTime);
    }
}
