using Characters.Players;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothSpeed = 10f;

    private void Start()
    {
        var playerController = target.GetComponent<PlayerController>();
        playerController.PlayerCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, smoothSpeed * Time.deltaTime);
    }


}
