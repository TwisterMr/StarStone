﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mineScript : MonoBehaviour
{
    private bool minePrimed;
    public float mineExplosionDistance;

    public float mineDamage;

    public float minePrimeTimer;
    private float currentMineTimer;

    private bool hasChangedMaterial;

    public Material primedMaterial;

    [HideInInspector]
    public CharacterVariantOne playerScript;

    private Rigidbody rigidBody;
    private Transform mainCamera;
    private MeshRenderer meshRenderer;

    public AudioClip explosionSound;
    public AudioClip primedClip;

    public GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
        //James' Work\\
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        currentMineTimer = minePrimeTimer;
        //~~~~~~~~~~~~\\
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.AddForce(mainCamera.forward * 500f);
    }

    // Update is called once per frame
    void Update()
    {
        currentMineTimer -= Time.deltaTime;
        minePrimed = currentMineTimer <= 0;

        if (minePrimed)
        {
            //James' Work\\
            if(!hasChangedMaterial)
            {
                meshRenderer.material = primedMaterial;
                AudioSource.PlayClipAtPoint(primedClip, transform.position, 0.25f);
                hasChangedMaterial = true;
            }
            //~~~~~~~~~~~~\\
            foreach (Collider collider in Physics.OverlapSphere(transform.position, mineExplosionDistance))
            {
                if(collider.gameObject.GetComponent<enemyBase>() != null)
                {
                    detonateMine(collider);
                }
            }           
        }

    }

    void detonateMine(Collider enemyCollided)
    {
        enemyCollided.gameObject.GetComponent<enemyBase>().takeDamage(mineDamage);
        GameObject _pSystem = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        _pSystem.GetComponent<AudioSource>().PlayOneShot(explosionSound);
        playerScript.currentActiveMines--;
        Destroy(gameObject);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, mineExplosionDistance);
    }
}
