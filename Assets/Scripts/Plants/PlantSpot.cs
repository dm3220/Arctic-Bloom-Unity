using UnityEngine;

public class PlantSpot : MonoBehaviour
{
    public bool hasPlant;
    public GameObject plantPrefab;

    void OnMouseDown()
    {
        if (!hasPlant)
        {
            Instantiate(plantPrefab, transform.position, Quaternion.identity);
            hasPlant = true;
        }
    }
}
