using UnityEngine;

public class DontDestroyRoot : MonoBehaviour
{
    private static DontDestroyRoot _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
