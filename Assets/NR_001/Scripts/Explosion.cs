using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// By Pellegrino ~thp~ Principe
namespace NR_001
{
    public class Explosion : MonoBehaviour
    {
        struct Particle
        {
            public GameObject p;
            public float x, y;
            public float vx, vy;
            public float age;
            public void Draw() { if (p) p.transform.position = new Vector3(x, y, 0); }
            public void Remove() { Destroy(p); }
        }

        Material particleMaterial;
        Transform particleParent;

        // the size of the screen
        const int WIDTH = 800;
        const int HEIGHT = 600;

        // how much a particle slows down by each second
        float DRAG = 0.8f;

        // the colour of each particle in R, G, B values
        Color PARTICLE_COLOR = new Color32(255, 230, 128, 255);

        // the time in seconds for which a particle is displayed
        int MAX_AGE = 3;

        // a list to hold the details of the explosion particles on the screen
        List<Particle> particles = new List<Particle>();

        // call the random explosion function every 1.5 seconds
        void Start()
        {
            SetParticlesParent();
            SetParticlesMaterial();
            StartCoroutine(ExplodeRandom());

            print("s");

        }

        // Update is called once per frame
        void Update()
        {
            // to update the particle array, create a new empty array
            var new_particles = new List<Particle>();

            // loop through the existing particle array
            for (int ix = 0; ix < particles.Count; ix++)
            {
                var particle = particles[ix];

                // if a particle was created more than a certain time ago, it can be removed
                if (particle.age + Time.deltaTime > MAX_AGE)
                {
                    particle.Remove();
                    continue;
                }

                // update the particle's velocity - they slow down over time
                var drag = Mathf.Pow(DRAG, Time.deltaTime);
                particle.vx *= drag;
                particle.vy *= drag;

                // update the particle's position according to its velocity
                particle.x += particle.vx * Time.deltaTime;
                particle.y += particle.vy * Time.deltaTime;

                // update the particle's age
                particle.age += Time.deltaTime;

                // add the particle's new position, velocity and age to the new array
                new_particles.Add(particle);
            }

            // replace the current array with the new one
            particles.Clear();
            particles.AddRange(new_particles);

            foreach (var particle in particles)
            {
                particle.Draw();
            }
        }

        void Explode(float x, float y, float speed = 8)
        {
            // these are new particles, so set their age to zero
            var age = 0;

            // generate 100 particles per explosion
            foreach (var _ in Enumerable.Range(0, 100))
            {
                // for each particle, generate a random angle and distance
                var angle = Random.Range(0, 2 * Mathf.PI);
                var radius = Mathf.Pow(Random.Range(0, 1f), 0.5f);

                // convert angle and distance from the explosion point into x and y velocity for the particle
                var vx = speed * radius * Mathf.Sin(angle);
                var vy = speed * radius * Mathf.Cos(angle);

                // add the particle's position, velocity and age to the array
                particles.Add(new Particle { p = CreateParticleImage(), x = x, y = y, vx = vx, vy = vy, age = age });
            }
        }

        IEnumerator ExplodeRandom()
        {
            while (true)
            {
                // select a random position on the screen
                var x = Random.Range(0, WIDTH + 1);
                var y = Random.Range(0, HEIGHT + 1);
                var worldCoord = Camera.main.ScreenToWorldPoint(new Vector2(x, y));

                // call the explosion function for that position
                Explode(worldCoord.x, worldCoord.y);
                yield return new WaitForSeconds(1.5f);
            }
        }

        GameObject CreateParticleImage()
        {
            var p = GameObject.CreatePrimitive(PrimitiveType.Cube);
            p.transform.localScale *= 0.02f;
            p.GetComponent<MeshRenderer>().material = particleMaterial;
            p.transform.SetParent(particleParent);
            return p;
        }

        void SetParticlesMaterial()
        {
            particleMaterial = new Material(Shader.Find("Unlit/Color"));
            particleMaterial.color = PARTICLE_COLOR;
        }

        void SetParticlesParent()
        {
            particleParent = GameObject.Find("Particles").transform;
        }
    }
}