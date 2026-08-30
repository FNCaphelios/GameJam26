using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    [Header("Wall Status")]
    
    public bool isSolid = true;

    private Collider wallCollider;

    void Start()
    {
        
        wallCollider = GetComponent<Collider>();

        
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null)
        {
            mesh.enabled = false;
        }
    }

    void Update()
    {

        if (wallCollider != null && wallCollider.enabled != isSolid)
        {
            wallCollider.enabled = isSolid;
        }
    }

    public void SetWallState(bool state)
    {
        isSolid = state;
    }
}