
namespace InterOrbital.WorldSystem
{
    public class Cell
    {
        private int x;
        private int y;
        public string biomeType;
        private bool haveDetail;
        private bool locked;
        private bool spaceshipArea;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            biomeType = null;
            haveDetail = false;
            locked = false;
            spaceshipArea = false;
        }

        public Cell(int x, int y, string biomeType)
        {
            this.x = x;
            this.y = y;
            this.biomeType = biomeType;
            haveDetail = false;
            locked = false;
            spaceshipArea = false;
        }


        public void AddDetail()
        {
            haveDetail = true;
        }

        public void LockCell() 
        {
            locked = true;
        }

        public void UnlockCell()
        {
            locked = false;
        }

        public bool IsLocked()
        {
            return locked;
        }


        public void MakeSpaceshipArea()
        {
            spaceshipArea = true;
        }

        public void UndoSpaceshipArea()
        { 
            spaceshipArea = false; 
        }

        public bool IsSpaceShipArea()
        {
            return spaceshipArea;
        }

        public bool HaveDetail()
        {
            return haveDetail;
        }
    }
}
