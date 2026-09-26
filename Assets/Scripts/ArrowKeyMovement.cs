using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour
{
    Rigidbody rb;

    public float movement_speed = 4.0f;
    public float gridSize = 0.5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 current_input = GetInput();

        rb.linearVelocity = current_input;
    }

    Vector2 GetInput()
    {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        return CalculateMovement(horizontal_input, vertical_input);
    }

    // calculate movement vector based on inputs
    // assume params are either -1.0f, 0.0f, 1.0f
    Vector2 CalculateMovement(float horizontal_input, float vertical_input)
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
                vertical_movement = vertical_input * movement_speed;
                horizontal_movement = 0;
            }
        }

        return new Vector2(horizontal_movement, vertical_movement);
    }

    bool AxisAligned(float pos)
    {
        float remainder = pos % gridSize;
        return Mathf.Abs(remainder) < 0.001f || Mathf.Abs(remainder - gridSize) < 0.001f;
    }
    
    float GetAlignDir(float pos)
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

    float GetAlignSpeed(float pos)
    {
        float remainder = pos % gridSize;
        if (remainder < 0)
            remainder += gridSize;
        float distance = Mathf.Min(remainder, gridSize - remainder);

        float maxSpeed = distance / Time.fixedDeltaTime;
        return Mathf.Min(movement_speed, maxSpeed);
    }
}
