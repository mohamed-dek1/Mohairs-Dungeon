using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    BoxCollider  boxcollider;
    SpriteRenderer spriterenderer;
    [SerializeField] private Sprite openDoor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxcollider = GetComponent<BoxCollider>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        spriterenderer.sprite = openDoor;
        boxcollider.enabled = false;
    }
}
