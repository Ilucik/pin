namespace pin.Infrastructure.Models
{
    public class Pagination
    {
        public int Skip;
        public readonly int Take;

        public Pagination(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }
    }
}
