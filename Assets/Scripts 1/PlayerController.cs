using UnityEngine;
using UnityEngine.InputSystem; // Falls du das neue Input System nutzt

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    private Vector2 moveInput;
    private bool isMoving;
    private bool isRunning;

   

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // 1. Aktuelle Geschwindigkeit basierend auf Sprinten bestimmen
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // 2. Bewegung anwenden
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);

        // 3. Charakter in Laufrichtung drehen (Flippen)
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false; // Schaut nach rechts
        }
        else if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;  // Schaut nach links
        }

        // 4. Animationen sauber im FixedUpdate setzen (Verhindert Null-Pointer-Fehler beim Start)
        isMoving = moveInput.x != 0; 
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning && isMoving); 
    }

    // Wird vom Input System aufgerufen bei Bewegung
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Wird vom Input System aufgerufen bei Sprinten
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunning = true;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }
}