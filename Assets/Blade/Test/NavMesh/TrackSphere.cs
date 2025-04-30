using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace Blade.Test.NavMesh
{
    public class TrackSphere : MonoBehaviour
    {
        [SerializeField] private Transform trackObject;

        private SplineContainer _splineContainer;

        private void Awake()
        {
            _splineContainer = GetComponent<SplineContainer>();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                StartCoroutine(TrackCoroutine());
            }
        }

        private IEnumerator TrackCoroutine()
        {
            float trackTime = 5f;
            float current = 0;
            float percent = 0;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / trackTime;

                Vector3 position = _splineContainer.EvaluatePosition(percent);
                trackObject.position = position;
                yield return null;
            }
        }
    }
}