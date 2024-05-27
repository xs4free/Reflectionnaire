using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Shared;
using Riok.Mapperly.Abstractions;

namespace Reflectionnaire.Api.Mappers
{
    [Mapper] // https://mapperly.riok.app/
    internal partial class ReflectionnaireMapper
    {
        public static partial ReflectionnaireData ReflectionnaireEntityToReflectionnaire(ReflectionnaireEntity entity);

    }
}
