using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    const float SPEED = 5;
    const float ROTATE_SPEED = 360;
    private Vector2 DirToPlayer => (Player.Instance.transform.position - transform.position).normalized;
    private Rigidbody2D _rb2d;

    private Vector2 _originalDir;
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _originalDir = DirToPlayer;
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * ROTATE_SPEED));
    }
    private void FixedUpdate()
    {
        _rb2d.velocity = _originalDir * SPEED;
    }
}
