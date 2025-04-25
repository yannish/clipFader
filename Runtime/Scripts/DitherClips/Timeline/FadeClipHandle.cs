using UnityEngine;

public class FadeClipHandle : MonoBehaviour
{
    private MaterialPropertyBlock _matPropBlock;
    public MaterialPropertyBlock MatPropBlock
    {
        get
        {
            if(_matPropBlock == null)
                _matPropBlock = new MaterialPropertyBlock();
            return _matPropBlock;
        }
    }
    
    public Renderer[] _renderers;
    public Renderer[] Renderers
    {
        get
        {
            if(_renderers == null)
                _renderers = GetComponentsInChildren<Renderer>();
            return _renderers;
        }
    }


    public string shaderPropertyName = "";
    public string cachedShaderPropertyName;
    public int _shaderPropertyID = -1;
    public int ShaderPropertyID
    {
        get
        {
            if (_shaderPropertyID <= 0 || shaderPropertyName != cachedShaderPropertyName)
            {
                Debug.LogWarning("Updated cached shader ID");
                _shaderPropertyID = Shader.PropertyToID(shaderPropertyName);
                cachedShaderPropertyName = shaderPropertyName;
            }
            return _shaderPropertyID;
        }
    }
    
    public void Fade(float level)
    {
        // Debug.LogWarning($"Fade level: {level}");
        MatPropBlock.SetFloat(ShaderPropertyID, level);
        foreach(var renderer in Renderers)
            renderer.SetPropertyBlock(MatPropBlock);
    }

    public void Initialize()
    {
        // Debug.LogWarning($"Initializing fadeClipHandle: {this.gameObject.name}", gameObject);
        _matPropBlock = new MaterialPropertyBlock();
        _renderers = GetComponentsInChildren<Renderer>();
    }
}
