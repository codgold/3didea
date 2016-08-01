using UnityEngine;
using System.Collections;

public class CharacterControllerLogic : MonoBehaviour 
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _damping = .25f;

    private float speed = 0.0f;
    private float h = 0.0f;
    private float v = 0.0f;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if(_animator.layerCount >= 2)
        {
            _animator.SetLayerWeight(1, 1);
        }
    }

    void Update()
    {
        if(_animator)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            speed = h * h + v * v;

            _animator.SetFloat("Speed", speed);
            _animator.SetFloat("Direction", h, _damping, Time.deltaTime);
        }
    }
}
