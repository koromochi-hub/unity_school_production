﻿using System.Linq;
using UnityEditor;

namespace UTJ
{
    [CustomEditor(typeof(SpringSphereCollider))]
    [CanEditMultipleObjects]
    public class SpringSphereColliderInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (boneSelector == null)
            {
                boneSelector = SpringColliderBoneSelector.Create<SpringSphereCollider>(
                    targets, (bone, colliders) => bone.sphereColliders.Any(collider => colliders.Contains(collider)));
            }
            boneSelector.ShowInspector();
        }

        // private

        private SpringColliderBoneSelector boneSelector;
    }
}