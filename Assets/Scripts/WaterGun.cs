using System.Collections;
using UnityEngine;

public class WaterGun : Hazard
{
    [SerializeField] private float timeUsingWater;

    [SerializeField] private float timeWithoutWater;

    [SerializeField] private ParticleSystem waterParticleSystem;

    protected override void Start()
    {
        StartCoroutine(UseWater());
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            var component = other.GetComponent<Character>();
            if (component.IsAbleToMove())
            {
                component.PreventMoving();
                component.OnDying();
            }
        }
    }

    private IEnumerator UseWater()
    {
        waterParticleSystem.Play();
        playerShouldStop = true;
        for (var i = 0; i < charactersInHazard.Count; i++) charactersInHazard[i].AddHazardToStopFrom(this);
        yield return new WaitForSeconds(timeUsingWater);
        StartCoroutine(StopWater());
    }

    private IEnumerator StopWater()
    {
        waterParticleSystem.Stop();
        playerShouldStop = false;
        yield return new WaitForSeconds(1f);
        for (var i = 0; i < charactersInHazard.Count; i++) charactersInHazard[i].RevoveHazardToStopFrom(this);
        yield return new WaitForSeconds(timeWithoutWater);
        StartCoroutine(UseWater());
    }
}