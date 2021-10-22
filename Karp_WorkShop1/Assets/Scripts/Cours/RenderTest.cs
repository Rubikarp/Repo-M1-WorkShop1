using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTest : MonoBehaviour
{
    public SkinnedMeshRenderer selfMR;

    MaterialPropertyBlock propBlock;

    void Start()
    {
        //permet d'overide les param sans modif le mat ou créer d'instance
        propBlock = new MaterialPropertyBlock();
        
        //Recup Data
        selfMR.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetColor("_Color", Color.blue);

        //Push Data
        selfMR.SetPropertyBlock(propBlock);

        //ou osef
        //selfMR.sharedMaterial.SetColor("_Color", Color.blue);
    }

    void Update()
    {
        
    }
}
