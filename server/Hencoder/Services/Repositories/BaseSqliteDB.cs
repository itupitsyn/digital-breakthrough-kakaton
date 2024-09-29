﻿using SQLite;
using System.Linq.Expressions;
using ZeroLevel;
using ZeroLevel.Services.FileSystem;

namespace Hencoder.Services.Repositories
{
    public abstract class BaseSqliteDB<T>
        : IRepository<T>
        where T : class, new()
    {
        protected SQLiteConnection _db;
        public BaseSqliteDB(string name)
        {
            _db = new SQLiteConnection(PrepareDb(name));
        }

        public IEnumerable<T> Query(string query, params object[] args)
        {
            return _db.Query<T>(query, args);
        }

        public IEnumerable<K> SelectByQuery<K>(string query, params object[] args)
            where K : class, new()
        {
            return _db.Query<K>(query, args);
        }

        public int Append(T record)
        {
            return _db.Insert(record);
        }

        public int Append(IEnumerable<T> records)
        {
            return _db.InsertAll(records);
        }
        public CreateTableResult CreateTable()
        {
            return _db.CreateTable<T>();
        }

        public int DropTable()
        {
            return _db.DropTable<T>();
        }

        public IEnumerable<T> SelectAll()
        {
            return _db.Table<T>();
        }

        public IEnumerable<T> SelectBy(Expression<Func<T, bool>> predicate)
        {
            return _db.Table<T>().Where(predicate);
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return _db.Table<T>().FirstOrDefault(predicate);
        }

        public T Single<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> orderBy, bool desc = false)
        {
            if (desc)
            {
                return _db.Table<T>().Where(predicate).OrderByDescending(orderBy).FirstOrDefault();
            }
            return _db.Table<T>().Where(predicate).OrderBy(orderBy).FirstOrDefault();
        }

        public T Single<U>(Expression<Func<T, U>> orderBy, bool desc = false)
        {
            if (desc)
            {
                return _db.Table<T>().OrderByDescending(orderBy).FirstOrDefault();
            }
            return _db.Table<T>().OrderBy(orderBy).FirstOrDefault();
        }

        public IEnumerable<T> SelectBy(int N, Expression<Func<T, bool>> predicate)
        {
            return _db.Table<T>().Where(predicate).Take(N);
        }

        public long Count()
        {
            return _db.Table<T>().Count();
        }

        public long Count(Expression<Func<T, bool>> predicate)
        {
            return _db.Table<T>().Count(predicate);
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            return _db.Table<T>().Delete(predicate);
        }

        public int Update(T record)
        {
            return _db.Update(record);
        }

        protected static string PrepareDb(string path)
        {
            if (Path.IsPathRooted(path) == false)
            {
                path = Path.Combine(FSUtils.GetAppLocalDbDirectory(), path);
            }
            return Path.GetFullPath(path);
        }

        protected abstract void DisposeStorageData();

        public void Dispose()
        {
            DisposeStorageData();
            try
            {
                _db?.Close();
                _db?.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[BaseSqLiteDB] Fault close db connection");
            }
        }
    }
}
