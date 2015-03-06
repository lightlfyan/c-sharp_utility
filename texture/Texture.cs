using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Texture : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        combine ();
    }
	

    void combine ()
    {
        SkinnedMeshRenderer[] smr = GetComponentsInChildren<SkinnedMeshRenderer> ();
        List<CombineInstance> lcom = new List<CombineInstance> ();
        //List<Material> lmat = new List<Material> ();
       
        List<Transform> bones = new List<Transform> ();        
        List<BoneWeight> boneWeights = new List<BoneWeight> (); 
        
        
        Texture2D[] textures = new Texture2D[smr.Length];
        for (int i=0; i<smr.Length; i++) {
            textures [i] = (Texture2D)smr [i].renderer.material.mainTexture;
        }
        
        Material mat = new Material (Shader.Find ("Custom/OutLine"));
        mat.hideFlags = HideFlags.DontSave;
        
        Texture2D newMainTexture = Flatten (textures);
        Rect[] uvs = newMainTexture.PackTextures (textures, 0, newMainTexture.width);
        
        mat.mainTexture = newMainTexture;
        
        Vector2[] uva, uvb;
        
        for (int j=0; j < smr.Length; j++) {
            uva = (Vector2[])(smr [j].sharedMesh.uv);
            uvb = new Vector2[uva.Length];
            for (int k=0; k < uva.Length; k++) {
                uvb [k] = new Vector2 ((uva [k].x * uvs [j].width) + uvs [j].x, (uva [k].y * uvs [j].height) + uvs [j].y);
            }
            Mesh msh = (Mesh)Instantiate (smr [j].sharedMesh);
            smr [j].sharedMesh = msh;
            smr [j].sharedMesh.uv = uvb;
        }   
        
        foreach (var obj in smr) {
            obj.gameObject.renderer.sharedMaterials = new Material[]{mat};
        }

        int boneOffset = 0;
        List<Matrix4x4> bindposes = new List<Matrix4x4> ();
        
        for (int i=0; i<smr.Length; i++) {
            //lmat.AddRange (smr [i].materials);
            bones.AddRange (smr [i].bones);
            
            bones.ForEach ((b) => bindposes.Add (b.worldToLocalMatrix * transform.localToWorldMatrix));
            
            for (int sub=0; sub < smr[i].sharedMesh.subMeshCount; sub++) {
                CombineInstance ci = new CombineInstance ();
                ci.mesh = smr [i].sharedMesh;
                ci.subMeshIndex = sub;
                ci.transform = smr [i].localToWorldMatrix;
                lcom.Add (ci);
            }
            
            BoneWeight[] meshBoneweight = smr [i].sharedMesh.boneWeights;
            
            // May want to modify this if the renderer shares bones as unnecessary bones will get added.
            foreach (BoneWeight bw in meshBoneweight) {
                BoneWeight bWeight = bw;
                
                bWeight.boneIndex0 += boneOffset;
                bWeight.boneIndex1 += boneOffset;
                bWeight.boneIndex2 += boneOffset;
                bWeight.boneIndex3 += boneOffset;                
                
                boneWeights.Add (bWeight);
            }
            boneOffset += smr [i].bones.Length;
            if (smr [i].gameObject != gameObject) {
                Destroy (smr [i].gameObject);
            }
            //smr [i].gameObject.SetActive (false);
        }

        #region combine mesh
        SkinnedMeshRenderer r = gameObject.GetComponent<SkinnedMeshRenderer> ();
        if (r == null) {
            r = gameObject.AddComponent<SkinnedMeshRenderer> ();
        }
        
        
        
        r.materials = new Material[]{ mat };
        r.rootBone = transform;
        r.bones = bones.ToArray ();
        
        r.sharedMesh = new Mesh ();
        r.sharedMesh.CombineMeshes (lcom.ToArray (), true, false);
        r.sharedMesh.boneWeights = boneWeights.ToArray ();
        r.sharedMesh.RecalculateBounds ();
        //gameObject.SetActive (true);
        #endregion
    }
    
    public static Texture2D Flatten (Texture2D[] combines)
    {
        
        Texture2D newTexture = new Texture2D (0, 0);
        
        foreach (Texture2D resizeTex in combines) {
            if (resizeTex.width > newTexture.width) {
                newTexture.Resize (resizeTex.width, newTexture.height);
            }
            if (resizeTex.height > newTexture.height) {
                newTexture.Resize (newTexture.width, resizeTex.height);
            }
        }
        
        return newTexture;
    }
}
