using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
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

        public static int ObtainNumName(GameObject gameObject)
        {
            string nombre = gameObject.name;
            int numero = 0;

            // Utilizar expresiones regulares para buscar el número en el nombre
            Match match = Regex.Match(nombre, @"\(\d+\)");
            if (match.Success)
            {
                // Obtener el número y convertirlo a entero
                string numeroString = match.Value.Trim('(', ')');
                int.TryParse(numeroString, out numero);
            }

            return numero;
        }
    }
}
