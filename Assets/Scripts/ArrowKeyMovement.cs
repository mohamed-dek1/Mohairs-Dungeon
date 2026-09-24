using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour
{
    Rigidbody rb;

    public float movement_speed = 4.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 current_input = GetInput();
        rb.linearVelocity = current_input * movement_speed;
    }

    Vector2 GetInput()
    {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        if(Mathf.Abs (horizontal_input) > 0.0f)
        {
            vertical_input = 0.0f;
        }

        return new Vector2(horizontal_input, vertical_input);
    }
}
