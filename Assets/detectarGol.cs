using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectarGol : MonoBehaviour
{
    public static detectarGol instanciaGol;
    public bool gooooooooool = false;

    private void Awake()
    {
        instanciaGol = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            gooooooooool = true;
        }
    }
}
