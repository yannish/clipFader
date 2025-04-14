using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public class WrappedAnimator : MonoBehaviour
{
    public string shaderPropName;

    private int _shaderPropID = -1;
    public int ShaderPropID
    {
        get
        {
            if(_shaderPropID < 0)
                _shaderPropID = Shader.PropertyToID(shaderPropName);
            return _shaderPropID;
        }
    }
    
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
    
    private Renderer[] _renderers;
    public Renderer[] Renderers
    {
        get
        {
            if(_renderers == null)
                _renderers = GetComponentsInChildren<Renderer>();
            return _renderers;
        }
    }
    
    private Animator animator;
    
    public PlayableGraph graph { get; set; }
    
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = null;
    }

    public void ApplyMaterialPropBlock(float value)
    {
        MatPropBlock.SetFloat(ShaderPropID, value);
        foreach (var rend in Renderers)
        {
            rend.SetPropertyBlock(MatPropBlock);
        }
    }

}
