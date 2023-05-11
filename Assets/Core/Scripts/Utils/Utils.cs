using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Utils
{
    public static class Utils
    {
        public static void ChangueAlphaColor(this Image image, float newAlpha )
        {
           if(image != null)
           {
                var imageColor = image.color;
                imageColor.a = newAlpha;
                image.color = imageColor;
            }
        }
    }
}
