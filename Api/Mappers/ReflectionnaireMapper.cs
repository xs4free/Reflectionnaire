using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Api.Mappers
{
    internal static class ReflectionnaireMapper
    {
        public static ReflectionnaireData ReflectionnaireEntityToReflectionnaire(ReflectionnaireEntity entity)
        {
            return new ReflectionnaireData
            {
                Name = entity.Name,
                Description = entity.Description,
                Questions = []
            };
        }
    }
}
