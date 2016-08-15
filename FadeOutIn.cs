using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeOutIn : MonoBehaviour
{
	/// <summary>
	/// Determina si se hace visible o se desvanece
	/// Valor por defecto es false
	/// </summary>
    bool isOut;

	// Variable prueba global
	static bool logoAppear  = true;

	/// <summary>
	/// The fade background.
	/// </summary>
    Image fadeBackground;
    public GameObject intro;
    /// <summary>
    /// The color.
    /// </summary>
    Color color;
    
	/// <summary>
	/// The escena.
	/// </summary>
    int escena;
    void Awake()
    {
        isOut = false;
        fadeBackground = GetComponent<Image> ();
        fadeBackground.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        StartCoroutine(fade());
    }

    /// <summary>
    /// Llamar the specified esc.
    /// </summary>
    /// <param name="esc">Esc.</param>
    public void Llamar(int esc)
    {
        escena = esc;
        StartCoroutine(fade());
    }

	/// <summary>
	/// Fade this instance.
	/// </summary>
    IEnumerator fade()
    {
        color = fadeBackground.color;
        if (isOut == false)
        {
			if (SceneManager.GetActiveScene().name == "MenuEscena" && logoAppear)
            {
                StartCoroutine(fadeIn());
                yield return new WaitForSeconds(3f);
                StartCoroutine(fadeOut());
            }
            yield return new WaitForSeconds(0.7f);
            StartCoroutine(fadeIn());


           // Debug.Log("hola");
        }
        else
        {
            StartCoroutine(fadeOut());
        }
        yield break;
    }
    IEnumerator fadeIn()
    {
        if (logoAppear && SceneManager.GetActiveScene().name == "MenuEscena")
        {
            intro.SetActive(true);
        }
        while (color.a > 0)
        {
            color.a -= 0.1f;
            fadeBackground.color = color;
            yield return new WaitForSeconds(0.01f);
        }
        fadeBackground.enabled = false;
        isOut = true;
        yield break;
    }
    IEnumerator fadeOut()
    {
        fadeBackground.enabled = true;
        while (color.a < 1)
        {

            color.a += 0.1f;
            fadeBackground.color = color;
            yield return new WaitForSeconds(0.01f);
        }

		if (logoAppear && SceneManager.GetActiveScene().name == "MenuEscena")
        {
            intro.SetActive(false);
			logoAppear = false;
        }
        else
        {
            SceneManager.LoadScene(escena);
        }
        isOut = false;
        yield break;
    }
}
