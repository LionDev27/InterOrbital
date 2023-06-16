
namespace InterOrbital.WorldSystem
{
    public class Cell
    {
        private int x;
        private int y;
        public string biomeType;
        private bool detail;
        private bool animationTile;
        private bool locked;
        private bool spaceshipArea;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            biomeType = null;
            detail = false;
            animationTile = false;
            locked = false;
            spaceshipArea = false;
        }

        public Cell(int x, int y, string biomeType)
        {
            this.x = x;
            this.y = y;
            this.biomeType = biomeType;
            detail = false;
            animationTile = false;
            locked = false;
            spaceshipArea = false;
        }


        public void AddDetail()
        {
            detail = true;
        }
        
        public void AddAnimationTile()
        {
            animationTile = true;
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
            return detail;
        }

        public bool HaveAnimationTile()
        {
            return animationTile;
        }
    }
}
