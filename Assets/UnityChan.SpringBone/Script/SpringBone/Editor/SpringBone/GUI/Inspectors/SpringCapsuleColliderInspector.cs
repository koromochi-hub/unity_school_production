using System.Linq;
using UnityEditor;

namespace UTJ
{
    [CustomEditor(typeof(SpringCapsuleCollider))]
    [CanEditMultipleObjects]
    public class SpringCapsuleColliderInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (boneSelector == null)
            {
                boneSelector = SpringColliderBoneSelector.Create<SpringCapsuleCollider>(
                    targets,
                    (bone, colliders) =>
                        bone.capsuleColliders.Any(collider => colliders.Contains(collider))
                );
            }
            boneSelector.ShowInspector();
        }

        // --- private メンバ ---
        private SpringColliderBoneSelector boneSelector;
    }
}
