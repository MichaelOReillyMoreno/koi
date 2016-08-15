using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// Desarrollado por Michael O'Reilly grupo 3
/// gestinoa el codigo del medidor de metros recorridos
/// existen tres digitos cuyo sprite va cambiando con el numero de metros recorridos
/// </summary>
public class distanciaControlador : MonoBehaviour {
    //si es flor usa el vector de posiciones del icono flor
    public bool esFlor;
    //digitos
    public GameObject dig1;
	public GameObject dig2;
	public GameObject dig3;
    public GameObject icono;
    Vector2[] posicionFlor;
    Vector2[] posicionEme;
    public GameObject Record;
    //------------------------
    //lleva la cuenta interna de metros recorridos
    private int contador;
    //auxiliares necesarios para desgranar los digitos del contador
    private int AuxN1;
    private int AuxN2;
    private int AuxN3;
    //digitos finales de game over
    public GameObject dFinal1;
    public GameObject dFinal2;
    public GameObject dFinal3;
    public GameObject dRec1;
    public GameObject dRec2;
    public GameObject dRec3;
    public GameObject carpa;
	public GameObject newRecord;
    void Start () {
        //el contador empieza en 1 y los digitos 1 y 3 se desactivan hasta que el contador sea un numero de dos y tres digitos respectivamente
        contador = 1;
        AuxN1 = 0;
        AuxN2 = 0;
        AuxN3 = 0;
        dig1.SetActive(false);
        dig3.SetActive(false);
        posicionEme = new[] { new Vector2(32f, 3f), new Vector2(50.6f, -1.8f) };
        posicionFlor = new[] { new Vector2(6.3f, 16.472f), new Vector2(19.6f, 16.472f) };

    }

