using UnityEngine;

public class Enemy : MonoBehaviour
{
    const float SPEED = 5;
    const float ROTATE_SPEED = 360;

    [SerializeField] GameObject _deathParticlePrefab;

    private Rigidbody2D _rb2d;
    private Vector2 DirToPlayer => (Player.Instance.transform.position - transform.position).normalized;


    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * ROTATE_SPEED));
    }

    private void FixedUpdate()
    {
        _rb2d.velocity = DirToPlayer * SPEED;
    }

    public void Death()
    {
        GameManager.Instance.AddScore();
        GameManager.Instance.HitStop();

        Destroy(Instantiate(_deathParticlePrefab, transform.position, Quaternion.identity), 1);
        Destroy(gameObject);
    }
}
