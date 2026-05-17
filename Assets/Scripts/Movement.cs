
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Check")] 
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheck;

    [Header("Terrain / Slope")]
    [SerializeField] private float groundRayDistance = 1.5f;
    [SerializeField] private float maxSlopeAngle = 45f;

    [Header("Mouse Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseLookSensitivity = 200f;
    [SerializeField] private float maxLoockAngle = 80f;
    
    [Header("Fire Info")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private float _xRotation;
    private bool _isGrounded;

    private Rigidbody _rb;

    private Vector3 _groundNormal = Vector3.up;
    private bool _onTooSteepSlope;

    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Щоб персонаж не падав набік від фізики
        _rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    

    private void FixedUpdate()
    {
        CheckGroundNormal();
        Move();
    }

    private void Update()
    {
        IsGrounded();
        Jump();
        Look();
        //Fire();
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 shootDirection = cameraTransform.forward;

            GameObject bullet = Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.LookRotation(shootDirection)
            );

            /*Bullet bull = bullet.GetComponent<Bullet>();

            if (bull != null)
            {
                bull.SpawnBullet(shootDirection);
            }*/
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = transform.right * horizontal;
        movementDirection += transform.forward * vertical;
        movementDirection = movementDirection.normalized;

        // Якщо ми стоїмо на землі, рух підлаштовується під нахил Terrain
        if (_isGrounded)
        {
            movementDirection = Vector3.ProjectOnPlane(movementDirection, _groundNormal).normalized;
        }

        // Якщо схил занадто крутий — не даємо рухатися вверх по ньому
        if (_onTooSteepSlope)
        {
            movementDirection = Vector3.zero;
        }

        Vector3 velocity = movementDirection * moveSpeed;

        // Залишаємо поточну вертикальну швидкість, щоб працювали стрибок і гравітація
        velocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = velocity;
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseLookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseLookSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -maxLoockAngle, maxLoockAngle);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void IsGrounded()
    {
        _isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void CheckGroundNormal()
{
    // За замовчуванням вважаємо, що поверхня рівна
    // Vector3.up — це напрямок вгору (0, 1, 0)
    _groundNormal = Vector3.up;

    // За замовчуванням вважаємо, що схил НЕ занадто крутий
    _onTooSteepSlope = false;

    // Створюємо промінь від позиції гравця вниз
    Ray ray = new Ray(transform.position, Vector3.down);

    // Пускаємо промінь вниз і перевіряємо, чи потрапив він у землю
    // groundRayDistance — максимальна довжина променя
    // groundLayer — шар, який вважається землею
    if (Physics.Raycast(ray, out RaycastHit hit, groundRayDistance, groundLayer))
    {
        // Зберігаємо нормаль поверхні, в яку потрапив промінь
        // normal — це напрямок, перпендикулярний до поверхні
        _groundNormal = hit.normal;

        // Рахуємо кут між напрямком вгору і нормаллю поверхні
        // Чим більший кут, тим крутіший схил
        float slopeAngle = Vector3.Angle(Vector3.up, _groundNormal);

        // Якщо кут схилу більший за максимально дозволений
        if (slopeAngle > maxSlopeAngle)
        {
            // Позначаємо, що гравець стоїть на занадто крутому схилі
            _onTooSteepSlope = true;
        }
    }
}

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_onTooSteepSlope)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;

        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * groundRayDistance);
    }
}
