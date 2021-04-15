using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class comportamientoRoller : Agent
{
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform tranformTarget;

    public override void OnEpisodeBegin()
    {
        //respawnear en caso de caerse
        if(this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        //mover target en random
        tranformTarget.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //percibir las pos de agente y objetivo 
        sensor.AddObservation(tranformTarget.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        //percibir la vel del agente
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 10;

    public override void OnActionReceived(ActionBuffers actions)
    {
        //vectores de accion
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        //Recompensas
        float distanciaTarget = Vector3.Distance(this.transform.localPosition, tranformTarget.localPosition);

        //Distancia al objetivo
        if (distanciaTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        //Si se cae de la plataforma
        else if (this.transform.localPosition.y < 0)
        {
            SetReward(-2.0f);
            EndEpisode();
        }
    }
}
