using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class comportamientoAgente : Agent
{
    Rigidbody rBody;
    public Rigidbody rBodyBall;
    private bool tocoPared = false;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform tranformBall;
    public Transform tranformGoal;

    public override void OnEpisodeBegin()
    {
        //respawnear en caso de caerse
        //if(this.transform.localPosition.y < 0)
        //{
            //this.rBody.angularVelocity = Vector3.zero;
            //this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        //}

        /*if (tranformBall.localPosition.y < 0)
        {
            tranformBall.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);

        }*/

        //mover target en random
        rBodyBall.angularVelocity = Vector3.zero;
        rBodyBall.velocity = Vector3.zero;
        tranformBall.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //percibir las pos de agente bola y porteria 
        sensor.AddObservation(tranformBall.localPosition); //observacion pelota
        sensor.AddObservation(tranformGoal.localPosition); //observacion porteria
        sensor.AddObservation(this.transform.localPosition);

        //percibir la vel del agente
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //vectores de accion
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.velocity = new Vector3(controlSignal.x * 10, rBody.velocity.y, controlSignal.z * 10);

        /////////////////Recompensas///////////////////////////
        
        float distanciaPelota = Vector3.Distance(this.transform.localPosition, tranformBall.localPosition);
        float distanciaPelotaPorteria = Vector3.Distance(tranformGoal.localPosition, tranformBall.localPosition);

        //Distancia a pelota - porteria
        if (distanciaPelotaPorteria >= 15)
        {
            SetReward(-0.5f);
        }else if (distanciaPelotaPorteria < 15 && distanciaPelota > 5)
        {
            SetReward(-0.2f);
        }
        else if (distanciaPelotaPorteria <= 5)
        {
            SetReward(-0.1f);
        }
        if (detectarGol.instanciaGol.gooooooooool)
        {
            SetReward(15.0f);
            detectarGol.instanciaGol.gooooooooool = false;
            EndEpisode();
        }

        //Distancia agente - pelota
        if (distanciaPelota <= 5f)
        {
            SetReward(0.5f);
        } else if (distanciaPelota < 1.1f) // tocar
        {
            SetReward(1f);
        }

        

        /*if (tranformBall.localPosition.y < 0)
        {
            SetReward(-20.0f);
            EndEpisode();
        }

        if (transform.localPosition.y < 0)
        {
            SetReward(-10.0f);
            EndEpisode();
        }*/

        //SetReward(-0.02f); //Penalizacion por tiempo

        if(tocoPared)
        {
            tocoPared = false;
            SetReward(-0.01f);
        }
            
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            tocoPared = true;
        }
    }
}
