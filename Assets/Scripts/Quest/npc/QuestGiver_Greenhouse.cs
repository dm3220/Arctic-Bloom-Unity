using UnityEngine;

public class QuestGiver_Greenhouse : MonoBehaviour
{
    [SerializeField] private GreenhouseQuestState quest;

    private bool playerInRange;

    private void Awake()
    {
        if (quest == null)
            quest = Object.FindAnyObjectByType<GreenhouseQuestState>();
    }

    private void Update()
    {
        if (!quest)
            quest = Object.FindAnyObjectByType<GreenhouseQuestState>();

        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
            Interact();
        if (Input.GetKeyDown(KeyCode.Escape))
            SimpleDialogueUI.Instance?.Hide();
    }

    private void Interact()
    {
        if (quest == null)
        {
            Debug.LogError("QuestGiver_Greenhouse: quest is NULL. Нет GreenhouseQuestState в сцене/переходе.");
            return;
        }
        if (!quest)
            quest = Object.FindAnyObjectByType<GreenhouseQuestState>();

        switch (quest.CurrentStep)
        {
            case GreenhouseQuestState.Step.NotTaken:
                quest.StartQuest();
                SimpleDialogueUI.Instance?.Show("Зайди в теплицу, осмотрись и приведи грядки в порядок.");
                break;

            case GreenhouseQuestState.Step.GoToGreenhouse:
            case GreenhouseQuestState.Step.InspectDone:
                SimpleDialogueUI.Instance?.Show("Теплица ждёт. Грядки нужно привести в хороший вид.");
                break;

            case GreenhouseQuestState.Step.BedsFixed:
                quest.CompleteQuest();
                SimpleDialogueUI.Instance?.Show("Отличная работа! Теперь грядками можно пользоваться.");
                // TODO: награда (очки/предмет/открыть доступ к посадке)
                break;

            case GreenhouseQuestState.Step.Completed:
                SimpleDialogueUI.Instance?.Show("Спасибо! Теплица снова в строю.");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        SimpleDialogueUI.Instance?.Show("Нажми E, чтобы поговорить.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        SimpleDialogueUI.Instance?.Hide();
    }
}
