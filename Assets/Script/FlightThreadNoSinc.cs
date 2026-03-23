using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightThreadNoSinc : MonoBehaviour
{

    public float speed = 50f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform;
    public Vector2 movementInput;

    //Control de iteraciones
    public int turbulenceIterations = 1000000;


    //Lista de vectores de posición calculados
    private List<Vector3> turbulenceForces = new List<Vector3>();

    //VAriables para manipular el hilo secundario
    private Thread turbulenceThread; //La instancia del hulo secundario
    private bool isTurbulenceRunning = false; //BVandera para saber si sigue el cálculo
   // private bool isTurbulenceThread = false; //BAndera para dsaber si el hilo secundario terminó
    private bool stopTurbulenceThread = true;
    private float capturedTime; //Variable para almacenar el tiempo transcurrido


    //Bandera de control sobre la lectura
    public bool read = false;

    //Ruta de almacenamiento de archivo
    string filepath;

    //Método para mover la nave
    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();


    }

    //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        filepath = Application.dataPath + "/TurbulenceData.txt";
        Debug.Log("Ruta al archivo: " + filepath);
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("No hay cámara asignada.");
            return;
        }


        //tiempo transcurrido 
        capturedTime = Time.time;

        //Proceso pesado en hilo secundario
        if (!isTurbulenceRunning)
        {
            isTurbulenceRunning = true;
            stopTurbulenceThread = false;

            turbulenceThread = new Thread(() =>
            SimulateTurbulence(capturedTime));
            turbulenceThread.Start();

        }

        SimulateTurbulence(capturedTime);


        //MOVER LA NAVE DE FORMA LINEAL
        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        //Mover la nave en rotación
        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0,yaw,0);

        //Actividad 3: Método para la lectura del archivo
        TryReadFile();

    }

    public void SimulateTurbulence(float time)
    {
        turbulenceForces.Clear();

        //Repeticiones

        for (int i = 0; i < turbulenceIterations; i++)
        {
            //Verificar si se debe detener el hilo
            if (stopTurbulenceThread)
            {
                break;
            }
            Vector3 force = new Vector3(
                   Mathf.PerlinNoise(i * 0.001f,time) * 2 - 1,
                   Mathf.PerlinNoise(i * 0.002f,time) * 2 - 1,
                   Mathf.PerlinNoise(i * 003f,time) * 2 - 1
                );
            turbulenceForces.Add(force);
        }

        //Señal en consola de inicio del hilo
        Debug.Log("Iniciamos simulación de turbulencia");

        //Simulación completa
        isTurbulenceRunning = false;

        //Actividad 3: Método para la Escritura del archivo
        //Escritura del archivo

        using (StreamWriter writer = new StreamWriter(filepath,false))
        {
            foreach (var force in turbulenceForces)
            {
                writer.WriteLine(force.ToString());
            }
            writer.Flush(); //Limpiar buffer
        }
        Debug.Log("Archivo Escrito");
        //Simulación completa
        isTurbulenceRunning = false;
    }

    void TryReadFile()
    {
        try
        {
            string content = File.ReadAllText(filepath);
            Debug.Log("Archivo Leído: " + content);
        }
        catch (IOException ex)
        {
            Debug.LogError("Error de ACCESO AL ARCHIVO " + ex.Message);

        }

    }

    private void OnDestroy()
    {
        stopTurbulenceThread = true;

        //Verficar si el hilo existye y se está ejecutando
        if (turbulenceThread != null && turbulenceThread.IsAlive)
        {
            //LO unimos al principal y cerramos ejecución
            turbulenceThread.Join();
        }
    }
}