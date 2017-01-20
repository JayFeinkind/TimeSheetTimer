using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TimeSheetTimer.Mobile.ClassLibrary;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.DataAccess
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity, new()
    {
        private delegate Task<int> ActOnEntities(List<TEntity> entities);
        private delegate Task<int> ActOnEntity(object entity);

        private ISqliteConnectionService _context;

        public BaseRepository(ISqliteConnectionService context)
        {
            _context = context;
        }

        // This method is intended to be used with sqlite methods UpdateAllAsync/DeleteAllAsync/CreateAllAsync.
        //  Pass in desired function as dbAction parameter.
        private async Task<List<TEntity>> ModifyEntities(List<TEntity> entityList, ActOnEntities action)
        {
            var result = new List<TEntity>();

            try
            {
                if ((await action(entityList)) < 0)
                {
                    throw new Exception("Error occurred");
                }
                else
                {
                  
                    result = entityList;
                }

            }
            catch (Exception e)
            {
                
            }

            return result;
        }

        private async Task<TEntity> ModifyEntity(TEntity entityList, ActOnEntity action)
        {
            var result = new TEntity();

            try
            {
                if ((await action(entityList)) < 0)
                {
                    throw new Exception("Error occurred");
                }
                else
                {
                    result = entityList;
                }
            }
            catch (Exception e)
            {
                
            }

            return result;
        }


        // Only pass in expressions that can be converted by SQLITE.  
        //		For instance a nullable object's HasValue property will throw an exception.  Coalesce operator will also throw an error
        public virtual async Task<List<TEntity>> ReadAllEntitiesWhere(Expression<Func<TEntity, bool>> predicate)
        {
            var result = new List<TEntity>();

            try
            {
                result = await _context.Instance.Table<TEntity>().Where(predicate).ToListAsync();
            }
			catch (SQLite.SQLiteException)
			{
				await _context.Instance.CreateTableAsync<TEntity> ();
				await ReadAllEntitiesWhere (predicate);
			}
            catch (Exception e)
            {
              
            }

            return result;
        }

        public virtual async Task<List<TEntity>> ReadAllEntities()
        {
            var result = new List<TEntity>();

			try
			{
				result = await _context.Instance.Table<TEntity> ().ToListAsync ();
			}
			catch (SQLite.SQLiteException)
			{
				await _context.Instance.CreateTableAsync<TEntity> ();
				await ReadAllEntities ();
			}
			catch (Exception e)
			{

			}

            return result;
        }

        // Only pass in expressions that can be converted by SQLITE.  
        //		For instance a nullable object's HasValue property will throw an exception.  Coalesce operator will also throw an error
        public virtual async Task<TEntity> ReadEntityWhere(Expression<Func<TEntity, bool>> predicate)
        {
            var result = new TEntity();

            try
            {
                result = await _context.Instance.Table<TEntity>().Where(predicate).FirstOrDefaultAsync();
            }
			catch (SQLite.SQLiteException)
			{
				await _context.Instance.CreateTableAsync<TEntity> ();
				await ReadEntityWhere (predicate);
			}
            catch (Exception e)
            {
               
            }

            return result;
        }

        #region Create/Update/Delete
        // Update, Delete, and Create methods do not set CurrentState for entity.

        public virtual async Task<List<TEntity>> UpdateAllEntities(List<TEntity> entities)
        {
            return await ModifyEntities(entities, _context.Instance.UpdateAllAsync).ConfigureAwait(false);
        }

        public virtual async Task DeleteAllEntities(List<TEntity> entities)
        {
            //sqlite does not support multiple deletes
            foreach (var entity in entities)
            {
                var result = await ModifyEntity(entity, _context.Instance.DeleteAsync).ConfigureAwait(false);
            }
        }

        // create entity or update if it already exists
        public virtual async Task<List<TEntity>> CreateAllEntities(List<TEntity> entities)
        {
           var createResult = new List<TEntity>();

            try
            {
                createResult = await ModifyEntities(entities, _context.Instance.InsertAllAsync).ConfigureAwait(false);               
            }
			catch (SQLite.SQLiteException)
			{
				await _context.Instance.CreateTableAsync<TEntity> ();
				await CreateAllEntities(entities);
			}
            catch (Exception e)
            {
               
            }

            return createResult;
        }

        #endregion
    }
}
