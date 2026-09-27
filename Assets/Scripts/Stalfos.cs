using UnityEngine;

public class Stalfos : GridWalk
{
    public float keepStraightChance = 0.6f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override Vector2 ChooseDirection()
    {
        return RandomDirection(keepStraightChance);
    }
}
