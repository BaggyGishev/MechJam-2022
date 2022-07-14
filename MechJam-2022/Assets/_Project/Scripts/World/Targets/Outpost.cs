using UnityEngine;

namespace Gisha.MechJam.World.Targets
{
    public class Outpost : Target
    {
        [SerializeField] private Transform buildingTrans;

        [SerializeField] private Material placeholderMaterial;
        [SerializeField] private Material capturedMaterial;

        public override void FinishCapture()
        {
            Debug.Log("Captured");
            SwitchMaterials();
        }

        private void SwitchMaterials()
        {
            var meshRenderers = buildingTrans.GetComponentsInChildren<MeshRenderer>();

            foreach (var mr in meshRenderers)
            {
                Material[] newMaterials = mr.sharedMaterials;
                for (var i = 0; i < mr.sharedMaterials.Length; i++)
                {
                    if (mr.sharedMaterials[i] == placeholderMaterial)
                        newMaterials[i] = capturedMaterial;
                }

                mr.materials = newMaterials;
            }
        }
    }
}