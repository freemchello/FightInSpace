using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;


namespace Core.Galaxy
{

    [BurstCompile]
    public struct MovableJob : IJobParallelForTransform
    {

        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> Accelerations;
        public NativeArray<Vector3> Positions;
      
        [ReadOnly]
        public float DeltaTime;


        public void Execute(int index, TransformAccess transform)
        {

            Vector3 velocity = Velocities[index] + Accelerations[index];

            transform.position += velocity * DeltaTime;
             
            Velocities[index] = velocity;
            Positions[index] = transform.position;
            Accelerations[index] = Vector3.zero;
        }
    }
}