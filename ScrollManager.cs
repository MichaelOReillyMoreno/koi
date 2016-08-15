using UnityEngine;
using System.Collections;

/// <summary>
/// Desarrollado a partir del codigo del profesor por Michael O'Reilly grupo 3
/// Controla la velocidad global de desplazamiento del fondo y el rio,gestiona su colocacion
/// Determina cual es el sprite mas a abajo
/// Los sprites deben estar orenados en la jerarquia en el modo editor, antes de comenzar el juego
/// coloca un nuevo sprite aleatoriamente en la posicion superior
/// </summary>
public class ScrollManager : MonoBehaviour
{
    /// <summary>
    /// maxSpeed velocidad maxima de desplazamiento del fondo/rio
    /// speed velocidad inicial de desplazamiento del fondo/rio
    /// </summary>
    public float maxSpeed = 0;
    public float speed = 0;
    public int numeroDeFondos;
    private int numeroMinimoDeFondos = 1;
    public bool esFondo;
    public SpriteRenderer mostUpBackground;
    /// <summary>
    ///mostDownBackground el fonfo que esta mas abajo
    /// random valor aleatorio para el fondo que determinara que sprite carga
    /// lastRamdom anteriorrandom generado almacenado para no repetirlo justo despues(en el futuro se sustituira por un array de los ultimos ramdom generados para q la posibilidad de repeticion inmediatamente despues sea nula
    /// </summary>
    public SpriteRenderer mostDownBackground;
    private int random;
    private int lastRamdom;

    public GameObject distancia;
    private distanciaControlador DistanciaControlador;
    /// <summary>
    /// Direccion de desplazamiento.
    /// </summary>

    // Use this for initialization
    void Start()
    {
        if (esFondo)
            DistanciaControlador = distancia.GetComponent<distanciaControlador>();
        lastRamdom = 99;
        Random.seed = 42;
        //Numera a los hijos // childCount cuenta los hijos que tiene 
        for (int index = 0; index < transform.childCount; index++)
            transform.GetChild(index).GetComponent<BackGroundItemController>().SetIndex(index);

        //El primer hijo es el que esta mas arriba
        mostDownBackground = transform.GetChild(0).GetComponent<SpriteRenderer>();

        //El ultimo hijo es el que esta mas abajo // childCount es 3 y le resta 1 para que sea 2( el mas bajo)
        mostUpBackground = transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>();

    }
    //Llamada por el trigger controller
    public void SetUpBackGround(Transform t)//cambia la posicion de los 3 fondos.
    {
        //Se posiciona el transform del background a su nueva ubicacion, arriba del todo
        if (esFondo)
        {
            //t.position = new Vector3(0, mostUpBackground.transform.position.y + mostUpBackground.sprite.bounds.size.y - 5.13f, 0);
            t.position = new Vector3(0, mostUpBackground.transform.position.y + mostUpBackground.sprite.bounds.size.y, 0);
            DistanciaControlador.CambiarNumeros();
        }
        else
        {
            t.position = new Vector3(0, mostUpBackground.transform.position.y + mostUpBackground.sprite.bounds.size.y, 0);
        }
        ChangeSprite(t);

        //Ahora el que esta mas a arriba es este
        mostUpBackground = t.GetComponent<SpriteRenderer>();

        //Leo su indice
        int index = t.GetComponent<BackGroundItemController>().GetIndex();

        //Ahora se debe establecer cual es el que ahora estara mas abajo pues ahora era este 
        if (index == transform.childCount - 1)
            //Si tenia el indice maximo, estableco al primero
            mostDownBackground = transform.GetChild(0).GetComponent<SpriteRenderer>();
        else
            //Si no tenia el indice maximo, estableco al siguiente
            mostDownBackground = transform.GetChild(index + 1).GetComponent<SpriteRenderer>();

    }
    private void ChangeSprite(Transform t)//genera un numero aleatorio diferente al ultimo y despues establece a que sprite esta asociado y lo iguala al fondo
    {
        do
        {
            random = Random.Range(numeroMinimoDeFondos, numeroDeFondos + 1);
        }
        while (random == lastRamdom);

        lastRamdom = random;

        if (this.transform.tag == "backgroundItem")
        {
            t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("fondo" + random);
        }
        if (this.transform.tag == "riverItem")
        {
            t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("rio" + random);
        }

    }
    public void Pararfondos()//reinicia la escena actual.
    {
        for (int i = 0; i <= numeroDeFondos - 1; i++)
        {
            transform.GetChild(i).GetComponent<BackGroundItemController>().stop = true;
        }

    }
    public void Reanudarfondos()//reinicia la escena actual.
    {
        for (int i = 0; i <= numeroDeFondos - 1; i++)
        {
            transform.GetChild(i).GetComponent<BackGroundItemController>().stop = false;
        }
    }
    public float GetVelocidad()
    {
        return transform.GetChild(0).GetComponent<BackGroundItemController>().speed;
    }
    public void SetVelocidad(float velocidad)
    {   // antes 2
        for (int i = 0; i <= numeroDeFondos - 1; i++)
        {
            transform.GetChild(i).GetComponent<BackGroundItemController>().speed = velocidad;
        }
    }

    public void SetFondos(int numeroDeFondos, int numeroMinimoDeFondos)
    {
        this.numeroDeFondos = numeroDeFondos;
        this.numeroMinimoDeFondos = numeroMinimoDeFondos;
    }
}