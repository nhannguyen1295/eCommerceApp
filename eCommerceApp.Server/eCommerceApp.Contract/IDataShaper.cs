using System.Collections.Generic;
using System.Dynamic;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Contract
{
    public interface IDataShaper<T>
    {
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);
        ShapedEntity ShapeData(T entity, string fieldsString);
    }
}