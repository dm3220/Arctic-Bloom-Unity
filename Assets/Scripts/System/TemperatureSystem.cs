using UnityEngine;
using UnityEngine.Rendering;

public class TemperatureSystem : MonoBehaviour
{
    [Tooltip("Охлаждение на улице, °C/сек (ставь отрицательное значение)")]
    public float outdoorCoolingPerSec = -0.05f;

    private void Update()
    {
        var gm = GameManager.Instance; if (gm == null) return;
        if (!gm.inShelter)
        {
            gm.ChangeTemperature(outdoorCoolingPerSec * Time.deltaTime);
            Debug.Log(outdoorCoolingPerSec);
        }
    }
}
