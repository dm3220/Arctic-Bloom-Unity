using UnityEngine;

public class EnergyDrainSystem : MonoBehaviour
{
    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        // Базовая скорость (в %/сек)
        float drain = gm.baseEnergyDrainPerSec;

        // Если в укрытии — дренаж медленнее
        if (gm.inShelter)
            drain *= gm.shelterDrainMultiplier;

        // Сколько снять за этот кадр
        float delta = drain * Time.deltaTime;

        if (delta > 0f)
            gm.ChangeEnergy(-delta);
    }
}
