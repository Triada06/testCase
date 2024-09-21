

namespace Domain.Base
{
    public abstract class BaseEntity
    {
        public  int Id { get; }
        private static int _counter = 1;
        public BaseEntity()
        {
           Id = _counter++;
        }

    }
}
