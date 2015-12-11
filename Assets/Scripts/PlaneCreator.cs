using UnityEngine;
using System.Collections;

public class PlaneCreator : MonoBehaviour {
    private Camera cam;
    private Plane[] planes;
    void Start() {
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        int i = 0;
        //fsdafa
        while (i < planes.Length - 2) {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            DestroyImmediate(p.GetComponent<MeshCollider>());
            p.AddComponent<EdgeCollider2D>();
            p.layer = LayerMask.NameToLayer("EnemyPassthrough");
            p.name = "Plane " + i.ToString();
            p.transform.parent = this.gameObject.transform;
            p.transform.position = -planes[i].normal * planes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
            p.transform.localScale = new Vector3(2, 1, 1);
            i++;
        }
    }
}