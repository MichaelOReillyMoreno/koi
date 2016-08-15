using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// Desarrollado por Michael O'Reilly grupo 3
/// Gestiona los menus del juego y el final del mismo.
/// </summary>
public class CanavasController : MonoBehaviour {
    //diversos objetos que forman parte del menu.
    public GameObject izquierda;
    public GameObject derecha;
    public GameObject sonido;
    public GameObject menu;
    public GameObject gameoverMenu;
    public GameObject pausa;//Que es mas optimo buscarlo por codigo o hacerlo publico?
    public GameObject metros;
    public GameObject puntos;
    public GameObject record;
    public GameObject reiniciar;
    public GameObject VolverMenu;
    private distanciaControlador controladorFlores;
    private distanciaControlador controladorMetros;

    public GameObject fade;
    private FadeOutIn fadeScript;
    private bool sonidoff;
    private bool partidaFinalizada;
    private bool partidaPausada;
    //variables que controlan la visibilidad y la realentizacion del tiempo.
    private float visible;
    private float realentizacion;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Juego")
        {
            pausa.GetComponent<Image>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            controladorFlores = puntos.GetComponent<distanciaControlador>();
            controladorMetros = metros.GetComponent<distanciaControlador>();

        }
        partidaFinalizada = false;
        partidaPausada = false;
        sonidoff = false;
        fadeScript = fade.GetComponent<FadeOutIn>();
    }
    void Update()
    {
        //cuando se pulsa la tecla atras en android realiza distintos comportamintos.
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SceneManager.GetActiveScene().name == "Juego")
                {
                    if (partidaFinalizada == false)
                    {
                        PararContinuar();//si la partida esta en curso la pausa.
                    }
                    else
                    {
                        SalirMenu();//si la partida ha finalizado sale al menu.
                    }
                }
                if (SceneManager.GetActiveScene().name == "MenuEscena")
                {
                    SalirApp();//si esta en el menu sale del juego.
                }


          }

        }
    }

    public void ReiniciarEscena()//reinicia la escena actual.
    {
        fadeScript.Llamar(1);
        Time.timeScale = 1;
    }
    
    public void PararContinuar()//para el tiempo, hace visible el menu y cambia el sptite del boton pause a play o a pause de nuevo y oculta el menu.
    {
        if(PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial")!=0)
        {
            if (partidaFinalizada == false)
            {
                if (partidaPausada == false)
                {
                    GetComponent<PararTiempo>().Parar();
                    menu.SetActive(true);
                    partidaPausada = true;
                    pausa.GetComponent<Image>().sprite = Resources.Load<Sprite>("play");
                }
                else if (partidaPausada == true)
                {
                    GetComponent<PararTiempo>().Continuar();
                    menu.SetActive(false);
                    partidaPausada = false;
                    pausa.GetComponent<Image>().sprite = Resources.Load<Sprite>("pause");
                }

            }
        }
    }


    public void Sonido()
    {
        if (sonidoff == false)
        {
            sonido.GetComponent<Image>().sprite = Resources.Load<Sprite>("off");
            AudioListener.pause = true;
            sonidoff = true;
        }
        else
        {
            sonido.GetComponent<Image>().sprite = Resources.Load<Sprite>("on");
            AudioListener.pause = false;
            sonidoff = false;
        }
    }

    public void SalirMenu()//vuelve al menu principal.
    {
        fadeScript.Llamar(0);
    }
    public void SalirApp()//cierra aplicacion.
    {
        Application.Quit();
    }
    public void Jugar()//carga el juego desde el menu.
    {
        fadeScript.Llamar(1);
    }
    public void GameOver()//que el juego ha terminado e inicia la corrutina EsperaDesaparece().
    {
        partidaFinalizada = true;
        StartCoroutine(EsperaDesaparece());
        StopCoroutine(EsperaDesaparece());

    }
    IEnumerator EsperaDesaparece()//corrutina que cuando se acaba la partida hace progresivamente invisible todo el menu,
                                 //despues hace progresivamente visible el menu de game over y para el tiempo.
    {
        visible = 1f;
        realentizacion = 1f;

        controladorFlores.DigitosGameOVer();
        controladorMetros.DigitosGameOVer();
        controladorFlores.Almacenar();
        controladorMetros.Almacenar();

        while (visible >= 0)
        {
            pausa.GetComponent<Image>().material.color = new Color(visible, visible, visible, visible);
            visible = visible - 0.033f;
            yield return new WaitForSeconds(0.033f);
        }

        menu.SetActive(true);
        gameoverMenu.SetActive(true);
        record.SetActive(true);
        VolverMenu.transform.localPosition = new Vector2(0f, -68.8f);
        VolverMenu.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(30f, 30f);
        reiniciar.transform.localPosition = new Vector2(0f, -11.3f);
		VolverMenu.transform.localPosition = new Vector2(2.9f, -68.8f);
        reiniciar.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(90f, 90f);
        pausa.SetActive(false);
        metros.SetActive(false);
        puntos.SetActive(false);
        izquierda.SetActive(false);
        derecha.SetActive(false);
        visible = 0;
        while (visible <= 1)
        {
            pausa.GetComponent<Image>().material.color = new Color(visible, visible, visible, visible);
            visible = visible + 0.033f;
            yield return new WaitForSeconds(0.033f);
        }

        pausa.GetComponent<Image>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        while (realentizacion >= 0.2f)//esto genera el bug que hace q el tiempo empieze realentizado a veces
        {
            Time.timeScale = realentizacion;
            realentizacion = realentizacion - 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<PararTiempo>().Parar();
        Time.timeScale = 1;
        yield break;//es necesario para cerrar la corutina
    }
}