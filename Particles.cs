using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject spherePrefab;
    int numberOfSpheres = 0;
    float minDistance = 5f; // Minimum distance from the cube
    float maxDistance = 30f; // Maximum distance from the cube

    bool particlesCreated = false;

    private List<Transform> particles = new List<Transform>();

    void Start()
    {
        
    }

    void Update()
    {
        if(ExperimentManager.createParticles && !particlesCreated)
        {
            
            for (int i = 0; i < numberOfSpheres; i++)
            {
                // Instantiate spheres within the specified distance range
                float distance = Random.Range(minDistance, maxDistance); // create a random distance between min and max
                Vector3 randomPosition = Random.onUnitSphere * distance; // create a vector [distance, distance, distance]
                GameObject sphere = Instantiate(spherePrefab, randomPosition, Quaternion.identity); // create one GameObject from a [sphere, position, orientation]
                particles.Add(sphere.transform); // add the sphere to particles list
            }
        particlesCreated = true;
        }

    }

}
