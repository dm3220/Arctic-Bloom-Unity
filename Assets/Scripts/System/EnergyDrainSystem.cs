using UnityEngine;

public class EnergyDrainSystem : MonoBehaviour
{
    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        float drain = gm.baseEnergyDrainPerSec;

        if (gm.inShelter)
            drain *= gm.shelterDrainMultiplier;

        float delta = drain * Time.deltaTime;

        if (delta > 0f)
            gm.ChangeEnergy(-delta);
    }
}
