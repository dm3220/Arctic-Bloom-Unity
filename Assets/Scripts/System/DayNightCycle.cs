using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Tooltip("Сколько реальных секунд = 1 игровой минуте")]
    public float realSecondsPerGameMinute = 1f;
    private float acc;

    private void Update()
    {
        acc += Time.deltaTime;
        while (acc >= realSecondsPerGameMinute)
        {
            acc -= realSecondsPerGameMinute;
            var gm = GameManager.Instance; if (gm != null) gm.AddMinutes(1);
        }
    }
}
