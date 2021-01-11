using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float speed = 5f;
    
    private float _horizontal;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxis($"Horizontal") * speed;
        _rigidbody2D.velocity = new Vector2(_horizontal, _rigidbody2D.velocity.y);
    }
}