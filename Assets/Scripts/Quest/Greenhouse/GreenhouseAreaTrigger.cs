using UnityEngine;

public class GreenhouseAreaTrigger : MonoBehaviour
{
    [SerializeField] private GreenhouseQuestState quest;

    private void Awake()
    {
        if (quest == null)
            quest = Object.FindAnyObjectByType<GreenhouseQuestState>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Greenhouse trigger touched by: " + other.name);

        if (!other.CompareTag("Player")) return;
        Debug.Log("Player entered greenhouse trigger!");

        if (quest.CurrentStep == GreenhouseQuestState.Step.GoToGreenhouse)
        {
            quest.MarkInspectDone();
            SimpleDialogueUI.Instance?.Show("Ты в теплице. Осмотрись и приведи грядки в порядок (нажми E у каждой).");
        }
    }
}
