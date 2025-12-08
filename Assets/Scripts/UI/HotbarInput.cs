using UnityEngine;

public class HotbarInput : MonoBehaviour
{
    Inventory inv;

    void Awake()
    {
        inv = Inventory.Instance;
        if (inv == null) inv = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (inv == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) inv.SetActive(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inv.SetActive(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inv.SetActive(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) inv.SetActive(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) inv.SetActive(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) inv.SetActive(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) inv.SetActive(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) inv.SetActive(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) inv.SetActive(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) inv.SetActive(9);
        if (Input.GetKeyDown(KeyCode.Minus)) inv.SetActive(10);
        if (Input.GetKeyDown(KeyCode.Equals)) inv.SetActive(11);
    }
}
