using UnityEngine;
using System.Collections;

public enum Direction { Up = 0, Down = 1, Left = 2, Right = 3 }

public class ArrowKeyMovement : MonoBehaviour
{
    // ==========================
    // Public fields
    // ==========================
    Rigidbody rb;
    public float movement_speed = 4.0f;
    public float gridSize = 0.5f;
    public float attackTime = 0.5f;

    // ==========================
    // Serialized fields
    // ==========================
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject[] weapons; // Up, Down, Left, Right, Arrow
    [SerializeField] private GameObject swordBeamPrefab;
    [SerializeField] private float hitStunDuration = 1.0f;
    [SerializeField] private float knockbackForce = 0.8f;
    [SerializeField] private float arrowTimeout = 1.0f;

    // ==========================
    // Private fields
    // ==========================
    private bool isAttacking = false;
    private bool isStunned = false;
    private bool isArrowOnCooldown = false;
    private Direction directionFacing = Direction.Down;
    private int swordOffset = 0;
    private int arrowOffset = 4;
    private GameObject activeBeam;

    public bool getIsStunned()
    {
        return isStunned;
    }
    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public Direction GetDirectionFacing()
    {
        return directionFacing;
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        foreach (GameObject weapon in weapons)
            weapon.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && !isArrowOnCooldown)
            StartCoroutine(Attack());

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isArrowOnCooldown && !isAttacking && inventory.CurrentItem == ItemType.Bow && inventory.Coins > 0){
            StartCoroutine(ThrowArrow());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.god_mode = !GameManager.god_mode;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        Vector2 current_input = GetInput();
        rb.linearVelocity = current_input;
    }

    [ContextMenu("Trigger Stun")]
    public void TriggerStun()
    {
        if (isStunned) return;
        StopCoroutine(nameof(StunRoutine));
        Vector2 forceDirection = -GetDirectionVector(directionFacing);
        StartCoroutine(StunRoutine(forceDirection, knockbackForce)); // Stun duration: 1 second
    }

    private IEnumerator StunRoutine(Vector2 forceDirection, float forceStrength)
    {
        isStunned = true;
        rb.linearVelocity = Vector2.zero;

        rb.AddForce(forceDirection * forceStrength, ForceMode.Impulse);

        yield return new WaitForSeconds(hitStunDuration);

        rb.linearVelocity = Vector2.zero;
        isStunned = false;
    }

    IEnumerator ThrowArrow()
    {
        isArrowOnCooldown = true;
        inventory.UseArrow();
        GameObject weapon = Instantiate(weapons[arrowOffset], transform.position, Quaternion.identity);
        weapon.SetActive(true);

        if (weapon.TryGetComponent<Arrow>(out var arrow))
            arrow.Launch(GetDirectionVector(directionFacing));

        yield return new WaitForSeconds(arrowTimeout);

        isArrowOnCooldown = false;
    }
    
    IEnumerator Attack()
    {
        isAttacking = true;
        GameObject weapon = weapons[(int)directionFacing + (4 * swordOffset)];
        weapon.SetActive(true);

        if (activeBeam == null)
            FireSwordBeam();

        yield return new WaitForSeconds(attackTime);

        weapon.SetActive(false);
        isAttacking = false;
    }

    private void FireSwordBeam()
    {
        activeBeam = Instantiate(swordBeamPrefab, transform.position, Quaternion.identity);

        if (activeBeam.TryGetComponent<SwordBeam>(out var beam))
            beam.Launch(GetDirectionVector(directionFacing));
    }

    private Vector2 GetDirectionVector(Direction direction)
    {
        if (direction == Direction.Up)
            return Vector2.up;
        else if (direction == Direction.Down)
            return Vector2.down;
        else if (direction == Direction.Left)
            return Vector2.left;
        else
            return Vector2.right;
    }

    Vector2 GetInput()
    {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        return CalculateMovement(horizontal_input, vertical_input);
    }

    // calculate movement vector based on inputs
    // assume params are either -1.0f, 0.0f, 1.0f
    private Vector2 CalculateMovement(float horizontal_input, float vertical_input)
    {
        float horizontal_movement = 0f;
        float vertical_movement = 0f;
        if (horizontal_input != 0)
        {
            //Align vertical first
            if (!AxisAligned(rb.position.y))
            {
                horizontal_movement = 0;
                vertical_movement = GetAlignDir(rb.position.y) * GetAlignSpeed(rb.position.y);
            }
            else
            {
                if (horizontal_input > 0)
                    directionFacing = Direction.Right;
                else
                    directionFacing = Direction.Left;

                horizontal_movement = horizontal_input * movement_speed;
                vertical_movement = 0;
            }
        }
        else if (vertical_input != 0)
        {
            //Align horizontal first
            if (!AxisAligned(rb.position.x))
            {
                vertical_movement = 0;
                horizontal_movement = GetAlignDir(rb.position.x) * GetAlignSpeed(rb.position.x);
            }
            else
            {
                if (vertical_input > 0)
                    directionFacing = Direction.Up;
                else
                    directionFacing = Direction.Down;

                vertical_movement = vertical_input * movement_speed;
                horizontal_movement = 0;
            }
        }

        return new Vector2(horizontal_movement, vertical_movement);
    }

    private bool AxisAligned(float pos)
    {
        float remainder = pos % gridSize;
        return Mathf.Abs(remainder) < 0.001f || Mathf.Abs(remainder - gridSize) < 0.001f;
    }
    
    private float GetAlignDir(float pos)
    {
        float remainder = pos % gridSize;

        // C# modulo can give negative
        if (remainder < 0) 
            remainder += gridSize;

        if (remainder < 0.001f)
            return 0f;
        else if (remainder > gridSize / 2f)
            return 1f;
        else
            return -1f;
    }

    private float GetAlignSpeed(float pos)
    {
        float remainder = pos % gridSize;
        if (remainder < 0)
            remainder += gridSize;
        float distance = Mathf.Min(remainder, gridSize - remainder);

        float maxSpeed = distance / Time.fixedDeltaTime;
        return Mathf.Min(movement_speed, maxSpeed);
    }
}
