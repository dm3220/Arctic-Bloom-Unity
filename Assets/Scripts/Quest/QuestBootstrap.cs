using UnityEngine;

public class QuestBootstrap : MonoBehaviour
{
    private void Awake()
    {
        var quest = Object.FindAnyObjectByType<GreenhouseQuestState>();
        if (quest == null)
        {
            var go = new GameObject("QuestManager");
            go.AddComponent<GreenhouseQuestState>();
        }
    }
}
