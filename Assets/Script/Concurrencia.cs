using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Concurrencia : MonoBehaviour {
    [Header("Activa los metodos")]
    public bool useSincrono;
    public bool useThreads;
    public bool useTasks;
    public bool useCorutine;

    [Header("Esfera a mover")]
    public Transform sincronoSphere;
    public Transform threadSphere;
    public Transform taskSphere;
    public Transform coroutineSphere;
    public Transform mainCube;

    private Queue<Action> mainThreadActions = new Queue<Action>();

    void Start() {
        // Nombres corregidos para que coincidan con las variables declaradas
        if (useSincrono) MoveSincrono();
        if (useThreads) MoveWithThread();
        if (useTasks) MoveWithTask();
        if (useCorutine) StartCoroutine(MoveWithCoroutine());
    }

    void Update() {
        mainCube.Rotate(Vector3.up, 50 * Time.deltaTime);

        lock (mainThreadActions) {
            while (mainThreadActions.Count > 0) {
                mainThreadActions.Dequeue().Invoke();
            }
        }
    } // <-- Aquí debe cerrar Update

    // --- MÉTODOS FUERA DE UPDATE ---

    void MoveSincrono() {
        for (int i = 0; i < 100; i++) {
            sincronoSphere.position += Vector3.right * 0.05f;
        }
        Thread.Sleep(50);
    }

    void MoveWithThread() {
        new Thread(() => {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                lock (mainThreadActions) {
                    mainThreadActions.Enqueue(() => {
                        threadSphere.position += Vector3.right * 0.05f;
                    });
                }
            }
        }).Start();
    }

    async void MoveWithTask() {
        await Task.Run(() => {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                lock (mainThreadActions) {
                    mainThreadActions.Enqueue(() => {
                        taskSphere.position += Vector3.right * 0.05f;
                    });
                }
            }
        });
    }

    IEnumerator MoveWithCoroutine() {
        for (int i = 0; i <= 100; i++) {
            coroutineSphere.position += Vector3.right * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}