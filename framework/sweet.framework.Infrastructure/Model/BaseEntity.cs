using sweet.framework.Infrastructure.Interfaces;
using sweet.framework.Utility.Generation.IdWorker;

namespace sweet.framework.Infrastructure.Model
{
    public class BaseEntity : IEntity<long>
    {
        private static readonly IdWorkerGenerator IdWorker = new IdWorkerGenerator(1, 1);

        public BaseEntity()
        {
            this.Id = IdWorker.NextId();
        }

        public virtual long Id { set; get; }
    }
}