using UnityEngine;

public class PlayerSpawnOnLoad : MonoBehaviour
{
    private void Start()
    {
        // Ищем первого попавшегося игрока и телепортируем на этот маркер
        var player = FindFirstObjectByType<PlayerController2D>();
        if (player != null)
        {
            player.transform.position = transform.position;
        }
    }
}
