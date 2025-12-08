using UnityEngine;

public class Plant : MonoBehaviour
{
    public Sprite[] stages;
    public float growthTime = 30f;
    private SpriteRenderer sr;
    private int currentStage = 0;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = stages[0];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= growthTime && currentStage < stages.Length - 1)
        {
            currentStage++;
            sr.sprite = stages[currentStage];
            timer = 0;
        }
    }
}
