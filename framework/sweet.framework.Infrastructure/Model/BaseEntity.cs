using sweet.framework.Infrastructure.Interfaces;
using sweet.framework.Utility.Generation.IdWorker;

namespace sweet.framework.Infrastructure.Model
{
    public class BaseEntity : IEntity
    {
        private static readonly IdWorkerGenerator IdWorker = new IdWorkerGenerator(1, 1);

        private long _id;

        public long Id
        {
            get
            {
                if (_id == 0)
                {
                    _id = IdWorker.NextId();
                }

                return _id;
            }
        }
    }
}