using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//gestiona las esferas de vida que rotan y las destruyen si pierde vida.
public class VidaController : MonoBehaviour {
    float speedCirculo;
    float speedEsferas;
    bool stop;
    public GameObject esfera1;
    public GameObject esfera2;
    public GameObject esfera3;
    public Image circulo;
    private Animator orb1;
    private Animator orb2;
    private Animator orb3;
    private int nEsferas;

    void Start () {
        orb1 = this.transform.GetChild(0).GetChild(0).transform.GetComponent<Animator>();
        orb2 = this.transform.GetChild(0).GetChild(1).transform.GetComponent<Animator>();
        orb3 = this.transform.GetChild(0).GetChild(2).transform.GetComponent<Animator>();
        nEsferas = 3;
	}

    // Update is called once per frame
    void Update()
    {
         if (!stop)
         { 
            if (nEsferas >= 3)
            {
                esfera1.transform.Rotate(Vector3.forward * 100f * Time.deltaTime);

            }

            if (nEsferas >= 2)
            {
                esfera2.transform.Rotate(Vector3.forward * 100f * Time.deltaTime);

            }

            if (nEsferas >= 1)
            {
                circulo.transform.Rotate(Vector3.forward * -100f * Time.deltaTime);
                esfera3.transform.Rotate(Vector3.forward * 100f * Time.deltaTime);
            }
        }
    }
    public void Restarvida()//destruye una esfera cuando pierde vida
    {
        nEsferas--;
        if (nEsferas == 2)
        {
            orb1.SetTrigger("Destruccion");
        }
        if (nEsferas == 1)
        {
            orb2.SetTrigger("Destruccion");
        }
        if (nEsferas == 0)
        {
            orb3.SetTrigger("Destruccion");
            circulo.enabled = false;
        }

    }
    public float GetVelocidadEsferas()
    {
        return speedEsferas;
    }
    public void SetVelocidadEsferas(float velocidad)
    {
        speedEsferas = velocidad;
    }
    public float GetVelocidadCirculo()
    {
        return speedCirculo;
    }
    public void SetVelocidadCirculo(float velocidad)
    {
        speedCirculo = velocidad;
    }
    public void SetStop(bool para)
    {
        stop = para;
    }
    public void StopAnimaciones(bool para)
    {
        if (para == true)
        {
            esfera1.GetComponent<Animator>().SetFloat("speed", 0);
            esfera2.GetComponent<Animator>().SetFloat("speed", 0);
            esfera3.GetComponent<Animator>().SetFloat("speed", 0);
        }
        if (para == false)
        {
            esfera1.GetComponent<Animator>().SetFloat("speed", 1);
            esfera2.GetComponent<Animator>().SetFloat("speed", 1);
            esfera3.GetComponent<Animator>().SetFloat("speed", 1);
        }

    }
}
