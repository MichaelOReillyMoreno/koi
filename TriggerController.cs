using UnityEngine;
using System.Collections;

/// <summary>
/// Desarrollado a partir del codigo del profesor por Michael O'Reilly grupo 3
/// Gestiona las colisiones de los backgrounds y cualquier otro objeto que colisione con el
/// </summary>
public class TriggerController : MonoBehaviour
{
    public GameObject distancia;
    public ScrollManager scrollManager;
    public ScrollManager scrollManagerRiver;
	private distanciaControlador DistanciaControlador;
    void Start()
	{
		DistanciaControlador=distancia.GetComponent<distanciaControlador>();
	}
    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.tag == "backgroundItem") {
			//Se envia al background de arriba si colisiona con el trigger
			scrollManager.SetUpBackGround (trigger.transform);
                DistanciaControlador.CambiarNumeros();
        }
		else
		{
			if (trigger.tag == "riverItem") {
				//Se envia al background de arriba si el contador es par suma uno a la distancia recorrida
				scrollManagerRiver.SetUpBackGround (trigger.transform);

			}
			else//si no posee tag de fondo o rio lo destruye
			{
				Destroy (trigger.gameObject);
			}
		}
    }

}
