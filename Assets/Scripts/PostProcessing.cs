using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessing : MonoBehaviour {

    [SerializeField]
    private Material _material;

    private void OnRenderImage(RenderTexture source, RenderTexture dest){
        Graphics.Blit(source, dest, _material);
    }
}