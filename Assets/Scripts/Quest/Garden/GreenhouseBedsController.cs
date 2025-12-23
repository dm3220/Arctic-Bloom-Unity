using UnityEngine;

public class GreenhouseBedsController : MonoBehaviour
{
    [SerializeField] private GreenhouseQuestState quest;
    [SerializeField] private int totalBeds = 3;

    private int fixedBeds = 0;

    private void Awake()
    {
        if (quest == null)
            quest = Object.FindAnyObjectByType<GreenhouseQuestState>();
    }


    public void NotifyBedFixed()
    {
        fixedBeds++;

        if (fixedBeds >= totalBeds)
        {
            quest.MarkBedsFixed();
            SimpleDialogueUI.Instance?.Show("Все грядки готовы! Вернись к NPC.");
        }
    }
}
