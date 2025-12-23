using UnityEngine;

public class GreenhouseQuestState : MonoBehaviour
{
    public enum Step { NotTaken, GoToGreenhouse, InspectDone, BedsFixed, Completed }
    public Step CurrentStep {  get; private set; } = Step.NotTaken;

    private void Awake()
    {
        var all = Object.FindObjectsByType<GreenhouseQuestState>(FindObjectsSortMode.None);
        if (all.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void StartQuest() => CurrentStep = Step.GoToGreenhouse;
    public void MarkInspectDone()
    {
        if (CurrentStep == Step.GoToGreenhouse)
            CurrentStep = Step.InspectDone;
    }

    public void MarkBedsFixed()
    {
        if (CurrentStep == Step.InspectDone)
            CurrentStep = Step.BedsFixed;
    }

    public void CompleteQuest()
    {
        if (CurrentStep == Step.BedsFixed)
            CurrentStep = Step.Completed;
    }
}
