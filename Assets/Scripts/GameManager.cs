using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool god_mode = false;

    void Start()
    {
        Screen.SetResolution(1024, 960, false);
    }
}