    public void CambiarNumeros()//gestiona el medidor de metros y flores y la posicion y visibilidad de sus digitos.
    {
        if (PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial") != 0)
        {
            if (carpa != null)
            {
                if (contador == 1)
                {
                    CambiarPosicion(0);
                }
                //mientras el contador sea menor de 10 cambia el valor del digito 2
                if (contador <= 9)
                {
                    CambiarDigito(dig2, contador);
                }
                //cuando el contador llega a 10 activa el digito 1
                if (contador == 10)
                {
                    dig1.SetActive(true);

                }
                //mientras el contador este entre 10 y 99 cambia sus valores
                if (contador >= 10 && contador <= 99)
                {
                    CambiarCifras();
                    CambiarDigito(dig1, AuxN1);
                    CambiarDigito(dig2, AuxN2);

                }
                //cuando el contador llega a 100 activa el digito 3
                if (contador == 100)
                {
                    dig3.SetActive(true);
                    CambiarPosicion(1);
                }
                //mientras el contador este entre 10 y 99 cambia sus valores
                if (contador >= 100 && contador <= 999)
                {
                    CambiarCifras();
                    CambiarDigito(dig1, AuxN1);
                    CambiarDigito(dig2, AuxN2);
                    CambiarDigito(dig3, AuxN3);

                }
                //al terminar todo el proceso le suma 1 al contador
                contador++;
            }
        }
    }
    public void CambiarCifras() //desgrana las cifras de contador en numeros independientes y los asigna a los numeros auxialiares a fin de poder manejarlos como valores independientes
                                 //que poder asignar a los digitos
    {
        string aux = contador.ToString();
        AuxN1 = (int)char.GetNumericValue(aux[0]);
        AuxN2 = (int)char.GetNumericValue(aux[1]);
        if (contador >= 100 && contador <= 999)
        {
            AuxN3 = (int)char.GetNumericValue(aux[2]);
        }
        }
    
    public void CambiarDigito(GameObject dig,int numero)//cambia los digitos en funcion del valor del auxiliar en un rango de 0-9
    {
        dig.GetComponent<Image>().sprite = Resources.Load<Sprite>("" + numero);
    }

    public void CambiarPosicion(int posicion)//cambia la posicion de los digitos en funcion de un array de posiciones
    {
        if (esFlor == true)
        {
            icono.transform.localPosition = posicionFlor[posicion];
        }
        else
            {
                icono.transform.localPosition = posicionEme[posicion];
            }

    }
    public void DigitosGameOVer()//establece los valores finales de los metros recorridos y flores recogidas en el game over
    {
        if (contador <= 9)
        {
            dFinal1.SetActive(false);
            dFinal3.SetActive(false);
            dFinal2.GetComponent<Image>().sprite = dig2.GetComponent<Image>().sprite;
        }
        if (contador >= 10 && contador <= 99)
        {
            dFinal1.SetActive(false);
            dFinal2.GetComponent<Image>().sprite = dig1.GetComponent<Image>().sprite;
            dFinal3.GetComponent<Image>().sprite = dig2.GetComponent<Image>().sprite;
            if (esFlor == true)
            {
                dFinal2.transform.localPosition = new Vector2(-10.1f, -31.1f);
                dFinal3.transform.localPosition = new Vector2(12.9f, -31.1f);
            }
            else
            {
                dFinal2.transform.localPosition = new Vector2(-10.1f, -31.1f);
                dFinal3.transform.localPosition = new Vector2(12.9f, -31.1f);
            }
        }
        if (contador >= 100 && contador <= 999)
        {
            dFinal1.GetComponent<Image>().sprite = dig1.GetComponent<Image>().sprite;
            dFinal2.GetComponent<Image>().sprite = dig2.GetComponent<Image>().sprite;
            dFinal3.GetComponent<Image>().sprite = dig3.GetComponent<Image>().sprite;
        }
    }
    public void Almacenar()//almacena los valores de flores y metros recorridos en la memoria del dispositivo, muestra el record de metros recorridos y los digitos de record
                            //que se van a mostrar en funcion de los digitos que posea
    {
        float auxM=0;
        string auxStr = "";
        int auxR1 = 0;
        int auxR2 = 0;
        int auxR3 = 0;
        if (esFlor)
        {
            if (PlayerPrefs.HasKey("flores"))
            {
                float Auxf = PlayerPrefs.GetFloat("flores")+(contador-1);
                PlayerPrefs.SetFloat("flores", Auxf);
            }
            else
            {
                PlayerPrefs.SetFloat("flores", contador - 1);
            }
            //Debug.Log("Flores en total : "+PlayerPrefs.GetFloat("flores"));
        }
        else
        {
            if (PlayerPrefs.HasKey("metros"))
            {
                if ((contador - 1) > PlayerPrefs.GetFloat("metros"))
                {
                    PlayerPrefs.SetFloat("metros", contador - 1);
					newRecord.SetActive(true);
                }
            }
            else
            {
                PlayerPrefs.SetFloat("metros", contador - 1);
            }
            auxM = PlayerPrefs.GetFloat("metros");
            auxStr= auxM.ToString();
            if (auxM <= 9)
            {
                auxR1 = (int)char.GetNumericValue(auxStr[0]);
                dRec1.SetActive(false);
                dRec3.SetActive(false);
                CambiarDigito(dRec2, auxR1);
            }

            if (auxM >= 10 && contador <= 99)
            {
                auxR1 = (int)char.GetNumericValue(auxStr[0]);
                auxR2 = (int)char.GetNumericValue(auxStr[1]);
                dRec1.SetActive(false);
                CambiarDigito(dRec2, auxR1);
                CambiarDigito(dRec3, auxR2);
                dRec2.transform.localPosition = new Vector2(-8.4f, -15.6f);
                dRec3.transform.localPosition = new Vector2(7.6f, -15.6f);

            }

            if (auxM >= 100 && contador <= 999)
            {
                auxR1 = (int)char.GetNumericValue(auxStr[0]);
                auxR2 = (int)char.GetNumericValue(auxStr[1]);
                auxR3 = (int)char.GetNumericValue(auxStr[2]);
                CambiarDigito(dRec1, auxR1);
                CambiarDigito(dRec2, auxR2);
                CambiarDigito(dRec3, auxR3);

            }
            //Debug.Log("MaxMetros record : " + PlayerPrefs.GetFloat("metros"));
        }
    }
}
