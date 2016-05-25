using sweet.framework.Infrastructure.Interfaces;
using sweet.framework.Utility.Generation.IdWorker;

namespace sweet.framework.Infrastructure.Model
{
    public abstract class PrimaryEntity : IEntity
    {
        private static readonly IdWorkerGenerator IdWorker = new IdWorkerGenerator(1, 1);

        public PrimaryEntity()
        {
            this.Id = IdWorker.NextId();
        }

        public virtual long Id { set; get; }
    }
}