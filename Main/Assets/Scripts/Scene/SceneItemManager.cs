using UnityEngine;
using System.Collections;
/// <summary>
/// TODO:support cube item only. 
/// </summary>
public class SceneItemManager
{
    Texture3D texture;
    public static void SaveItems(Vector3[] points) {
        //
    }
    public void InitItems() {

    }
    public void LoadItems() {
        GameObject sceneRoot = GameObject.Find("SceneRoot");
        if (sceneRoot) {

        }
    }
    //public 
    public static Texture3D CreateIdentityLut(int x,int y,int z)
    {
        //width,height,depth
        //x,y,z
        Texture3D tex3D = new Texture3D(x, y, z, TextureFormat.ARGB32, false);
        Color[] newC = new Color[x * y * z];
        //float oneOverDim  = 1.0f / (1.0f * dim - 1.0f);
        //for (int i  = 0; i < dim; i++) {
        //    for (int j  = 0; j < dim; j++) {
        //        for (int k = 0; k < dim; k++) {
        //            newC[i + (j * dim) + (k * dim * dim)] = new Color((i * 1.0f) * oneOverDim, (j * 1.0f) * oneOverDim, (k * 1.0f) * oneOverDim, 1.0f);
        //        }
        //    }
        //}
        tex3D.SetPixels(newC);
        tex3D.Apply();
        return tex3D;
    }
}