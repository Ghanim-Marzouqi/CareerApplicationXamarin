﻿using System;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using Firebase.Database.Query;

namespace CareerApplication.Core.Providers
{
    public class DatabaseProvider
    {
        private readonly FirebaseClient _db;

        public DatabaseProvider(FirebaseClient db)
        {
            _db = db;
        }

        public async Task<bool> Add<T>(string resource, T entity) where T : new()
        {
            var result = await _db.Child(resource).PostAsync(JsonSerializer.Serialize(entity));
            return string.IsNullOrEmpty(result.Key) == false;
        }

        public async Task<IEnumerable<T>> GetAll<T>(string resource, Func<FirebaseObject<T>, T> selector) where T : new()
        {
            return (await _db.Child(resource).OnceAsync<T>()).Select(selector).ToList();
        }

        public async Task<T?> GetById<T>(string resource, Func<T, bool> predicate, Func<FirebaseObject<T>, T> selector) where T : new()
        {
            var items = (await _db.Child(resource).OnceAsync<T>()).Select(selector).ToList();
            return (items != null && items.Count > 0) ? items.FirstOrDefault(predicate) : new T();
        }

        public async Task<bool> Delete<T>(string resource, string key, Func<T, bool> predicate, Func<FirebaseObject<T>, T> selector) where T : new()
        {
            var item = await GetById(resource, predicate, selector);

            if (item == null)
                return false;

            await _db.Child(resource).Child(key).DeleteAsync();

            return true;
        }

        private async Task<bool> Update<T>(string resource, string key, Func<T, bool> predicate, Func<FirebaseObject<T>, T> selector, T entity) where T : new()
        {
            var item = await GetById(resource, predicate, selector);

            if (item == null)
                return false;

            await _db
              .Child(resource)
              .Child(key)
              .PutAsync(entity);

            return true;
        }
    }
}
