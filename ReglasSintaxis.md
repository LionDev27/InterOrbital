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
><br>
> `private int _rotateSpeed;`

## 5. Usar lo mínimo posible variables publicas

Para ello utilizar Propiedades, o también, variables privadas usando la etiqueta [SerializedField]

## 6. Seguir el siguiente orden en cada clase Monobehaviour 

 1. Variables Estáticas
 2. Variables Privadas
 3. Variables Publicas
 4. Métodos de MonoBehaviour -> Awake -> Start -> Update -> FixedUpdate -> Todos los demás
 5. Métodos Privados
 6. Métodos Públicos

>`public class MyClass : MonoBehaviour`
>{` 
>private static int SomeStaticVariable;
 
 private bool _someBoolVariable;
 
 public float rotateSpeed;
 
 private void Awake()
 {
  //code here
 }
 
 private void Start()
 {
  //code here
 }
 
 private void Update()
 {
  //code here
 }
 
 private void FixedUpdate()
 {
  //code here
 }
 
 private void MethodPrivate()
 {
  //code here
 }
 
 public void FixedUpdate()
 {
  //code here
 }
 
}`
 
 
## 7. Cada llave está en un sola línea

>MAL
>
>`public void CreateSomething(){`
><br>
>    `// code`
><br>
>`}`
>
>BIEN
>
>`public void CreateSomething()`
><br>
>`{`
><br>
>    `//code`
><br>
>`}`
> 

## 8. Usar Singleton para todos los Managers (GameManager, AudioManager,...)

## 9. Meter todo el código en Namespaces para evitar posibles choques problemas de librerias 
