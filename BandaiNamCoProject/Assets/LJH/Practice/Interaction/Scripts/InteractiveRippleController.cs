using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRippleController : MonoBehaviour
{
    private struct ShaderPropertyIDs
    {
        public int _BaseColor;
        public int _RippleColor;
        public int _RippleCenter;
        public int _RippleStartTime;
    }

    private MeshRenderer mr;
    private Material material;
    private Color prevColor;
    private ShaderPropertyIDs shaderIDs;
    private void Start()
    {
        shaderIDs = new ShaderPropertyIDs()
        {
            _BaseColor = Shader.PropertyToID("_BaseColor"),
            _RippleColor = Shader.PropertyToID("_RippleColor"),
            _RippleCenter = Shader.PropertyToID("_RippleCenter"),
            _RippleStartTime = Shader.PropertyToID("_RippleStartTime")
        };
        mr = GetComponent<MeshRenderer>();
        material = mr.sharedMaterial;
        prevColor = material.GetColor(shaderIDs._BaseColor);
        material.SetColor(shaderIDs._RippleColor, prevColor);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
    }

    private void CastRay()
    {
        var camera = Camera.main;
        var mousePosition = Input.mousePosition;
        var ray = camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane));

        if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject == gameObject)
        {
            StartRipple(hit.point);
        }
    }

    private void StartRipple(Vector3 center)
    {
        Color randomColor = Color.HSVToRGB(Random.value, 1, 1);
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetVector(shaderIDs._RippleCenter, center);
        block.SetFloat(shaderIDs._RippleStartTime, Time.time);
        block.SetColor(shaderIDs._BaseColor, prevColor);
        block.SetColor(shaderIDs._RippleColor, randomColor);
/*        material.SetVector(shaderIDs._RippleCenter, center);
        material.SetFloat(shaderIDs._RippleStartTime, Time.time);

        material.SetColor(shaderIDs._BaseColor, prevColor);
        material.SetColor(shaderIDs._RippleColor, randomColor);*/
        mr.SetPropertyBlock(block);
        prevColor = randomColor;
    }
}
