using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public ParticleSystem snow;
    public ParticleSystem storm;
    public static bool isSnow;
    public void Start()
    {
        if (isSnow)
        {
            snow.Play();
            storm.Stop();
        }
    else
        {
            snow.Stop();
            snow.Play();

        }
    }

}
