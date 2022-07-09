namespace Gisha.MechJam.World
{
    public class Grid
    {
        public static Grid[,] Nodes { private set; get; }

        public static void CreateGrid(int xSize, int ySize)
        {
            Nodes = new Grid[xSize, ySize];
        }
    }

    public class Node
    {
    }
}