using UnityEngine;
using System.Collections;

public class CharacterControllerLogic : MonoBehaviour 
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _damping = .25f;
    [SerializeField] private float _directionSpeed = 3.0f;
    [SerializeField] private float _rotationDegreePerSecond = 120f;
    [SerializeField] private ThirdPersonCamera _gameCam;
   

    private float _speed = 0.0f;
    private float _direction = 0.0f;
    private float _h = 0.0f;
    private float _v = 0.0f;

    private int _m_LocomotionId = 0;
    private AnimatorStateInfo _stateInfo;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if(_animator.layerCount >= 2)
        {
            _animator.SetLayerWeight(1, 1);
        }

        _m_LocomotionId = Animator.StringToHash("Base Layer.Locomotion");
    }

    void Update()
    {
        if(_animator)
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");

            _speed = _h * _h + _v * _v;

            _animator.SetFloat("Speed", _speed);
            _animator.SetFloat("Direction", _direction, _damping, Time.deltaTime);
            StickToWorldspace(this.transform, _gameCam.transform, ref _direction, ref _speed);
        }
    }

    public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
    {
        Vector3 rootDirection = root.forward;
        Vector3 stickDirection = new Vector3(_h, 0, _v);

        speedOut = stickDirection.sqrMagnitude;

        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        //Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
        //Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);

        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

        angleRootToMove /= 180f;

        directionOut = angleRootToMove * _directionSpeed;
    }

    private void FixedUpdate()
    {
        if(IsInLocomotion() && ((_direction >= 0 && _h >= 0) || (_direction < 0 && _h < 0)))
        {
            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, _rotationDegreePerSecond * (_h < 0f ? -1f : 1f), 0f), Mathf.Abs(_h));
        }
    }

    private bool IsInLocomotion()
    {
        return _stateInfo.nameHash == _m_LocomotionId;
    }
}
