using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public static class MovementUtility
    {
        private static int ignoreRaycast = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
        private static Dictionary<Transform, AudioSource[]> transformAudioSourceMap;

        // Cast a sphere with the desired distance. Check each collider hit to see if it is within the field of view. Set objectFound
        // to the object that is most directly in front of the agent
        public static Transform WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, LayerMask objectLayerMask)
        {
            Transform objectFound = null;
            var hitColliders = Physics.OverlapSphere(transform.position, viewDistance, objectLayerMask);
            if (hitColliders != null) {
                float minAngle = Mathf.Infinity;
                for (int i = 0; i < hitColliders.Length; ++i) {
                    float angle;
                    Transform obj;
                    // Call the WithinSight function to determine if this specific object is within sight
                    if ((obj = WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, hitColliders[i].transform, false, out angle, objectLayerMask.value)) != null) {
                        // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                        if (angle < minAngle) {
                            minAngle = angle;
                            objectFound = obj;
                        }
                    }
                }
            }
            return objectFound;
        }

#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        // Cast a circle with the desired distance. Check each collider hit to see if it is within the field of view. Set objectFound
        // to the object that is most directly in front of the agent
        public static Transform WithinSight2D(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, LayerMask objectLayerMask)
        {
            Transform objectFound = null;
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, viewDistance, objectLayerMask);
            if (hitColliders != null) {
                float minAngle = Mathf.Infinity;
                for (int i = 0; i < hitColliders.Length; ++i) {
                    float angle;
                    Transform obj;
                    // Call the 2D WithinSight function to determine if this specific object is within sight
                    if ((obj = WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, hitColliders[i].transform, true, out angle, objectLayerMask.value)) != null) {
                        // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                        if (angle < minAngle) {
                            minAngle = angle;
                            objectFound = obj;
                        }
                    }
                }
            }
            return objectFound;
        }
#endif

        // Public helper function that will automatically create an angle variable that is not used. This function is useful if the calling object doesn't
        // care about the angle between transform and targetObject
        public static Transform WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Transform targetObject)
        {
            float angle;
            return WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, false, out angle, ignoreRaycast);
        }

        // Public helper function that will automatically create an angle variable that is not used. This function is useful if the calling object doesn't
        // care about the angle between transform and targetObject
        public static Transform WithinSight2D(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Transform targetObject)
        {
            float angle;
            return WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, true, out angle, ignoreRaycast);
        }

        // Determines if the targetObject is within sight of the transform. It will set the angle regardless of whether or not the object is within sight
        private static Transform WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Transform targetObject, bool usePhysics2D, out float angle, int layerMask)
        {
            // The target object needs to be within the field of view of the current object
            var direction = targetObject.position - (transform.TransformPoint(positionOffset));
            direction.y = 0;
            if (usePhysics2D) {
                angle = Vector3.Angle(direction, transform.up);
            } else {
                angle = Vector3.Angle(direction, transform.forward);
            }
            if (direction.magnitude < viewDistance && angle < fieldOfViewAngle * 0.5f) {
                // The hit agent needs to be within view of the current agent
                if (LineOfSight(transform, positionOffset, targetObject, usePhysics2D, layerMask) != null) {
                    return targetObject; // return the target object meaning it is within sight
                } else if (targetObject.GetComponent<Collider>() == null) {
                    // If the linecast doesn't hit anything then that the target object doesn't have a collider and there is nothing in the way
                    if (targetObject.gameObject.activeSelf)
                        return targetObject;
                }
            }
            // return null if the target object is not within sight
            return null;
        }

        public static Transform LineOfSight(Transform transform, Vector3 positionOffset, Transform targetObject, bool usePhysics2D, int layerMask)
        {
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            if (usePhysics2D) {
                RaycastHit2D hit;
                if ((hit = Physics2D.Linecast(transform.TransformPoint(positionOffset), targetObject.position))) {
                    if (hit.transform.Equals(targetObject)) {
                        return targetObject; // return the target object meaning it is within sight
                    }
                }
            } else {
#endif
                RaycastHit hit;
                if (Physics.Linecast(transform.TransformPoint(positionOffset), targetObject.position, out hit, layerMask)) {
                    if (ContainsTransform(targetObject, hit.transform)) {
                        return targetObject; // return the target object meaning it is within sight
                    }
                }
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            }
#endif
            return null;
        }

        // Cast a sphere with the desired radius. Check each object's audio source to see if audio is playing. If audio is playing
        // and its audibility is greater than the audibility threshold then return the object heard
        public static Transform WithinHearingRange(Transform transform, Vector3 positionOffset, float audibilityThreshold, float hearingRadius, LayerMask objectLayerMask)
        {
            Transform objectHeard = null;
            var hitColliders = Physics.OverlapSphere(transform.TransformPoint(positionOffset), hearingRadius, objectLayerMask);
            if (hitColliders != null) {
                float maxAudibility = 0;
                for (int i = 0; i < hitColliders.Length; ++i) {
                    float audibility = 0;
                    Transform obj;
                    // Call the WithinHearingRange function to determine if this specific object is within hearing range
                    if ((obj = WithinHearingRange(transform, positionOffset, audibilityThreshold, hitColliders[i].transform, ref audibility)) != null) {
                        // This object is within hearing range. Set it to the objectHeard GameObject if the audibility is less than any of the other objects
                        if (audibility > maxAudibility) {
                            maxAudibility = audibility;
                            objectHeard = obj;
                        }
                    }
                }
            }
            return objectHeard;
        }
        
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        // Cast a circle with the desired radius. Check each object's audio source to see if audio is playing. If audio is playing
        // and its audibility is greater than the audibility threshold then return the object heard
        public static Transform WithinHearingRange2D(Transform transform, Vector3 positionOffset, float audibilityThreshold, float hearingRadius, LayerMask objectLayerMask)
        {
            Transform objectHeard = null;
            var hitColliders = Physics2D.OverlapCircleAll(transform.TransformPoint(positionOffset), hearingRadius, objectLayerMask);
            if (hitColliders != null) {
                float maxAudibility = 0;
                for (int i = 0; i < hitColliders.Length; ++i) {
                    float audibility = 0;
                    Transform obj;
                    // Call the WithinHearingRange function to determine if this specific object is within hearing range
                    if ((obj = WithinHearingRange(transform, positionOffset, audibilityThreshold, hitColliders[i].transform, ref audibility)) != null) {
                        // This object is within hearing range. Set it to the objectHeard GameObject if the audibility is less than any of the other objects
                        if (audibility > maxAudibility) {
                            maxAudibility = audibility;
                            objectHeard = obj;
                        }
                    }
                }
            }
            return objectHeard;
        }
