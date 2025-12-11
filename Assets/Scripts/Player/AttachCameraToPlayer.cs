using UnityEngine;
using Unity.Cinemachine;   // ← ОБЯЗАТЕЛЬНО для новых камер

public class AttachCameraToPlayer : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        // если камеру не указали в инспекторе – попробуем взять с этого же объекта
        if (cinemachineCamera == null)
            cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        // ищем игрока по тегу
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null || cinemachineCamera == null)
        {
            Debug.LogWarning("AttachCameraToPlayer: не нашли Player или CinemachineCamera");
            return;
        }

        // подписываем камеру на игрока
        cinemachineCamera.Follow = player.transform;
        cinemachineCamera.LookAt = player.transform;
    }
}
