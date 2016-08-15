using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// Desarrollado por Michael O'Reilly grupo 3
/// mueve la carpa de izquierda a derecha
/// </summary>
public class CarpaScript : MonoBehaviour {
    /// <summary>
    /// moveSpeed es la velocidad de movimiento de la carpa
    ///vida es los puntos de vida de la carpa
    /// movement vector que contiene la X e Y de la carpa
    /// direcion almacena la direcion de la carpa en el eje X(-1, 0, 1)
    /// invulnerable es un booleano que durante unos segundos despues de recivir daño se activa para impedir q la carpa reciva mucho daño seguido
    /// body compoenente Rigidbody2D
    /// canavasController y controlador son el script q controla el Canavas y el gameobject donde se encuentra.
    /// PuntosControlador y puntos son el script q controla los puntos y el gameobject donde se encuentra.
    /// SaludControlador y salud son el script q controla las esferas de vida y el gameobject donde se encuentra.
    /// </summary>
    /// 
    public Image derecha;
    public Image izquierda;
    public float vMuerte;
    private Animator ani;
    public float moveSpeed;
	public int vida;
    private Vector2 movement;
    private float direccion;
	private bool invulnerable;
    private Rigidbody2D body;
	private CanavasController canavasController;
	public GameObject controlador;
    public GameObject puntos;
    private distanciaControlador PuntosControlador;
    public GameObject salud;
    private VidaController SaludControlador;
    bool stop;

    public float tiempoInvulnerabilidad = 2.5f;

    void Start () {
        ani = this.transform.GetComponent<Animator>();
        stop = false;
        invulnerable = false;
		vida = 3;
        this.movement = new Vector2();
        //recoje el Rigidbody2D de la carpa
        this.body = this.GetComponent<Rigidbody2D>();
        direccion = 0;
        //busca al controlador y su script dentro de la escena para su posterior uso  
		canavasController = controlador.GetComponent<CanavasController>();
        PuntosControlador = puntos.GetComponent<distanciaControlador>();
        SaludControlador = salud.GetComponent<VidaController>();
    }

    void FixedUpdate()
    {
        if(PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial") != 0)
		    Mover();
       
        //moverTeclado();
        this.body.velocity = this.movement;
    }
	
	void Mover()
	{
		 if (transform.position.x >= -1.30f && direccion < 0 && PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial") != 0) //si esta en el centro y te mueves hacia la izquierda entonces te mueves 
        {
            this.movement.x = direccion * moveSpeed * Time.deltaTime;
        }
        else if (transform.position.x <= 1.30f && direccion > 0 && PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial") != 0)  //si esta en el centro y te mueves hacia la derecha entonces te mueves 
        {
            this.movement.x = direccion * moveSpeed * Time.deltaTime;
        }
        else
            this.movement.x = 0 * moveSpeed * Time.deltaTime; //si estas "fuera" del centro entonces no te mueves, o tambien si no estás pulsando ningun botón  
        
		
	}

    void moverTeclado()
    {
        float movHorizontal = Input.GetAxisRaw("Horizontal");

        if (transform.position.x >= -1.30f && movHorizontal < 0)  // mov izquierda
        {
            direccion = -1;
            izquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("flecha1");
        }
        else if (transform.position.x <= 1.30f && movHorizontal > 0)  // mov derecha
        {
            direccion = 1;
            derecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("flecha1");
        }
        else
        {
            direccion = 0;
            derecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("flecha2");
            izquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("flecha2");
        }


        this.movement.x = direccion * moveSpeed * Time.deltaTime;

    }
    
    //funciones q manejan direccion controladas por botones de direccion invisibles
    public void moverDerecha()
    {
        if (!stop && vida > 0 && PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial") != 0)
        {
            direccion = 1;
            derecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("flecha1");

        }

    }
    public void moverIzquierda()
    {
        if (!stop && vida > 0 && PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial") != 0)
        {
            direccion = -1;
            izquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("flecha2");
        }

    }
    public void soltarDireccion()
    {
        direccion = 0;
        derecha.GetComponent<Image>().sprite = Resources.Load<Sprite>("flecha2");
        izquierda.GetComponent<Image>().sprite = Resources.Load<Sprite>("flecha2");
    }

    void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "coin")//si colisiona con flor suma uno a la puntuacion
		{
			//Destroy(collider.gameObject);
            PuntosControlador.CambiarNumeros();
        }
		if (collider.tag == "enemy" && invulnerable==false)//si es enemigo le resta uno a la vida e inicia una corutina para hacerlo invlunerable durante x segundos
		{

            vida--;
            SaludControlador.Restarvida();
			//StartCoroutine(invulnerabilidad());
            //StopCoroutine(invulnerabilidad());
            if (vida <= 0)//si la vida llega a cero lo destruye e incia la funcion del canavas de game over que muestra resultados de partida
            {
                canavasController.GameOver();
                ani.SetTrigger("muerte");
                // GetComponent<SpriteRenderer>().enabled = false;
                this.movement.y =  -vMuerte * Time.deltaTime;
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                StartCoroutine(invulnerabilidad());
                StopCoroutine(invulnerabilidad());
                ani.SetTrigger("danyo");
            }
        }	
	}
    //corutina que gestiona invulnerabilidad
	public IEnumerator invulnerabilidad()
	{
		invulnerable = true;
        yield return new WaitForSeconds(0.8f);
        Material material = GetComponent<Renderer>().material;

        for (float i = 0; i < tiempoInvulnerabilidad; i += 0.5f)
        {
            float aux = 0.1f;

            material.color = new Color(0, 0, 0, 0);
            yield return new WaitForSeconds(aux);

            material.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.5f - aux);
        }
        //gameObject.GetComponent<Renderer>().material.color = new Color (0,0,0,0);

        invulnerable = false;
        yield break;//es necesario para cerrar la corutina
    }
    public float GetVelocidad()
    {
        return moveSpeed;
    }
    public void SetVelocidad(float velocidad)
    {
        moveSpeed = velocidad;
    }
    public void SetStop(bool para)
    {
        stop = para;
    }
}
