using sweet.framework.Infrastructure.Attr;
using System.Collections.Generic;
using System.Linq;
using test.UI.Respository.Contract;
using test.UI.Respository.Entities;
using test.UI.Service.Contract;
using test.UI.Service.ServiceDtos;

namespace test.UI.Service
{
    public class AuthService : IAuthService
    {
        private IRoleRepository _roleRepository;

        public AuthService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [Cache(KeyName = "GetUserRole{userId}", Subscribe = new[] { "SetUserRole{userId}" })]
        public List<RoleDto> GetUserRole(long userId)
        {
            var list = _roleRepository.LoadEntities(x => x.Id == userId).ToList().Select(x => new RoleDto { Id = x.Id, Name = x.Name }).ToList();

            return list;
        }

        [Cache(Publish = "SetUserRole{userId}")]
        public bool SetUserRole(long userId, List<RoleDto> roles)
        {
            var entityList = roles.Select(x => new RoleEntity { Name = x.Name }).ToList();

            return _roleRepository.InsertTransaction(entityList) > 0;
        }
    }
}