using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour
{

    public float scrollSpeed;
    private Vector2 savedOffset;
    public Renderer renderer;
    Vector3 previousCamPos;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        savedOffset = renderer.sharedMaterial.GetTextureOffset("_MainTex");
        previousCamPos = cam.transform.position;
    }

    void Update()
    {
        float x = (previousCamPos.x - cam.transform.position.x) * scrollSpeed;
        Vector2 offset = new Vector2(savedOffset.x + x, savedOffset.y);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);

        savedOffset = renderer.sharedMaterial.GetTextureOffset("_MainTex");
        previousCamPos = cam.transform.position;
    }

    void OnDisable()
    {
        renderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
