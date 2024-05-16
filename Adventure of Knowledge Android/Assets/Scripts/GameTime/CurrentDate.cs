
namespace AdventureOfKnowledge
{
    public struct CurrentDate
    {
        public int day;
        public int month;
        public int year;

        public CurrentDate(int day, int month, int year)
        {
            this.day = day;
            this.month = month;
            this.year = year;
        }

        public bool CheckItTheCurrentDate() => year == System.DateTime.Now.Year && month == System.DateTime.Now.Month && day == System.DateTime.Now.Day;

    }
}
