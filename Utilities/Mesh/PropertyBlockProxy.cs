using UnityEngine;
using UnityEngine.Rendering;

namespace Egsp.Utils.MeshUtilities
{
    public class PropertyBlockProxy
    {
        /// <summary>
        /// Материал, которому будет устанавливаться блок значений.
        /// </summary>
        public readonly Renderer TargetRenderer; 
        
        private readonly MaterialPropertyBlock _propertyBlock;

        
        public PropertyBlockProxy(Renderer targetRenderer)
        {
            this.TargetRenderer = targetRenderer;
            _propertyBlock = new MaterialPropertyBlock();
            targetRenderer.GetPropertyBlock(_propertyBlock);
        }

        public PropertyBlockProxy SetInt(string fieldName, int value)
        {
            _propertyBlock.SetInt(fieldName, value);
            return this;
        }
        
        public PropertyBlockProxy SetFloat(string fieldName, float value)
        {
            _propertyBlock.SetFloat(fieldName,value);
            return this;
        }

        public PropertyBlockProxy SetColor(string fieldName, Color value)
        {
            _propertyBlock.SetColor(fieldName,value);
            return this;
        }

        public PropertyBlockProxy SetTexture(string fieldName, Texture texture)
        {
            _propertyBlock.SetTexture(fieldName, texture);
            return this;
        }
        
        public PropertyBlockProxy SetTexture(string fieldName, RenderTexture renderTexture, RenderTextureSubElement type)
        {
            _propertyBlock.SetTexture(fieldName, renderTexture, type);
            return this;
        }

        public Texture GetTexture(string fieldName = "_MainTex")
        {
            return _propertyBlock.GetTexture(fieldName);
        }
        

        public void Apply()
        {
            TargetRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}