#endif

        // Public helper function that will automatically create an audibility variable that is not used. This function is useful if the calling call doesn't
        // care about the audibility value
        public static Transform WithinHearingRange(Transform transform, Vector3 positionOffset, float audibilityThreshold, Transform targetObject)
        {
            float audibility = 0;
            return WithinHearingRange(transform, positionOffset, audibilityThreshold, targetObject, ref audibility);
        }

        private static Transform WithinHearingRange(Transform transform, Vector3 positionOffset, float audibilityThreshold, Transform targetObject, ref float audibility)
        {
            AudioSource[] colliderAudioSource;
            // Check to see if the hit agent has an audio source and that audio source is playing
            if ((colliderAudioSource = GetAudioSources(targetObject)) != null) {
                for (int i = 0; i < colliderAudioSource.Length; ++i) {
                    if (colliderAudioSource[i].isPlaying) {
                        var distance = Vector3.Distance(transform.position, targetObject.position);
                        if (colliderAudioSource[i].rolloffMode == AudioRolloffMode.Logarithmic) {
                            audibility = colliderAudioSource[i].volume / Mathf.Max(colliderAudioSource[i].minDistance, distance - colliderAudioSource[i].minDistance);
                        } else { // linear
                            audibility = colliderAudioSource[i].volume * Mathf.Clamp01((distance - colliderAudioSource[i].minDistance) / (colliderAudioSource[i].maxDistance - colliderAudioSource[i].minDistance)); 
                        }
                        if (audibility > audibilityThreshold) {
                            return targetObject;
                        }
                    }
                }
            }
            return null;
        }

        // Draws the line of sight representation
        public static void DrawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, bool usePhysics2D)
        {
#if UNITY_EDITOR
            var oldColor = UnityEditor.Handles.color;
            var color = Color.yellow;
            color.a = 0.1f;
            UnityEditor.Handles.color = color;

            var halfFOV = fieldOfViewAngle * 0.5f;
            var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D ? Vector3.right : Vector3.up)) * (usePhysics2D ? transform.up : transform.forward);
            UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(positionOffset), (usePhysics2D ? transform.right : transform.up), beginDirection, fieldOfViewAngle, viewDistance);

            UnityEditor.Handles.color = oldColor;
#endif
        }

        // Returns true if the target transform is a child of the parent transform
        private static bool ContainsTransform(Transform target, Transform parent)
        {
            if (target == null) {
                return false;
            }
            if (target.Equals(parent)) {
                return true;
            }
            return ContainsTransform(target.parent, parent);
        }

        // Caches the AudioSource GetComponents for quick lookup
        private static AudioSource[] GetAudioSources(Transform target)
        {
            if (transformAudioSourceMap == null) {
                transformAudioSourceMap = new Dictionary<Transform, AudioSource[]>();
            }

            AudioSource[] audioSources;
            if (transformAudioSourceMap.TryGetValue(target, out audioSources)) {
                return audioSources;
            }

            audioSources = target.GetComponentsInChildren<AudioSource>();
            transformAudioSourceMap.Add(target, audioSources);
            return audioSources;
        }
    }
}