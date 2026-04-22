using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Material mat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = transform.GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        mat.color = Color.red;
    }
}
