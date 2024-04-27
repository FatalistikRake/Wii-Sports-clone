using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public void RispostaSi() => SceneManager.LoadScene("Tris");

    public void RispostaNo() => SceneManager.LoadScene("NoAnswer");

}
