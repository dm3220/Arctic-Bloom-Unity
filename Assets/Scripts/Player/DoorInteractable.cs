using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Door target")]
    [SerializeField] private string targetScene = "House";   // им€ сцены из Build Settings
    [SerializeField] private string targetSpawnId = "";      // опционально, если используешь спавн по id

    [Header("Gizmo (не об€з.)")]
    [SerializeField] private Transform interactionTransform;
    [SerializeField] private float gizmoRadius = 1.2f;

    public void Interact()
    {
        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogWarning("[DoorInteractable] Target scene is empty.");
            return;
        }

        // если у теб€ есть GameSession с поддержкой nextSpawnId Ч передадим его (без ошибок, если нет)
        var gs = FindObjectOfType<GameSession>();
        if (gs != null && !string.IsNullOrEmpty(targetSpawnId))
        {
            // предположим, что в GameSession есть публичное поле/свойство nextSpawnId
            gs.nextSpawnId = targetSpawnId;
        }

        // ѕлавный переход, если SceneTransition в сцене есть; иначе обычна€ загрузка
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(targetScene);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var p = interactionTransform ? interactionTransform.position : transform.position;
        Gizmos.DrawWireSphere(p, gizmoRadius);
    }
#endif
}
