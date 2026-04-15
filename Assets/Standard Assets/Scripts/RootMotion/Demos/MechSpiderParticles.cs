using UnityEngine;
using UnityEngine.Android;

namespace RootMotion.Demos
{
    public class MechSpiderParticles : MonoBehaviour
    {
        public MechSpiderController mechSpiderController;

        private ParticleSystem particles;

        private void Start()
        {
            particles = (ParticleSystem)GetComponent(typeof(ParticleSystem));
        }

        private void Update()
        {
            float magnitude = mechSpiderController.inputVector.magnitude;
            float constant = Mathf.Clamp(magnitude * 50f, 30f, 50f);

            var emission = particles.emission;


            emission.rateOverTime = new ParticleSystem.MinMaxCurve(constant);
            ParticleSystem.MainModule main = particles.main;
            Color color = particles.main.startColor.color;
            float r = color.r;
            Color color2 = particles.main.startColor.color;
            float g = color2.g;
            Color color3 = particles.main.startColor.color;
            main.startColor = new Color(r, g, color3.b, Mathf.Clamp(magnitude, 0.4f, 1f));
        }
    }
}
