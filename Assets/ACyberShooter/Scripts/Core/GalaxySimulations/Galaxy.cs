using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;


namespace Core.Galaxy
{

    [Serializable]
    public class Galaxy : IDisposable
    {

        [SerializeField] private int _entitiesCount = 10;
        [SerializeField] private int _varriableDistance = 10;
        [SerializeField] private float _varriableVelocity = 10;
        [SerializeField] private float _varriableMass = 10; 
        [SerializeField] private float _gravitationModifier = 10;

        [SerializeField] private List<GameObject> _entitisFabs = new();

        private TransformAccessArray _transformsEntities;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _accelerations;
        private NativeArray<Vector3> _positions;
        private NativeArray<float> _masses;

        public void Dispose()
        {
            _positions.Dispose();
            _velocities.Dispose(); 
         
            _accelerations.Dispose();  
            _masses.Dispose();
            _transformsEntities.Dispose();
            Debug.Log("Disposed");
        }

        public void InitGalaxy()
        {
             
            _velocities = new NativeArray<Vector3>(_entitiesCount, Allocator.Persistent);
            _accelerations = new NativeArray<Vector3>(_entitiesCount, Allocator.Persistent);
            _positions = new NativeArray<Vector3>(_entitiesCount, Allocator.Persistent);
            _masses = new NativeArray<float>(_entitiesCount, Allocator.Persistent);

            Transform[] transforms = new Transform[_entitiesCount];
             
            for(int i = 0; i < _entitiesCount; i++)
            {
                 
                _positions[i] = Random.insideUnitSphere * Random.Range(1, _varriableDistance);
                _velocities[i] = Random.insideUnitSphere * Random.Range(1 , _varriableVelocity);
                _masses[i] =  Random.Range(1, _varriableMass);
                _accelerations[i] = Vector3.zero;

                transforms[i] 
                    = GameObject.Instantiate(_entitisFabs[Random.Range(0, _entitisFabs.Count - 1)],
                    _positions[i],
                    Quaternion.identity).transform;

               
            }
            _transformsEntities = new TransformAccessArray(transforms);
        }


        public void UpdateGalaxy(float deltaTime)
        {
            
            GravitationJob gravitation = new GravitationJob()
            {
                Accelerations = _accelerations,
                DeltaTime = deltaTime,
                Positions = _positions,
                Velocities = _velocities,
                Masses = _masses,
                GravitationModifier = _gravitationModifier,
                
            };
           
            var gravityHandle = gravitation.Schedule(_entitiesCount, 0);
            
            MovableJob movable = new MovableJob()
            {
                Accelerations = _accelerations,
                DeltaTime = deltaTime,
                Positions = _positions,
                Velocities = _velocities,
                 
            };

            var movableHandle = movable.Schedule(_transformsEntities, gravityHandle);
            movableHandle.Complete();
        }
 
       
    }
}