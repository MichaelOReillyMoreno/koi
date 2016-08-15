using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PararTiempo : MonoBehaviour {
    bool stop;
    public GameObject maestro;
    MasterThrowScript propGenerador;
    public GameObject vida;
    VidaController vidaContr;
    public GameObject carpa;
    public GameObject scrollF;
    public GameObject scrollR;
    CarpaScript carSc;
    ScrollManager sMaF;
    ScrollRiverManager sMaR;
    PropController propsCode;
    float velocidadFondos;
    float velocidadRio;
    float velocidadFSlow;
    float velocidadRSlow;
    float fAux;
    float rAux;

    float cAux;
    float velocidadCarpa;
    float velocidadCarpaSlow;

    float esfAux;
    float velocidadEsf;
    float velocidadEsfSlow;

    float cirAux;
    float velocidadCir;
    float velocidadCirSlow;

    public GameObject botonPausa;
    void Start () {
        vidaContr = vida.GetComponent<VidaController>();
        carSc = carpa.GetComponent<CarpaScript>();
        sMaF = scrollF.GetComponent<ScrollManager>();
        sMaR = scrollR.GetComponent<ScrollRiverManager>();
        velocidadFondos = 0;
        velocidadRio = 0;
        propGenerador = maestro.GetComponent<MasterThrowScript>();
    }
	
    public void Parar()
    {
        propGenerador.SetStop(true);
        stop = true;
        vidaContr.SetStop(true);
        vidaContr.StopAnimaciones(true);
        velocidadEsf = vidaContr.GetVelocidadEsferas();
        velocidadCir = vidaContr.GetVelocidadCirculo();
        velocidadCarpa = carSc.GetVelocidad();
        velocidadCarpa = carSc.GetVelocidad();
        //carpa.GetComponent<Animator>().Stop();
        carpa.GetComponent<Animator>().SetFloat("speed", 0);
        velocidadFondos = sMaF.GetVelocidad();
        velocidadRio = sMaR.GetVelocidad();
        velocidadFSlow = velocidadFondos / 10;
        velocidadRSlow= velocidadRio / 10;
        velocidadCarpaSlow = velocidadCarpa / 10;
        velocidadCirSlow= velocidadCir / 10;
        velocidadEsfSlow = velocidadEsf / 10;
        carSc.SetStop(true);
        sMaF.Pararfondos();
        sMaR.Pararfondos();
        PararContinuarProps();
    }
    public void Continuar()
    {
        stop = false;
        //carpa.GetComponent<Animator>().Play(stateInfo.fullPathHash);
        carpa.GetComponent<Animator>().SetFloat("speed", 1);
        botonPausa.GetComponent<EventTrigger>().enabled = false;
        StartCoroutine(delay());
    }
    public void PararContinuarProps()
    {
        //prop = GameObject.Find("fondo1");
        foreach (GameObject prop in GameObject.FindGameObjectsWithTag("coin"))
        {
            propsCode = prop.GetComponent<PropController>();
            if (stop)
            {
                propsCode.stop = true;
                //propsCode.GetComponent<Animator>().SetFloat("speed", 0);
            }
            if (!stop)
            {
                propsCode.stop = false;
                //propsCode.GetComponent<Animator>().SetFloat("speed", 1);
            }
        }
        foreach (GameObject prop in GameObject.FindGameObjectsWithTag("enemy"))
        {
            propsCode = prop.GetComponent<PropController>();
            if (stop)
            {
                propsCode.stop = true;
                propsCode.GetComponent<Animator>().SetFloat("speed", 0);
            }
            if (!stop)
            {
                propsCode.stop = false;
                propsCode.GetComponent<Animator>().SetFloat("speed", 1);
            }
        }
    }
    IEnumerator delay()
    {
        fAux = velocidadFSlow;
        rAux = velocidadRSlow;
        cAux = velocidadCarpaSlow;
        esfAux = velocidadEsfSlow;
        cirAux = velocidadCirSlow;
        yield return new WaitForSeconds(0.5f);
        carSc.SetStop(false);
        PararContinuarProps();
        propGenerador.SetStop(false);
        vidaContr.SetStop(false);
        vidaContr.StopAnimaciones(false);
        sMaF.Reanudarfondos();
        sMaR.Reanudarfondos();
        //Debug.Log("vRioSl "+ velocidadRSlow+"vFondoSL "+velocidadFSlow);
        //Debug.Log("vRioAC " + velocidadRio + "vFondoAC " + velocidadFondos);
        while ((velocidadFSlow < velocidadFondos) || (velocidadRSlow < velocidadRio))
        {
            velocidadCirSlow += cirAux;
            velocidadEsfSlow += esfAux;
            velocidadCarpaSlow += cAux;
            velocidadFSlow += fAux;
            velocidadRSlow += rAux;
            carSc.SetVelocidad(velocidadCarpaSlow);
            sMaF.SetVelocidad(velocidadFSlow);
            sMaR.SetVelocidad(velocidadRSlow);
            vidaContr.SetVelocidadCirculo(velocidadCirSlow);
            vidaContr.SetVelocidadEsferas(velocidadEsfSlow);
            yield return new WaitForSeconds(0.1f);
        }
        //Debug.Log("HOLA ");
        botonPausa.GetComponent<EventTrigger>().enabled = true;
        yield break;
    }
}
