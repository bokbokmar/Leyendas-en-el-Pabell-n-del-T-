using UnityEngine;

public class MirarCamara : MonoBehaviour
{
    private Transform camara;

    private void Start()
    {
        if (Camera.main != null)
            camara = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (camara == null) return;


        transform.LookAt(transform.position + camara.rotation * Vector3.forward,
                         camara.rotation * Vector3.up);
    }
}