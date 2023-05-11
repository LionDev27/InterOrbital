
namespace InterOrbital.WorldSystem
{
    public class Cell
    {
        public int x;
        public int y;
        public string biomeType;
        public bool haveDetail;
        public bool occuped;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            biomeType = null;
            haveDetail = false;
            occuped = false;
        }

        public Cell(int x, int y, string biomeType)
        {
            this.x = x;
            this.y = y;
            this.biomeType = biomeType;
            haveDetail = false;
            occuped = false;
        }

        public void AddDetail()
        {
            haveDetail = true;
        }

        public bool HaveDetail()
        {
            return haveDetail;
        }
    }
}
