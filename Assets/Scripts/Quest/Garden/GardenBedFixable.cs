using UnityEngine;

public class GardenBedFixable : MonoBehaviour
{
    [SerializeField] private GreenhouseQuestState quest;

    [Header("Visuals")]
    [SerializeField] private GameObject dirtyView;
    [SerializeField] private GameObject cleanView;

    [Header("Quest")]
    [SerializeField] private GreenhouseBedsController bedsController;

    private bool playerInRange;
    private bool fixedOnce;

    private void Awake()
    {
        if (quest == null)
            quest = Object.FindAnyObjectByType<GreenhouseQuestState>();

        if (bedsController == null)
            bedsController = Object.FindAnyObjectByType<GreenhouseBedsController>();
    }

    private void Start()
    {
        SetFixed(false); // стартуем "грязной"
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (fixedOnce) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed near bed: " + gameObject.name);

            if (quest == null)
            {
                Debug.LogError("GardenBedFixable: quest == null");
                return;
            }

            if (quest.CurrentStep != GreenhouseQuestState.Step.InspectDone)
            {
                SimpleDialogueUI.Instance?.Show("Сначала нужно попасть в теплицу и осмотреться.");
                return;
            }

            fixedOnce = true;
            SetFixed(true);

            if (bedsController == null)
            {
                Debug.LogError("GardenBedFixable: bedsController == null");
                return;
            }

            bedsController.NotifyBedFixed();
            SimpleDialogueUI.Instance?.Show("Грядка приведена в порядок.");
        }
    }

    private void SetFixed(bool isFixed)
    {
        if (dirtyView != null) dirtyView.SetActive(!isFixed);
        if (cleanView != null) cleanView.SetActive(isFixed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;

        Debug.Log("Player entered bed trigger: " + gameObject.name);

        if (!fixedOnce)
            SimpleDialogueUI.Instance?.Show("Нажми E, чтобы привести грядку в порядок.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        SimpleDialogueUI.Instance?.Hide();
    }
}
