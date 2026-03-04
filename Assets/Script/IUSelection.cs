using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;



public class IUSelection : MonoBehaviour
{
    public static bool gazedAt;
    [SerializeField]//la linea inmediata, (13) el sig campo lo asigna el inspctpr pero sigue siendo protegiod 
    public float fillTime = 5f;
    public Image radialImage;
    public UnityEvent onFillComplete; //EVvento genérico que se asigna a 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Proceso asíncrono 
    private Coroutine fillCoroutine;

    void Start()
    {
        gazedAt = false;
        radialImage.fillAmount = 0;

    }

    public void OnPointerEnter() 
    {
        gazedAt = true;
        if(fillCoroutine != null )
        {
            StopCoroutine( fillCoroutine );

        }
        fillCoroutine = StartCoroutine(FillRadial());


    }
    public void OnPointerExit()
    {
        gazedAt = false;
        if (fillCoroutine != null) {
            StopCoroutine(fillCoroutine); //Detiene el llenado 

        }
        radialImage.fillAmount = 0f;//Reinicia el llenado a 0

    }
    private IEnumerator FillRadial() 
    {
        float elapsedTime = 0f;
        while(elapsedTime < fillTime) //Dejan de ver el boton 
        {
            if (!gazedAt) {
                yield break;
            }

            //elapsedTime = elapsedTime + Time.deltaTime;
            elapsedTime += Time.deltaTime;
            radialImage.fillAmount = Mathf.Clamp01(elapsedTime/fillTime);
            yield return null;
        }

        //Efecto a ejecuta
        onFillComplete?.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
