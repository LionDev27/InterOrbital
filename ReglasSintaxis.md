# Reglas Sintaxis Scripts

## 1. Forma de escribir diferentes campos

* ### PascalCase 

  * Namespaces            `ClawsomeGames.UI.MainMenu`
  * Clases                `public class MyClass` 
  * Métodos               `public void MethodName`
  * Propiedades           `public string PropertyName`
  * Variables Estáticas   `public static int SomeValue`

* ### camelCase

  * Parámetros            `public void MethodName(bool someParameter)`
  * Variables Públicas    `public int somePublicVariable`
  * Variables Privadas CON UN _ `private bool _somePrivateVariable`


## 2. No omitir la visibilidad de cada elemento. 

>MAL
>
>`int speed;`
>
>BIEN
>
>`private int speed;`


## 3. Para cada ***'Event'*** o ***'Action'*** añadir el prefijo 'On'

>EJEMPLO
>
>`public UnityEvent OnDeath;`


## 4. Usar declaraciones singulares por línea

>MAL
>
>`private int _speed, _rotateSpeed;`
>
>BIEN
>
>`private int _speed;`
>`private int _rotateSpeed;`

## 5. Usar Singleton para todos los Managers (GameManager, AudioManager,...)

## 6. Usar lo mínimo posible variables publicas

Para ello utilizar Propiedades, o también, variables privadas usando la etiqueta [SerializedField]

## 7. Meter todo el código en Namespaces para evitar posibles choques problemas de librerias 
