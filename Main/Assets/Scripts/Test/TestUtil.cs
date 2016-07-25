using UnityEngine;
using System.Collections;

public class TestUtil : MonoBehaviour {
    public Texture3D texture;
    public MeshRenderer cube;
    [ContextMenu("Gen 3D Texture")]
    public void Gen3DTexture() {
        //texture = SceneItemManager.CreateIdentityLut(16);
        //cube.material.SetTexture("_MainTex", texture);
    }

}
