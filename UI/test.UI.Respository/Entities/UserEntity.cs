using System;

namespace test.UI.Respository.Entities
{
    [Serializable]
    public class UserEntity
    {
        public long Id { get; set; }

        public string UserName { get; set; }
    }
}