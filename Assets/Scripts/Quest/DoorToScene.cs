using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToScene : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Greenhouse";
    [SerializeField] private bool requirePressE = true;

    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange) return;

        if (!requirePressE) return;

        if (Input.GetKeyDown(KeyCode.E))
            Load();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;

        if (requirePressE)
            SimpleDialogueUI.Instance?.Show("Нажми E, чтобы войти в теплицу.");
        else
            Load();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        SimpleDialogueUI.Instance?.Hide();
    }

    private void Load()
    {
        SimpleDialogueUI.Instance?.Hide();
        SceneManager.LoadScene(sceneToLoad);
    }
}
