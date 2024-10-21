using UnityEngine;
namespace Utils
{
    public class LoadAssetAttribute : PropertyAttribute
    {
        public string assetName;
        public LoadAssetAttribute(string assetName)
        {
            this.assetName = assetName;
        }
    }
}