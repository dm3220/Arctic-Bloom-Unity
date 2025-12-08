using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    // Куда ставить игрока после загрузки следующей сцены
    public string nextSpawnId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // переживает загрузку сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
