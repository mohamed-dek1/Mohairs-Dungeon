
using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
public class GelColorDecorator : MonoBehaviour
{
    // ==========================
    // Sprite options
    // ==========================
    [System.Serializable]
    public class SpritePair
    {
        public Sprite frame1;
        public Sprite frame2;
    }

    [SerializeField] private SpritePair[] colorOptions = new SpritePair[5];

    // ==========================
    // Rainbow settings
    // ==========================
    [SerializeField] private bool rainbow = false;
    [SerializeField] private float rainbowInterval = 0.5f;

    // ==========================
    // Private fields
    // ==========================
    private EnemyAnimator animator;
    private int selectedColor;
    private float rainbowTimer;

    // ==========================
    // Setup
    // ==========================
    void Start()
    {
        animator = GetComponent<EnemyAnimator>();

        if (colorOptions.Length == 0)
            return;

        selectedColor = Random.Range(0, colorOptions.Length);
        ApplyColor();
    }

    // ==========================
    // Rainbow animation
    // ==========================
    void Update()
    {
        if (!rainbow || colorOptions.Length < 2)
            return;

        rainbowTimer += Time.deltaTime;

        if (rainbowTimer >= rainbowInterval)
        {
            rainbowTimer = 0f;
            selectedColor = (selectedColor + 1) % colorOptions.Length;
            ApplyColor();
        }
    }

    // ==========================
    // Apply selected sprites
    // ==========================
    private void ApplyColor()
    {
        SpritePair selected = colorOptions[selectedColor];

        animator.SetMoveDown(new Sprite[]
        {
            selected.frame1,
            selected.frame2
        });
    }
}
