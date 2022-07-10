using UnityEngine;

namespace Gisha.MechJam.World.Building
{
    public class Structure : MonoBehaviour
    {
        [SerializeField] private StructureData structureData;

        private Cell[] _takenArea;

        private void Start()
        {
            _takenArea = WorldManager.Grid.GetCellsArea(transform.position,
                structureData.GetDimensions(WorldManager.Grid.CellSize), 0f);

            for (int i = 0; i < _takenArea.Length; i++)
                _takenArea[i].isBlockedByStructure = true;
        }
    }
}