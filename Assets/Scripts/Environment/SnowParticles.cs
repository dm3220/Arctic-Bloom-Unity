using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SnowParticles : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        var gm = GameManager.Instance; if (gm == null) return;

        var em = ps.emission;
        // Чем холоднее, тем больше снег (t <= 0 -> максимум)
        float k = Mathf.Clamp01((-gm.temperature + 10f) / 10f);
        em.rateOverTime = 50f * k;
    }
}
