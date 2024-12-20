using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float coef;
    [SerializeField] float power;
    [SerializeField] float radius;
    [SerializeField] float verticalPower;
    [SerializeField] float damage;
    [SerializeField] float falloff;
    [SerializeField] float minDamage;
    TeamAssign team;
    HealthManager health;
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player") && collision.gameObject.layer != LayerMask.NameToLayer("Gun") && collision.gameObject.layer != LayerMask.NameToLayer("NonExplosive"))
        {
            Explode();
            Destroy(gameObject);
        }
    }
    void Explode()
    {
        Instantiate (explosion, transform.position, Quaternion.identity);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            TeamAssign team = hit.GetComponent<TeamAssign>();
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, verticalPower);
            health = hit.gameObject.GetComponent<HealthManager>();
            if(hit.gameObject == this.transform.parent.gameObject)
            {
                health.TakeDamage(0);
            }
            if (hit.gameObject != this.transform.parent.gameObject)
            {
                if (health != null && team.ReturnTeam() != this.transform.parent.gameObject.GetComponent<TeamAssign>().ReturnTeam())
                {
                    health.TakeDamage(System.Convert.ToInt32(Mathf.Clamp(System.Convert.ToSingle((damage - (Vector3.Distance(transform.position, hit.transform.position)) * coef)) - (Vector3.Distance(hit.transform.position, this.transform.parent.position) * falloff), minDamage, damage)));
                }
            }
        }
    }
}
