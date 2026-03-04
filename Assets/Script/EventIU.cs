using NUnit.Framework;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class EventIU : MonoBehaviour
{
    public List<GameObject>listaInstrucciones;
    public int currentIndex = 0;
    public List<GameObject> mensajesInstrucciones;
    public TextMeshProUGUI TextMeshProUGUI;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        //Actualiar visibilidad de panenes
        UpdateVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ExitGame() {
        Debug.Log("vA A SALIR");
        Application.Quit();
        Debug.Log("Ya salió");

    }
    //Mérodo para actualizar visibilidad de paneles
    private void UpdateText() {
        if (mensajesInstrucciones.Count >0 ) { 
        
        }
    }
    private void UpdateVisibility() {
        for (int i = 0; i <listaInstrucciones.Count; i++) {
            //Solo el panel en el indice actual esta activo
            listaInstrucciones[i].SetActive(i == currentIndex);
        }
    }
    public void CycleObject() {
        //Implements el indice y vuel ve al inicio 
        currentIndex = (currentIndex + 1) % listaInstrucciones.Count;

        //Actualizar visibilidad
        UpdateVisibility();


    }
    //Metodo para actualizar el texto mostrado 

    public void ChangeSceneByIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ChangeSceneByName(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
