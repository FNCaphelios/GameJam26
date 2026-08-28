using UnityEngine;

public class BillBoardUI : MonoBehaviour
{
    private Transform mainCam;

    private void Start()
    {
        if (Camera.main != null) mainCam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (mainCam == null) return;

        // Match camera rotation so text stays flat relative to the view angle
        transform.rotation = mainCam.rotation;
    }
}
