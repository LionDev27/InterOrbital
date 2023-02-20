# Reglas Sintaxis Scripts

## Forma de escribir diferentes campos

* ### PascalCase 

  * Namespaces            `ClawsomeGames.UI.MainMenu`
  * Clases                `public class MyClass` 
  * Métodos               `public void MethodName`
  * Propiedades           `public string PropertyName`
  * Variables Estáticas   `public static int SomeValue`

* ### camelCase

  * Parámetros            `public void MethodName(bool someParameter)`


## No omitir la visibilidad de cada elemento. 

>MAL
>
>`int speed;`
>
>BIEN
>
>`private int speed;`


## Para cada ***'Event'*** o ***'Action'*** añadir el prefijo 'On'

>EJEMPLO
>
>`public UnityEvent OnDeath;`


## Usar declaraciones singulares por línea

>MAL
>
>`private int _speed, _rotateSpeed;`
>
>BIEN
>
>`private int _speed;`
>`private int _rotateSpeed;`

