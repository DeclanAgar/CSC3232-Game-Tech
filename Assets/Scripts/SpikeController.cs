using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeController : MonoBehaviour
{
    [SerializeField]
    Grid grid;
    [SerializeField]
    GameObject spikeZone;
    [SerializeField]
    private float spikeTime;
    private float timer;
    private bool spikesActive;
    private TilemapRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<TilemapRenderer>();
        spikesActive = false;
        renderer.enabled = spikesActive;
        spikeZone.SetActive(spikesActive);
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Continually running timer to control when spikes activate/deactivate

        // If timer elapsed spikeTime, enable spikes and inform grid map has changed
        if (timer > spikeTime)
        {
            spikesActive = !spikesActive;
            renderer.enabled = spikesActive;
            spikeZone.SetActive(spikesActive);
            timer = 0f;
            grid.SetMapChanged(true);
        }
    }

    public bool GetSpikesActive()
    {
        return spikesActive;
    }
}
