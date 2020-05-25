using System;
using UnityEngine;

namespace LinkDots
{
    [Serializable]
    public class ColorData
    {
        public int ColorId;
        public string ColorHexString;
        private Color _unityColor;
        public Color UnityColor
        {
            get
            {
                if (_unityColor == Color.clear)
                {
                    if (ColorUtility.TryParseHtmlString(ColorHexString, out Color unityColor))
                    {
                        _unityColor = unityColor;
                    }
                    else
                    {
                        Debug.LogError($"Invalid Color String!{ColorHexString}");
                        _unityColor= Color.magenta;
                    }
                }  
                return _unityColor;
            }
        }
    }
}