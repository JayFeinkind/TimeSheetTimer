using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetTimer.Mobile.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<List<TEntity>> ReadAllEntities();
        Task<List<TEntity>> ReadAllEntitiesWhere(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> ReadEntityWhere(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> UpdateAllEntities(List<TEntity> entities);
        Task<List<TEntity>> CreateAllEntities(List<TEntity> entities);
        Task DeleteAllEntities(List<TEntity> entities);
		Task PermanentlyDeleteAll(List<TEntity> entities);
       
    }
}
