using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace Core.Galaxy
{

    [BurstCompile]
    public struct GravitationJob : IJobParallelFor
    {

        [ReadOnly]
        public NativeArray<Vector3> Velocities;

        public NativeArray<Vector3> Accelerations;

        [ReadOnly]
        public NativeArray<Vector3> Positions;

        [ReadOnly]
        public NativeArray<float> Masses;

        [ReadOnly]
        public float GravitationModifier;

        [ReadOnly]
        public float DeltaTime;


        public void Execute(int index)
        {

            for (int i = 0; i < Positions.Length; i++)
            {

                if (i == index) continue;

                Vector3 direction = Positions[i] - Positions[index];
             
                float distance = Vector3.Distance(Positions[i], Positions[index]);
                 
                Vector3 gravity = 
                    ( direction * Masses[i] * GravitationModifier )
                                / 
                    ( Masses[index] * Mathf.Pow(distance,2) );
               
                Accelerations[index] += gravity * DeltaTime;
               
            }
        }
    }
}