using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
class MaskMesh : MonoBehaviour
{
    public Mesh hexMesh;
    MeshCollider meshCollider;
    Material material;
    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        hexMesh.name = "Mask Mesh";
        meshCollider.sharedMesh = hexMesh;

    }
}
