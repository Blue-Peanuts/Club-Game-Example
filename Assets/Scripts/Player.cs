using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    const float SPEED = 10;
    const float TORQUE = 720;
    const float DASH_TIME = 0.5f;
    const float DASH_SPEED = 60f;
    const float DASH_COOLDOWN = 2;
    const float INVINCIBILITY_BONUS = 0.25f;

    private Rigidbody2D _rb2d;
    private SpriteRenderer _sr;
    private Vector2 MousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private Vector2 MouseDirection => (MousePosition - (Vector2)transform.position).normalized;
    private float TargetAngle => Vector2.SignedAngle(Vector2.right, MouseDirection);

    private bool _dashInput => Input.GetKeyDown(KeyCode.Mouse0);
    private float _dashTime = 0;
    private float _dashCooldown = 0;
    private float _invincibilityTime = 0;
    private bool IsDashing => _dashTime > 0;
    private bool CanDash => _dashCooldown <= 0;
    private bool IsInvincible => _invincibilityTime > 0;
    
    [SerializeField] ParticleSystem _dashParticle;
    [SerializeField] ParticleSystem _chargedParticle;
    [SerializeField] TrailRenderer _trail;
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _dashColor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private bool _preCanDash = false;

    private void Update()
    {
        if (IsDashing)
            _trail.startColor = _sr.color = Color.Lerp(_defaultColor, _dashColor, Mathf.Sqrt(_dashTime / DASH_TIME));
        else if (CanDash)
            _trail.startColor = _sr.color = _dashColor;
        else
            _trail.startColor = _trail.endColor = _sr.color = _defaultColor;

        _trail.endColor = Color.black * 0;

        //Start dash if input & cooldown is valid
        if (_dashInput && CanDash)
        {
            Dash();
        }
        
        //Dash cooldown timer
        if(!IsDashing)
            _dashCooldown -= Time.deltaTime;

        if (CanDash && !_preCanDash)
            _chargedParticle.Play();

        _preCanDash = CanDash;
    }

    private void Dash()
    {
        _dashTime = DASH_TIME;
        _dashCooldown = DASH_COOLDOWN;
        _invincibilityTime = DASH_TIME + INVINCIBILITY_BONUS;
        _dashParticle.Play();
    }

    private void FixedUpdate()
    {
        //Create a quaternion containing target angle, this will be the quaternion value that the player rotate to
        Quaternion targetQuaternion = Quaternion.Euler(new Vector3(0, 0, TargetAngle));

        //Rotate player towards targetQuaternion with the speed of Torque
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, Time.fixedUnscaledDeltaTime * TORQUE * (IsDashing? 2 : 1));
        /*
        if (IsDashing)
            transform.rotation = targetQuaternion;*/

        //This if statement prevents player from moving when near pointer
            float speed = SPEED;
            
            //Timer for dashing. Change speed while dashing.
            if (IsDashing)
            {
                speed = Mathf.Max(0, DASH_SPEED * _dashTime / DASH_TIME);
            }

            _rb2d.velocity = transform.right * speed;

        _dashTime -= Time.fixedDeltaTime;
        _invincibilityTime -= Time.fixedDeltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInvincible && collision.CompareTag("Enemy"))
        {
            Dash();
            collision.GetComponent<Enemy>().Death();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((!IsInvincible && collision.collider.CompareTag("Enemy")) 
            || collision.collider.CompareTag("Projectile"))
            Death();
    }


    private void Death()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
