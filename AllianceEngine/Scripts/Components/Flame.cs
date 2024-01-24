using System;
using System.Numerics;

namespace AllianceEngine
{
    public class Flame: Light
    {
        private readonly float intensityNoise;
        private readonly Random rand = new();
        
        public Flame(Shader shader, Vector3 color, float ia, float il, float @is, float intensityNoise) : base(shader, color, ia, il, @is)
        {
            this.intensityNoise = intensityNoise;
        }

        public override void Update(double deltaTime)
        {
            float randNormal = RandNormal(0.1f, intensityNoise );
            
            Il = Math.Clamp( Il + (float) (2*rand.NextDouble() - 1) * randNormal, 0f, 1f);
            
            base.Update(deltaTime);
        }

        private float RandNormal(float mean, float std)
        {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            
            return (float) (mean + std * randStdNormal);
        }
    }
}