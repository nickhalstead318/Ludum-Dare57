using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody _rb;
    private bool _isGrounded;
    private GameManagerBehavior _gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Game Manager
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerBehavior>();

        // Sets up the player RigidBody component
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogWarning("Player object is missing RigidBody component");
        }

        // Ensures ground check (feet/bottom of player) transform is set up
        if (groundCheck == null)
        {
            Debug.LogWarning("Player object does not have a ground check transform defined");
        }

        // Ensures ground layer is defined
        if (groundCheck == null)
        {
            Debug.LogWarning("Player object does not have a ground layer defined");
        }

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckGrounded();
        Jump();
        Reset();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        _rb.linearVelocity = new Vector3(moveX * moveSpeed, _rb.linearVelocity.y, 0f);
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, 0.05f, groundLayer);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, jumpForce, 0f);
        }
    }

    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            _gameManager.ResetScene();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }
}
